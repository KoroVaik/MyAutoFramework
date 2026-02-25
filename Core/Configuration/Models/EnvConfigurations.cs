using System.ComponentModel.DataAnnotations;

namespace Core.Configuration.Models;

public class EnvConfigurations
{
    [Required] public string Environment { get; init; } = null!;
}