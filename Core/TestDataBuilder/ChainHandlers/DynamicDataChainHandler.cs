using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Core.TestDataBuilder.ChainHandlers
{
    /// <summary>
    /// Replace placeholders in the format $$<MethodName(arg1,arg2,...)> with generated data
    /// </summary>
    public class DynamicDataChainHandler : AbstractDataChainHandler
    {
        readonly Regex dynamicDataRegex = new Regex(@"\$\$<(.*?)\((.*?)\)>");

        public override void Handle(JsonDataChain chain)
        {
            HandleToken<string>(chain.Data, ReplaceWithGeneratedData);
        }

        private string ReplaceWithGeneratedData(string inputData)
        {
            var result = dynamicDataRegex.Replace(inputData, match => {
                var methodName = match.Groups[1].Value;
                var methodArgsRawData = match.Groups[2].Value;
                var methodArgs = string.IsNullOrEmpty(methodArgsRawData) 
                ? [] 
                : methodArgsRawData.Split(",").Select(arg => ParseToObject(arg)).ToArray();
                return InvokeMethod(methodName, methodArgs);
            });
            return result;
        }

        private object ParseToObject(string input)
        {
            if (int.TryParse(input, out int intValue))
            {
                return intValue;
            }
            else if (float.TryParse(input, out float floatValue))
            {
                return floatValue;
            }
            else if (double.TryParse(input, out double doubleValue))
            {
                return doubleValue;
            }
            else
            {
                return input;
            }
        }

        private string InvokeMethod(string methodName, object[] args)
        {
            var methods = typeof(DataGenerator).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .ToList();

            var method = FindDataGenerationMethod(methodName, args);

            var invokeResult = method.Invoke(null, args.Cast<object>().ToArray());

            if (invokeResult is string strResult)
                return strResult;
            else if (invokeResult != null)
                return invokeResult.ToString()!;
            else
                throw new Exception("Not found mrthod result");
        }

        private MethodInfo FindDataGenerationMethod(string methodName, object[] args)
        {
            var method = typeof(DataGenerator).GetMethod(
                methodName, 
                BindingFlags.Public | BindingFlags.Static, 
                args.Select(a => a.GetType()).ToArray());

            if (method == null)
                throw new Exception($"Could not find method '{methodName}' with arguments '{args}'");

            return method;
        }

        protected void HandleToken<T>(JToken token, Func<T, T> tokenUpdateAction)
        {
            if (token is JObject JObject)
            {
                foreach (var childProp in JObject.Properties())
                {
                    HandleToken(childProp.Value, tokenUpdateAction);
                }
                return;
            }

            if (token is JArray JArray)
            {
                foreach (var childToken in JArray)
                {
                    HandleToken(childToken, tokenUpdateAction);
                }
                return;
            }

            var targetValue = (JValue)token;
            if (targetValue.Value is T stringValue)
            {
                var newValue = tokenUpdateAction(stringValue);
                targetValue.Value = newValue != null ? newValue : "null";
            }
        }
    }
}
