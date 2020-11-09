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

	protected override Skin ApplySkin(Skin skin)
	{
		var buttonSkin = skin.GetChild("class:Button");
		if (buttonSkin != null)
		{
			return buttonSkin;
		}

		return base.ApplySkin(Skin);
	}

	public override void Resize()
	{
		base.Resize();

		var size = Point.Zero;
		foreach (var child in Children.list)
		{
			size.X = Math.Max(size.X, ((int)child.LocalPosition.X) + child.Size.X);
			size.Y = Math.Max(size.Y, ((int)child.LocalPosition.Y) + child.Size.Y);

			var childPosition = child.LocalPosition;
			childPosition.X += Skin.Patch.Margin.X;
			childPosition.Y += Skin.Patch.Margin.Y;
			child.LocalPosition = childPosition;
		}
		size.X += Skin.Patch.Margin.X * 2;
		size.Y += Skin.Patch.Margin.Y * 2;
		Size = size;
	}

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
			spriteBatch.DrawPatch(Skin.PatchAlt, Position, Size, Enabled || !Skin.DisabledColor.HasValue ? Skin.Color : Skin.DisabledColor.Value);
		}
		else
		{
			spriteBatch.DrawPatch(Skin.Patch, Position, Size, Enabled ? Color : SkinDEP.DisabledColor);
		}

		base.Draw(spriteBatch);
	}

	public override string ToString()
	{
		return string.Format("[Button pos={0} pressed={1}]", Position, pressed);
	}
}