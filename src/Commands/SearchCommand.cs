using System.ComponentModel;
using TikTok.Console.Infrastructure;
using Spectre.Console;
using Spectre.Console.Cli;

namespace TikTok.Console.Commands;

public sealed class SearchCommand : AsyncCommand<SearchCommand.Settings>
{
    public sealed class Settings : GlobalSettings
    {
        [CommandArgument(0, "<QUERIES>")]
        [Description("Comma-separated search queries")]
        public required string Queries { get; init; }

        [CommandOption("--limit <N>")]
        [Description("Number of results per query (default: 1)")]
        public int? Limit { get; init; }

        [CommandOption("--section <SECTION>")]
        [Description("Search section: top, video, user (default: top)")]
        public string? Section { get; init; }

        [CommandOption("--profiles <N>")]
        [Description("Number of profiles per query when searching users (default: 10)")]
        public int? Profiles { get; init; }

        [CommandOption("--comments <N>")]
        [Description("Max comments per post")]
        public int? Comments { get; init; }
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellation)
    {
        var queries = settings.Queries.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        AnsiConsole.MarkupLine($"[grey]Searching {queries.Length} query/queries (this may take a while)...[/]");

        var searchSection = (settings.Section?.ToLowerInvariant()) switch
        {
            "video" => "/video",
            "user" => "/user",
            _ => ""
        };

        using var client = settings.CreateClient();
        var body = new
        {
            searchQueries = queries,
            resultsPerPage = settings.Limit ?? 1,
            searchSection,
            maxProfilesPerQuery = settings.Profiles,
            commentsPerPost = settings.Comments
        };

        var doc = await client.ScrapeAsync(body);
        YamlOutput.Write(doc);

        return 0;
    }
}
