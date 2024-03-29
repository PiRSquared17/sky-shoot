﻿using System;
using System.ServiceModel;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Statistics;
using SkyShoot.Contracts.SynchroFrames;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Contracts.Service
{
	[ServiceContract]
	public interface ISkyShootService
	{
		#region регистрация, логин и создание игры

		[OperationContract(IsInitiating = true)]
		AccountManagerErrorCode Register(string username, string password);

		[OperationContract(IsInitiating = true)]
		Guid? Login(string username, string password, out AccountManagerErrorCode errorCode);

		[OperationContract]
		AccountManagerErrorCode Logout();

		[OperationContract]
		GameDescription[] GetGameList();

		[OperationContract]
		GameDescription CreateGame(GameMode mode, int maxPlayers, TileSet level, int teams);

		[OperationContract]
		bool JoinGame(GameDescription game);

		[OperationContract]
		void LeaveGame();

		/// <summary>
		/// проверка началась ли игра
		/// </summary>
		/// <param name="gameId">идентификатор игры</param>
		/// <returns>если игра не началась возвращает null</returns>
		[OperationContract]
		GameLevel GameStart(int gameId);

		[OperationContract]
		long GetServerGameTime();

		/// <summary>
		/// возвращает список игроков
		/// </summary>
		/// <returns>массив имен игроков</returns>
		[OperationContract]
		String[] PlayerListUpdate();

		#endregion

		#region процесс игры

		[OperationContract]
		void Move(Vector2 direction);

		[OperationContract]
		void Shoot(Vector2 direction);

		[OperationContract]
		void ChangeWeapon(Weapon.WeaponType type);

		[OperationContract]
		AGameEvent[] GetEvents();

		/// <summary>
		/// Получение синхрокадра от сервера
		/// </summary>
		/// <returns>Синхрокадр или null, если игра закончилась</returns>
		[OperationContract]
		SynchroFrame SynchroFrame();

		[OperationContract] // Выдает таблицу данных о уровне, опыте, фрагах
		Stats? GetStats();

		#endregion
	}
}
