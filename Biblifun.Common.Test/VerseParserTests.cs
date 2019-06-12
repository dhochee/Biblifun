using Biblifun.Common.Test.Mocks;
using NUnit.Framework;
using Shouldly;

namespace Biblifun.Common.Test
{
    [TestFixture]
    public class VerseParserTests
    {
        private const int BOOK_ID_MATTHEW = 40;
        private const int BOOK_ID_1TIMOTHY = 54;
        private const int BOOK_ID_JUDE = 65;

        private BibleBookProviderMock _bibleBookProviderMock;

        private VerseParser _verseParser;

        [SetUp]
        public void Setup()
        {
            _bibleBookProviderMock = new BibleBookProviderMock();

            _verseParser = new VerseParser(_bibleBookProviderMock.Object);
        }

        [TestCase("40024013014","Matthew 24:13,14")]
        [TestCase("40024013015", "Matthew 24:13-15")]
        [TestCase("65001012", "Jude 12")]
        [TestCase("54001001020", "1 Timothy 1:1-20")]
        public void GetVerseDisplayText_When_valid_Then_returns_display_text(string input, string expected)
        {
            // ARRANGE+ACT
            var actual = _verseParser.GetVerseDisplayText(input);

            // ASSERT
            actual.ShouldBe(expected);
        }

        [Test]
        public void TryParseVerseString_When_single_verse_Then_success()
        {
            // Arrange
            var testInput = "Matthew 24:14";

            // Act
            var result = _verseParser.TryParseVerseString(testInput, out IVerseSetId output);

            // Assert
            result.ShouldBe(VerseParseResult.Success);

            output.BookId.ShouldBe(BOOK_ID_MATTHEW);
            output.Chapter.ShouldBe(24);
            output.Start.ShouldBe(14);
            output.End.ShouldBe(14);
        }

        [Test]
        public void TryParseVerseString_When_comma_separated_verses_Then_success()
        {
            // Arrange
            var testInput = "Matthew 24:13,14";

            // Act
            var result = _verseParser.TryParseVerseString(testInput, out IVerseSetId output);

            // Assert
            result.ShouldBe(VerseParseResult.Success);

            output.BookId.ShouldBe(BOOK_ID_MATTHEW);
            output.Chapter.ShouldBe(24);
            output.Start.ShouldBe(13);
            output.End.ShouldBe(14);
        }

        [Test]
        public void TryParseVerseString_When_hyphen_separated_verses_Then_success()
        {
            // Arrange
            var testInput = "Matthew 24:12-14";

            // Act
            var result = _verseParser.TryParseVerseString(testInput, out IVerseSetId output);

            // Assert
            result.ShouldBe(VerseParseResult.Success);

            output.BookId.ShouldBe(BOOK_ID_MATTHEW);
            output.Chapter.ShouldBe(24);
            output.Start.ShouldBe(12);
            output.End.ShouldBe(14);
        }

        [Test]
        public void TryParseVerseString_When_book_contains_number_Then_success()
        {
            // Arrange
            var testInput = "1 Tim 4:12-14";

            // Act
            var result = _verseParser.TryParseVerseString(testInput, out IVerseSetId output);

            // Assert
            result.ShouldBe(VerseParseResult.Success);

            output.BookId.ShouldBe(BOOK_ID_1TIMOTHY);
            output.Chapter.ShouldBe(4);
            output.Start.ShouldBe(12);
            output.End.ShouldBe(14);
        }

        [Test]
        public void TryParseVerseString_When_single_chapter_book__Then_success()
        {
            // Arrange
            var testInput = "Jude 11,12";

            // Act
            var result = _verseParser.TryParseVerseString(testInput, out IVerseSetId output);

            // Assert
            result.ShouldBe(VerseParseResult.Success);

            output.BookId.ShouldBe(BOOK_ID_JUDE);
            output.Chapter.ShouldBe(1);
            output.Start.ShouldBe(11);
            output.End.ShouldBe(12);
        }

        [TestCase("Jude 11,13")]
        [TestCase("Matthew 24:10,13")]
        [TestCase("1 Tim 3:11,13")]
        [TestCase("1 Tim 3:11,10")]
        public void TryParseVerseString_When_commas_separated_verses_are_not_sequential_Then_invalid_syntax(string testInput)
        {
            // Arrange + Act
            var result = _verseParser.TryParseVerseString(testInput, out IVerseSetId output);

            // Assert
            result.ShouldBe(VerseParseResult.InvalidSyntax);
            output.ShouldBeNull();
        }

        [TestCase("Jude 41")]
        [TestCase("1 Tim 3:67")]
        [TestCase("Matthew 29:1")]
        [TestCase("Matthew 24:1-70")]
        [TestCase("Matthew 24:30,31")]
        public void TryParseVerseString_When_chapter_or_verse_are_out_of_range_Then_invalid_verse(string testInput)
        {
            // Arrange + Act
            var result = _verseParser.TryParseVerseString(testInput, out IVerseSetId output);

            // Assert
            result.ShouldBe(VerseParseResult.InvalidVerse);
            output.ShouldBeNull();
        }

        [TestCase("Matthew24:14")]
        [TestCase("1 Timothy3:1-2")]
        public void TryParseVerseString_When_book_name_not_followed_by_space_Then_invalid_verse(string testInput)
        {
            // Arrange + Act
            var result = _verseParser.TryParseVerseString(testInput, out IVerseSetId output);

            // Assert
            result.ShouldBe(VerseParseResult.InvalidVerse);
            output.ShouldBeNull();
        }

        [TestCase("Matthea 24:14")]
        [TestCase("Judeas 1-2")]
        public void TryParseVerseString_When_book_name_not_found_Then_invalid_verse(string testInput)
        {
            // Arrange + Act
            var result = _verseParser.TryParseVerseString(testInput, out IVerseSetId output);

            // Assert
            result.ShouldBe(VerseParseResult.InvalidVerse);
            output.ShouldBeNull();
        }

    }
}