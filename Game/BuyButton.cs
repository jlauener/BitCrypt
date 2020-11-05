class BuyButton : Button
{
	public int Cost { get; set; }

	public BuyButton SetCost(int cost)
	{
		Cost = cost;
		return this;
	}

	// TODO auto label update, also need to create label here...
	// TODO automate payment...
	public override void Update()
	{
		base.Update();

		// TODO make this reactive ?
		// TODO Enabled inheritance...
		Enabled = Computer.Coin.Value >= Cost;
		Get<Label>().Enabled = Computer.Coin.Value >= Cost;
	}
}