using Microsoft.Xna.Framework;
using System;

class Mine : Window
{
	private const string TITLE_FORMAT = "Mine v1.{0}";
	private const string COIN_PER_SEC_FORMAT = "{0} coin/sec";
	private const string UPGRADE_FORMAT = "Upgrade ({0})";

	public IntValue Coin { get; } = new IntValue(0, 128);
	public float CoinPerSec { get; private set; } = 16f;

	private float coinCounter;

	private int level = 0;
	private int upgradeCost = 256;

	private readonly TextLabel coinPerSecLabel;
	private readonly Button transferButton;
	private readonly Button upgradeButton;

	public Mine()
	{
		Title = string.Format(TITLE_FORMAT, level);

		(coinPerSecLabel = Add<TextLabel>())
			.SetText(COIN_PER_SEC_FORMAT, CoinPerSec)
			.SetSize(88, 8)
		;

		var coinBar = new Bar(Coin) // TODO cannot add directly because of Bar's OnAdded relying on Parent's size..
			.SetSize(88, 12)
		;
		Add(coinBar) // TODO need to call it before adding the label to have the skin working...
			.Add<ValueLabel>()
			.SetFormat("coin ({0}/{1})")
			.SetValue(Coin)
			.Center()
		;

		(transferButton = Add<Button>())
			.SetOnPressed(() => Coin.MoveTo(Computer.Coin))
			.SetSize(88, 12)
			.Add<TextLabel>().SetText("transfer").Center();
		;

		(upgradeButton = Add<Button>())
			.SetOnPressed(Upgrade)
			.SetSize(88, 12)
			.Add<TextLabel>().SetText(UPGRADE_FORMAT, upgradeCost).Center();
		;

		Pack();
	}

	public override void Update()
	{
		base.Update();

		coinCounter += Time.DeltaTime * CoinPerSec;
		var coinToAdd = (int)Math.Floor(coinCounter);
		if (coinToAdd > 0)
		{
			coinCounter -= coinToAdd;
			Coin.Add(coinToAdd);
		}

		var labelColor = Coin.Value == Coin.Max ? Color.Red : Skin.TextColor;
		transferButton.Get<Label>().Color = labelColor;
		coinPerSecLabel.Color = labelColor;
	}

	private void Upgrade()
	{
		if (Computer.Coin.Pay(upgradeCost))
		{
			level++;
			Title = string.Format(TITLE_FORMAT, level);

			Coin.ModifyMax(Coin.Max);
			CoinPerSec *= 1.5f;
			coinPerSecLabel.SetText(COIN_PER_SEC_FORMAT, CoinPerSec);

			upgradeCost *= 2;
			upgradeButton.Get<TextLabel>().SetText(UPGRADE_FORMAT, upgradeCost);
		}
	}
}