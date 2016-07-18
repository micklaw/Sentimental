using System.Collections.Generic;

namespace Classifier.Service
{
	/// <summary>
	/// serves to exclude certain words from the bayesian classification</summary>
	public class ExcludedWords  
	{
		/// <summary>
		/// List of english words i'm not interested in</summary>
		/// <remarks>
		/// You might use frequently used words for this list
		/// </remarks>
		private static readonly string[] _common =
		{
			 "the", 
			 "to", 
			 "and", 
			 "a", 
			 "an", 
			 "in", 
			 "is", 
			 "it", 
			 "you", 
			 "that", 
			 "was", 
			 "for", 
			 "on", 
			 "are", 
			 "with", 
			 "as", 
			 "be", 
			 "been", 
			 "at", 
			 "one", 
			 "have", 
			 "this", 
			 "what", 
			 "which", 
		};

	    public int MinWordLength = 3;

		private readonly Dictionary<string, int> _stopDictionary;

		public ExcludedWords()
		{
			_stopDictionary = new Dictionary<string, int>();
		}

        public ExcludedWords(int minWordLength) : this()
        {
            MinWordLength = minWordLength;
        }

        /// <summary>
        /// Initializes for english</summary>
        public void InitDefault()
		{
			Init(_common);
		}
		public void Init(string[] excluded)
		{
			_stopDictionary.Clear();

			for (var i = 0; i < excluded.Length; i++)
			{
				_stopDictionary.Add(excluded[i], i);
			}
		}
		/// <summary>
		/// checks to see if a word is to be excluded</summary>
		public bool IsExcluded(string word)
		{
			return word.Length <= MinWordLength || _stopDictionary.ContainsKey(word);
		}

	}
}
