using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

enum HorizontalAlign
{
	Left,
	Center,
	Right
}

enum VerticalAlign
{
	Top, 
	Center,
	Bottom
}

abstract class Label : Widget
{
	protected string text;

	public HorizontalAlign HorizontalAlign { get; set; } = HorizontalAlign.Left;
	public Label SetHorizontalAlign(HorizontalAlign align)
	{
		HorizontalAlign = align;
		return this;
	}

	public VerticalAlign VerticalAlign { get; set; } = VerticalAlign.Top;
	public Label SetVerticalAlign(VerticalAlign align)
	{
		VerticalAlign = align;
		return this;
	}

	public Label Center()
	{
		HorizontalAlign = HorizontalAlign.Center;
		VerticalAlign = VerticalAlign.Center;
		return this;
	}

	public Label()
	{
		StyleClass = Style.Label;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		if (text != null)
		{
			var position = Position;
			var size = CurrentStyle.Font.GetSize(text); // TODO Cache text size, again use dirty flag + Repaint() method !

			switch (HorizontalAlign)
			{
				case HorizontalAlign.Center:
					position.X += Size.X / 2 - size.X / 2;
					break;
				case HorizontalAlign.Right:
					position.X += Size.X - size.X;
					break;
			}

			switch (VerticalAlign)
			{
				case VerticalAlign.Center:
					position.Y += Size.Y / 2 - size.Y / 2;
					break;
				case VerticalAlign.Bottom:
					position.Y += Size.Y - size.Y;
					break;
			}

			if (CurrentStyle.ShadowOffset != Vector2.Zero)
			{
				spriteBatch.DrawText(CurrentStyle.Font, position + CurrentStyle.ShadowOffset, text, Color.Black);
			}
			spriteBatch.DrawText(CurrentStyle.Font, position, text, CurrentStyle.Color);
		}
	}

	public override string ToString()
	{
		return string.Format("[Label pos={0} text={1}]", Position, text);
	}
}

class TextLabel : Label
{
	public string Text
	{
		get => text;
		set
		{
			text = value;
		}
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

	//public override void Resize()
	//{
	//	base.Resize();

	//	if (!size.HasValue)
	//	{
	//		Size = CurrentStyle.Font.GetSize(Text);
	//	}
	//}
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

	public override void OnAdded()
	{
		base.OnAdded();
		HandleValueChanged(IntValueChangedEvent.Init);
	}

	public override void OnRemoved()
	{
		base.OnRemoved();
		value.OnChanged -= HandleValueChanged;
	}

	private void HandleValueChanged(IntValueChangedEvent evt)
	{
		// TODO
		//Color = Value.Full ? SkinDEP.ErrorColor : SkinDEP.Color;
		text = string.Format(Format, Value.Value, Value.Max, Value.Free);
	}
}