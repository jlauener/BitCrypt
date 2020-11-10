using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// TODO support other bar orientation
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

	private Vector2 barOffset;
	private Point barSize;
	private Point currentSize;

	public Bar()
	{
		StyleClass = Style.Bar;
	}

	public override void OnAdded()
	{
		base.OnAdded();
		barOffset = new Vector2(Style.PatchAlt.Size.X, Style.PatchAlt.Size.Y);
		barSize = Size;
		barSize.X -= Style.PatchAlt.Size.X * 2;
		barSize.Y -= Style.PatchAlt.Size.Y * 2;
		HandleValueChanged(IntValueChangedEvent.Init);
	}

	public override void OnRemoved()
	{
		base.OnRemoved();
		value.OnChanged -= HandleValueChanged;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.DrawPatch(CurrentStyle.PatchAlt, Position, Size, CurrentStyle.Color);
		spriteBatch.DrawPatch(CurrentStyle.Patch, Position + barOffset, currentSize, CurrentStyle.Color);
		base.Draw(spriteBatch);
	}

	private void HandleValueChanged(IntValueChangedEvent evt)
	{
		var pct = Value.Value / ((float)Value.Max);
		if (Inverse)
		{
			pct = 1f - pct;
		}

		currentSize.X = (int)(pct * barSize.X);
		currentSize.Y = barSize.Y;
	}
}