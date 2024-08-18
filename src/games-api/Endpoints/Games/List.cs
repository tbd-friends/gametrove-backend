using Ardalis.ApiEndpoints;
using Games.Infrastructure;
using Games.Infrastructure.Database;
using Games.Infrastructure.ResultModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Games.Endpoints.Games;

public class List(IDbContextFactory<GameTrackingContext> factory) : EndpointBaseAsync
    .WithRequest<List.Query>
    .WithResult<ResultSet<GameListResultModel>>
{
    [HttpGet("[namespace]")]
    public override async Task<ResultSet<GameListResultModel>> HandleAsync([FromQuery] Query request,
        CancellationToken cancellationToken = new())
    {
        await using var context = await factory.CreateDbContextAsync(cancellationToken);

        var games = from g in context.Games
            join p in context.Platforms on g.PlatformId equals p.Id
            join pb in context.Publishers on g.PublisherId equals pb.Id into pbx
            from pb in pbx.DefaultIfEmpty()
            select new
            {
                g,
                p,
                pb
            };

        if (request.Search is not null)
        {
            games = from x in games
                where x.g.Name.Contains(request.Search) ||
                      (from gc in context.GameCopies
                          where gc.GameId == x.g.Id
                                && gc.Upc.Contains(request.Search)
                          select gc
                      ).Any()
                select x;
        }

        var transform = from x in games
            let g = x.g
            let p = x.p
            let pb = x.pb
            orderby p.Name, g.Name
            select new GameListResultModel
            {
                Id = g.Identifier,
                Description = g.Name,
                Platform = new PlatformResultModel
                {
                    Id = p.Identifier,
                    Description = p.Name
                },
                Publisher = pb != null
                    ? new PublisherResultModel
                    {
                        Id = pb.Identifier,
                        Description = pb.Name
                    }
                    : null,
                Copies = (from cp in context.GameCopies
                    where cp.GameId == g.Id
                    select cp.Id).Count()
            };

        var results =
            transform
                .Skip(request.Start)
                .Take(request.PageSize);

        return new ResultSet<GameListResultModel>
        {
            Results = results.ToList(),
            Total = games.Count(),
            PageSize = request.PageSize,
            Starting = request.Start
        };
    }

    public sealed record Query
    {
        public int Start { get; set; }
        public int PageSize { get; set; } = 30;
        public string? Search { get; set; }
    }
}