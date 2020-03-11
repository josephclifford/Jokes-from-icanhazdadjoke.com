using System.Collections.Generic;

namespace Jokes.Interfaces
{
    /// <summary>
    /// Interface that represents a collection of jokes.
    /// The names must match the fields returned from the web API
    /// in order to deserialize from JSON properly.
    /// </summary>
    public interface IJokeDataCollection
    {
        IEnumerable<IJokeData> Results { get; set; }
    }
}
