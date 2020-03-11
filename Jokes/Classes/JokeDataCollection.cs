using Jokes.Interfaces;
using System.Collections.Generic;

namespace Jokes.Classes
{
    /// <summary>
    /// Represents a collection of jokes.
    /// The names must match the fields returned from the web API
    /// in order to deserialize from JSON properly.
    /// </summary>
    public class JokeDataCollection: IJokeDataCollection
    {
        public JokeDataCollection()
        {
            Results = new List<JokeData>();
        }

        public IEnumerable<IJokeData> Results { get; set; }
    }
}