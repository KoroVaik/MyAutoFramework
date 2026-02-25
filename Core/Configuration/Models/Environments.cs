using System.ComponentModel;

namespace Core.Configuration.Models;

public enum Environments
{
    [Description("dev")]
    DEV,

    [Description("qa")]
    QA,

    [Description("staging")]
    STAGING,

    [Description("production")]
    PRODUCTION
}