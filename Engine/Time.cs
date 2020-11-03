using Microsoft.Xna.Framework;

static class Time
{
	public static float DeltaTime { get; private set; }
	public static float FPS { get; private set; }

	public static void Update(GameTime gameTime)
	{
		DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
		FPS = 1f / DeltaTime;
	}
}