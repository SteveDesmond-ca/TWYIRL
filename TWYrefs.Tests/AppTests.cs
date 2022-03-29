using System.IO.Abstractions;
using System.Text;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Xunit;

namespace TWYrefs.Tests;

public class AppTests
{
    [Fact]
    public async Task CanTweetUnsentReference()
    {
        //arrange
        var fs = Substitute.For<IFileSystem>();
        fs.File.ReadAllLinesAsync(Arg.Any<string>()).Returns(new[] { "lyrics" });

        var twitter = Substitute.For<ITwitterClient>();
        twitter.Timelines.GetUserTimelineAsync(Arg.Any<string>()).Returns(Array.Empty<ITweet>());

        int rng(int x) => 0;

        var console = new StringBuilder();
        void log(string s) => console.AppendLine(s);

        var app = new App(fs, twitter, rng, log);

        //act
        var result = await app.Run();

        //assert
        await twitter.Tweets.Received().PublishTweetAsync("lyrics");
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task TriesAgainIfAlreadyTweetedCanTweetUnsentReference()
    {
        //arrange
        var fs = Substitute.For<IFileSystem>();
        fs.File.ReadAllLinesAsync(Arg.Any<string>()).Returns(new[] { "lyrics 123", "lyrics 456" });

        var twitter = Substitute.For<ITwitterClient>();
        var tweet = Substitute.For<ITweet>();
        tweet.Text.Returns("lyrics 123");
        twitter.Timelines.GetUserTimelineAsync(Arg.Any<IGetUserTimelineParameters>()).Returns(new[] { tweet });

        var sent = 0;
        int rng(int x) => ++sent - 1;

        var console = new StringBuilder();
        void log(string s) => console.AppendLine(s);

        var app = new App(fs, twitter, rng, log);

        //act
        var result = await app.Run();

        //assert
        Assert.Contains(@"Already tweeted ""lyrics 123""", console.ToString());
        await twitter.Tweets.Received().PublishTweetAsync("lyrics 456");
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task FailsOnException()
    {
        //arrange
        var fs = Substitute.For<IFileSystem>();
        fs.File.ReadAllLinesAsync(Arg.Any<string>()).Returns(new[] { "lyrics" });

        var twitter = Substitute.For<ITwitterClient>();
        twitter.Tweets.PublishTweetAsync(Arg.Any<string>()).Throws(new Exception("unit test error"));

        int rng(int x) => 0;

        var console = new StringBuilder();
        void log(string s) => console.AppendLine(s);

        var app = new App(fs, twitter, rng, log);

        //act
        var result = await app.Run();

        //assert
        Assert.Contains("unit test error", console.ToString());
        Assert.Equal(1, result);
    }
}