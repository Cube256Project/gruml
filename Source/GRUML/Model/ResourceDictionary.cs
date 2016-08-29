using System;
using Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRUML.Model
{
    /// <summary>
    /// A collection of tagged <see cref="DictionaryElement"/> objects.
    /// </summary>
    /// <remarks>
    /// <para>A tag can occur only once.</para>
    /// </remarks>
    public class ResourceDictionary : Element
    {
        #region Private

        private Dictionary<string, DictionaryElement> _items = new Dictionary<string, DictionaryElement>();

        #endregion

        /// <summary>
        /// Returns the items.
        /// </summary>
        public IReadOnlyDictionary<string, DictionaryElement> Items {  get { return _items; } }

        /// <summary>
        /// Adds a <see cref="DictionaryElement"/> to the dictionary.
        /// </summary>
        /// <param name="argument"></param>
        public override void AddChild(object argument)
        {
            if (argument is DictionaryElement)
            {
                var e = (DictionaryElement)argument;
                if(!e.Tags.Any())
                {
                    throw new ArgumentException("dictionary element [" + argument + "] does not have any tags.");
                }

                foreach (var tag in e.Tags)
                {
                    if (_items.ContainsKey(tag.Value))
                    {
                        throw new Exception("duplicate template tag " + tag.Value.Quote() + ".");
                    }

                    _items.Add(tag.Value, e);
                }
            }
            else
            {
                base.AddChild(argument);
            }
        }

        public void Merge(ResourceDictionary other)
        {
            foreach (var item in other.Items)
            {
                if(_items.ContainsKey(item.Key))
                {
                    throw new Exception("unable to merge dictionaries, conflicting key " + item.Key.Quote());
                }

                _items.Add(item.Key, item.Value);
            }
        }
    }
}
