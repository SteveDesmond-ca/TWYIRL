using System.IO.Abstractions;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace TWYrefs;

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
            var refs = GetRefs();
            var tweets = GetTweets();
            var random = GetNewRef(await refs, await tweets);
            await Tweet(random);
            return 0;
        }
        catch (Exception e)
        {
            _log(e.Message);
            return 1;
        }
    }

    private string GetNewRef(string[] refs, IEnumerable<ITweet> tweets)
        => GetNewRef(refs, tweets.Select(t => t.Text).ToArray());

    private string GetNewRef(IReadOnlyList<string> refs, IReadOnlyList<string> tweets)
    {
        while (true)
        {
            var random = _rng(refs.Count);
            var tweet = refs[random];

            if (!tweets.Contains(tweet))
            {
                return tweet;
            }

            _log($@"Already tweeted ""{tweet}""");
        }
    }

    private async Task Tweet(string quote)

    {
        _log($@"Tweeting ""{quote}""");
        await _twitter.Tweets.PublishTweetAsync(quote);
    }

    private async Task<string[]> GetRefs()
        => await _fs.File.ReadAllLinesAsync("refs.txt");

    private async Task<ITweet[]> GetTweets()
    {
        var options = new GetUserTimelineParameters("TWY_refs")
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