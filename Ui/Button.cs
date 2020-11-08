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

		if (Enabled)
		{
			pressed = true;
			Children.ForEach((child) => child.Offset = new Vector2(1f, 1f));
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
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		if (pressed)
		{
			spriteBatch.DrawPatch(SkinDEP.ButtonPressedPatch, Position, Size, Enabled ? Color : SkinDEP.DisabledColor);
		}
		else
		{
			spriteBatch.DrawPatch(SkinDEP.ButtonReleasedPatch, Position, Size, Enabled ? Color : SkinDEP.DisabledColor);
		}

		base.Draw(spriteBatch);
	}

	public override string ToString()
	{
		return string.Format("[Button pos={0} pressed={1}]", Position, pressed);
	}
}