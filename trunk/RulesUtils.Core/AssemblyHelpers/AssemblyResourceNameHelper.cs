using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Microsoft.FxCop.Sdk;
using RulesUtils.Core.AssemblyHelpers;

namespace RulesUtils.Core
{
    public class AssemblyResourceNameHelper : AssemblyNodeEnumerationHelper<string>
    {
        /// <summary>
        /// Gets a AssemblyResourceNameHelper instance for the specified AssemblyNode.
        /// </summary>
        /// <param name="assemblyNode">AssemblyNode.</param>
        /// <returns>AssemblyResourceNameHelper instance</returns>
        public static AssemblyResourceNameHelper Get(AssemblyNode assemblyNode)
        {
            return Get(assemblyNode, n => new AssemblyResourceNameHelper(n));
        }

        /// <summary>
        /// Initializes a new instance of the AssemblyResourceNameHelper class.
        /// </summary>
        /// <param name="assemblyNode">AssemblyNode of which to enumerate resource strings.</param>
        protected AssemblyResourceNameHelper(AssemblyNode assemblyNode)
            : base(assemblyNode)
        {
        }
      
        /// <summary>
        /// Called when analysis encounters a MethodCall.
        /// </summary>
        /// <param name="call">Current MethodCall.</param>
        /// <param name="caller">Current Method.</param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Only called by code analysis tool.")]
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Only called by code analysis tool.")]
        protected override void VisitMethodCall(MethodCall call, Method currentMethod)
        {
            base.VisitMethodCall(call, currentMethod);

            var callee = call.Callee as MemberBinding;
            if (null != callee)
            {
                var boundMethod = callee.BoundMember as Method;
                if (null != boundMethod)
                {
                    if ((boundMethod.DeclaringType.IsAssignableTo(FrameworkTypes.ResourceManager)) &&
                        (("GetString" == boundMethod.Name.Name) ||
                         ("GetStream" == boundMethod.Name.Name) ||
                         ("GetObject" == boundMethod.Name.Name)))
                    {
                        // Call to ResourceManager.GetString/GetStream/GetObject
                        if (CallGraph.CallersFor(currentMethod).Any() ||
                            !currentMethod.DeclaringType.Attributes.Where(a => ("System.CodeDom.Compiler.GeneratedCodeAttribute" == a.Type.FullName) && ("System.Resources.Tools.StronglyTypedResourceBuilder" == (string)((Literal)a.GetPositionalArgument(0)).Value)).Any())
                        {
                            // At least one caller of the Resources.Property OR a direct call to ResourceManager.GetXxx by something other than the generated Resources class
                            var literal = call.Operands.FirstOrDefault() as Literal;
                            if ((null != literal) && (FrameworkTypes.String == literal.Type))
                            {
                                Items.Add((string)literal.Value);
                            }
                        }
                    }
                }
            }
        }
    }
}
