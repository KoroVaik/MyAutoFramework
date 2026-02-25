using System.ComponentModel;
using System.Reflection;

namespace Core.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString())!;
        var attribute = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() 
                           ?? throw new InvalidOperationException("No description attribute found");
        
        return (attribute as DescriptionAttribute)!.Description;
    }
}