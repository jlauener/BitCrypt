using System;

class Mine : Window
{
	private const string TITLE_FORMAT = "Mine v1.{0}";
	private const string COIN_FORMAT = "Coin ({0}/{1})";
	private const string UPGRADE_FORMAT = "Upgrade ({0})";

	public int Coin { get; private set; }
	public int CoinMax { get; private set; } = 100;
	public float CoinPerSec { get; private set; } = 10f;

	private float coinCounter;

	private int level = 0;
	private int upgradeCost = 200;

	private readonly Label coinLabel;
	private readonly Button transferButton;
	private readonly Button upgradeButton;

	public Mine()
	{
		Skin = Computer.Instance.PlayerSkin;
		Title = string.Format(TITLE_FORMAT, level);

		(coinLabel = Add<Label>())
			.SetText(string.Format(COIN_FORMAT, Coin, CoinMax))
			.SetSize(88, 8)
		;

		(transferButton = Add<Button>())
			.SetOnPressed(() =>
			{
				Coin -= Computer.Coin.Add(Coin);
				coinLabel.Text = string.Format(COIN_FORMAT, Coin, CoinMax);
			})
			.SetSize(88, 12)
			.Add<Label>().SetText("transfer").Center();
		;

		(upgradeButton = Add<Button>())
			.SetOnPressed(() =>
			{
				if (Computer.Coin.Pay(upgradeCost))
				{
					level++;
					Title = string.Format(TITLE_FORMAT, level);

					CoinMax *= 2;
					CoinPerSec *= 1.1f;
					coinLabel.SetText(string.Format(COIN_FORMAT, Coin, CoinMax));

					upgradeCost *= 2;
					upgradeButton.Get<Label>().SetText(UPGRADE_FORMAT, upgradeCost);
				}
			})
			.SetSize(88, 12)
			.Add<Label>().SetText(UPGRADE_FORMAT, upgradeCost).Center();
		;

		Pack();
	}

	public override void Update()
	{
		base.Update();

		coinCounter += Time.DeltaTime * 10f;
		var coinToAdd = (int)Math.Floor(coinCounter);
		if (coinToAdd > 0)
		{
			coinCounter -= coinToAdd;
			Coin = Math.Min(Coin + coinToAdd, CoinMax);
			coinLabel.Text = string.Format(COIN_FORMAT, Coin, CoinMax);
		}
	}
}