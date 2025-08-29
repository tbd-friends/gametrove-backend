namespace igdb_application.Query.Games.Models;

public record CoverDto(string ImageId, string Url, int Height, int Width, bool AlphaChannel, bool Animated)
    : ImageDto(ImageId, Url, Height, Width);