using Biblifun.Common;
using Biblifun.Common.Test.Mocks;
using Biblifun.WebLookup;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Threading.Tasks;

namespace Biblifun.IntegrationTests
{
    public class SearchCountRetrieverTests
    {
        BibleBookProviderMock _bibleBookProviderMock;
        IVerseParser _verseParser;
        Mock<ILogger<SearchCountRetriever>> _loggerMock;

        [SetUp]
        public void SetupTest()
        {

            _bibleBookProviderMock = new BibleBookProviderMock();

            _verseParser = new VerseParser(_bibleBookProviderMock.Object);

            _loggerMock = new Mock<ILogger<SearchCountRetriever>>();
        }

        private SearchCountRetriever GetSearchCountRetriever()
        {
            return new SearchCountRetriever(_verseParser,
                                            _loggerMock.Object);
        }

        [TestCase("Matthew 24:14", 3000, 4000)]
        [TestCase("Matthew 24:11-12", 1100, 1300)]
        [TestCase("Jude 11", 100, 150)]
        public async Task GetSearchCountAsync_When_valid_verse_set_Then_returns_count(string verse, int expectedMin, int expectedMax)
        {
            // ARRANGE
            var searchCountRetriever = GetSearchCountRetriever();

            _verseParser.TryParseVerseString(verse, out VerseSetDescriptor verseSet);

            // ACT
            var count = await searchCountRetriever.GetSearchCountAsync(verseSet);

            // ASSERT
            count.ShouldNotBeNull();

            count.Value.ShouldBeGreaterThanOrEqualTo(expectedMin);

            count.Value.ShouldBeLessThanOrEqualTo(expectedMax);
        }
    }
}
