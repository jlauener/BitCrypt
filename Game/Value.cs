using System;

public struct IntValueChangedEvent
{
	public static readonly IntValueChangedEvent Init;

	public int Delta { get; set; }
	public int MaxDelta { get; set; }
}

class IntValue
{
	public event Action<IntValueChangedEvent> OnChanged;

	public int Value { get; private set; }
	public int Max { get; private set; }
	public int Free => Max - Value;
	public bool Full => Value >= Max;

	public IntValue(int value, int max = 0)
	{
		Value = value;
		Max = max;
	}

	public void ModifyMax(int delta)
	{
		Max += delta;
		OnChanged?.Invoke(new IntValueChangedEvent { MaxDelta = delta });
	}

	public int Add(int value)
	{
		if (Max > 0 && Value + value > Max)
		{
			value = Max - Value;
		}

		if (value > 0)
		{
			Value += value;
			OnChanged?.Invoke(new IntValueChangedEvent { Delta = value });
		}
		return value;
	}

	public int Remove(int value)
	{
		if (value > Value)
		{
			value = Value;
		}

		if (value > 0)
		{
			Value -= value;
			OnChanged?.Invoke(new IntValueChangedEvent { Delta = -value });
		}
		return value;
	}

	public bool Pay(int value)
	{
		if (value == 0) return true;

		if (Value < value)
		{
			return false;
		}

		Remove(value);
		return true;
	}

	public int MoveTo(IntValue other, int amount)
	{
		if (amount == 0) return 0;
		return Remove(other.Add(amount));
	}

	public int MoveTo(IntValue other)
	{
		return MoveTo(other, other.Value);
	}

	public override string ToString()
	{
		return Max == 0 ? Value.ToString() : Value + "/" + Max;
	}

}