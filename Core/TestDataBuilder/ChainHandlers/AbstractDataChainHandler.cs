using Newtonsoft.Json.Linq;

namespace Core.TestDataBuilder.ChainHandlers
{
    public abstract class AbstractDataChainHandler
    {
        public abstract void Handle(JsonDataChain chain);
    }

    public class ChainHandlerException : Exception
    {
        public ChainHandlerException(string message) : base(message) { }
    }
}
