using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Microsoft.FxCop.Sdk;

namespace RulesUtils.Core.AssemblyHelpers
{
  public class AssemblyNodeEnumerationHelper<T> : BinaryReadOnlyVisitor, IEnumerable<T>
  {
    /// <summary>
    /// Stores a cache of AssemblyNode to AssemblyNodeEnumerationHelper(T).
    /// </summary>
    private static Dictionary<AssemblyNode, AssemblyNodeEnumerationHelper<T>> _cache = new Dictionary<AssemblyNode, AssemblyNodeEnumerationHelper<T>>();

    /// <summary>
    /// Gets a AssemblyNodeEnumerationHelper(T) for the specified AssemblyNode.
    /// </summary>
    /// <typeparam name="H">Type of AssemblyNodeEnumerationHelper(T).</typeparam>
    /// <param name="assemblyNode">AssemblyNode.</param>
    /// <param name="constructor">Function that creates a AssemblyNodeEnumerationHelper(T) from an AssemblyNode.</param>
    /// <returns>AssemblyNodeEnumerationHelper(T) instance.</returns>
    protected static H Get<H>( AssemblyNode assemblyNode, Func<AssemblyNode, H> constructor ) where H : AssemblyNodeEnumerationHelper<T>
    {
      lock( _cache )
      {
        if( !_cache.ContainsKey( assemblyNode ) )
        {
          _cache.Add( assemblyNode, constructor( assemblyNode ) );
        }
      }
      return (H)_cache[assemblyNode];
    }

    /// <summary>
    /// Stores a reference to the AssemblyNode.
    /// </summary>
    private readonly AssemblyNode _assemblyNode;

    /// <summary>
    /// Stores a reference to the current Method during a Visit.
    /// </summary>
    private Method _currentMethod;

    /// <summary>
    /// Gets the list of items.
    /// </summary>
    protected List<T> Items { get; private set; }

    /// <summary>
    /// Initializes a new instance of the AssemblyNodeEnumerationHelper class.
    /// </summary>
    /// <param name="assemblyNode">AssemblyNode of which to enumerate items.</param>
    protected AssemblyNodeEnumerationHelper( AssemblyNode assemblyNode )
    {
      _assemblyNode = assemblyNode;
    }

    /// <summary>
    /// Called when analysis encounters a Method.
    /// </summary>
    /// <param name="memberBinding">Current Method.</param>
    public override void VisitMethod( Method method )
    {
      _currentMethod = method;
      base.VisitMethod( method );
    }

    /// <summary>
    /// Called when analysis encounters a MethodCall.
    /// </summary>
    /// <param name="call">Current MethodCall.</param>
    [SuppressMessage( "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Only called by code analysis tool." )]
    public override void VisitMethodCall( MethodCall call )
    {
      base.VisitMethodCall( call );
      VisitMethodCall( call, _currentMethod );
    }

    /// <summary>
    /// Called when analysis encounters a MethodCall.
    /// </summary>
    /// <param name="call">Current MethodCall.</param>
    /// <param name="caller">Current Method.</param>
    protected virtual void VisitMethodCall( MethodCall call, Method currentMethod ) { }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An IEnumerator(T) that can be used to iterate through the collection.</returns>
    public IEnumerator<T> GetEnumerator()
    {
      lock( _assemblyNode ) // Any non-null private member variable
      {
        if( null == Items )
        {
          // Generate the list of items
          Items = new List<T>();
          Visit( _assemblyNode );
        }
      }

      // Return the enumerator
      return Items.AsReadOnly().GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
