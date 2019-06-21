using System;
using System.Collections.Generic;
using System.Text;

namespace Biblifun.Data
{
    public class LanguageSettings
    {
        public string ScriptureLookupUrlTemplate { get; set; }

        public string BibleMetaDataJsonUrl { get; set; }
    }

    public class LanguageSettingsProvider : ILanguageSettingsProvider
    {
        public LanguageSettings GetLanguageSettings(string language)
        {
            var result = new LanguageSettings();

            // TODO: Lookup in the DB

            switch (language)
            {
                case "en":

                    result.ScriptureLookupUrlTemplate = 
                        "https://wol.jw.org/en/wol/bl/r1/lp-e?q={book}%20{chapter}{verseStart}{verseEnd}";

                    result.BibleMetaDataJsonUrl = "https://www.jw.org/en/publications/bible/study-bible/books/json/data";

                    break;

                case "es":
                    result.ScriptureLookupUrlTemplate =
                        "https://wol.jw.org/es/wol/bl/r4/lp-s?q={book}%20{chapter}{verseStart}{verseEnd}";

                    result.BibleMetaDataJsonUrl = "https://www.jw.org/es/publicaciones/biblia/bi12/libros/json/data";

                    break;
            }

            return result;
        }
    }
}
