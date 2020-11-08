using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
		UpdateCoroutines();
	}

	protected override void Draw(GameTime gameTime)
	{
		base.Draw(gameTime);

		GraphicsDevice.SetRenderTarget(renderTarget);
		GraphicsDevice.Clear(Core.BackgroundColor);

		// TODO figure out performance difference between Immediate and Deferred
		spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, samplerState: SamplerState.PointClamp);
		scene.Draw(spriteBatch);
		if (debug) scene.DrawDebug(spriteBatch);
		spriteBatch.End();

		GraphicsDevice.SetRenderTarget(null);
		spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, samplerState: SamplerState.PointClamp);
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

	#region Coroutine

	private readonly List<Coroutine> coroutines = new List<Coroutine>();

	public static Coroutine StartCoroutine(IEnumerator routine)
	{
		var coroutine = new Coroutine(routine);
		Instance.coroutines.Add(coroutine);
		Debug.Log("start " + routine);
		return coroutine;
	}

	public static Coroutine WaitForSeconds(float duration)
	{
		return StartCoroutine(WaitForSecondsRoutine(duration));
	}

	private static IEnumerator WaitForSecondsRoutine(float duration)
	{
		while (duration > 0f)
		{
			duration -= Time.DeltaTime;
			yield return null;
		}
		yield break;
	}

	private void UpdateCoroutines()
	{
		foreach (Coroutine coroutine in coroutines.Reverse<Coroutine>())
		{
			if (coroutine.routine.Current is Coroutine)
				coroutine.waitForCoroutine = coroutine.routine.Current as Coroutine;

			if (coroutine.waitForCoroutine != null && coroutine.waitForCoroutine.finished)
				coroutine.waitForCoroutine = null;

			if (coroutine.waitForCoroutine != null)
				continue;

			// update coroutine

			if (coroutine.finished)
			{
				Debug.Log("stop " + coroutine.routine + " (external)");
				coroutines.Remove(coroutine);
			}
			else if (!coroutine.routine.MoveNext())
			{
				Debug.Log("stop " + coroutine.routine + " (internal)");
				coroutines.Remove(coroutine);
				coroutine.finished = true;
			}
			else if (coroutine.finished)
			{
				Debug.Log("stop " + coroutine.routine + " (external)");
				coroutines.Remove(coroutine);
			}
		}
	}

	#endregion

}
