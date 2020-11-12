using BitUi;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// TODO implement cursor class
class GameScene : Scene
{
	private Widget rootWidget;
	private Widget desktop;

	private Style styleDefault;
	private Style style1;
	//private TextLabel debugLabel;

	public override void Init()
	{
		base.Init();

		rootWidget = new RootWidget().SetSize(Core.Size);
		desktop = rootWidget.Add<Widget>()
			.SetLocalPosition(0f, 12f)
			.SetSize(Core.Size.X, Core.Size.Y - 12);

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

		//var windowSkin = new Skin
		//{
		//	Texture = Asset.DefaultTexture,
		//	Font = Asset.DefaultFont,
		//	Color = Color.White,
		//	ErrorColor = new Color(0xBE, 0x26, 0x33),
		//	DisabledColor = new Color(0x9D, 0x9D, 0x9D),
		//	Patch = new Patch(Asset.DefaultTexture, new Point(32, 100), new Point(4, 4)),
		//};

		//var titleSkin = windowSkin.CreateChild("Title");
		//titleSkin.Color = Color.Black;
		//titleSkin.Patch = new Patch(Asset.DefaultTexture, new Point(44, 100), new Point(2, 2));

		//var contentSkin = windowSkin.CreateChild("Content");
		//contentSkin.Color = Color.White;
		//contentSkin.Patch = new Patch(Asset.DefaultTexture, new Point(44, 105), new Point(2, 2));

		//var buttonSkin = windowSkin.CreateChild("class:Button");
		//buttonSkin.Patch = new Patch(Asset.DefaultTexture, new Point(49, 100), new Point(2, 2));
		//buttonSkin.PatchAlt = new Patch(Asset.DefaultTexture, new Point(49, 105), new Point(2, 2));

		styleDefault = CreateDefaultStyle();
		style1 = CreateStyle1();

		CreateWindow("DEFAULT", styleDefault);

		//window.Content.Layout = PanelLayout.Vertical;
		//window.Content.Add<TextLabel>().SetText("label 1");
		//window.Content.Add<TextLabel>().SetText("label 2");
		//window.Content.Add<Button>().Add<TextLabel>().SetText("button").Center().SetSize(88, 12);
		//window.Content.Add<Button>().Add<TextLabel>().SetText("another button").Center();
		//window.Content.Add<Button>().Add<Image>().SetSprite(new Sprite(Asset.DefaultTexture, new Rectangle(32, 0, 16, 16)));

		// TODO, bar, close button, over fx, disabled fx, shadow and DONE!

		//desktop.Add<TextLabel>()
		//	.SetText("Hello World!")
		//	.SetSkin(WindowData.PlayerSkin)
		//	.SetLocalPosition(108, 108)
		//;
	}

	private Style CreateDefaultStyle()
	{
		// TODO it is not clear that this applies to the window frame, it's also inherited...
		// it is good to inherir color and font, patch, shadowpatch...
		// inhestir font/color is not much needed, default rules..
		var style = new Style
		{
			Font = Asset.DefaultFont,
			Color = Color.White,
			Patch = new Patch(new Point(0, 100), new Point(5, 5)),
			Margin = new Point(6, 6),
			ShadowPatch = new Patch(new Point(0, 144), new Point(5, 5)),
			WindowDragSfx = Asset.LoadSoundEffect("sfx/window_drag.wav"),
			WindowDropSfx = Asset.LoadSoundEffect("sfx/window_drop.wav"),
		};

		var windowTitle = style.AddClass(Style.WindowTitle, new Style
		{
			Patch = new Patch(new Point(11, 100), new Point(2, 2)),
			Margin = new Point(3, 3),
		});

		windowTitle.AddClass(Style.Label, new Style
		{
			Font = Asset.LoadBitmapFont("font/04b_19.fnt"),
			Color = new Color(0xDE, 0xEE, 0xD6),
			ShadowOffset = new Vector2(1f, 1f)
		})
		.AddState(StyleState.Overed, new Style
		{
			Color = new Color(0xDA, 0xD4, 0x5E),
		});

		style.AddClass(Style.Panel, new Style
		{
			Color = Color.Black,
			Patch = new Patch(new Point(16, 100), new Point(2, 2)),
		});

		style.AddClass(Style.Label, new Style
		{
			Color = new Color(0xDE, 0xEE, 0xD6),
		})
		.AddState(StyleState.Disabled, new Style
		{
			Color = new Color(0x4E, 0x4A, 0x4E),
		});

		var buttonStyle = style.AddClass(Style.Button, new Style
		{
			Color = Color.White,
			Patch = new Patch(new Point(49, 100), new Point(2, 2)),
			ButtonPressedSfx = Asset.LoadSoundEffect("sfx/button_pressed.wav"),
			ErrorSfx = Asset.LoadSoundEffect("sfx/error.wav"),
		})
		.AddState(StyleState.Alt, new Style
		{
			Patch = new Patch(new Point(49, 105), new Point(2, 2)),
		})
		.AddState(StyleState.Disabled, new Style
		{
			Patch = new Patch(new Point(54, 100), new Point(2, 2)),
		})
		.AddState(StyleState.Overed, new Style
		{
			Patch = new Patch(new Point(59, 100), new Point(2, 2)),
		});

		buttonStyle.AddClass(Style.Label, new Style
		{
			Color = Color.LightGray,
		})
		.AddState(StyleState.Overed, new Style
		{
			Color = Color.White,
		})
		.AddState(StyleState.Disabled, new Style
		{
			Color = Color.DarkGray,
		});

		style.AddClass(Style.Bar, new Style
		{
			Patch = new Patch(new Point(44, 112), new Point(1, 1)),
			PatchAlt = new Patch(new Point(48, 112), new Point(1, 1)),
		});

		return style;
	}

	private Style CreateStyle1()
	{
		var style = new Style
		{
			Font = Asset.DefaultFont,
			Color = Color.White,
			Patch = new Patch(new Point(32, 100), new Point(4, 4)),
		};

		var windowTitle = style.AddClass(Style.WindowTitle, new Style
		{
			Patch = new Patch(new Point(44, 105), new Point(2, 2)),
		});

		windowTitle.AddClass(Style.Label, new Style
		{
			Color = Color.DarkGray,
		})
		.AddState(StyleState.Overed, new Style
		{
			Color = Color.LightGray,
		});

		style.AddClass(Style.Panel, new Style
		{
			Color = Color.Black,
			Patch = new Patch(new Point(44, 100), new Point(2, 2)),
		});

		style.AddClass(Style.Label, new Style
		{
			Color = Color.White,
		})
		.AddState(StyleState.Disabled, new Style
		{
			Color = Color.LightGray,
		});

		var buttonStyle = style.AddClass(Style.Button, new Style
		{
			Color = Color.White,
			Patch = new Patch(new Point(49, 100), new Point(2, 2)),
		})
		.AddState(StyleState.Alt, new Style
		{
			Patch = new Patch(new Point(49, 105), new Point(2, 2)),
		})
		.AddState(StyleState.Disabled, new Style
		{
			Patch = new Patch(new Point(54, 100), new Point(2, 2)),
		})
		.AddState(StyleState.Overed, new Style
		{
			Patch = new Patch(new Point(59, 100), new Point(2, 2)),
		});

		buttonStyle.AddClass(Style.Label, new Style
		{
			Color = Color.LightGray,
		})
		.AddState(StyleState.Overed, new Style
		{
			Color = Color.White,
		})
		.AddState(StyleState.Disabled, new Style
		{
			Color = Color.DarkGray,
		});

		style.AddClass(Style.Bar, new Style
		{
			Patch = new Patch(new Point(44, 112), new Point(1, 1)),
			PatchAlt = new Patch(new Point(48, 112), new Point(1, 1)),
		});

		return style;
	}

	private void CreateWindow(string name, Style style)
	{
		var window = desktop.Add<Window>();
		window
			.SetTitle(name)
			.SetStyle(style)
			.SetSize(90, 120)
		;

		var panel = window.Add<Panel>().SetLocalPosition(6, 26).SetSize(78, 87);

		panel.Add<TextLabel>().SetText("Enabled").SetLocalPosition(5, 6).SetSize(68, 8);
		
		var disabledLabel = panel.Add<TextLabel>().SetText("Disabled").SetLocalPosition(5, 18).SetSize(68, 8);
		disabledLabel.Enabled = false;

		panel.Add<Button>().SetLocalPosition(5, 30).SetSize(68, 12)
			.Add<TextLabel>().SetText("Enabled").Center().SetSize(68, 12);
		;

		var disabledButton = panel.Add<Button>().SetLocalPosition(5, 44).SetSize(68, 12);
		disabledButton.Add<TextLabel>().SetText("Disabled").Center().SetSize(68, 12);
		disabledButton.Enabled = false;

		var barValue = new IntValue(12, 30);
		panel.Add<Bar>().SetValue(barValue).SetLocalPosition(5, 58).SetSize(68, 12);

		window.SetLocalPosition(window.GetRandomPosition());
	}

	public override void Update()
	{
		base.Update();

		if (Input.WasKeyPressed(Keys.D1))
		{
			CreateWindow("DEFAULT", styleDefault);
			//Computer.CreateWindow(WindowData.Mine);
		}

		if (Input.WasKeyPressed(Keys.D2))
		{
			CreateWindow("style 1", style1);
			//Computer.CreateWindow(WindowData.Vault);
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
