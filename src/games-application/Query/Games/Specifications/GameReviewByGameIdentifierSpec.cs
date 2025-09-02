using Ardalis.Specification;
using games_application.Query.Games.Models;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Games.Specifications;

public class GameReviewByGameIdentifierSpec : Specification<Review, GameReviewDto>,
    ISingleResultSpecification<Review, GameReviewDto>
{
    public GameReviewByGameIdentifierSpec(Guid identifier)
    {
        Query
            .Include(g => g.Game)
            .AsNoTracking()
            .Where(r => r.Game.Identifier == identifier)
            .Select( r => new GameReviewDto
            {
                Title = r.Title,
                Content = r.Content,
                Graphics = r.Graphics,
                Sound = r.Sound,
                Gameplay = r.Gameplay,
                Replayability = r.Replayability,
                OverallRating = r.OverallRating,
                Completed = r.Completed,
                DateAdded = r.DateAdded,
                LastModified = r.LastModified
            });
    }
}