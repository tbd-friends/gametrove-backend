namespace igdb_api.Infrastructure.Models;

public class GameResponse : ApiResponseBase
{
    public int FirstReleaseDate { get; set; }

    public List<BasicEntityInfo> Genres { get; set; } = [];
    public List<BasicEntityInfo> Keywords { get; set; } = [];
    public string Name { get; set; } = null!;
    public List<PlatformSummary> Platforms { get; set; } = [];
    public List<ImageResponseBase> Screenshots { get; set; } = [];

    public string Storyline { get; set; } = null!;
    public string Summary { get; set; } = null!;

    public List<BasicEntityInfo> Themes { get; set; } = [];
    public List<VideoResponse> Videos { get; set; } = [];
    public List<int> Remakes { get; set; } = [];
}

public class ImageResponseBase : ApiResponseBase
{
    public int Game { get; set; }

    public string ImageId { get; set; }
    public string Url { get; set; }

    public int Height { get; set; }
    public int Width { get; set; }
}

public class CoverResponse : ImageResponseBase
{
    public bool AlphaChannel { get; set; }
    public bool Animated { get; set; }
}

public class ArtworkResponse : ImageResponseBase
{
    public bool AlphaChannel { get; set; }
    public bool Animated { get; set; }
    public int ArtworkType { get; set; }
}

public class AlternativeNameResponse : ApiResponseBase
{
    public string Comment { get; set; }
    public int Game { get; set; }
    public string Name { get; set; }
}

public class VideoResponse : ApiResponseBase
{
    public string Name { get; set; }
    public string VideoId { get; set; }
}