using Jokes.Interfaces;

namespace Jokes.Classes
{
    /// <summary>
    /// Represents a joke.
    /// The names must match the fields returned from the web API
    /// in order to deserialize from JSON properly.
    /// </summary>
    public class JokeData: IJokeData
    {
        public string Id { get; set; }

        public string Joke { get; set; }

        public string Status { get; set; }
    }
}
