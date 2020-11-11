using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BitUi
{

	public class Sprite
	{
		public Point Size => sourceRect.Size;

		private readonly Texture2D texture;
		private readonly Rectangle sourceRect;

		public Sprite(Texture2D texture, Rectangle sourceRect)
		{
			this.texture = texture;
			this.sourceRect = sourceRect;
		}

		public Sprite(Texture2D texture) : this(texture, texture.Bounds) { }

		public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
		{
			spriteBatch.Draw(texture, position, sourceRect, color);
		}
	}

}
