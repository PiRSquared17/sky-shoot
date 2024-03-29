﻿using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nuclex.Input;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using SkyShoot.Game.Screens;

namespace SkyShoot.Game.Input
{
	internal class KeyboardAndMouse : Controller
	{
		private KeyboardState _currentKeyboardState;
		private KeyboardState _lastKeyboardState;

		private MouseState _currentMouseState;
		private MouseState _lastMouseState;

		private Vector2 _oldRunVector;

		public KeyboardAndMouse(InputManager inputManager)
			: base(inputManager)
		{
			_currentKeyboardState = InputManager.GetKeyboard().GetState();
			_currentMouseState = InputManager.GetMouse().GetState();
		}

		public override Vector2? RunVector
		{
			get
			{
				Vector2 currentRunVector = GetRunVector(_currentKeyboardState);
				if (!currentRunVector.Equals(_oldRunVector))
				{
					_oldRunVector = currentRunVector;
					return currentRunVector;
				}

				return null;
			}
		}

		public override Vector2 SightPosition
		{
			get { return new Vector2(_currentMouseState.X, _currentMouseState.Y); }
		}

		public override ButtonState ShootButton
		{
			get { return _currentMouseState.LeftButton; }
		}

		private Vector2 GetRunVector(KeyboardState keyboardState)
		{
			Vector2 runVector = Vector2.Zero;
			switch (Settings.Default.KeyboardLayout)
			{
				case 0:
					if (keyboardState.IsKeyDown(Keys.W)) runVector -= Vector2.UnitY;
					if (keyboardState.IsKeyDown(Keys.S)) runVector += Vector2.UnitY;
					if (keyboardState.IsKeyDown(Keys.A)) runVector -= Vector2.UnitX;
					if (keyboardState.IsKeyDown(Keys.D)) runVector += Vector2.UnitX;
					break;
				case 1:
					if (keyboardState.IsKeyDown(Keys.Up)) runVector -= Vector2.UnitY;
					if (keyboardState.IsKeyDown(Keys.Down)) runVector += Vector2.UnitY;
					if (keyboardState.IsKeyDown(Keys.Left)) runVector -= Vector2.UnitX;
					if (keyboardState.IsKeyDown(Keys.Right)) runVector += Vector2.UnitX;
					break;
			}
			if (runVector.Length() > 0)
				runVector.Normalize();
			return runVector;
		}

		public override void Update()
		{
			_lastKeyboardState = _currentKeyboardState;
			_lastMouseState = _currentMouseState;

			_currentKeyboardState = InputManager.GetKeyboard().GetState();
			_currentMouseState = InputManager.GetMouse().GetState();

			if (IsNewKeyPressed(Keys.Down) || IsNewKeyPressed(Keys.Tab))
			{
				Index++;
				Index %= Length;
				// FocusChanged();
			}
			if (IsNewKeyPressed(Keys.Up))
			{
				Index--;
				if (Index == -1)
					Index = Length - 1;
				// FocusChanged();
			}

			if (IsNewKeyPressed(Keys.Enter))
			{
				NotifyListeners(Index);
			}

			if (IsNewKeyPressed(Keys.Escape))
			{
				ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.GameMenuScreen);
			}
		}

		public override void AddListener(Control control, EventHandler buttonPressed)
		{
			base.AddListener(control, buttonPressed);

			Debug.Assert(control is ButtonControl);
			(control as ButtonControl).Pressed += buttonPressed;
		}

		public bool IsNewKeyPressed(Keys key)
		{
			return _currentKeyboardState.IsKeyUp(key) && _lastKeyboardState.IsKeyDown(key);
		}

		public int MouseWheelValue
		{
			get
			{
				int currentScroolWheel = _currentMouseState.ScrollWheelValue;
				int prevScrollWheel = _lastMouseState.ScrollWheelValue;
				return currentScroolWheel - prevScrollWheel;
			}
		}
	}
}
