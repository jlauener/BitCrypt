using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// TODO support other bar orientation (needed?)
class Bar : Widget
{
	public IntValue Value { get; }
	private Rectangle cutOut;

	public bool Inverse;

	private Point barSize;

	public Bar(IntValue value)
	{
		Value = value;
	}

	public override void OnAdded()
	{
		base.OnAdded();

		barSize = Size;
		barSize.X -= 4;
		barSize.Y -= 4;
	}

	public override void Update()
	{
		base.Update();
		// FIXME should react to value change...

		var pct = Value.Value / ((float)Value.Max);
		if (Inverse)
		{
			pct = 1f - pct;
		}

		cutOut.Width = (int)(pct * barSize.X);
		cutOut.Height = barSize.Y;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Skin.BarBackgroundPatch, ScreenPosition, Size, Color);
		spriteBatch.Draw(Skin.BarFramePatch, ScreenPosition + new Vector2(2f, 2f), barSize, Color, cutOut);

		base.Draw(spriteBatch);
	}
}