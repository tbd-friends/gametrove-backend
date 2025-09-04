using Ardalis.Specification;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_infrastructure_pricecharting_api.WorkerServices.Specifications;

internal class MappingsUpdatedBeforeDateSpec : Specification<PriceChartingSnapshot>
{
    public MappingsUpdatedBeforeDateSpec(DateTime executionDateTime)
    {
        Query.Where(s => s.LastUpdated.Date < executionDateTime.Date);
    }
}