using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BitUi
{

	public class Button : Widget
	{
		public event Action OnPressed;
		public Button SetOnPressed(Action action)
		{
			OnPressed += action;
			return this;
		}

		private bool pressed;

		public Button()
		{
			StyleClass = Style.Button;
		}

		public override void Resize()
		{
			base.Resize();

			if (Size.X == 0 || Size.Y == 0)
			{
				var size = Point.Zero;
				foreach (var child in Children.list)
				{
					var childPosition = child.LocalPosition;
					size.X = Math.Max(size.X, ((int)childPosition.X) + child.Size.X);
					size.Y = Math.Max(size.Y, ((int)childPosition.Y) + child.Size.Y);
					childPosition.X += Style.Margin.X;
					childPosition.Y += Style.Margin.Y;
					child.LocalPosition = childPosition;
				}
				size.X += Style.Margin.X * 2;
				size.Y += Style.Margin.Y * 2;
				ApplyAutoSize(size);
			}
		}

		public override void OnMouseLeave()
		{
			base.OnMouseLeave();

			if (pressed)
			{
				pressed = false;
				Children.ForEach((child) => child.Offset = Vector2.Zero);

				Style.ButtonReleasedSfx?.Play();
			}
		}

		public override void OnMousePressed()
		{
			base.OnMousePressed();

			if (Enabled)
			{
				pressed = true;
				Children.ForEach((child) => child.Offset = new Vector2(1f, 1f));

				Style.ButtonPressedSfx?.Play();
			}
			else
			{
				Style.ErrorSfx?.Play();
			}

			// TODO what if button is disabled/enabled while pressed?
		}

		public override void OnMouseReleased()
		{
			base.OnMouseReleased();

			if (Enabled)
			{
				OnPressed?.Invoke();
				pressed = false;
				Children.ForEach((child) => child.Offset = Vector2.Zero);

				Style.ButtonReleasedSfx?.Play();
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			var currentStyle = pressed ? Style.GetState(StyleState.Alt) : CurrentStyle;
			spriteBatch.DrawPatch(currentStyle.Patch, Position, Size, currentStyle.Color);
			base.Draw(spriteBatch);
		}

		public override string ToString()
		{
			return string.Format("[Button pos={0} pressed={1}]", Position, pressed);
		}
	}

}