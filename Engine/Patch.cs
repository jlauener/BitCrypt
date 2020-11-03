using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

abstract class Patch
{
	private readonly Texture2D texture;
	protected readonly Point origin;
	protected readonly int patchSize;

	public Patch(Texture2D texture, Point origin, int patchSize)
	{
		this.texture = texture;
		this.origin = origin;
		this.patchSize = patchSize;
	}

	public abstract void Draw(SpriteBatch spriteBatch, Vector2 position, Point size);

	protected void DrawQuad(SpriteBatch spriteBatch, Vector2 position, int x, int y, int quadX, int quadY)
	{
		spriteBatch.Draw(
			texture,
			new Vector2(position.X + x * patchSize, position.Y + y * patchSize),
			new Rectangle(origin.X + quadX * patchSize, origin.Y + quadY * patchSize, patchSize, patchSize),
			Color.White
		);
	}
}

static class PatchExtensions
{
	public static void Draw(this SpriteBatch spriteBatch, Patch patch, Vector2 position, Point size)
	{
		patch.Draw(spriteBatch, position, size);
	}
}

class SixPatch : Patch
{
	public SixPatch(Texture2D texture, Point origin, int patchSize) : base(texture, origin, patchSize) { }

	public override void Draw(SpriteBatch spriteBatch, Vector2 position, Point size)
	{
		int w = size.X / patchSize;
		int h = size.Y / patchSize;

		// corners
		DrawQuad(spriteBatch, position, 0, 0, 0, 0);
		DrawQuad(spriteBatch, position, w - 1, 0, 2, 0);
		DrawQuad(spriteBatch, position, 0, h - 1, 0, 1);
		DrawQuad(spriteBatch, position, w - 1, h - 1, 2, 1);

		// center
		for (var ix = 1; ix < w - 1; ix++)
		{
			DrawQuad(spriteBatch, position, ix, 0, 1, 0);
			DrawQuad(spriteBatch, position, ix, h - 1, 1, 1);
		}
	}
}

class NinePatch : Patch
{
	public NinePatch(Texture2D texture, Point origin, int patchSize) : base(texture, origin, patchSize) {}

	public override void Draw(SpriteBatch spriteBatch, Vector2 position, Point size)
	{
		int w = size.X / patchSize;
		int h = size.Y / patchSize;

		// corners
		DrawQuad(spriteBatch, position, 0, 0, 0, 0);
		DrawQuad(spriteBatch, position, w - 1, 0, 2, 0);
		DrawQuad(spriteBatch, position, 0, h - 1, 0, 2);
		DrawQuad(spriteBatch, position, w - 1, h - 1, 2, 2);

		// top, bottom
		for (var ix = 1; ix < w - 1; ix++)
		{
			DrawQuad(spriteBatch, position, ix, 0, 1, 0);
			DrawQuad(spriteBatch, position, ix, h - 1, 1, 2);
		}

		// left, right
		for (var iy = 1; iy < h - 1; iy++)
		{
			DrawQuad(spriteBatch, position, 0, iy, 0, 1);
			DrawQuad(spriteBatch, position, w - 1, iy, 2, 1);
		}

		// center
		for (var iy = 1; iy < h - 1; iy++)
		{
			for (var ix = 1; ix < w - 1; ix++)
			{
				DrawQuad(spriteBatch, position, ix, iy, 1, 1);
			}
		}
	}
}
