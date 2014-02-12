using Microsoft.FxCop.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RulesUtils.Core;

namespace Demo.Rules
{
    public sealed class EnforceProperNotationRule : BaseRule
    {
      private static int m_Foo;

        public EnforceProperNotationRule()
        : base( "EnforceProperNotation" )
        {
        }

        public override TargetVisibilities TargetVisibility
        {
            get
            {
                return TargetVisibilities.NotExternallyVisible;
            }
        }

        public override ProblemCollection Check(Member member)
        {
            Field field = member as Field;
            if (field == null)
            {
                return null;
            }

            if (field.IsPrivate)
            {
                CheckFieldPrefix(field, "_");
            }

            // By default the Problems collection is empty so no violations will be reported
            return Problems;
        }

        private void CheckFieldPrefix(Field field, string expectedPrefix)
        {
            if (!field.Name.Name.StartsWith(expectedPrefix, StringComparison.Ordinal))
            {
                Resolution resolution = GetResolution(
                  field,  // Field {0} is not in Hungarian notation.
                  expectedPrefix  // Field name should be prefixed with {1}.
                );
                Problem problem = new Problem(resolution);
                Problems.Add(problem);
            }
        }
    }
}
