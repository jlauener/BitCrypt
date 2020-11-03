using Microsoft.Xna.Framework;

class Computer
{
	public static Computer Instance { get; set; }

	public Color DesktopColor { get; set; }
	public Skin StatusBarSkin { get; set; }
	public Skin PlayerSkin { get; set; }
	public Skin EnemySkin { get; set; } // TODO should be somewhere else, EnemyData ?

	public int Disk { get; private set; }
	public int DiskMax { get; private set; }

	public int Cpu { get; private set; }
	public int CpuMax { get; private set; }

	public static IntValue Coin { get; private set; }

	public Computer(int diskMax, int cpuMax, int coinMax)
	{
		DiskMax = diskMax;
		CpuMax = cpuMax;
		Coin = new IntValue(coinMax, coinMax);

		DesktopColor = new Color(0x00, 0x00, 0x12);

		StatusBarSkin = new Skin
		{
			Font = Asset.DefaultFont,
			Color = Color.Black,
			Patch = new NinePatch(Asset.SkinTexture, new Point(52, 0), 4),
		};

		PlayerSkin = new Skin
		{
			Font = Asset.DefaultFont,
			Color = Color.White,
			WindowTitleFont = Asset.DefaultFont,
			WindowTitleTextColor = Color.Black,
			WindowTitlePatch = new SixPatch(Asset.SkinTexture, new Point(0, 8), 4),
			WindowFramePatch = new NinePatch(Asset.SkinTexture, new Point(0, 16), 4),
			ButtonReleasedPatch = new NinePatch(Asset.SkinTexture, new Point(12, 8), 4),
			ButtonPressedPatch = new NinePatch(Asset.SkinTexture, new Point(12, 20), 4),
		};

		EnemySkin = new Skin
		{
			Font = Asset.DefaultFont,
			Color = Color.White,
			WindowTitleFont = Asset.DefaultFont,
			WindowTitleTextColor = Color.Black,
			WindowTitlePatch = new SixPatch(Asset.SkinTexture, new Point(0, 32), 4),
			WindowFramePatch = new NinePatch(Asset.SkinTexture, new Point(0, 40), 4),
			ButtonReleasedPatch = new NinePatch(Asset.SkinTexture, new Point(12, 32), 4),
			ButtonPressedPatch = new NinePatch(Asset.SkinTexture, new Point(12, 44), 4),
		};
	}
}