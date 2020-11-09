using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// TODO implement cursor class
class GameScene : Scene
{
	private Widget rootWidget;
	private Widget desktop;

	private TextLabel debugLabel;

	public override void Init()
	{
		base.Init();

		rootWidget = new RootWidget().SetSize(Core.ScreenSize);
		desktop = rootWidget.Add<Widget>()
			.SetLocalPosition(0f, 12f)
			.SetSize(Core.ScreenSize.X, Core.ScreenSize.Y - 12);

		Computer.Init(desktop, diskMax: 1024, memMax: 512, coinMax: 512);

		Core.BackgroundColor = Computer.DesktopColor;

		//var statusBar = rootWidget.Add<Panel>()
		//	.SetSkinDEP(Computer.StatusBarSkin)
		//	.SetSize(Core.ScreenSize.X, 16);

		//(debugLabel = statusBar.Add<TextLabel>())
		//	.SetHorizontalAlign(HorizontalAlign.Right)
		//	.SetVerticalAlign(VerticalAlign.Center)
		//	.SetLocalPosition(Core.ScreenSize.X / 2, 2f)
		//	.SetSize(Core.ScreenSize.X / 2 - 4, 12)
		//;

		//statusBar.Add<Bar>()
		//	.SetValue(Computer.Disk)
		//	.SetLocalPosition(4f, 2f)
		//	.SetSize(56, 12)
		//	.Add<TextLabel>()
		//		.SetText("DISK")
		//		.Center()
		//;

		//statusBar.Add<Bar>()
		//	.SetValue(Computer.Mem)
		//	.SetInverse(true)
		//	.SetLocalPosition(66f, 2f)
		//	.SetSize(60, 12)		
		//	.Add<ValueLabel>()
		//		.SetFormat("MEM {2}Kb")
		//		.SetValue(Computer.Mem)
		//		.Center()
		//;

		//statusBar.Add<ValueLabel>()
		//	.SetFormat("COIN {0}/{1}")
		//	.SetValue(Computer.Coin)
		//	.SetVerticalAlign(Vertical.Center)
		//	.SetLocalPosition(140, 2f)
		//	.SetSize(56, 12)
		//;

		//Computer.CreateWindow(WindowData.Mine);		

		var windowSkin = new Skin
		{
			Texture = Asset.DefaultTexture,
			Font = Asset.DefaultFont,
			Color = Color.White,
			ErrorColor = new Color(0xBE, 0x26, 0x33),
			DisabledColor = new Color(0x9D, 0x9D, 0x9D),
			Patch = new Patch(Asset.DefaultTexture, new Point(32, 100), new Point(4, 4)),
		};

		var titleSkin = windowSkin.CreateChild("Title");
		titleSkin.Color = Color.Black;
		titleSkin.Patch = new Patch(Asset.DefaultTexture, new Point(44, 100), new Point(2, 2));

		var contentSkin = windowSkin.CreateChild("Content");
		contentSkin.Color = Color.White;
		contentSkin.Patch = new Patch(Asset.DefaultTexture, new Point(44, 105), new Point(2, 2));

		var buttonSkin = windowSkin.CreateChild("class:Button");
		buttonSkin.Patch = new Patch(Asset.DefaultTexture, new Point(49, 100), new Point(2, 2));
		buttonSkin.PatchAlt = new Patch(Asset.DefaultTexture, new Point(49, 105), new Point(2, 2));


		var window = desktop.Add<Window>();
		window
			.SetTitle("Hello!")
			.SetSkin(windowSkin)
			.SetLocalPosition(100, 100)
		;

		window.Content.Layout = PanelLayout.Vertical;
		window.Content.Add<TextLabel>().SetText("label 1");
		window.Content.Add<TextLabel>().SetText("label 2");
		window.Content.Add<Button>().Add<TextLabel>().SetText("button").Center().SetSize(88, 12);
		window.Content.Add<Button>().Add<TextLabel>().SetText("another button").Center();
		window.Content.Add<Button>().Add<Image>().SetSprite(new Sprite(Asset.DefaultTexture, new Rectangle(32, 0, 16, 16)));

		// TODO, bar, close button, over fx, disabled fx, shadow and DONE!

		//desktop.Add<TextLabel>()
		//	.SetText("Hello World!")
		//	.SetSkin(WindowData.PlayerSkin)
		//	.SetLocalPosition(108, 108)
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
			Computer.CreateWindow(WindowData.Booster);
		}

		if (Input.WasKeyPressed(Keys.D4))
		{
			Computer.CreateWindow(WindowData.Malware);
		}

		if (Input.WasKeyPressed(Keys.D5))
		{
			Computer.CreateWindow(WindowData.Shop);
		}

		if (Input.WasKeyPressed(Keys.F1))
		{
			Computer.Coin.Add(128);
		}

		if (Input.WasKeyPressed(Keys.F2))
		{
			Computer.Coin.Add(1024);
		}

		rootWidget.Update();

		//debugLabel.Text = string.Format("{0},{1}", Input.MousePosition.X, Input.MousePosition.Y);

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
