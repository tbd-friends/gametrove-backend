using FastEndpoints;
using games_application.Command.PriceCharting;
using Mediator;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.PriceCharting;

public class Update(ISender sender) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("pricecharting/update");

        Policies("AuthPolicy");

        Options(options => options.WithRequestTimeout("long-timeout"));

        Summary(g => { g.Description = "Will begin download of PriceCharting file to processing directory"; });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await sender.Send(new BeginPriceChartingUpdate.Command(), ct);
    }
}