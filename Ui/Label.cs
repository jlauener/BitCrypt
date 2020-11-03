using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

enum Horizontal
{
	Left,
	Center,
	Right
}

enum Vertical
{
	Top, 
	Center,
	Bottom
}

class Label : Widget
{
	public string Text { get; set; }
	public Label SetText(string text)
	{
		Text = text;
		return this;
	}

	public Label SetText(string format, params object[] args)
	{
		return SetText(string.Format(format, args));
	}

	public Horizontal HorizontalAlign { get; set; } = Horizontal.Left;
	public Label SetHorizontalAlign(Horizontal align)
	{
		HorizontalAlign = align;
		return this;
	}

	public Vertical VerticalAlign { get; set; } = Vertical.Top;
	public Label SetVerticalAlign(Vertical align)
	{
		VerticalAlign = align;
		return this;
	}

	public Label Center()
	{
		HorizontalAlign = Horizontal.Center;
		VerticalAlign = Vertical.Center;
		return this;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		if (Text != null)
		{
			var position = ScreenPosition;
			var size = Skin.Font.GetSize(Text); // TODO Cache text size, again use dirty flag + Repaint() method !

			switch (HorizontalAlign)
			{
				case Horizontal.Center:
					position.X += Size.X / 2 - size.X / 2;
					break;
				case Horizontal.Right:
					position.X += Size.X - size.X;
					break;
			}

			switch (VerticalAlign)
			{
				case Vertical.Center:
					position.Y += Size.Y / 2 - size.Y / 2;
					break;
				case Vertical.Bottom:
					position.Y += Size.Y - size.Y;
					break;
			}

			spriteBatch.DrawText(Skin.Font, position, Text, Skin.Color);
		}
	}

	public override string ToString()
	{
		return string.Format("[Label pos={0} text={1}]", ScreenPosition, Text);
	}
}