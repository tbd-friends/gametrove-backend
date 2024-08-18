using Client.Infrastructure.Clients;
using Client.Infrastructure.Clients.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Client.Components.Lists;

public partial class GamesList : ComponentBase
{
    [Inject] private GamesClient GamesClient { get; set; } = null!;

    public int PageSize { get; set; } = 30;
    public string TitleFilter { get; set; } = string.Empty;
    public string PlatformFilter { get; set; } = string.Empty;

    public IQueryable<GameListResultModel> Games { get; set; } = null!;

    private async ValueTask<GridItemsProviderResult<GameListResultModel>> FetchGames(
        GridItemsProviderRequest<GameListResultModel> request)
    {
        var resultSet =
            await GamesClient.FetchPagedResultSetFromIndex(request.StartIndex, request.Count ?? PageSize,
                PlatformFilter);

        return new GridItemsProviderResult<GameListResultModel>
        {
            Items = resultSet.Results.ToList(),
            TotalItemCount = resultSet.Total
        };
    }

    private void HandlePlatformFilter(ChangeEventArgs args)
    {
        if (args.Value is string value)
        {
            PlatformFilter = value;
        }
    }

    private void HandleClear()
    {
        if (string.IsNullOrWhiteSpace(PlatformFilter))
        {
            PlatformFilter = string.Empty;
        }
    }
}