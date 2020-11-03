class IntValue
{
	public int Value { get; private set; }
	public int Max { get; private set; }

	public IntValue(int value, int max = 0)
	{
		Value = value;
		Max = max;
	}

	public void ModifyMax(int delta)
	{
		Max += delta;
	}

	public int Add(int value)
	{
		if (Max > 0 && Value + value > Max)
		{
			value = Max - Value;
		}

		Value += value;
		return value;
	}

	public int Remove(int value)
	{
		var removed = value;
		if (value > Value)
		{
			value = Value;
		}

		Value -= value;
		return value;
	}

	public bool Pay(int value)
	{
		if (Value < value)
		{
			return false;
		}

		Remove(value);
		return true;
	}

	public override string ToString()
	{
		return Max == 0 ? Value.ToString() : Value + "/" + Max;
	}

}