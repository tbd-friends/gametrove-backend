namespace games_application.Contracts;

public interface IPricingManagementService
{
    ValueTask BeginPriceChartingUpdate(string userIdentifier, CancellationToken cancellationToken = default);
}