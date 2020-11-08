using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Patch
{
	private const int TOP_LEFT = 0;
	private const int TOP_RIGHT = 1;
	private const int BOTTOM_LEFT = 2;
	private const int BOTTOM_RIGHT = 3;
	private const int TOP = 4;
	private const int BOTTOM = 5;
	private const int LEFT = 6;
	private const int RIGHT = 7;
	private const int CENTER = 8;

	private readonly Texture2D texture;
	public Point PatchSize { get; }

	private readonly Rectangle[] patches;

	public Patch(Texture2D texture, Point patchOrigin, Point patchSize)
	{
		this.texture = texture;
		this.PatchSize = patchSize;

		patches = new Rectangle[9];
		patches[TOP_LEFT] = new Rectangle(patchOrigin.X, patchOrigin.Y, patchSize.X, patchSize.Y);
		patches[TOP_RIGHT] = new Rectangle(patchOrigin.X + patchSize.X + 1, patchOrigin.Y, patchSize.X, patchSize.Y);
		patches[BOTTOM_LEFT] = new Rectangle(patchOrigin.X, patchOrigin.Y + patchSize.Y + 1, patchSize.X, patchSize.Y);
		patches[BOTTOM_RIGHT] = new Rectangle(patchOrigin.X + patchSize.X + 1, patchOrigin.Y + patchSize.Y + 1, patchSize.X, patchSize.Y);
		patches[TOP] = new Rectangle(patchOrigin.X + patchSize.X, patchOrigin.Y, 1, patchSize.Y);
		patches[BOTTOM] = new Rectangle(patchOrigin.X + patchSize.X, patchOrigin.Y + patchSize.Y + 1, 1, patchSize.Y);
		patches[LEFT] = new Rectangle(patchOrigin.X, patchOrigin.Y + patchSize.Y, patchSize.X, 1);
		patches[RIGHT] = new Rectangle(patchOrigin.X + patchSize.X + 1, patchOrigin.Y + patchSize.Y, patchSize.X, 1);
		patches[CENTER] = new Rectangle(patchOrigin.X + patchSize.X, patchOrigin.Y + patchSize.Y, 1, 1);
	}

	public void Draw(SpriteBatch spriteBatch, Vector2 position, Point size, Color color)
	{
		DrawPatch(spriteBatch, position, TOP_LEFT, color, 0, 0, PatchSize.X, PatchSize.Y);
		DrawPatch(spriteBatch, position, TOP_RIGHT, color, size.X - PatchSize.X, 0, PatchSize.X, PatchSize.Y);
		DrawPatch(spriteBatch, position, BOTTOM_LEFT, color, 0, size.Y - PatchSize.Y, PatchSize.X, PatchSize.Y);
		DrawPatch(spriteBatch, position, BOTTOM_RIGHT, color, size.X - PatchSize.X, size.Y - PatchSize.Y, PatchSize.X, PatchSize.Y);

		DrawPatch(spriteBatch, position, TOP, color, PatchSize.X, 0, size.X - 2 * PatchSize.X, PatchSize.Y);
		DrawPatch(spriteBatch, position, BOTTOM, color, PatchSize.X, size.Y - PatchSize.Y, size.X - 2 * PatchSize.X, PatchSize.Y);
		DrawPatch(spriteBatch, position, LEFT, color, 0, PatchSize.Y, PatchSize.X, size.Y - 2 * PatchSize.Y);
		DrawPatch(spriteBatch, position, RIGHT, color, size.X - PatchSize.X, PatchSize.Y, PatchSize.X, size.Y - 2 * PatchSize.Y);

		DrawPatch(spriteBatch, position, CENTER, color, PatchSize.X, PatchSize.Y, size.X - 2 * PatchSize.X, size.Y - 2 * PatchSize.Y);
	}

	private void DrawPatch(SpriteBatch spriteBatch, Vector2 position, int quadId, Color color, int x, int y, int width, int height)
	{
		x += (int)position.X;
		y += (int)position.Y;
		spriteBatch.Draw(texture, new Rectangle(x, y, width, height), patches[quadId], color);
	}
}