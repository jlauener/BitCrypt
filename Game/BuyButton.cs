using System;

class BuyButton : Button
{
	public int Cost { get; set; }

	public BuyButton SetCost(int cost)
	{
		Cost = cost;
		return this;
	}

	public float BuyDuration { get; set; }

	public BuyButton SetBuyDuration(float duration)
	{
		BuyDuration = duration;
		return this;
	}

	public BuyButton SetOnBuy(Action action)
	{
		OnPressed += () =>
		{
			if (Computer.Coin.Pay(Cost))
			{
				if (BuyDuration > 0f)
				{
					Enabled = false;
					Get<Label>().Enabled = false;
					buyCounter = BuyDuration;
				}
				action();
			}
		};
		return this;
	}

	public bool BuyEnabled { get; set; } = true;

	private float buyCounter;

	// TODO auto label update, also need to create label here...
	public override void Update()
	{
		base.Update();

		if (buyCounter > 0f)
		{
			buyCounter -= Time.DeltaTime;
			// TODO display bar...
		}
		else if (BuyEnabled)
		{
			// TODO make this reactive ?
			// TODO Enabled inheritance...
			Enabled = Computer.Coin.Value >= Cost;
			Get<Label>().Enabled = Enabled;
		}
		else
		{
			Enabled = false;
			Get<Label>().Enabled = false;
		}
	}
}