using Microsoft.FxCop.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Rules
{
    public abstract class BaseRule : BaseIntrospectionRule
    {
        public BaseRule( string ruleName )
            : base( ruleName, "Demo.Rules.RuleMetadata", typeof(BaseRule).Assembly )
        {
        }
    }
}
