using Microsoft.Xna.Framework.Graphics;

class Image : Widget
{
	private Sprite sprite;
	public Sprite Sprite
	{
		get => sprite;
		set
		{
			sprite = value;
			Size = sprite.Size;
		}
	}

	public Image SetSprite(Sprite sprite)
	{
		Sprite = sprite;
		return this;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.DrawSprite(Sprite, Position, ColorDEP);
		base.Draw(spriteBatch);
	}
}