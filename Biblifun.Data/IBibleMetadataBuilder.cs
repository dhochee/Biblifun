using System.Collections.Generic;
using Biblifun.Data.Models;

namespace Biblifun.Data
{
    public interface IBibleMetadataBuilder
    {
        List<BibleBook> LoadMetaDataFromFile();
        void SaveMetadataToFile();
    }
}