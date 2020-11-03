using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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

	private Label statusLabel;
	private Label debugLabel;

	public override void Init()
	{
		base.Init();

		Computer.Instance = new Computer(1024, 1000, 500);

		Core.BackgroundColor = Computer.Instance.DesktopColor;

		rootWidget = new Widget().SetSize(Core.ScreenSize);

		statusBar = rootWidget.Add<Panel>()
			.SetSkin(Computer.Instance.StatusBarSkin)
			.SetSize(Core.ScreenSize.X, 12);

		(statusLabel = statusBar.Add<Label>())
			.SetVerticalAlign(Vertical.Center)
			.SetPosition(4f, 0f)
			.SetSize(Core.ScreenSize.X / 2, 12)
		;

		(debugLabel = statusBar.Add<Label>())
			.SetHorizontalAlign(Horizontal.Right)
			.SetVerticalAlign(Vertical.Center)
			.SetPosition(Core.ScreenSize.X / 2, 0f)
			.SetSize(Core.ScreenSize.X / 2 - 4, 12)
		;

		desktop = rootWidget.Add<Widget>()
			.SetPosition(0f, 12f)
			.SetSize(Core.ScreenSize.X, Core.ScreenSize.Y - 12);

		SpawnWindow<Mine>();
		//desktop.Add<Mine>().SetPosition(300f, 200f);
		//desktop.Add<Vault>().SetPosition(500f, 180f);
		////desktop.Add<Vault>().SetPosition(510f, 190f);
		//desktop.Add<Vault>().SetPosition(520f, 200f);

		//desktop.Add<Malware>().SetPosition(510f, 190f);

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

	private void SpawnWindow<T>() where T : Window
	{
		var window = Activator.CreateInstance(typeof(T)) as T;
		desktop.Add(window);
		window.SetPosition(window.GetRandomPosition());
	}

	public override void Update()
	{
		base.Update();

		if (Input.WasKeyPressed(Keys.D1))
		{
			SpawnWindow<Mine>();
		}

		if (Input.WasKeyPressed(Keys.D2))
		{
			SpawnWindow<Vault>();
		}

		if (Input.WasKeyPressed(Keys.D3))
		{
			SpawnWindow<Malware>();
		}

		if (Input.WasKeyPressed(Keys.D0))
		{
			for (var i = 0; i < 50; i++)
			{
				SpawnWindow<Mine>();
				SpawnWindow<Vault>();
				SpawnWindow<Malware>();
			}
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

		statusLabel.Text = string.Format("DSK {0}/{1} - MEM 10/1024 - CPU {2}/{3} - coin {4}",
			Computer.Instance.Disk, Computer.Instance.DiskMax,
			Computer.Instance.Cpu, Computer.Instance.CpuMax,
			Computer.Coin
		);

		debugLabel.Text = string.Format("{0},{1} WND={2} FPS={3:0.00}",
			Input.MousePosition.X,
			Input.MousePosition.Y,
			desktop.Children.Count,
			Time.FPS
		);

		rootWidget.Draw(spriteBatch);

		spriteBatch.Draw(Asset.SkinTexture, Input.MousePosition, new Rectangle(0, 0, 8, 8), Color.White);
	}
}
