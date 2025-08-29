using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Conditions.Dtos;

public class ConditionDto
{
    public int Value { get; set; }
    public required string Name { get; set; }
}