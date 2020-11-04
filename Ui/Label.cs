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

abstract class Label : Widget
{
	protected string text;

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
		if (text != null)
		{
			var position = ScreenPosition;
			var size = Skin.Font.GetSize(text); // TODO Cache text size, again use dirty flag + Repaint() method !

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

			spriteBatch.DrawText(Skin.Font, position, text, Color);
		}
	}

	protected override void ApplySkin()
	{
		base.ApplySkin();
		Color = Skin.TextColor;
	}

	public override string ToString()
	{
		return string.Format("[Label pos={0} text={1}]", ScreenPosition, text);
	}
}

class TextLabel : Label
{
	public string Text
	{
		get => text;
		set => text = value;
	}

	public TextLabel SetText(string text)
	{
		Text = text;
		return this;
	}

	public TextLabel SetText(string format, params object[] args)
	{
		return SetText(string.Format(format, args));
	}
}

class ValueLabel : Label
{
	private IntValue value;
	public IntValue Value
	{
		get => value;
		set
		{
			if (this.value != value)
			{
				if (this.value != null) this.value.OnChanged -= HandleValueChanged;
				value.OnChanged += HandleValueChanged;
				text = string.Format(Format, value.Value, value.Max, value.Free);
				this.value = value;
			}
		}
	}

	public ValueLabel SetValue(IntValue value)
	{
		Value = value;
		return this;
	}

	public string Format { get; set; } = "{0}/{1}";

	public ValueLabel SetFormat(string format)
	{
		Format = format;
		return this;
	}

	public override void OnRemoved()
	{
		base.OnRemoved();
		value.OnChanged -= HandleValueChanged;
	}

	private void HandleValueChanged(IntValueChangedEvent evt)
	{
		text = string.Format(Format, Value.Value, Value.Max, Value.Free);
	}
}