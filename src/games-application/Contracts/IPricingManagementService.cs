namespace games_application.Contracts;

public interface IPricingManagementService
{
    ValueTask BeginPriceChartingUpdate(CancellationToken cancellationToken = default);
}