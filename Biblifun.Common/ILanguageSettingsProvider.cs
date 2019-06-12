namespace Biblifun.Common
{
    public interface ILanguageSettingsProvider
    {
        LanguageSettings GetLanguageSettings(string language);
    }
}