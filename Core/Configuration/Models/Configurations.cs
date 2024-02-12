namespace Core.Configuration.Models
{
    public class Configurations
    {
        public string? Environment { get; set; }
    }

    public class Configurations<T> : Configurations where T : class
    {
        public T? EnironmentConfigurations { get; set; }
    }
}
