class Random
{
	private readonly System.Random random;

	public Random()
	{
		random = new System.Random();
	}

	public float NextFloat()
	{
		return (float)random.NextDouble();
	}

	public float NextFloat(float max)
	{
		return NextFloat() * max;
	}

	public float NextFloat(float min, float max)
	{
		return min + NextFloat() * (max - min);
	}

	public int NextInt(int max)
	{
		return random.Next(max);
	}

	public int NextInt(int min, int max)
	{
		return random.Next(min, max);
	}
}