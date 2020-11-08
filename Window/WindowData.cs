using Microsoft.Xna.Framework;
using System;
using System.Runtime.InteropServices;

class WindowData
{
	public Type WindowClass { get; set; }
	public SkinDEP Skin { get; set; }
	public int Cost { get; set; }
	public int Mem { get; set; }

	public static SkinDEP PlayerSkin = new SkinDEP
	{
		Font = Asset.DefaultFont,
		Color = Color.White,
		ErrorColor = new Color(0xBE, 0x26, 0x33),
		DisabledColor = new Color(0x9D, 0x9D, 0x9D),
		WindowTitleFont = Asset.DefaultFont,
		WindowTitleTextColor = Color.Black,
		WindowFramePatch = new Patch(Asset.DefaultTexture, new Point(32, 100), new Point(4, 4)),
		WindowTitlePatch = new Patch(Asset.DefaultTexture, new Point(44, 100), new Point(2, 2)),
		//WindowTitlePatch = new SixPatchDEP(Asset.DefaultTexture, new Point(0, 8), 4),
		//WindowPanelPatch = TODO,
		//ButtonReleasedPatch = new NinePatchDEP(Asset.DefaultTexture, new Point(12, 8), 4),
		//ButtonPressedPatch = new NinePatchDEP(Asset.DefaultTexture, new Point(12, 20), 4),
		//BarBackgroundPatch = new NinePatchDEP(Asset.DefaultTexture, new Point(24, 8), 2),
		//BarFramePatch = new NinePatchDEP(Asset.DefaultTexture, new Point(24, 14), 2),
	};

	public static SkinDEP MalwareSkin = new SkinDEP
	{
		Font = Asset.DefaultFont,
		Color = Color.White,
		ErrorColor = new Color(0xBE, 0x26, 0x33),
		DisabledColor = new Color(0x9D, 0x9D, 0x9D),
		WindowTitleFont = Asset.DefaultFont,
		WindowTitleTextColor = Color.Black,
		//WindowTitlePatch = new SixPatchDEP(Asset.DefaultTexture, new Point(0, 32), 4),
		//WindowFramePatch = new NinePatchDEP(Asset.DefaultTexture, new Point(0, 40), 4),
		//ButtonReleasedPatch = new NinePatchDEP(Asset.DefaultTexture, new Point(12, 32), 4),
		//ButtonPressedPatch = new NinePatchDEP(Asset.DefaultTexture, new Point(12, 44), 4),
		//BarBackgroundPatch = new NinePatchDEP(Asset.DefaultTexture, new Point(24, 32), 2),
		//BarFramePatch = new NinePatchDEP(Asset.DefaultTexture, new Point(24, 38), 2),
	};

	public static SkinDEP ShopSkin = new SkinDEP
	{
		Font = Asset.DefaultFont,
		Color = Color.White,
		ErrorColor = new Color(0xBE, 0x26, 0x33),
		DisabledColor = new Color(0x9D, 0x9D, 0x9D),
		WindowTitleFont = Asset.DefaultFont,
		WindowTitleTextColor = Color.Black,
		//WindowTitlePatch = new SixPatchDEP(Asset.DefaultTexture, new Point(32, 32), 4),
		//WindowFramePatch = new NinePatchDEP(Asset.DefaultTexture, new Point(32, 40), 4),
		//ButtonReleasedPatch = new NinePatchDEP(Asset.DefaultTexture, new Point(44, 32), 4),
		//ButtonPressedPatch = new NinePatchDEP(Asset.DefaultTexture, new Point(44, 44), 4),
		//BarBackgroundPatch = new NinePatchDEP(Asset.DefaultTexture, new Point(56, 32), 2),
		//BarFramePatch = new NinePatchDEP(Asset.DefaultTexture, new Point(56, 38), 2),
	};

	public static readonly WindowData Mine = new WindowData
	{
		WindowClass = typeof(Mine),
		Skin = PlayerSkin,
		Cost = 1024,
		Mem = 16,
	};

	public static readonly WindowData Booster = new WindowData
	{
		WindowClass = typeof(Booster),
		Skin = PlayerSkin,
		Cost = 128,
		Mem = 0,
	};

	public static readonly WindowData Vault = new WindowData
	{
		WindowClass = typeof(Vault),
		Skin = PlayerSkin,
		Cost = 256,
		Mem = 64,
	};

	public static readonly WindowData Malware = new WindowData
	{
		WindowClass = typeof(Malware),
		Skin = MalwareSkin,
	};

	public static readonly WindowData Shop = new WindowData
	{
		WindowClass = typeof(Shop),
		Skin = ShopSkin,
	};
}