namespace Biblifun.Data
{
    public interface ILanguageSettingsProvider
    {
        LanguageSettings GetLanguageSettings(string language);
    }
}