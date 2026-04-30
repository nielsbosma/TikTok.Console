using System.ComponentModel;
using TikTok.Console.Infrastructure;
using Spectre.Console;
using Spectre.Console.Cli;

namespace TikTok.Console.Commands;

public sealed class HashtagCommand : AsyncCommand<HashtagCommand.Settings>
{
    public sealed class Settings : GlobalSettings
    {
        [CommandArgument(0, "<HASHTAGS>")]
        [Description("Comma-separated hashtags (e.g. fitness,workout)")]
        public required string Hashtags { get; init; }

        [CommandOption("--limit <N>")]
        [Description("Number of videos per hashtag (default: 1)")]
        public int? Limit { get; init; }

        [CommandOption("--comments <N>")]
        [Description("Max comments per post")]
        public int? Comments { get; init; }
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellation)
    {
        var hashtags = settings.Hashtags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        AnsiConsole.MarkupLine($"[grey]Scraping {hashtags.Length} hashtag(s) (this may take a while)...[/]");

        using var client = settings.CreateClient();
        var body = new
        {
            hashtags,
            resultsPerPage = settings.Limit ?? 1,
            commentsPerPost = settings.Comments
        };

        var doc = await client.ScrapeAsync(body);
        YamlOutput.Write(doc);

        return 0;
    }
}
