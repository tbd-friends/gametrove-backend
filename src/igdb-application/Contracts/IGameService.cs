using Ardalis.Result;
using igdb_domain.Entities;

namespace igdb_application.Contracts;

public interface IGameService
{
    ValueTask<Result<Game>> GetGameByIdAsync(int id, CancellationToken cancellationToken);

    Task<IEnumerable<Game>> SearchAsync(
        string term,
        int platformId,
        CancellationToken cancellationToken);
}