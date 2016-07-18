using System.Collections.Generic;

namespace Classifier.Service.Abstract
{
	/// <summary>
	/// Classifier methods.
	/// </summary>
	public interface IClassifier
	{
        /// <summary>
        /// Classify a peice of string from text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
	    ClassifierResult Classify(string text);

        /// <summary>
        /// A list of categories for classifying too
        /// </summary>
        SortedDictionary<string, ICategory> Categories { get; set; }

        /// <summary>
        /// Trains this Category from a word or phrase
        /// </summary>
        void TeachPhrases(string cat, string[] phrases);

		/// <summary>
		/// Trains this Category from a word or phrase
		/// </summary>
		void TeachCategory(string cat, System.IO.TextReader tr);

        /// <summary>
        /// Classifies a text
        /// </summary>
        /// <returns>
        /// returns classification values for the text, the higher, the better is the match.</returns>
        ClassifierResult Classify(System.IO.StreamReader reader);
	}
}
