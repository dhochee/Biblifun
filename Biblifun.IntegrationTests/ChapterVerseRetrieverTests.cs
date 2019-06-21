using Biblifun.Data;
using Biblifun.WebLookup;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Biblifun.IntegrationTests
{
    public class ChapterVerseRetrieverTests
    {
        [TestCase(1, 1, 31)]
        public void RetrieveChapterDetails_When_valid_Then_returns_correct_count(int bookId, int chapter, int expectedVerseCount)
        {
            var detailRetriever = new BibleMetadataBuilder();

            detailRetriever.LoadVerseCountByChapter();

            var chapterDetails = detailRetriever.RetrieveChapterDetails(1, chapter);

            chapterDetails.ShouldNotBeNull();

            chapterDetails.TotalVerses.ShouldBe(expectedVerseCount);
        }

        [Test]
        public void GetBibleMetadata_Returns_complete_data()
        {
            var detailRetriever = new BibleMetadataBuilder();

            var metaData = detailRetriever.GetBibleMetadata();

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };

            var metaDataJson = JsonConvert.SerializeObject(metaData, settings);

            File.WriteAllText(@".\BibleMetaDataJSON\BibleMetaData.json", metaDataJson);
        }
    }
}
