using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TbdDevelop.GameTrove.GameApi.Infrastructure.Database;
using TbdDevelop.GameTrove.GameApi.Infrastructure.ResultModels;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.Games;

public class Get(IDbContextFactory<GameTrackingContext> factory)
    : Endpoint<Get.Query, Results<Ok<GameDetailResultModel>, NoContent>>
{
    public override void Configure()
    {
        Get("games/{identifier}");
        
        Policies("AuthPolicy");
    }

    public override async Task<Results<Ok<GameDetailResultModel>, NoContent>> ExecuteAsync(Query request,
        CancellationToken ct)
    {
        await using var context = await factory.CreateDbContextAsync(ct);

        var currentContext = context;

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
                Copies = (from cp in currentContext.GameCopies
                    join pc in currentContext.PriceChartingGameCopyAssociations on cp.Id equals pc.GameCopyId into pcx
                    from pc in pcx.DefaultIfEmpty()
                    let isNew = cp.IsNew
                    let isCompleteInBox = cp.IsCompleteInBox
                    let isLoose = cp.IsLoose
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

        var result = await game.FirstOrDefaultAsync(ct);

        return result != null ? TypedResults.Ok(result) : TypedResults.NoContent();
    }

    public sealed record Query
    {
        public Guid Identifier { get; set; }
    }
}