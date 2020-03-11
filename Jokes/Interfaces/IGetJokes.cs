using Jokes.Classes;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Jokes.Interfaces
{
    /// <summary>
    /// This is the interface for the consuming of the joke APIs and packaging the returned data.
    /// </summary>
    public interface IGetJokes
    {
        IEnumerable<IFilterCategory> FilterCategories { get; }

        string RandomReturnJson(HttpClient httpClient);

        string RandomReturnHtml(HttpClient httpClient);

        Task<JokeData> Random(HttpClient httpClient);

        string SearchReturnJason(HttpClient httpClient, string term, int limit);

        string SearchReturnHtml(HttpClient httpClient, string term, IEnumerable<IFilterCategory> filterCategories, int limit);

        Task<JokeDataCollection> Search(HttpClient httpClient, string term, int limit);
    }
}
