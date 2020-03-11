using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jokes.Interfaces;
using Newtonsoft.Json;

namespace Jokes.Classes
{
    public abstract class GetJokesBase: IGetJokes
    {
        /// <summary>
        /// This is the base class for the consuming of the joke APIs and packaging the returned data.
        /// </summary>
        public virtual IEnumerable<IFilterCategory> FilterCategories
        {
            get
            {
                return
                    new List<FilterCategory>
                    {
                        new FilterCategory
                        {
                            Name = "Short (<10 words)",
                            CompareFunc = (x) => x.WordCount() > 0 && x.WordCount() < 10,
                        },
                        new FilterCategory
                        {
                            Name = "Medium (<20 words)",
                            CompareFunc = (x) => x.WordCount() >= 10 && x.WordCount() < 20,
                        },
                        new FilterCategory
                        {
                            Name = "Long (>= 20 words)",
                            CompareFunc = (x) => x.WordCount() >= 20 && x.WordCount() <= int.MaxValue,
                        }
                    };
            }
        }

        /// <summary>
        /// Gets a random joke.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <returns>
        /// JSON
        /// </returns>
        public virtual string RandomReturnJson(HttpClient httpClient)
        {
            return JsonConvert.SerializeObject(Random(httpClient).Result);
        }

        /// <summary>
        /// Gets a random joke.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <returns>
        /// HTML
        /// </returns>
        public virtual string RandomReturnHtml(HttpClient httpClient)
        {
            return Random(httpClient).Result.Joke.Replace(Environment.NewLine, "<br/>");
        }

        /// <summary>
        /// Gets a random joke.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <returns>
        /// JokeData class
        /// </returns>
        public virtual async Task<JokeData> Random(HttpClient httpClient)
        {
            var response = await httpClient.GetStringAsync("/");
            return JsonConvert.DeserializeObject<JokeData>(response);
        }

        /// <summary>
        /// Searches for jokes.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="term">
        /// Multiple words can be used if seperated by at least one space. Case insensitive.
        /// </param>
        /// <param name="limit"></param>
        /// <returns>
        /// JSON
        /// </returns>
        public virtual string SearchReturnJason(HttpClient httpClient, string term, int limit = 30)
        {
            return JsonConvert.SerializeObject(Search(httpClient, term, limit).Result);
        }

        /// <summary>
        /// Searches for jokes.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="term">
        /// Multiple words can be used if seperated by at least one space. Case insensitive.
        /// </param>
        /// <param name="filterCategories"></param>
        /// <param name="limit"></param>
        /// <returns>
        /// Brings back an HTML bulleted list categorized into groups based on the word sizes in the jokes.
        /// The words found in the text are highlighted yellow.
        /// The default groups are; Short (<10 words), Medium (<20 words), Long (>= 20 words).
        /// These can be changed by passing in your own filterCategories.
        /// </returns>
        public virtual string SearchReturnHtml(HttpClient httpClient, string term, IEnumerable<IFilterCategory> filterCategories = null, int limit = 30)
        {
            if (filterCategories == null)
                filterCategories = FilterCategories;

            var html = "<h4>Here are your search results grouped by word length.</h4>";

            var jokes = Search(httpClient, term, limit).Result;

            var foundAnyJokes = false;

            foreach (var fitlerCategory in filterCategories)
            {
                html += $"<div><label style=\"font-weight:bold\">{fitlerCategory.Name}</label></div>";

                var matchedJokes = jokes.Results.Where(x => fitlerCategory.CompareFunc(x.Joke)).OrderBy(x => x.Joke.Length);
                if (!matchedJokes.Any())
                {
                    html += "<div>No jokes found.</div>";
                    continue;
                }

                foundAnyJokes = true;

                html += "<ul>";

                foreach (var jokeItem in matchedJokes)
                {
                    if (String.IsNullOrWhiteSpace(term))
                    {
                        html += $"<li>{jokeItem.Joke}</li>";
                        continue;
                    }

                    var wordsToHighlight = term.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    html += HighlightWords($"<li>{jokeItem.Joke}</li>", term);
                }

                html += "</ul>";
            }

            if (!foundAnyJokes)
                html = "<div>No jokes found.</div>";

            return html;
        }

        /// <summary>
        /// Searches for jokes.
        /// Case insensitive.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="term">
        /// Multiple words can be used if seperated by at least one space. Case insensitive.
        /// </param>
        /// <param name="limit"></param>
        /// <returns>
        /// JokeDataCollection class
        /// </returns>
        public virtual async Task<JokeDataCollection> Search(HttpClient httpClient, string term, int limit = 30)
        {
            var uriBuilder = new UriBuilder(httpClient.BaseAddress);
            uriBuilder.Query = $"term={term}&limit={limit}";

            var response = await httpClient.GetStringAsync(uriBuilder.Uri);
            return JsonConvert.DeserializeObject<JokeDataCollection>(response);
        }

        /// <summary>
        /// Highlights found words with yellow highlighting.
        /// </summary>
        /// <param name="text">
        /// This is the string to search for the "term"(s) in.
        /// </param>
        /// <param name="term">
        /// Multiple words can be used if seperated by at least one space. Case insensitive.
        /// </param>
        /// <returns></returns>
        private string HighlightWords(string text, string term)
        {
            var wordsToHighlight = term.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in wordsToHighlight)
            {
                text = Regex.Replace(text, word, matchEvaluator => LocalReplaceMatchCase(matchEvaluator, word), RegexOptions.IgnoreCase);
            }

            return text;
        }

        /// <summary>
        /// This is to keep the capitalization of the first letter in found words.
        /// </summary>
        /// <param name="matchExpression"></param>
        /// <param name="replaceWith"></param>
        /// <returns></returns>
        private string LocalReplaceMatchCase(Match matchExpression, string replaceWith)
        {
            var highlighting = "<label style=\"background-color:yellow\">{0}</label>";
            replaceWith = replaceWith.ToLower();

            // Test whether the match is capitalized
            if (Char.IsUpper(matchExpression.Value[0]))
            {
                // Capitalize the replacement string
                System.Text.StringBuilder replacementBuilder = new System.Text.StringBuilder(replaceWith);
                replacementBuilder[0] = Char.ToUpper(replacementBuilder[0]);
                return String.Format(highlighting, replacementBuilder.ToString());
            }
            else
            {
                return String.Format(highlighting, replaceWith);
            }
        }
    }
}
