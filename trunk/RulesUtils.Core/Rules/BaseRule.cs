using Microsoft.FxCop.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesUtils.Core
{
    public abstract class BaseRule : BaseIntrospectionRule
    {
        public BaseRule(string ruleName)
            : base(ruleName, "RulesUtils.Core.RuleMetadata", typeof(BaseRule).Assembly)
        {
        }
    }
}
