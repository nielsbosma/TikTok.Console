using TikTok.Console.Commands;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    config.SetApplicationName("tiktok");

    config.AddCommand<ProfileCommand>("profile")
        .WithDescription("Scrape videos from TikTok user profiles");

    config.AddCommand<HashtagCommand>("hashtag")
        .WithDescription("Scrape videos by TikTok hashtag");

    config.AddCommand<SearchCommand>("search")
        .WithDescription("Search TikTok for videos or users");

    config.AddCommand<VideoCommand>("video")
        .WithDescription("Scrape specific TikTok videos by URL");
});

return app.Run(args);
