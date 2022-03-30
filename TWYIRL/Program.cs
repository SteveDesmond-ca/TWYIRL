namespace TWYIRL;

public static class Program
{
	public static async Task<int> Main(string[] args)
		=> await Factory.App().Run();
}