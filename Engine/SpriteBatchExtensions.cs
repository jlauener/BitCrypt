using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

static class SpriteBatchExtensions
{
	public static void DrawSprite(this SpriteBatch spriteBatch, Sprite sprite, Vector2 position, Color color)
	{
		sprite.Draw(spriteBatch, position, color);
	}

	public static void DrawText(this SpriteBatch spriteBatch, BitmapFont font, Vector2 position, string text, Color color)
	{
		font.Draw(spriteBatch, position, text, color);
	}

	public static void DrawPatch(this SpriteBatch spriteBatch, Patch patch, Vector2 position, Point size, Color color)
	{
		patch.Draw(spriteBatch, position, size, color);
	}

	#region Shape

	private static Texture2D fillTexture;
	private static Texture2D GetTexture(SpriteBatch spriteBatch)
	{
		if (fillTexture == null)
		{
			fillTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			fillTexture.SetData(new[] { Color.White });
		}

		return fillTexture;
	}

	public static void DrawLine(this SpriteBatch spriteBatch, Vector2 a, Vector2 b, Color color, float thickness = 1f)
	{
		var distance = Vector2.Distance(a, b);
		var angle = (float)Math.Atan2(b.Y - a.Y, b.X - a.X);
		DrawLine(spriteBatch, a, distance, angle, color, thickness);
	}

	public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color, float thickness = 1f)
	{
		var origin = new Vector2(0f, 0.5f);
		var scale = new Vector2(length, thickness);
		spriteBatch.Draw(GetTexture(spriteBatch), point, null, color, angle, origin, scale, SpriteEffects.None, 0);
	}

	public static void DrawRect(this SpriteBatch spriteBatch, Rectangle rect, Color color, float thickness = 1f)
	{
		DrawLine(spriteBatch, new Vector2(rect.Left, rect.Top), new Vector2(rect.Right, rect.Top), color, thickness);
		DrawLine(spriteBatch, new Vector2(rect.Left, rect.Bottom), new Vector2(rect.Right, rect.Bottom), color, thickness);
		DrawLine(spriteBatch, new Vector2(rect.Left, rect.Top), new Vector2(rect.Left, rect.Bottom), color, thickness);
		DrawLine(spriteBatch, new Vector2(rect.Right, rect.Top), new Vector2(rect.Right, rect.Bottom), color, thickness);
	}

	#endregion
}