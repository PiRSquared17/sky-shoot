﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nuclex.Input;

namespace SkyShoot.Game.Input
{
	internal class Gamepad : Controller
	{
		private GamePadState _currentGamePadState;
		private GamePadState _lastGamePadState;

		public Gamepad(InputManager inputManager)
			: base(inputManager)
		{
			// todo windows | XBOX
			_currentGamePadState = InputManager.GetGamePad(ExtendedPlayerIndex.Five).GetState();
		}

		public override Vector2? RunVector
		{
			// TODO, 10%!
			get { return GetRunVector(_currentGamePadState); }
		}

		public override Vector2 SightPosition
		{
			get { return new Vector2(); }
		}

		public override ButtonState ShootButton
		{
			get { return _currentGamePadState.Buttons.X; }
		}

		public bool IsNewButtonPressed(Buttons button)
		{
			return (_currentGamePadState.IsButtonUp(button) && _lastGamePadState.IsButtonDown(button));
		}

		public override void Update()
		{
			// todo wrong, 10%
			_lastGamePadState = _currentGamePadState;

			_currentGamePadState = InputManager.GetGamePad(ExtendedPlayerIndex.Five).GetState();

			if (IsNewButtonPressed(Buttons.DPadDown))
			{
				Index++;
				Index %= Length;
				Console.WriteLine("DOWN" + Index);
				FocusChanged();
			}
			if (IsNewButtonPressed(Buttons.DPadUp))
			{
				Index--;
				if (Index == -1)
					Index = Length - 1;
				Console.WriteLine("UP" + Index);
				FocusChanged();
			}

			if (IsNewButtonPressed(Buttons.X))
			{
				Console.WriteLine("Pressed");
				NotifyListeners(Index);
			}
		}

		private Vector2 GetRunVector(GamePadState gamePadState)
		{
			Vector2 runVector = Vector2.Zero;
			if (gamePadState.IsConnected)
			{
				runVector = gamePadState.ThumbSticks.Left;
				runVector.Y = -runVector.Y;
			}
			Console.WriteLine(runVector);
			return runVector;
		}
	}
}
