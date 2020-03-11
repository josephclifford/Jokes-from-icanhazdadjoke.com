using System.Net.Http;
using Jokes.Classes;
using Microsoft.AspNetCore.Mvc;

namespace Jokes.Controllers
{
    [ApiController]
    public class JokesApiController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// These APIs get jokes from the website https://icanhazdadjoke.com.
        /// In the Startup.cs file in the ConfigureServices method you can add named clients
        /// and use those to consume from different sites.
        /// </summary>
        /// <param name="httpClientFactory"></param>
        public JokesApiController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Gets a random joke
        /// </summary>
        /// <example>
        /// http://localhost:61970/jokesapi/get-random-return-html
        /// </example>
        /// <returns>
        /// Brings back HTML
        /// </returns>
        [HttpGet]        
        [Route("jokesapi/get-random-return-html")]
        public ActionResult GetRandomReturnHtml()
        {
            return Ok(new GetJokes().RandomReturnHtml(_httpClientFactory.CreateClient("icanhazdadjokeRandom")));
        }

        /// <summary>
        /// Gets a random joke
        /// </summary>
        /// <example>
        /// http://localhost:61970/jokesapi/get-random-return-json
        /// </example>
        /// <returns>
        /// Brings back a JSON string
        /// </returns>
        [HttpGet]
        [Route("jokesapi/get-random-return-json")]
        public ActionResult GetRandomReturnJson()
        {
            return Ok(new GetJokes().RandomReturnJson(_httpClientFactory.CreateClient("icanhazdadjokeRandom")));
        }

        /// <summary>
        /// Searches for jokes and returns a max of 30 jokes.
        /// Case insensitive. Multiple words can be used if seperated by at least one space.
        /// </summary>
        /// <example>
        /// http://localhost:61970/jokesapi/search-return-json?term=dad
        /// </example>
        /// <param name="term"></param>
        /// <returns>
        /// Brings back a JSON string
        /// </returns>
        [HttpGet]
        [Route("jokesapi/search-return-json")]
        public ActionResult SearchReturnJson(string term)
        {
            return Ok(new GetJokes().SearchReturnJason(_httpClientFactory.CreateClient("icanhazdadjokeSearch"), term, 30));
        }

        /// <summary>
        /// Searches for jokes and returns a max of 30 jokes.
        /// Case insensitive. Multiple words can be used if seperated by at least one space.
        /// </summary>
        /// <example>
        /// http://localhost:61970/jokesapi/search-return-html?term=dad
        /// </example>
        /// <param name="term"></param>
        /// <returns>
        /// Brings back an HTML bulleted list categorized into groups based on the word sizes in the jokes.
        /// The words found in the text are highlighted yellow.
        /// The default groups are; Short (<10 words), Medium (<20 words), Long (>= 20 words).
        /// These can be changed by passing in your own FilterCategories after the term param.
        /// </returns>
        [HttpGet]
        [Route("jokesapi/search-return-html")]
        public ActionResult SearchReturnHtml(string term)
        {
            return Ok(new GetJokes().SearchReturnHtml(_httpClientFactory.CreateClient("icanhazdadjokeSearch"), term));
        }
    }
}
