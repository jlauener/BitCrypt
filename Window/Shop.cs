using BitUi;
using Microsoft.Xna.Framework;

class Shop : Window
{
	public Shop()
	{
		Title = "Miner's supplies";
		SkinDEP = WindowData.ShopSkin; // TODO fix the skin issue!!! -> FIXED

		var layout = Add<Layout>();
		layout.Margin = Point.Zero;

		var topLayout = layout.Add<Layout>();
		topLayout.Pan.X = 8;
		// TODO better way to get the sprites, and stuffs from the skin.
		// maybe a simple description file?
		// also what about shop icons etc... auto sprite pack them? yuumm. If all images are 16x16 that's easy peasy...
		topLayout.Add<Image>().SetSprite(new Sprite(Asset.DefaultTexture, new Rectangle(4, 64, 28, 28)));
		topLayout.Add<TextLabel>().SetText("Hello! Welcome to the\nminer's paradise! Help\nyourself my \"friend\".");
		topLayout.PackHorizontally();

		var bottomLayout = layout.Add<Layout>();
		bottomLayout.Pan.X = 16;

		bottomLayout.Add<BuyButton>()
			.SetCost(1024)
			.SetOnPressed(() => Buy(WindowData.Mine))
			.SetSize(32, 32)
			.Add<TextLabel>().SetText("mine\n(1k)").Center();
		;

		bottomLayout.Add<BuyButton>()
			.SetCost(256)
			.SetOnPressed(() => Buy(WindowData.Vault))
			.SetSize(32, 32)
			.Add<TextLabel>().SetText("vault\n(256)").Center();
		;

		bottomLayout.Add<BuyButton>()
			.SetCost(2048)
			.SetOnPressed(() => Buy(WindowData.Vault))
			.SetSize(32, 32)
			.Add<TextLabel>().SetText("boost\n(2k)").Center();

		bottomLayout.PackHorizontally();

		layout.PackVertically();
	}

	private void Buy(WindowData window)
	{
		if (Computer.CanCreateWindow(window) && Computer.Coin.Pay(window.Cost))
		{
			Computer.CreateWindow(window);
		}
	}
}
