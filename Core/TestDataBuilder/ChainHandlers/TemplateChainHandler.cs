using Newtonsoft.Json.Linq;

namespace Core.TestDataBuilder.ChainHandlers
{
    /// <summary>
    /// Writes specific data over a template defined in the "##Template" property.
    /// </summary>
    public class TemplateChainHandler : AbstractDataChainHandler
    {
        const string TemplatePropName = "##Template";

        public override void Handle(JsonDataChain chain)
        {
            var allDataProps = chain.Data.Properties();

            foreach (var dataOfTypeProp in allDataProps)
            {
                var entityProps = (dataOfTypeProp.Value as JObject)?.Properties();

                try
                {
                    foreach (var jProp in entityProps!)
                        ApplyTemplateOnEntity(jProp, entityProps);
                }
                catch (KeyNotFoundException ex)
                {
                    throw 
                        new ChainHandlerException($"Error in file '{chain.FileName}': {ex.Message}");
                }
            }
        }

        protected void ApplyTemplateOnEntity(JProperty specificJProp, IEnumerable<JProperty> allJProps)
        {
            var templateName = specificJProp.Value[TemplatePropName]?.ToString();
            if (templateName != null)
            {
                var templateJProp = allJProps.FirstOrDefault(p => p.Name == templateName);
                if (templateJProp != null)
                {
                    var resultObj = templateJProp.Value.ToObject<JObject>()!;
                    resultObj.Merge(specificJProp.Value);
                    resultObj.Remove(TemplatePropName);
                    specificJProp.Value = resultObj;
                }
                else
                {
                    throw new KeyNotFoundException($"Template '{templateName}' not found for entity '{specificJProp.Name}'." +
                        $"\nExisting aliases:\n{string.Join("\n", allJProps.Select(p => p.Name))}");
                }
            }
        }
    }
}
