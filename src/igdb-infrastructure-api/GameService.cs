using Ardalis.Result;
using igdb_application.Contracts;
using igdb_domain.Entities;
using igdb_infrastructure_api.Client;

namespace igdb_infrastructure_api;

public class GameService(IgdbApiClient client) : IGameService
{
    public async ValueTask<Result<Game>> GetGameByIdAsync(int id, CancellationToken cancellationToken)
    {
        var matching = (await client.Query(
            new IGDBQuery<Game>
            {
                Endpoint = Endpoint.Games,
                Where = IgdbLanguage.Where($"id={id}")
            }, cancellationToken))?.SingleOrDefault();

        return matching is not null ? Result.Success(matching) : Result.NotFound();
    }

    public async Task<IEnumerable<Game>> SearchAsync(
        string term,
        int platformId,
        CancellationToken cancellationToken)
    {
        var matching = await client.Query(
            new IGDBQuery<Game>
            {
                Endpoint = Endpoint.Games,
                Search = IgdbLanguage.Search($"{term}"),
                Where = IgdbLanguage.Where($"platforms=({platformId})"),
                Limit = IgdbLanguage.Limit(15)
            }, cancellationToken);

        return matching ?? [];
    }
}