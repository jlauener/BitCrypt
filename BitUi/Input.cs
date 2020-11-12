using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BitUi
{

	public enum MouseButton
	{
		Left,
		Right,
		Middle
	}

	// TODO fix mouse position in window mode (doesn't scale with real mouse position outside of window)
	public static class Input
	{
		private static Vector2 mousePosition;
		public static Vector2 MousePosition => mousePosition;

		private static KeyboardState currentKeyboardState;
		private static KeyboardState previousKeyboardState;

		private static MouseState currentMouseState;
		private static MouseState previousMouseState;

		public static Vector2 ResolutionScale { get; set; } = Vector2.One;

		public static void Update()
		{
			previousKeyboardState = currentKeyboardState;
			previousMouseState = currentMouseState;

			currentKeyboardState = Keyboard.GetState();
			currentMouseState = Mouse.GetState();

			// TODO cast mouse to integer position to prevent window problem... dirty workaround?
			// TODO not castin lead to display artifacts, strange...
			mousePosition.X = (int)(currentMouseState.Position.X * ResolutionScale.X);
			mousePosition.Y = (int)(currentMouseState.Position.Y * ResolutionScale.Y);
		}

		public static bool IsKeyDown(Keys key)
		{
			return currentKeyboardState.IsKeyDown(key);
		}

		public static bool WasKeyPressed(Keys key)
		{
			return !previousKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyDown(key);
		}

		public static bool WasKeyReleased(Keys key)
		{
			return previousKeyboardState.IsKeyDown(key) && !currentKeyboardState.IsKeyDown(key);
		}

		public static bool IsMouseDown(MouseButton button)
		{
			return IsMouseDown(currentMouseState, button);
		}

		public static bool WasMousePressed(MouseButton button)
		{
			return !IsMouseDown(previousMouseState, button) && IsMouseDown(currentMouseState, button);
		}

		public static bool WasMouseReleased(MouseButton button)
		{
			return IsMouseDown(previousMouseState, button) && !IsMouseDown(currentMouseState, button);
		}

		private static bool IsMouseDown(MouseState mouseState, MouseButton button)
		{
			switch (button)
			{
				case MouseButton.Left:
					return mouseState.LeftButton == ButtonState.Pressed;
				case MouseButton.Right:
					return mouseState.RightButton == ButtonState.Pressed;
				default: // MouseButton.Middle:
					return mouseState.MiddleButton == ButtonState.Pressed;
			}
		}
	}

}