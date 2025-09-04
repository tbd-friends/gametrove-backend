using FastEndpoints;
using games_application.Contracts;
using TbdDevelop.GameTrove.Games.Domain.Events;
using IEventBus = shared_kernel_infrastructure.Contracts.IEventBus;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.PriceCharting;

public class Update(
    ICurrentUserService user,
    IEventBus eventBus) : EndpointWithoutRequest
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
        await eventBus.PublishAsync(new PricingUpdateRequested(user.UserId!));
    }
}