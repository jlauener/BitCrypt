using Microsoft.Xna.Framework.Graphics;

class Panel : Widget
{
	public override void Draw(SpriteBatch spriteBatch)
	{
		if (Skin.Patch != null)
		{
			Skin.Patch.Draw(spriteBatch, ScreenPosition, Size);
		}

		base.Draw(spriteBatch);
	}

	public override string ToString()
	{
		return string.Format("[Panel pos={0}]", ScreenPosition);
	}
}