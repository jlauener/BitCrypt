using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// TODO support other bar orientation (needed?)
class Bar : Widget
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

	public Bar SetValue(IntValue value)
	{
		Value = value;
		return this;
	}

	public bool Inverse { get; set; }

	public Bar SetInverse(bool inverse)
	{
		Inverse = inverse;
		return this;
	}

	private Rectangle cutOut;
	private Point barSize;

	public override void OnAdded()
	{
		base.OnAdded();

		barSize = Size;
		barSize.X -= 4;
		barSize.Y -= 4;
		HandleValueChanged(IntValueChangedEvent.Init);
	}

	public override void OnRemoved()
	{
		base.OnRemoved();
		value.OnChanged -= HandleValueChanged;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.DrawPatch(Skin.BarBackgroundPatch, ScreenPosition, Size, Color);
		spriteBatch.DrawPatch(Skin.BarFramePatch, ScreenPosition + new Vector2(2f, 2f), barSize, Color, cutOut);

		base.Draw(spriteBatch);
	}

	private void HandleValueChanged(IntValueChangedEvent evt)
	{
		var pct = Value.Value / ((float)Value.Max);
		if (Inverse)
		{
			pct = 1f - pct;
		}

		cutOut.Width = (int)(pct * barSize.X);
		cutOut.Height = barSize.Y;
	}
}