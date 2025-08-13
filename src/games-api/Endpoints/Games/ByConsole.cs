// using FastEndpoints;
// using games_application.Games;
// using Mediator;
// using Microsoft.AspNetCore.Http.HttpResults;
// using TbdDevelop.GameTrove.GameApi.Infrastructure;
// using TbdDevelop.GameTrove.GameApi.Infrastructure.ResultModels;
//
// namespace TbdDevelop.GameTrove.GameApi.Endpoints.Games;
//
// public class ByConsole(ISender sender)
//     : Endpoint<List.Query,
//         Results<Ok<ResultSet<GameListResultModel>>, NotFound>>
// {
//     public override void Configure()
//     {
//         Get("games/by-console");
//
//         Policies("AuthPolicy");
//     }
//
//     public override async Task<Results<Ok<ResultSet<GameListResultModel>>, NotFound>> ExecuteAsync(
//         Query request,
//         CancellationToken ct)
//     {
//         var result = await sender.Send(new FetchAllGames.Query(
//             request.Start,
//             request.PageSize,
//             request.Search
//         ), ct);
//
//         if (!result.IsSuccess)
//         {
//             return TypedResults.NotFound();
//         }
//
//         return TypedResults.Ok(new ResultSet<GameListResultModel>
//         {
//             Results = from g in result.Value.Results
//                 select new GameListResultModel
//                 {
//                     Id = g.Identifier,
//                     Description = g.Name,
//                     Platform = new PlatformResultModel
//                     {
//                         Id = g.Platform.Identifier,
//                         Description = g.Platform.Name,
//                     },
//                     Publisher = g.Publisher != null
//                         ? new PublisherResultModel
//                         {
//                             Id = g.Publisher.Identifier,
//                             Description = g.Publisher.Name,
//                         }
//                         : null,
//                     CopyCount = g.CopyCount
//                 },
//             Total = result.Value.Total,
//             PageSize = result.Value.PageSize,
//             Starting = result.Value.Starting
//         });
//     }
//
//     public sealed record Query
//     {
//         public int Start { get; set; }
//         public int PageSize { get; set; } = 30;
//         public string? Search { get; set; }
//     }
// }