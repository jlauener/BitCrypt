class Vault : Window
{
	private const string TITLE_FORMAT = "Vault v1.{0}";
	private const string LABEL_FORMAT = "+{0} MAX";
	private const string UPGRADE_FORMAT = "Upgrade ({0})";

	private readonly Label label;
	private readonly Button upgradeButton;

	private int upgradeCost = 100;
	private int coinModifier = 200;
	private int level = 0;

	public Vault()
	{
		Skin = Computer.Instance.PlayerSkin;
		Title = string.Format(TITLE_FORMAT, level);

		(label = Add<Label>())
			.SetText(LABEL_FORMAT, coinModifier)
			.SetSize(88, 8)
		;

		(upgradeButton = Add<Button>())
			.SetSize(88, 12)
			.Add<Label>().SetText(UPGRADE_FORMAT, upgradeCost).Center();
		;

		Pack();

		upgradeButton.OnPressed += () =>
		{
			if (Computer.Coin.Pay(upgradeCost))
			{
				level++;
				Title = string.Format(TITLE_FORMAT, level);

				Computer.Coin.ModifyMax(coinModifier);
				coinModifier *= 2;
				label.SetText(LABEL_FORMAT, coinModifier);

				upgradeCost *= 2;
				upgradeButton.Get<Label>().SetText(UPGRADE_FORMAT, upgradeCost);
			}
		};
	}

	public override void OnAdded()
	{
		base.OnAdded();

		Computer.Coin.ModifyMax(coinModifier);
	}

	public override void OnRemoved()
	{
		base.OnRemoved();

		Computer.Coin.ModifyMax(-coinModifier);
	}
}