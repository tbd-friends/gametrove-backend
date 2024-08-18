using Ardalis.ApiEndpoints;
using Games.Infrastructure.Database;
using Games.Infrastructure.ResultModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Games.Endpoints.Games;

public class Get
    : EndpointBaseAsync
        .WithRequest<Get.Query>
        .WithResult<GameDetailResultModel?>
{
    private readonly IDbContextFactory<GameTrackingContext> _factory;

    public Get(IDbContextFactory<GameTrackingContext> factory)
    {
        _factory = factory;
    }

    [HttpGet("[namespace]/{identifier}")]
    public override async Task<GameDetailResultModel?> HandleAsync([FromRoute] Query request,
        CancellationToken cancellationToken = new())
    {
        await using var context = await _factory.CreateDbContextAsync(cancellationToken);

        var game = from g in context.Games
            join p in context.Platforms on g.PlatformId equals p.Id
            join pb in context.Publishers on g.PublisherId equals pb.Id into pbx
            from pb in pbx.DefaultIfEmpty()
            where g.Identifier == request.Identifier
            select new GameDetailResultModel
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
                    join pc in context.PriceChartingGameCopyAssociations on cp.Id equals pc.GameCopyId into pcx
                    from pc in pcx.DefaultIfEmpty()
                    let isNew = (cp.Condition & 256) == 256
                    let isCompleteInBox = ((cp.Condition & 32) == 32) && ((cp.Condition & 256) == 0)
                    let isLoose = ((cp.Condition & 128) == 128) && ((cp.Condition & 32) == 0)
                    orderby cp.Cost, cp.PurchaseDate descending
                    where cp.GameId == g.Id
                    select new GameCopyResultModel
                    {
                        Id = cp.Identifier,
                        Description = pc.Name ?? g.Name,
                        Cost = cp.Cost,
                        PurchasedDate = cp.PurchaseDate,
                        UpdatedDate = cp.UpdatedDate,
                        LoosePrice = pc != null ? pc.LoosePrice : null,
                        CompleteInBoxPrice = pc != null ? pc.CompleteInBoxPrice : null,
                        NewPrice = pc != null ? pc.NewPrice : null,
                        EstimatedValue = pc != null
                            ? isNew ? pc.NewPrice :
                            isCompleteInBox ? pc.CompleteInBoxPrice :
                            isLoose ? pc.LoosePrice : null
                            : null,
                        Condition = isNew ? "New" : isCompleteInBox ? "Complete In Box" : isLoose ? "Loose" : null,
                        Upc = cp.Upc
                    }).ToList()
            };

        var result = await game.FirstOrDefaultAsync(cancellationToken);

        return result;
    }

    public sealed record Query
    {
        public Guid Identifier { get; set; }
    }
}