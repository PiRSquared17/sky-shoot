using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Statistics;
using SkyShoot.Contracts.SynchroFrames;
using SkyShoot.Contracts.Utils;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Game.Game;
using SkyShoot.Game.Screens;
using SkyShoot.Game.Utils;
using GameLevel = SkyShoot.Contracts.Session.GameLevel;
using Timer = System.Timers.Timer;

namespace SkyShoot.Game.Network
{
	internal class ConnectionManager : ISkyShootService
	{
		#region singleton

		private static ConnectionManager _localInstance;

		public static ConnectionManager Instance
		{
			get { return _localInstance ?? (_localInstance = new ConnectionManager()); }
		}

		private ConnectionManager()
		{
			_eventTimer = new Timer(EVENT_TIMER_DELAY_TIME);
			_synchroFrameTimer = new Timer(SYNCHRO_FRAME_DELAY_TIME);
			_pingTimer = new Timer(1000);
		}

		#endregion

		private ISkyShootService _service;

		private Queue<AGameEvent> _lastClientGameEvents;

		private const int MAX_SERVER_GAME_EVENTS = 100;

		/// <summary>
		/// ������� ������� ����� �������� � ��������
		/// </summary>
		// public static long DifferenceTime { get; private set; }

		#region local thread

		private readonly EventWaitHandle _queue = new AutoResetEvent(false);

		private readonly object _locker = new object();

		private Thread _thread;

		#endregion

		#region server game events and synchroFrame

		// received from server
		private List<AGameEvent> _lastServerGameEvents;
		private SynchroFrame _lastServerSynchroFrame;

		private readonly object _gameEventLocker = new object();
		private readonly object _synchroFrameLocker = new object();

		#endregion

		#region timers and constants

		private readonly Timer _eventTimer;
		private readonly Timer _synchroFrameTimer;
		private readonly Timer _pingTimer;

		private const int EVENT_TIMER_DELAY_TIME = 25;
		private const int SYNCHRO_FRAME_DELAY_TIME = 500;

		#endregion

		private void InitializeConnection()
		{
			try
			{
				var channelFactory = new ChannelFactory<ISkyShootService>("SkyShootEndpoint");
				_service = channelFactory.CreateChannel();

				_pingTimer.Elapsed += (sender, args) => _service.GetServerGameTime();
				_pingTimer.Start();
			}
			catch (Exception e)
			{
				FatalError(e);
			}
		}

		private void FatalError(Exception e)
		{
			Trace.WriteLine(e);

			MessageBox.Message = "Connection error!";
			MessageBox.Next = ScreenManager.ScreenEnum.LoginScreen;
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MessageBoxScreen);
		}

		#region run/stop thread, initialization

		private void InitializeThreadAndTimers()
		{
			_lastClientGameEvents = new Queue<AGameEvent>();

			_thread = new Thread(Run)
			{
				Name = "ConnectionManager"
			};
			_thread.Start();

			_eventTimer.Elapsed += (sender, args) =>
			{
				AGameEvent requestForEvents = new RequestForEvents(null, 0);
				AddClientGameEvent(requestForEvents);
			};
			_synchroFrameTimer.Elapsed += (sender, args) => GetLatestServerSynchroFrame();

			// todo ����� �������������?
			_lastServerGameEvents = new List<AGameEvent>(MAX_SERVER_GAME_EVENTS);

			// getting first synchroFrame
			GetLatestServerSynchroFrame();

			_eventTimer.Start();
			_synchroFrameTimer.Start();
		}

		public void Run()
		{
			while (true)
			{
				AGameEvent gameEvent = null;

				lock (_locker)
				{
					if (_lastClientGameEvents.Count > 0)
					{
						gameEvent = _lastClientGameEvents.Dequeue();
						if (gameEvent == null)
							return;
					}
				}

				if (gameEvent != null)
					SendClientGameEvent(gameEvent);
				else
					_queue.WaitOne();
			}
		}

		public void Stop()
		{
			// stopping thread
			AddClientGameEvent(null);
			_thread.Join();

			_eventTimer.Stop();
			_synchroFrameTimer.Stop();

		}

		public void Dispose()
		{
			// stopping thread
			AddClientGameEvent(null);
			_thread.Join();

			// close EventWaitHandle
			_queue.Close();

			_eventTimer.Dispose();
			_synchroFrameTimer.Dispose();
			_pingTimer.Dispose();
		}

		#endregion

		#region getting the last synchroFrame and game events from server

		public void GetLatestServerSynchroFrame()
		{
			try
			{
				lock (_synchroFrameLocker)
				{
					_lastServerSynchroFrame = _service.SynchroFrame();

//					if (_lastServerSynchroFrame != null)
//					{
//						Trace.Write("Diff time = " + DifferenceTime);
//					}
				}
			}
			catch (Exception exc)
			{
				Trace.WriteLine("game:SynchroFrame" + exc);
			}
		}

		public void GetLatestServerGameEvents()
		{
			AGameEvent[] newServerEvents;
			try
			{
				newServerEvents = _service.GetEvents();
			}
			catch (Exception e)
			{
				Trace.WriteLine("game:GetEvents" + e);
				FatalError(e);
				return;
			}
			lock (_gameEventLocker)
			{
				_lastServerGameEvents.AddRange(newServerEvents);
			}
		}

		#endregion

		#region sending client game events

		private void AddClientGameEvent(AGameEvent gameEvent)
		{
			lock (_locker)
				_lastClientGameEvents.Enqueue(gameEvent);
			_queue.Set();
		}

		private void SendClientGameEvent(AGameEvent gameEvent)
		{
			try
			{
				switch (gameEvent.Type)
				{
					case EventType.ObjectShootEvent:
						var objectShootEvent = gameEvent as ObjectShootEvent;
						if (objectShootEvent != null)
							_service.Shoot(objectShootEvent.ShootDirection);
						break;
					case EventType.ObjectDirectionChangedEvent:
						var objectDirectionChanged = gameEvent as ObjectDirectionChanged;
						if (objectDirectionChanged != null)
							_service.Move(objectDirectionChanged.NewRunDirection);
						break;
					case EventType.RequestForEvents:
						GetLatestServerGameEvents();
						break;
					case EventType.WeaponChangedEvent:
						var weaponChanged = gameEvent as WeaponChanged;
						if (weaponChanged != null)
							_service.ChangeWeapon(weaponChanged.WeaponType);
						break;
					default:
						throw new Exception("Invalid argument");
				}
			}
			catch (Exception e)
			{
				FatalError(e);
			}
		}

		#endregion

		#region service implementation

		/// <summary>
		/// ���������� ��������� ������� �� �������, ������� ���� �������� � ������� ������� ������
		/// ������������ ��������
		/// </summary>
		public AGameEvent[] GetEvents()
		{
			AGameEvent[] events;
			lock (_gameEventLocker)
			{
				events = _lastServerGameEvents.ToArray();
				_lastServerGameEvents.Clear();
			}
			// Logger.PrintEvents(events);
			return events;
		}

		public SynchroFrame SynchroFrame()
		{
			lock (_synchroFrameLocker)
			{
				return _lastServerSynchroFrame;
				// Trace.WriteLine("SYNCHRO_FRAME");
			}
		}

		public void Move(XNA.Framework.Vector2 direction)
		{
			AGameEvent moveGameEvent = new ObjectDirectionChanged(direction, null, 0);
			AddClientGameEvent(moveGameEvent);
		}

		public void Shoot(XNA.Framework.Vector2 direction)
		{
			AGameEvent shootGameEvent = new ObjectShootEvent(direction, null, 0);
			AddClientGameEvent(shootGameEvent);
		}

		public void ChangeWeapon(WeaponType type)
		{
			AGameEvent weaponChangedGameEvent = new WeaponChanged(null, type, null, 0);
			AddClientGameEvent(weaponChangedGameEvent);
		}

		public Stats? GetStats()
		{
			try
			{
				return _service.GetStats();
			}
			catch (Exception e)
			{
				FatalError(e);
				return null;
			}
		}

		#endregion

		#region other service methods

		public AccountManagerErrorCode Register(string username, string password)
		{
			// initialize connection
			InitializeConnection();

			try
			{
				return _service.Register(username, HashHelper.GetMd5Hash(password));
			}
			catch (Exception e)
			{
				FatalError(e);
				return AccountManagerErrorCode.UnknownExceptionOccured;
			}
		}

		public Guid? Login(string username, string password, out AccountManagerErrorCode accountManagerErrorCode)
		{
			// initialize connection
			InitializeConnection();

			accountManagerErrorCode = AccountManagerErrorCode.Ok;

			Guid? login = null;
			try
			{
				login = _service.Login(username, HashHelper.GetMd5Hash(password), out accountManagerErrorCode);
			}
			catch (Exception e)
			{
				FatalError(e);
			}

			if (!login.HasValue)
			{
				MessageBox.Message = "Login error!";
				MessageBox.Next = ScreenManager.ScreenEnum.LoginScreen;
				ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MessageBoxScreen);
			}
			return login;
		}

		public AccountManagerErrorCode Logout()
		{
			AccountManagerErrorCode errorCode = AccountManagerErrorCode.UnknownError;
			try
			{
				errorCode = _service.Logout();
			}
			catch (Exception e)
			{
				FatalError(e);
			}

			if (errorCode != AccountManagerErrorCode.Ok)
			{
				MessageBox.Message = "Logout error!";
				MessageBox.Next = ScreenManager.ScreenEnum.LoginScreen;
				ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MessageBoxScreen);
			}
			return errorCode;
		}

		public GameDescription[] GetGameList()
		{
			try
			{
				return _service.GetGameList();
			}
			catch (Exception e)
			{
				FatalError(e);
				return null;
			}
		}

		public GameDescription CreateGame(GameMode mode, int maxPlayers, TileSet tile, int teams)
		{
			try
			{
				return _service.CreateGame(mode, maxPlayers, tile, teams);
			}
			catch (Exception e)
			{
				FatalError(e);
				return null;
			}
		}

		public bool JoinGame(GameDescription game)
		{
			try
			{
				return _service.JoinGame(game);
			}
			catch (Exception e)
			{
				FatalError(e);
				return false;
			}
		}

		public void LeaveGame()
		{
			try
			{
				_service.LeaveGame();
			}
			catch (Exception e)
			{
				FatalError(e);
			}
		}

		public GameLevel GameStart(int gameId)
		{
			try
			{
				var level = _service.GameStart(gameId);
				if (level != null)
				{
					long serverTime = GetServerGameTime();
					// todo change
					GameController.StartTime = TimeHelper.NowMilliseconds - serverTime;

					_pingTimer.Stop();

					// DifferenceTime = serverTime;

					Trace.WriteLine("Game started on server");

					// todo ����� ����������� (~40 ��), ����� ���-�� ��������
					InitializeThreadAndTimers();

					Trace.WriteLine("ConnectionManager: thread and timers initialized");
				}
				return level;
			}
			catch (Exception e)
			{
				FatalError(e);
				throw;
			}
		}

		public long GetServerGameTime()
		{
			return _service.GetServerGameTime();
		}

		public string[] PlayerListUpdate()
		{
			try
			{
				return _service.PlayerListUpdate();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc);
				return new string[] { };
			}
		}

		#endregion
	}
}
