using System;
using System.Linq;

namespace Common.ParserFramework
{
    /// <summary>
    /// A reduction rule operating on a <see cref="ReductionStack"/>.
    /// </summary>
    public abstract class ReductionRule
    {
        #region Diagnostics

        public virtual string Name { get { return GetType().Name; } }

        #endregion

        /// <summary>
        /// Try to apply this action to the current state stack.
        /// </summary>
        /// <param name="s">The stack to apply the rule to.</param>
        /// <param name="la">The current look-ahead symbol.</param>
        /// <returns>True if the rule was applied, false otherwise.</returns>
        public abstract bool Apply(ReductionStack s, object la);

        #region Predicate Factories

        protected static Predicate MatchAny()
        {
            return new Predicate(r => true);
        }

        protected static Predicate ExactType(Type type)
        {
            return new Predicate(r => r.GetType() == type);
        }

        protected static Predicate ClassType(Type type)
        {
            return new Predicate(r => type.IsAssignableFrom(r.GetType()));
        }

        protected static Predicate ClassTypes(params Type[] types)
        {
            return new Predicate(r => types.Any(t => t.IsAssignableFrom(r.GetType())));
        }

        #endregion
    }
}
