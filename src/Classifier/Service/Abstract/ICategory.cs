namespace Classifier.Service.Abstract
{
    /// <summary>
    /// Category methods.
    /// </summary>
    public interface ICategory
    {
        int MinWordLength { get; }

        /// <summary>
        /// Category Name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Reset all trained data
        /// </summary>
        void Reset();

        /// <summary>
        /// Gets a PhraseCount for Phrase or 0 if phrase not present
        /// </summary>
        int GetPhraseCount(string phrase);

        /// <summary>
        /// Trains this Category from a file
        /// </summary>
        void TeachCategory(System.IO.TextReader reader);

        /// <summary>
        /// Trains this Category from a word or phrase
        /// </summary>
        /// 
        void TeachPhrase(string rawPhrase);

        /// <summary>
        /// Trains this Category from a word or phrase array
        /// </summary>
        void TeachPhrases(string[] words);

        /// <value>
        /// Gets total number of word occurences in this category
        /// </value>
        int TotalWords { get; }
    }
}