using System.ComponentModel;
using TikTok.Console.Infrastructure;
using Spectre.Console;
using Spectre.Console.Cli;

namespace TikTok.Console.Commands;

public sealed class VideoCommand : AsyncCommand<VideoCommand.Settings>
{
    public sealed class Settings : GlobalSettings
    {
        [CommandArgument(0, "<URLS>")]
        [Description("Comma-separated TikTok video URLs")]
        public required string Urls { get; init; }

        [CommandOption("--related <N>")]
        [Description("Scrape N related videos per URL")]
        public int? Related { get; init; }

        [CommandOption("--comments <N>")]
        [Description("Max comments per post")]
        public int? Comments { get; init; }
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellation)
    {
        var urls = settings.Urls.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        AnsiConsole.MarkupLine($"[grey]Scraping {urls.Length} video(s) (this may take a while)...[/]");

        using var client = settings.CreateClient();
        var body = new
        {
            postURLs = urls,
            scrapeRelatedVideos = settings.Related.HasValue,
            resultsPerPage = settings.Related ?? 1,
            commentsPerPost = settings.Comments
        };

        var doc = await client.ScrapeAsync(body);
        YamlOutput.Write(doc);

        return 0;
    }
}
