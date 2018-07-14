namespace VolskSoft.Alfa
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Exception that is thrown when config settings provider does not contain requested key
    /// </summary>
    [Serializable]
    public class SectionNotFoundException : Exception
    {
        /// <summary>
        /// Initialize new instance of <c>KeyNotFoundException</c> class.
        /// </summary>
        public SectionNotFoundException()
        {
        }

        /// <summary>
        /// Initialize new instance of <c>KeyNotFoundException</c> class.
        /// </summary>
        /// <param name="message">A message of the error.</param>
        public SectionNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initialize new instance of <c>KeyNotFoundException</c> class.
        /// </summary>
        /// <param name="message">A message of the error.</param>
        /// <param name="innerException">Inner exception</param>
        public SectionNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyNotFoundException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected SectionNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}