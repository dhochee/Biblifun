using Biblifun.Data.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Biblifun.Data.Test.Mocks
{
    public class BibleBookProviderMock : Mock<IBibleBookProvider>
    {
        private List<BibleBook> _allBooks;
        
        public BibleBookProviderMock()
        {

            _allBooks = (new BibleMetadataBuilder()).LoadMetaDataFromFile();

            this.Setup(b => b.BibleBooks).Returns(_allBooks);

            this.Setup(b => b.GetBookById(It.IsAny<int>()))
                .Returns((int bookId) => _allBooks.FirstOrDefault(b => b.BibleBookId == bookId));
        }
    }
}
