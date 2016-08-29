using System;
using System.Xml;

namespace GRUML.Model
{
    /// <summary>
    /// Baseclass for elements of the model.
    /// </summary>
    public abstract class Element
    {
        #region Private

        private static int _seed = 0;

        #endregion

        #region Properties

        internal DocumentReader Context { get; set; }

        /// <summary>
        /// Logical identifier of the element, can be null.
        /// </summary>
        public string ID { get; protected set; }

        /// <summary>
        /// Per project unique identifier of the element.
        /// </summary>
        public int SequenceNumber { get; private set; }

        public string UniqueName { get { return "__X_" + SequenceNumber; } }

        public virtual bool IsChild {  get { return true; } }

        #endregion

        #region Constructors

        protected Element()
        {
            SequenceNumber = ++_seed;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads element data from the corresponding XML element.
        /// </summary>
        /// <param name="e"></param>
        /// <returns>False if the process shall recurse into the children of the XML element, 
        /// true if the function has already processed them.</returns>
        public virtual bool Load(XmlElement e)
        {
            return false;
        }

        /// <summary>
        /// Adds a child into this element.
        /// </summary>
        /// <param name="child">The child to add.</param>
        public virtual void AddChild(object child)
        {
            throw new ArgumentException("cannot add [" + child + "] into [" + this + "].");
        }

        #endregion
    }
}
