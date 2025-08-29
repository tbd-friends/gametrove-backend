using games_infrastructure_pricecharting_api.Client;
using TbdDevelop.GameTrove.Games.Domain.Pricing;

namespace games_infrastructure_pricecharting_api.Constants;

public static class Helpers
{
    public static readonly Func<string, string> PriceChartingApiKey =
        identifier =>
            $"{new SanitizedIdentifier(identifier ?? throw new ArgumentNullException(nameof(identifier)))}_pricecharting-api-key";

    public readonly struct SanitizedIdentifier(string identifier)
    {
        private readonly string _value = new(identifier.Where(char.IsLetterOrDigit).ToArray());

        public static implicit operator string(SanitizedIdentifier identifier) => identifier._value;
        public override string ToString() => _value;
    }

    public static Product FromPriceChartingProduct(this PriceChartingProduct product)
    {
        return new Product
        {
            Id = int.Parse(product.Id),
            Name = product.ProductName,
            CompleteInBoxPrice = product.CibPrice.ToDollars(),
            LoosePrice = product.LoosePrice.ToDollars(),
            NewPrice = product.NewPrice.ToDollars()
        };
    }

    public static decimal ToDollars(this int pennies)
    {
        return pennies > 0 ? (decimal)pennies / 100 : 0;
    }
}