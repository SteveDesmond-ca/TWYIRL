using System.IO.Abstractions;
using System.Security.Cryptography;
using Tweetinvi;

namespace TWYrefs;

public static class Factory
{
	public static App App()
		=> new (new FileSystem(), Twitter(), RandomNumberGenerator.GetInt32, Console.WriteLine);

	private static ITwitterClient Twitter()
	{
		var consumerKey = Environment.GetEnvironmentVariable("ConsumerKey");
		var consumerSecret = Environment.GetEnvironmentVariable("ConsumerSecret");
		var accessKey = Environment.GetEnvironmentVariable("AccessKey");
		var accessSecret = Environment.GetEnvironmentVariable("AccessSecret");
		return new TwitterClient(consumerKey, consumerSecret, accessKey, accessSecret);
	}
}