namespace VolskSoft.Alfa
{
    using System;
    using System.Collections.Specialized;
    using System.Runtime.Serialization;

    public class SettingsCollection : NameValueCollection
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:VolskSoft.Alfa.SettingsCollection" /> class.
        /// </summary>
        public SettingsCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsCollection"/> class.
        /// </summary>
        /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> object that contains the information required to serialize the new <see cref="T:System.Collections.Specialized.NameValueCollection"></see> instance.</param>
        /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext"></see> object that contains the source and destination of the serialized stream associated with the new <see cref="T:System.Collections.Specialized.NameValueCollection"></see> instance.</param>
        protected SettingsCollection(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Adds an entry with the specified name and value to the <see cref="T:System.Collections.Specialized.NameValueCollection"></see>.
        /// </summary>
        /// <param name="name">The <see cref="T:System.String"></see> key of the entry to add. The key can be null.</param>
        /// <param name="value">The <see cref="T:System.String"></see> value of the entry to add. The value can be null.</param>
        /// <exception cref="T:System.NotSupportedException">The collection is read-only. </exception>
        public override void Add(string name, string value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            base.Add(name.ToUpperInvariant(), value);
        }

        /// <summary>
        /// Copies the elements of the System.Collections.ICollection to an System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="destination">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection. The System.Array must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        public void CopyTo(string[] destination, int index)
        {
            base.CopyTo(destination, index);
        }
    }
}