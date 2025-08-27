using FastEndpoints;
using igdb_application.Command.Caching;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace igdb_api.Endpoints.Cache;

public class Update(ISender sender) : Endpoint<Update.Parameters, Ok>
{
    public override void Configure()
    {
        Post("update/{id}");

        Policies("AuthPolicy");
        
        Summary(s =>
        {
            s.Summary = "Queue an igdb mapped game for caching details";
            s.Params["id"] = "The igdb id of the game";
        });
    }

    public override async Task<Ok> ExecuteAsync(Parameters parameters, CancellationToken ct)
    {
        await sender.Send(new EnqueueCacheRequest.Command(parameters.Id, "game"), ct);
        
        return TypedResults.Ok();
    }

    public class Parameters
    {
        public int Id { get; set; }
    }
}