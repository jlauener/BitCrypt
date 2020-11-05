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
	private readonly Button transferButton; // TODO make it blink/flash/text bounce when full?
	private readonly BuyButton upgradeButton;

	public Mine()
	{
		Title = string.Format(TITLE_FORMAT, level);

		(coinPerSecLabel = Add<TextLabel>())
			.SetText(COIN_PER_SEC_FORMAT, CoinPerSec)
			.SetSize(88, 8)
		;

		Add<Bar>()
			.SetValue(Coin)
			.SetSize(88, 12)
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

		(upgradeButton = Add<BuyButton>())
			.SetCost(upgradeCost)
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

		coinPerSecLabel.Color = labelColor;
		if (Coin.Value == Coin.Max)
		{
			coinPerSecLabel.Color = new Color(0xBE, 0x26, 0x33);
			coinPerSecLabel.SetText(COIN_PER_SEC_FORMAT, 0);
		}
		else
		{
			coinPerSecLabel.Color = Skin.TextColor;
			coinPerSecLabel.SetText(COIN_PER_SEC_FORMAT, CoinPerSec);
		}
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
			upgradeButton.SetCost(upgradeCost);
			upgradeButton.Get<TextLabel>().SetText(UPGRADE_FORMAT, upgradeCost);
		}
	}
}