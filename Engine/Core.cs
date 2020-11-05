using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

class Core : Game
{
	public static Point ScreenSize { get; private set; }
	public static Core Instance { get; private set; }

	public static Color BackgroundColor { get; set; }

	public static Random Random { get; } = new Random();

	private GraphicsDeviceManager graphics;
	private SpriteBatch spriteBatch;
	private RenderTarget2D renderTarget;

	private Rectangle windowBounds;
	private Rectangle fullscreenBounds;

	private Scene scene;

	private bool debug;
	
	public Core()
	{
		Instance = this;

		graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = false;

		// TODO What is the proper way to handle frame update?
		//TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);
		//IsFixedTimeStep = false;
		//graphics.SynchronizeWithVerticalRetrace = true;
	}

	protected override void Initialize()
	{
		Debug.LogInfo("screen size is {0}x{1}", GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height);

		// TODO support other monitor sizes
		var width = 640;
		var height = 360;

		windowBounds = new Rectangle(0, 0, width * 2, height * 2);
		fullscreenBounds = new Rectangle(0, 0, width * 3, height * 3);

		UpdateBackbufferSize();
		graphics.ApplyChanges();

		renderTarget = new RenderTarget2D(GraphicsDevice, width, height);

		ScreenSize = new Point(width, height);
		Debug.LogInfo("game size is {0}", ScreenSize);

		base.Initialize();
	}

	protected override void LoadContent()
	{
		spriteBatch = new SpriteBatch(GraphicsDevice);
		Asset.Init();
		Scene = new GameScene();
	}

	protected override void Update(GameTime gameTime)
	{
		base.Update(gameTime);

		Time.Update(gameTime);
		Input.Update();

		if (Input.IsKeyDown(Keys.Escape))
		{
			Exit();
			return;
		}

		if ((Input.IsKeyDown(Keys.LeftAlt) || Input.IsKeyDown(Keys.RightAlt)) && Input.WasKeyPressed(Keys.Enter))
		{
			graphics.ToggleFullScreen();
			UpdateBackbufferSize();
			graphics.ApplyChanges();
		}

		if (Input.IsKeyDown(Keys.LeftShift) && Input.WasKeyPressed(Keys.OemTilde))
		{
			debug = !debug;
		}

		scene.Update();
	}

	protected override void Draw(GameTime gameTime)
	{
		base.Draw(gameTime);

		GraphicsDevice.SetRenderTarget(renderTarget);
		GraphicsDevice.Clear(BackgroundColor);

		spriteBatch.Begin(samplerState : SamplerState.PointClamp);
		scene.Draw(spriteBatch);
		if (debug) scene.DrawDebug(spriteBatch);
		spriteBatch.End();

		GraphicsDevice.SetRenderTarget(null);
		spriteBatch.Begin(samplerState: SamplerState.PointClamp);
		spriteBatch.Draw(renderTarget, graphics.IsFullScreen ? fullscreenBounds : windowBounds, Color.White);
		spriteBatch.End();
	}

	private void UpdateBackbufferSize()
	{
		var bounds = graphics.IsFullScreen ? fullscreenBounds : windowBounds;
		graphics.PreferredBackBufferWidth = bounds.Width;
		graphics.PreferredBackBufferHeight = bounds.Height;
	}

	public static Scene Scene
	{
		get => Instance.scene;
		set
		{
			Instance.scene = value;
			Instance.scene.Init();
		}
	}
}
