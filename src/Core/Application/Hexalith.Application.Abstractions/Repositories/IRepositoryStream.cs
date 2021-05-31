#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

namespace Hexalith.Application.Repositories
{
    using System.Collections.Generic;

    /// <summary>
    /// The streamed repository interface.
    /// </summary>
    public interface IRepositoryStream
    {
        /// <summary>
        /// Postion of the stream. Returns 0 if the stream is empty.
        /// </summary>
        public int Position { get; }

        /// <summary>
        /// Add events to the stream.
        /// </summary>
        /// <param name="metadata">Metadata of the events.</param>
        /// <param name="events">The list of events.</param>
        /// <returns></returns>
        public int Add(IRepositoryMetadata metadata, IEnumerable<object> events);

        /// <summary>
        /// Read the events at the given position.
        /// </summary>
        /// <param name="position">
        /// The postion to read in the stream. The position must be equal or greater than one.
        /// </param>
        /// <returns>The metatdata and events.</returns>
        public (IRepositoryMetadata metadata, IEnumerable<object> events) Read(int position);
    }
}