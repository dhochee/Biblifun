namespace Biblifun.Data
{
    /// <summary>
    /// Used to specify the selected language of the user.
    /// </summary>
    public interface ILanguageProvider
    {
        /// <summary>
        /// The ISO
        /// </summary>
        string Language { get; }
    }
}
