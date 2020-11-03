class Debug
{
	public static void Log(string message)
	{
		System.Diagnostics.Debug.WriteLine("DEBUG " + message);
	}

	public static void Log(string format, params object[] args)
	{
		System.Diagnostics.Debug.WriteLine("DEBUG " + format, args);
	}

	public static void LogInfo(string message)
	{
		System.Diagnostics.Debug.WriteLine("INFO " + message);
	}

	public static void LogInfo(string format, params object[] args)
	{
		System.Diagnostics.Debug.WriteLine("INFO " + format, args);
	}

	public static void LogWarn(string message)
	{
		System.Diagnostics.Debug.WriteLine("WARN " + message);
	}

	public static void LogWarn(string format, params object[] args)
	{
		System.Diagnostics.Debug.WriteLine("WARN " + format, args);
	}

	public static void LogError(string message)
	{
		System.Diagnostics.Debug.WriteLine("ERROR " + message);
	}

	public static void LogError(string format, params object[] args)
	{
		System.Diagnostics.Debug.WriteLine("ERROR " + format, args);
	}

	public static void Insist(bool condition, string message)
	{
		if (!condition)
		{
			Fail(message);
		}
	}

	public static void Insist(bool condition, string format, params object[] args)
	{
		if (!condition)
		{
			Fail(format, args);
		}
	}

	public static void Fail(string format, params object[] args)
	{
		Fail(string.Format(format, args));
	}

	public static void Fail(string message)
	{
		// TODO improve failure management: display a dialog
		LogError(message);
		Core.Instance.Exit();
	}
}