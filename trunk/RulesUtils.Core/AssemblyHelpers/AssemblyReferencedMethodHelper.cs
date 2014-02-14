using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Microsoft.FxCop.Sdk;

namespace RulesUtils.Core.AssemblyHelpers
{
  public class AssemblyReferencedMethodHelper : AssemblyNodeEnumerationHelper<Method>
  {
    /// <summary>
    /// Gets a AssemblyReferencedMethodHelper instance for the specified AssemblyNode.
    /// </summary>
    /// <param name="assemblyNode">AssemblyNode.</param>
    /// <returns>AssemblyReferencedMethodHelper instance</returns>
    public static AssemblyReferencedMethodHelper Get( AssemblyNode assemblyNode )
    {
      return Get( assemblyNode, n => new AssemblyReferencedMethodHelper( n ) );
    }

    /// <summary>
    /// Initializes a new instance of the AssemblyReferencedMethodHelper class.
    /// </summary>
    /// <param name="assemblyNode">AssemblyNode of which to enumerate referenced methods.</param>
    protected AssemblyReferencedMethodHelper( AssemblyNode assemblyNode )
      : base( assemblyNode )
    {
    }

    /// <summary>
    /// Called when analysis encounters a Parameter.
    /// </summary>
    /// <param name="parameter">Parameter.</param>
    [SuppressMessage( "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Only called by code analysis tool." )]
    public override void VisitParameter( Parameter parameter )
    {
      base.VisitParameter( parameter );
      Items.Add( parameter.DeclaringMethod );
    }

    /// <summary>
    /// Called when analysis encounters a Construct.
    /// </summary>
    /// <param name="parameter">Construct.</param>
    [SuppressMessage( "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Only called by code analysis tool." )]
    public override void VisitConstruct( Construct construct )
    {
      base.VisitConstruct( construct );
      foreach( var method in
          construct
              .Operands
              .OfType<UnaryExpression>()
              .Select( ue => ue.Operand )
              .OfType<MemberBinding>()
              .Select( mb => mb.BoundMember )
              .OfType<Method>() )
      {
        Items.Add( method );
      }
    }
  }
}
