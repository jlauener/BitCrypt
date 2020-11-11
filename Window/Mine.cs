using BitUi;
using System;
using System.Collections;

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

	private readonly TextLabel label;
	private readonly Button transferButton; // TODO make it blink/flash/text bounce when full?
	private readonly BuyButton upgradeButton;

	public Mine()
	{
		Title = string.Format(TITLE_FORMAT, level);

		(label = Add<TextLabel>())
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
			.SetOnPressed(() =>
			{
				if (!Computer.Coin.Full)
				{
					SetState(Transfer());
				}
				else
				{
					// TODO report error
				}
			})
			.SetSize(88, 12)
			.Add<TextLabel>().SetText("transfer").Center();
		;

		(upgradeButton = Add<BuyButton>())
			.SetCost(upgradeCost)
			.SetBuyDuration(3f)
			.SetOnBuy(() => SetState(Upgrade()))
			.SetSize(88, 12)
			.Add<TextLabel>().SetText(UPGRADE_FORMAT, upgradeCost).Center();
		;
	}

	private Coroutine state;
	private void SetState(IEnumerator state)
	{
		if (this.state != null)
		{
			this.state.Stop();
			this.state = null;
		}

		if (state != null)
		{
			this.state = Core.StartCoroutine(state);
		}
	}

	public override void OnAdded()
	{
		base.OnAdded();

		SetState(Idle());
	}

	private IEnumerator Idle()
	{
		label.SetText(COIN_PER_SEC_FORMAT, CoinPerSec);

		transferButton.Enabled = true;
		transferButton.Get<Label>().Enabled = true;

		upgradeButton.BuyEnabled = true;

		while (true)
		{
			coinCounter += Time.DeltaTime * CoinPerSec;
			var coinToAdd = (int)Math.Floor(coinCounter);
			if (coinToAdd > 0)
			{
				coinCounter -= coinToAdd;
				Coin.Add(coinToAdd);

				if (Coin.Full) SetState(Full());
			}

			yield return null;
		}
	}

	private IEnumerator Full()
	{
		label.SetText(COIN_PER_SEC_FORMAT, 0);

		while (true)
		{
			if (!Coin.Full) SetState(Idle());
			yield return null;
		}
	}

	private IEnumerator Upgrade()
	{
		label.SetText("Upgrading...");

		transferButton.Enabled = false;
		transferButton.Get<Label>().Enabled = false;

		yield return Core.WaitForSeconds(upgradeButton.BuyDuration);

		level++;
		Title = string.Format(TITLE_FORMAT, level);

		Coin.ModifyMax(Coin.Max);
		CoinPerSec *= 1.5f;
		label.SetText(COIN_PER_SEC_FORMAT, CoinPerSec);

		upgradeCost *= 2;
		upgradeButton.SetCost(upgradeCost);
		upgradeButton.Get<TextLabel>().SetText(UPGRADE_FORMAT, upgradeCost);

		SetState(Idle());
	}

	private IEnumerator Transfer()
	{
		label.SetText("Transfering...");

		transferButton.Enabled = false;
		transferButton.Get<Label>().Enabled = false;

		upgradeButton.BuyEnabled = false;

		// TODO find a better way to transfer a floating value...
		var transferPerSec = 60f;
		while (Coin.Value > 0)
		{
			if (Coin.MoveTo(Computer.Coin, (int)(transferPerSec * Time.DeltaTime)) == 0)
			{
				SetState(Idle());
			}
			transferPerSec *= 1.1f;
			yield return null;
		}
		SetState(Idle());
	}
}