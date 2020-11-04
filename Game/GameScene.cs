using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

// TODO improve handling of mouse pressed/released event
// should be able to keep the mouse down, go out of the widget -> mouse released
// then, while keeping the button down, go back on the widget -> mouse pressed
// --> even more critical with enemies!!!

// TODO mouse cursor (maybe sprite class simply?)
class GameScene : Scene
{
	private readonly List<Widget> overedWidgets = new List<Widget>();
	private readonly List<Widget> nextOveredWidgets = new List<Widget>();
	private readonly HashSet<Widget> mousePressedWidgets = new HashSet<Widget>();

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

		rootWidget = new Widget().SetSize(Core.ScreenSize);
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

		var diskBar = new Bar(Computer.Disk);
		diskBar
			.SetPosition(4f, 2f)
			.SetSize(56, 12)
		;
		statusBar.Add(diskBar); // TODO need to call it before adding the label to have the skin working...
		diskBar.Add<TextLabel>()
			.SetText("DISK")
			.Center()
		;

		var memBar = new Bar(Computer.Mem);
		memBar.Inverse = true;
		memBar
			.SetPosition(66f, 2f)
			.SetSize(60, 12)
		;
		statusBar.Add(memBar); // TODO need to call it before adding the label to have the skin working...
		memBar.Add<ValueLabel>()
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

		Computer.CreateWindow(WindowData.Mine);

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

		//barValue.Add(1);

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
		rootWidget.Query(Input.MousePosition, nextOveredWidgets);

		for (var i = overedWidgets.Count - 1; i >= 0; i--)
		{
			var w = overedWidgets[i];
			if (!nextOveredWidgets.Contains(w))
			{
				w.IsMouseOver = false;
				overedWidgets.RemoveAt(i);
				mousePressedWidgets.Remove(w);
			}
		}

		foreach (var w in nextOveredWidgets)
		{
			if (!overedWidgets.Contains(w))
			{
				w.IsMouseOver = true;
				overedWidgets.Add(w);
			}
		}

		nextOveredWidgets.Clear();

		if (Input.WasMousePressed(MouseButton.Left))
		{
			foreach (var widget in overedWidgets)
			{
				widget.OnMousePressed();
				mousePressedWidgets.Add(widget);
			}
		}

		if (Input.WasMouseReleased(MouseButton.Left))
		{
			foreach (var w in mousePressedWidgets)
			{
				w.OnMouseReleased();
			}
			mousePressedWidgets.Clear();
		}

		if ((Input.IsKeyDown(Keys.LeftShift) | Input.IsKeyDown(Keys.RightShift)) && Input.WasKeyPressed(Keys.R))
		{
			Core.Scene = new GameScene();
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);

		//debugLabel.Text = string.Format("{0},{1} WND={2} FPS={3:0.00}",
		//	Input.MousePosition.X,
		//	Input.MousePosition.Y,
		//	desktop.Children.Count,
		//	Time.FPS
		//);

		debugLabel.Text = string.Format("{0},{1}", Input.MousePosition.X, Input.MousePosition.Y);

		rootWidget.Draw(spriteBatch);

		spriteBatch.Draw(Asset.SkinTexture, Input.MousePosition, new Rectangle(0, 0, 8, 8), Color.White);
	}
}
