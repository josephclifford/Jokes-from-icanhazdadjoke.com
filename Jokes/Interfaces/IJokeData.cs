namespace Jokes.Interfaces
{
    /// <summary>
    /// Interface that represents a joke.
    /// The names must match the fields returned from the web API
    /// in order to deserialize from JSON properly.
    /// </summary>
    public interface IJokeData
    {
        string Id { get; set; }

        string Joke { get; set; }

        string Status { get; set; }
    }
}
