using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

class Button : Widget
{
	public event Action OnPressed;
	public Button SetOnPressed(Action action)
	{
		OnPressed += action;
		return this;
	}

	private bool pressed;

	public override void OnMouseLeave()
	{
		base.OnMouseLeave();

		pressed = false;
		Children.ForEach((child) => child.Offset = Vector2.Zero);
	}

	public override void OnMousePressed()
	{
		base.OnMousePressed();

		pressed = true;
		Children.ForEach((child) => child.Offset = new Vector2(1f, 1f));
	}

	public override void OnMouseReleased()
	{
		base.OnMouseReleased();

		OnPressed?.Invoke();
		pressed = false;
		Children.ForEach((child) => child.Offset = Vector2.Zero);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		if (pressed)
		{
			spriteBatch.Draw(Skin.ButtonPressedPatch, ScreenPosition, Size, Color);
		}
		else
		{
			spriteBatch.Draw(Skin.ButtonReleasedPatch, ScreenPosition, Size, Color);
		}

		base.Draw(spriteBatch);
	}

	public override string ToString()
	{
		return string.Format("[Button pos={0} pressed={1}]", ScreenPosition, pressed);
	}
}