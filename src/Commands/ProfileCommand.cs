using System.ComponentModel;
using TikTok.Console.Infrastructure;
using Spectre.Console;
using Spectre.Console.Cli;

namespace TikTok.Console.Commands;

public sealed class ProfileCommand : AsyncCommand<ProfileCommand.Settings>
{
    public sealed class Settings : GlobalSettings
    {
        [CommandArgument(0, "<USERNAMES>")]
        [Description("Comma-separated TikTok usernames (e.g. nike,adidas)")]
        public required string Usernames { get; init; }

        [CommandOption("--limit <N>")]
        [Description("Number of videos per profile (default: 1)")]
        public int? Limit { get; init; }

        [CommandOption("--sort <ORDER>")]
        [Description("Sort order: latest, popular, oldest (default: latest)")]
        public string? Sort { get; init; }

        [CommandOption("--since <DATE>")]
        [Description("Only videos published after this date (e.g. 2025-01-01)")]
        public string? Since { get; init; }

        [CommandOption("--until <DATE>")]
        [Description("Only videos published before this date")]
        public string? Until { get; init; }

        [CommandOption("--sections <SECTIONS>")]
        [Description("Sections to scrape: videos, reposts (default: videos)")]
        public string? Sections { get; init; }

        [CommandOption("--exclude-pinned")]
        [Description("Exclude pinned posts")]
        public bool ExcludePinned { get; init; }

        [CommandOption("--comments <N>")]
        [Description("Max comments per post")]
        public int? Comments { get; init; }
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellation)
    {
        var usernames = settings.Usernames.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        AnsiConsole.MarkupLine($"[grey]Scraping {usernames.Length} profile(s) (this may take a while)...[/]");

        using var client = settings.CreateClient();
        var body = new
        {
            profiles = usernames,
            resultsPerPage = settings.Limit ?? 1,
            profileSorting = settings.Sort ?? "latest",
            profileScrapeSections = settings.Sections?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? new[] { "videos" },
            excludePinnedPosts = settings.ExcludePinned,
            oldestPostDateUnified = settings.Since,
            newestPostDate = settings.Until,
            commentsPerPost = settings.Comments
        };

        var doc = await client.ScrapeAsync(body);
        YamlOutput.Write(doc);

        return 0;
    }
}
