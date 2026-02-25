namespace Core.Configuration.Models;

public class GenericEnvConfigurations<T> : EnvConfigurations where T : BaseTestConfigurations
{
    public T Configs { get; init; } = null!;
}