class Shop : Window
{
	public Shop()
	{
		Title = "EtHaN's ShOp";

		Add<TextLabel>()
			.SetText("Welcome to EtHaN's ShOp!")
			.SetSize(128, 8)
		;

		Add<Button>()
			.SetOnPressed(() => Buy(WindowData.Mine))
			.SetSize(128, 12)
			.Add<TextLabel>().SetText("buy mine (1k)").Center();
		;

		Add<Button>()
			.SetOnPressed(() => Buy(WindowData.Vault))
			.SetSize(128, 12)
			.Add<TextLabel>().SetText("buy vault (256)").Center();
		;

		Pack();
	}

	private void Buy(WindowData window)
	{
		if (Computer.CanCreateWindow(window) && Computer.Coin.Pay(window.Cost))
		{
			Computer.CreateWindow(window);
		}
	}
}
