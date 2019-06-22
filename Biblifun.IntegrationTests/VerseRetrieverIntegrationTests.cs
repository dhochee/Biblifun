using Biblifun.Data;
using Biblifun.Data.Test.Mocks;
using Biblifun.WebLookup;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Threading.Tasks;

namespace Biblifun.IntegrationTests
{
    public class VerseRetrieverIntegrationTests
    {
        private const string ENGLISH = "en";
        private const string SPANISH = "es";

        ILanguageSettingsProvider _languageSettingsProvider;
        BibleBookProviderMock _bibleBookProviderMock;
        IVerseParser _verseParser;
        Mock<ILogger<VerseRetriever>> _loggerMock;

        [SetUp]
        public void SetupTest()
        {
            _languageSettingsProvider = new LanguageSettingsProvider();

            _bibleBookProviderMock = new BibleBookProviderMock();

            _verseParser = new VerseParser(_bibleBookProviderMock.Object);

            _loggerMock = new Mock<ILogger<VerseRetriever>>();
        }

        private VerseRetriever GetVerseRetriever()
        {
            return new VerseRetriever(_languageSettingsProvider,
                                      _verseParser,
                                      _loggerMock.Object);
        }

        [TestCase("Matthew 24:14", "And this good news of the Kingdom will be preached")]
        [TestCase("Matthew 24:13,14", "the one who has endured to the end")]
        [TestCase("Matthew 24:2-4", "the sign of your presence and of the conclusion of the system of things?")]
        [TestCase("Jude 11", "for they have followed the path of Cain and have rushed")]
        public async Task GetVerseHtml_When_valid_single_verse_Then_returns_html(string verse, string expectedText)
        {
            // ARRANGE
            var verseRetriever = GetVerseRetriever();

            _verseParser.TryParseVerseString(verse, "en", out VerseSetDescriptor verseSet);

            // ACT
            var html = await verseRetriever.GetVerseHtmlAsync(verseSet, ENGLISH);

            // ASSERT
            html.ShouldNotBeNull();

            html.ShouldContain(expectedText);
        }

        [TestCase("Mateo 24:14", "Y estas buenas nuevas del reino se predicarán en toda la tierra")]
        public async Task GetVerseHtml_When_language_spanish_Then_returns_expected_html(string verse, string expectedText)
        {
            // ARRANGE
            _bibleBookProviderMock = new BibleBookProviderMock();
            _verseParser = new VerseParser(_bibleBookProviderMock.Object);

            var verseRetriever = GetVerseRetriever();

            _verseParser.TryParseVerseString(verse, SPANISH, out VerseSetDescriptor verseSet);

            // ACT
            var html = await verseRetriever.GetVerseHtmlAsync(verseSet, SPANISH);

            // ASSERT
            html.ShouldNotBeNull();

            html.ShouldContain(expectedText);
        }
    }
}
