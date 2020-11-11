using BitUi;
using Microsoft.Xna.Framework;
using System;

static class Computer
{
	public static Color DesktopColor { get; set; }
	public static SkinDEP StatusBarSkin { get; set; }

	public static IntValue Disk { get; private set; }
	public static IntValue Mem { get; private set; }
	public static IntValue Coin { get; private set; }

	private static Widget desktop;

	public static void Init(Widget desktop, int diskMax, int memMax, int coinMax)
	{
		Computer.desktop = desktop;

		Disk = new IntValue(diskMax, diskMax);
		Mem = new IntValue(0, memMax);
		Coin = new IntValue(0, coinMax);

		//DesktopColor = new Color(0x00, 0x00, 0x12);
		DesktopColor = new Color(0x77, 0x77, 0x88);

		StatusBarSkin = new SkinDEP
		{
			Font = Asset.DefaultFont,
			Color = Color.Black,
			ErrorColor = new Color(0xBE, 0x26, 0x33),
			//Patch = new NinePatchDEP(Asset.DefaultTexture, new Point(52, 0), 4),
			//BarBackgroundPatch = new NinePatchDEP(Asset.DefaultTexture, new Point(52, 12), 2),
			//BarFramePatch = new NinePatchDEP(Asset.DefaultTexture, new Point(58, 12), 2),
		};
	}

	public static bool CanCreateWindow(WindowData windowData)
	{
		return windowData.Mem < Mem.Free;
	}

	public static bool CreateWindow(WindowData windowData)
	{
		if (!CanCreateWindow(windowData)) return false;

		Mem.Add(windowData.Mem);
		var window = Activator.CreateInstance(windowData.WindowClass) as Window;
		window.SetSkinDEP(windowData.Skin);
		window.OnRemovedEvent += () => Mem.Remove(windowData.Mem);
		desktop.Add(window);
		window.SetLocalPosition(window.GetRandomPosition());
		return true;
	}
}