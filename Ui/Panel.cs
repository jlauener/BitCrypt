using Microsoft.Xna.Framework.Graphics;

class Panel : Widget
{
	public override void Draw(SpriteBatch spriteBatch)
	{
		if (Skin.Patch != null)
		{
			spriteBatch.Draw(Skin.Patch, ScreenPosition, Size, Color);
		}

		base.Draw(spriteBatch);
	}

	public override string ToString()
	{
		return string.Format("[Panel pos={0}]", ScreenPosition);
	}
}