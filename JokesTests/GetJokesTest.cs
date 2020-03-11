using Jokes.Classes;
using Moq;
using Moq.Contrib.HttpClient;
using Moq.Protected;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace JokesTests
{
    public class GetJokesTest
    {
        /// <summary>
        /// I tried to figure a way to wrap the actual handler vs mocking it but kept runing into issues.
        /// </summary>
        [Fact]
        public void RandomJokeTest()
        {
            // mock the handler
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(
                        "{" +
                            "\"id\": \"R7UfaahVfFd\"," +
                            "\"joke\": \"My dog used to chase people on a bike a lot. It got so bad I had to take his bike away.\"," +
                            "\"status\": 200" +
                        "}")
               })
               .Verifiable();

            // use real http client with mocked handler
            var factory = handlerMock.CreateClientFactory();
            Mock.Get(factory).Setup(x => x.CreateClient("icanhazdadjokeRandom"))
                .Returns(() =>
                {
                    var client = handlerMock.CreateClient();
                    client.BaseAddress = new Uri("https://icanhazdadjoke.com");
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("User-Agent", "icanhazdadjoke API Random Test");
                    return client;
                });

            var joke = new GetJokes().Random(factory.CreateClient("icanhazdadjokeRandom")).Result;

            Assert.True(joke.Id == "R7UfaahVfFd");
        }

        /// <summary>
        /// Since this is using a mocked handler it will just bring back all the rows no matter
        /// what is searched for. So this ends up just being a test of the rows going in vs coming out.
        /// </summary>
        [Fact]
        public void SearchForJokesTest()
        {
            // mock the handler
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(
                        "{" +
                            "\"results\": " + 
                            "[" +
                                "{" +
                                    "\"id\": \"M7wPC5wPKBd\"," +
                                    "\"joke\": \"Did you hear the one about the guy with the broken hearing aid? Neither did he.\"" +
                                "}," +
                                "{" +
                                    "\"id\": \"MRZ0LJtHQCd\"," +
                                    "\"joke\": \"What do you call a fly without wings? A walk.\"" +
                                "}," +
                                "{" +
                                    "\"id\": \"usrcaMuszd\"," +
                                    "\"joke\": \"What's the worst thing about ancient history class? The teachers tend to Babylon.\"" +
                                "}" +
                            "]" +
                        "}")
                })
               .Verifiable();

            // use real http client with mocked handler
            var factory = handlerMock.CreateClientFactory();
            Mock.Get(factory).Setup(x => x.CreateClient("icanhazdadjokeSearch"))
                .Returns(() =>
                {
                    var client = handlerMock.CreateClient();
                    client.BaseAddress = new Uri("https://icanhazdadjoke.com/search");
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("User-Agent", "icanhazdadjoke API Search Test");
                    return client;
                });

            var jokes = new GetJokes().Search(factory.CreateClient("icanhazdadjokeSearch"), "dad").Result;

            Assert.True(jokes.Results.Count() == 3);

            //var jokes = new GetJokes().Search(factory.CreateClient("icanhazdadjokeSearch"), "the").Result;

            //Assert.True(jokes.Results.Count() == 2);
        }

        [Fact]
        public void WordCountTest()
        {
            Assert.True("   1   222   33  44 5   6    7  8   999  101010  11  22  13 14".WordCount() == 14);

            Assert.True((" Apart from counting words and characters, our online editor can help you to improve word choice and writing style," +
                        " and, optionally, help you to detect grammar mistakes and plagiarism. To check word count, simply place your cursor into" +
                        " the text box above and start typing. You'll see the number of characters and words increase or decrease as you type, delete," +
                        " and edit them. You can also copy and paste text from another program over into the online editor above. The Auto-Save feature" +
                        " will make sure you won't lose any changes while editing, even if you leave the site and come back later. Tip: Bookmark this page" +
                        " now. Knowing the word count of a text can be important.For example, if an author has to write a minimum or maximum amount of words" +
                        " for an article, essay, report, story, book, paper, you name it. WordCounter will help to make sure its word count reaches a specific" +
                        " requirement or stays within a certain limit. In addition, WordCounter shows you the top 10 keywords and keyword density of the" +
                        " article you're writing. This allows you to know which keywords you use how often and at what percentages. This can prevent you" +
                        " from over-using certain words or word combinations and check for best distribution of keywords in your writing. In the Details" +
                        " overview you can see the average speaking and reading time for your text, while Reading Level is an indicator of the education" +
                        " level a person would need in order to understand the words you’re using. Disclaimer: We strive to make our tools as accurate as" +
                        " possible but we cannot guarantee it will always be so.").WordCount() == 272);
        }
    }
}
