# TikTok.Console

CLI for scraping TikTok videos, profiles, hashtags, and search results via Apify. YAML-first output optimized for LLM agent consumption.

## Installation

```bash
dotnet tool install -g TikTok.Console
```

## Prerequisites

Set your Apify API token:

```bash
export APIFY_TOKEN=your-token-here
```

Or pass it per-command with `--api-key`.

## Usage

### Profile

```bash
# Scrape videos from a user profile
tiktok profile https://www.tiktok.com/@username
tiktok profile https://www.tiktok.com/@username --max-items 20
```

### Hashtag

```bash
# Scrape videos by hashtag
tiktok hashtag "coding"
tiktok hashtag "dotnet" --max-items 20
```

### Search

```bash
# Search for videos
tiktok search "machine learning"

# Search for users
tiktok search "tech" --users
```

### Video

```bash
# Scrape specific videos by URL
tiktok video "https://www.tiktok.com/@user/video/1234567890"
```

### Common options

```bash
--api-key <KEY>       Apify token (or set APIFY_TOKEN env var)
--max-items <N>       Maximum items to return (default: 10)
```

## License

MIT
