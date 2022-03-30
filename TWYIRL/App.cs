using System.IO.Abstractions;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace TWYIRL;

public class App
{
	private readonly IFileSystem _fs;
	private readonly Action<string> _log;
	private readonly Func<int, int> _rng;
	private readonly ITwitterClient _twitter;

	public App(IFileSystem fs, ITwitterClient twitter, Func<int, int> rng, Action<string> log)
	{
		_fs = fs;
		_log = log;
		_rng = rng;
		_twitter = twitter;
	}

	public async Task<int> Run()
	{
		try
		{
			var lyrics = GetLyrics("lyrics.md");
			var tweets = GetTweets("TWYIRL");
			var random = GetNewRef(await lyrics, await tweets);
			await Tweet(random);
			return 0;
		}
		catch (Exception e)
		{
			_log(e.Message);
			return 1;
		}
	}

	private string GetNewRef(IReadOnlyList<string> lines, IReadOnlyList<ITweet> tweets)
	{
		string[] lyrics = lines.Where(l => !string.IsNullOrWhiteSpace(l) && !l.StartsWith("#")).ToArray();
		string[] tweets1 = tweets.Select(t => t.Text).ToArray();
		return GetNewRef(lyrics, tweets1);
	}

	private string GetNewRef(IReadOnlyList<string> lyrics, IReadOnlyList<string> tweets)
	{
		var attempts = 0;
		while (attempts++ < lyrics.Count)
		{
			var random = _rng(lyrics.Count);
			var tweet = lyrics[random];

			if (!tweets.Contains(tweet))
			{
				return tweet;
			}

			_log($"Already tweeted [{tweet}]");
		}

		throw new KeyNotFoundException("No new lyrics to tweet");
	}

	private async Task Tweet(string quote)

	{
		_log($"Tweeting [{quote}]");
		await _twitter.Tweets.PublishTweetAsync(quote);
	}

	private async Task<string[]> GetLyrics(string file)
		=> await _fs.File.ReadAllLinesAsync(file);

	private async Task<ITweet[]> GetTweets(string user)
	{
		var options = new GetUserTimelineParameters(user)
		{
			PageSize = 365 * 2
		};

		var tweets = await _twitter.Timelines.GetUserTimelineAsync(options);

		foreach (var tweet in tweets)
		{
			Console.WriteLine(tweet.Text);
		}

		return tweets;
	}
}