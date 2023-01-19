using System.ComponentModel.DataAnnotations;
using Domain.Aggregates.Worlds;

namespace Net7ClassicApi.Models;

public class WorldModel
{
    [Required]
    [Range(1, int.MaxValue)]
    public int? Id { get; set; }

    [Required]
    [RegularExpression("^[a-zA-Z_-]+$")]
    public string? Name { get; set; }
}

