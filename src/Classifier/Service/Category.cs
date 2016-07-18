using System.Collections.Generic;
using System.Text.RegularExpressions;
using Classifier.Service.Abstract;

namespace Classifier.Service
{
    /// <summary>
	/// Represents a Bayesian category - that is contains a list of phrases with their occurence counts 
	/// </summary>
	public class Category : ICategory
	{
		protected System.Collections.Generic.SortedDictionary<string, PhraseCount> _phrases;
		private int _totalWords;
		private readonly string _name;
		private readonly ExcludedWords _excludedWords;

        /// <summary>
        /// The minimum words as defined by the exclude words
        /// </summary>
	    public int MinWordLength => _excludedWords.MinWordLength;

        /// <summary>
        /// The name of the category
        /// </summary>
	    public string Name => _name;

        /// <value>
		/// Gets total number of word occurences in this category
		/// </value>
		public int TotalWords => _totalWords;

        /// <summary>
        /// Phrases with score in the category
        /// </summary>
        private SortedDictionary<string, PhraseCount> Phrases => _phrases;

        public Category(string cat, ExcludedWords excludedWords)
		{
			_phrases = new SortedDictionary<string, PhraseCount>();
			_excludedWords = excludedWords;
			_name = cat;
		}

		/// <summary>
		/// Gets a Count for Phrase or 0 if not present
		/// </summary>
		public int GetPhraseCount(string phrase)
		{
			PhraseCount phraseCount;

		    if (_phrases.TryGetValue(phrase, out phraseCount))
		    {
		        return phraseCount.Count;
		    }

		    return 0;
		}

		/// <summary>
		/// Reset all trained data
		/// </summary>
		public void Reset()
		{
			_totalWords = 0;
			_phrases.Clear();
		}

        /// <summary>
		/// Trains this Category from a file
		/// </summary>
		public void TeachCategory(System.IO.TextReader reader)
		{
			var re = new Regex(@"(\w+)\W*", RegexOptions.Compiled);

			string line;
			while (null != (line = reader.ReadLine()))
			{
				var m = re.Match(line);
				while (m.Success)
				{
					var word = m.Groups[1].Value;

					TeachPhrase(word);
					m = m.NextMatch();
				}
			}
		}

		/// <summary>
		/// Trains this Category from a word or phrase array
		/// </summary>
		/// <seealso cref="DePhrase(string)">
		/// See DePhrase </seealso>
		public void TeachPhrases(string[] words)
		{
			foreach (var word in words)
			{
				TeachPhrase(word);
			}
		}

		/// <summary>
		/// Trains this Category from a word or phrase
		/// </summary>
		public void TeachPhrase(string rawPhrase)
		{
			if ((null != _excludedWords) && (_excludedWords.IsExcluded(rawPhrase)) && rawPhrase.Length > _excludedWords.MinWordLength)
				return;

			PhraseCount phraseCount;
			var phrase = DePhrase(rawPhrase);

			if (!_phrases.TryGetValue(phrase, out phraseCount))
			{
				phraseCount = new PhraseCount(rawPhrase);
				_phrases.Add(phrase, phraseCount);
			}

			phraseCount.Count++;
			_totalWords++;
		}

		private static readonly Regex _phraseRegex = new Regex(@"\W", RegexOptions.Compiled);

		/// <summary>
		/// Checks if a string is a phrase (that is a string with whitespaces)
		/// </summary>
		/// <returns>
		/// true or false</returns>
		public static bool CheckIsPhrase(string s)
		{
			return _phraseRegex.IsMatch(s);
		}

		/// <summary>
		/// Trnasforms a string into a phrase (that is a string with whitespaces)
		/// </summary>
		/// <returns>
		/// dephrased string</returns>
		/// <remarks>
		/// if something like "lone Rhino" is considered a sinlge Phrase, then our word matching algorithm is 
		/// is transforming it into a single Word "lone Rhino" -> "loneRhino"
		/// Currently this feature is not used!
		/// </remarks>
		public static string DePhrase(string s)
		{
			return _phraseRegex.Replace(s, @"");
		}
	}
}
