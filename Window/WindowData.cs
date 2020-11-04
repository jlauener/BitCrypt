using Microsoft.Xna.Framework;
using System;

class WindowData
{
	public Type WindowClass { get; set; }
	public Skin Skin { get; set; }
	public int Cost { get; set; }
	public int Mem { get; set; }

	public static Skin PlayerSkin = new Skin
	{
		Font = Asset.DefaultFont,
		TextColor = Color.White,
		WindowTitleFont = Asset.DefaultFont,
		WindowTitleTextColor = Color.Black,
		WindowTitlePatch = new SixPatch(Asset.SkinTexture, new Point(0, 8), 4),
		WindowFramePatch = new NinePatch(Asset.SkinTexture, new Point(0, 16), 4),
		ButtonReleasedPatch = new NinePatch(Asset.SkinTexture, new Point(12, 8), 4),
		ButtonPressedPatch = new NinePatch(Asset.SkinTexture, new Point(12, 20), 4),
		BarBackgroundPatch = new NinePatch(Asset.SkinTexture, new Point(24, 8), 2),
		BarFramePatch = new NinePatch(Asset.SkinTexture, new Point(24, 14), 2),
	};

	public static Skin MalwareSkin = new Skin
	{
		Font = Asset.DefaultFont,
		TextColor = Color.White,
		WindowTitleFont = Asset.DefaultFont,
		WindowTitleTextColor = Color.Black,
		WindowTitlePatch = new SixPatch(Asset.SkinTexture, new Point(0, 32), 4),
		WindowFramePatch = new NinePatch(Asset.SkinTexture, new Point(0, 40), 4),
		ButtonReleasedPatch = new NinePatch(Asset.SkinTexture, new Point(12, 32), 4),
		ButtonPressedPatch = new NinePatch(Asset.SkinTexture, new Point(12, 44), 4),
		BarBackgroundPatch = new NinePatch(Asset.SkinTexture, new Point(24, 32), 2),
		BarFramePatch = new NinePatch(Asset.SkinTexture, new Point(24, 38), 2),
	};

	public static Skin ShopSkin = new Skin
	{
		Font = Asset.DefaultFont,
		TextColor = Color.White,
		WindowTitleFont = Asset.DefaultFont,
		WindowTitleTextColor = Color.Black,
		WindowTitlePatch = new SixPatch(Asset.SkinTexture, new Point(32, 32), 4),
		WindowFramePatch = new NinePatch(Asset.SkinTexture, new Point(32, 40), 4),
		ButtonReleasedPatch = new NinePatch(Asset.SkinTexture, new Point(44, 32), 4),
		ButtonPressedPatch = new NinePatch(Asset.SkinTexture, new Point(44, 44), 4),
		BarBackgroundPatch = new NinePatch(Asset.SkinTexture, new Point(56, 32), 2),
		BarFramePatch = new NinePatch(Asset.SkinTexture, new Point(56, 38), 2),
	};

	public static readonly WindowData Mine = new WindowData
	{
		WindowClass = typeof(Mine),
		Skin = PlayerSkin,
		Cost = 1024,
		Mem = 16,
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