class Booster : Window
{
	private const string TITLE_FORMAT = "Booster v1.{0}";

	public IntValue Coin { get; } = new IntValue(128, 128);
	private int coinPerClick = 16;
	private int level = 1;

	public Booster()
	{
		Title = string.Format(TITLE_FORMAT, level);

		Add<Bar>()
			.SetValue(Coin)
			.SetSize(64, 12)
			.Add<ValueLabel>()
				.SetFormat("{0} left")
				.SetValue(Coin)
				.Center()
		;

		Add<Button>()
			.SetOnPressed(() =>
			{
				Coin.MoveTo(Computer.Coin, coinPerClick);
				if (Coin.Value == 0)
				{
					Remove();
				}
			})
			.SetSize(64, 64)
			.Add<TextLabel>().SetText("+" + coinPerClick).Center();
		;
	}
}