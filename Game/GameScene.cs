using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// TODO mouse cursor (maybe sprite class simply?)
class GameScene : Scene
{
	private Widget rootWidget;
	private Widget statusBar;
	private Widget desktop;

	//private TextLabel statusLabel;
	private TextLabel debugLabel;

	//private IntValue barValue;

	public override void Init()
	{
		base.Init();

		Core.BackgroundColor = Computer.DesktopColor;

		rootWidget = new RootWidget().SetSize(Core.ScreenSize);
		desktop = rootWidget.Add<Widget>()
			.SetPosition(0f, 12f)
			.SetSize(Core.ScreenSize.X, Core.ScreenSize.Y - 12);

		Computer.Init(desktop, diskMax: 1024, memMax: 512, coinMax: 512);

		statusBar = rootWidget.Add<Panel>()
			.SetSkin(Computer.StatusBarSkin)
			.SetSize(Core.ScreenSize.X, 16);

		(debugLabel = statusBar.Add<TextLabel>())
			.SetHorizontalAlign(Horizontal.Right)
			.SetVerticalAlign(Vertical.Center)
			.SetPosition(Core.ScreenSize.X / 2, 2f)
			.SetSize(Core.ScreenSize.X / 2 - 4, 12)
		;

		statusBar.Add<Bar>()
			.SetValue(Computer.Disk)
			.SetPosition(4f, 2f)
			.SetSize(56, 12)
			.Add<TextLabel>()
				.SetText("DISK")
				.Center()
		;

		statusBar.Add<Bar>()
			.SetValue(Computer.Mem)
			.SetInverse(true)
			.SetPosition(66f, 2f)
			.SetSize(60, 12)		
			.Add<ValueLabel>()
				.SetFormat("MEM {2}Kb")
				.SetValue(Computer.Mem)
				.Center()
		;

		statusBar.Add<ValueLabel>()
			.SetFormat("COIN {0}/{1}")
			.SetValue(Computer.Coin)
			.SetVerticalAlign(Vertical.Center)
			.SetPosition(140, 2f)
			.SetSize(56, 12)
		;

		//Computer.CreateWindow(WindowData.Mine);

		//var layout = desktop.Add<Layout>();
		//layout.SetSkin(WindowData.ShopSkin).SetPosition(40, 80);
		//layout.Background = WindowData.ShopSkin.WindowFramePatch;

		//var topLayout = layout.Add<Layout>();
		////vLayout.Background = WindowData.PlayerSkin.WindowFramePatch;
		//topLayout.Add<Image>().SetSprite(new Sprite(Asset.DefaultTexture, new Rectangle(4, 64, 28, 28)));
		//topLayout.Add<TextLabel>().SetText("Hello! Welcome to the\nminer's paradise!").Pack();
		//topLayout.PackHorizontally();


		//var hLayout = layout.Add<Layout>();
		////hLayout.Background = WindowData.PlayerSkin.WindowFramePatch;
		//hLayout.Add<Button>().SetSize(32, 32);
		//hLayout.Add<Button>().SetSize(32, 32);
		//hLayout.Add<Button>().SetSize(32, 32);
		//hLayout.PackHorizontally();

		//layout.PackVertically();

		//layout.Add<Panel>().SetSkin(WindowData.PlayerSkin);

		//barValue = new IntValue(0, 1000);

		//var bar = new Bar(barValue);
		//bar.Size = new Point(64, 12);
		//bar.Position = new Vector2(120f, 80f);
		//bar.BackgroundPatch = new NinePatch(Asset.SkinTexture, new Point(26, 22), 2);
		//bar.FramePatch = new NinePatch(Asset.SkinTexture, new Point(32, 22), 2);
		//bar.Add<ValueLabel>().SetValue(barValue).Center().SetSkin(Computer.PlayerSkin);
		//desktop.Add(bar);
		//var w = desktop.Add<Window>().SetTitle("Enemy!");
		//w.Draggable = false;
		//w.AlwaysOnTop = true;
		//w.SetSkin(Computer.Instance.EnemySkin).SetPosition(510f, 190f);
		//w.Add<Label>().SetText("Helloo").SetSize(80, 8);
		//var removeButton = w.Add<Button>();
		//removeButton.SetSize(80, 12);
		//removeButton.Add<Label>().SetText("Remove").Center();
		//removeButton.OnPressed += () => w.Remove();
		//w.Pack();

		//desktop.Add<Button>()
		//	.AddOnPressed(() => Debug.Log("test button pressed"))
		//	.SetSkin(Computer.Instance.PlayerSkin)
		//	.SetSize(64, 64).SetPosition(80f, 50f)
		//		.Add<Label>()
		//		.SetText("Hello!")
		//		.SetHorizontalAlign(Horizontal.Center)
		//		.SetVerticalAlign(Vertical.Center)
		//;
	}

	public override void Update()
	{
		base.Update();

		if (Input.WasKeyPressed(Keys.D1))
		{
			Computer.CreateWindow(WindowData.Mine);
		}

		if (Input.WasKeyPressed(Keys.D2))
		{
			Computer.CreateWindow(WindowData.Vault);
		}

		if (Input.WasKeyPressed(Keys.D3))
		{
			Computer.CreateWindow(WindowData.Malware);
		}

		if (Input.WasKeyPressed(Keys.D4))
		{
			Computer.CreateWindow(WindowData.Shop);
		}

		rootWidget.Update();

		debugLabel.Text = string.Format("{0},{1}", Input.MousePosition.X, Input.MousePosition.Y);

		if ((Input.IsKeyDown(Keys.LeftShift) | Input.IsKeyDown(Keys.RightShift)) && Input.WasKeyPressed(Keys.R))
		{
			Core.Scene = new GameScene();
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
		rootWidget.Draw(spriteBatch);
		spriteBatch.Draw(Asset.DefaultTexture, Input.MousePosition, new Rectangle(0, 0, 8, 8), Color.White);
	}

	public override void DrawDebug(SpriteBatch spriteBatch)
	{
		base.DrawDebug(spriteBatch);
		rootWidget.DrawDebug(spriteBatch);
		// redraw the cursor to make sure it's over the debug overlay
		spriteBatch.Draw(Asset.DefaultTexture, Input.MousePosition, new Rectangle(0, 0, 8, 8), Color.White);
	}
}
