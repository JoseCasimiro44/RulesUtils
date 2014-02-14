using Microsoft.FxCop.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.IO;
using System.Collections;
using RulesUtils.Core;

namespace Demo.Rules
{
  public sealed class ResourcesMustBeReferencedRule : BaseRule
  {
    public ResourcesMustBeReferencedRule()
      : base( "ResourcesMustBeReferenced" )
    {
    }

    public override TargetVisibilities TargetVisibility
    {
      get
      {
        return TargetVisibilities.NotExternallyVisible;
      }
    }

    public override ProblemCollection Check( Resource resource )
    {
      using( var stream = new MemoryStream( resource.Data ) )
      {
        try
        {
          using( ResourceReader reader = new ResourceReader( stream ) )
          {
            AssemblyResourceNameHelper nameHelper = AssemblyResourceNameHelper.Get( resource.DefiningModule.ContainingAssembly );

            IEnumerable<string> allTextResources = reader.Cast<DictionaryEntry>().Select( e => e.Key.ToString() );
            IEnumerable<string> allNotReferencedResources = allTextResources.Where( resourceName => !nameHelper.Contains( resourceName ) );

            foreach( var resourceName in allNotReferencedResources )
            {
              Problems.Add( new Problem( GetResolution( resourceName, resource.Name ), resourceName ) );
            }
          }
        }
        catch( Exception )
        {

        }
      }
      return Problems;
    }
  }
}
