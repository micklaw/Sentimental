using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Classifier.Service.Abstract;
using System.Linq;

namespace Classifier.Service
{
	/// <summary>
	/// Naive Bayesian classifier
	/// </summary>
	/// <remarks>
	/// It suppports exclusion of words but not Phrases 
	/// </remarks>
	public class Classifier : IClassifier
	{
	    private readonly decimal? _delta;
		public SortedDictionary<string, ICategory> Categories { get; set; }
		private readonly ExcludedWords _excludeWords;
		
		public Classifier(decimal? delta = null)
		{
			Categories = new SortedDictionary<string, ICategory>();
			_excludeWords = new ExcludedWords(4);
			_excludeWords.InitDefault();
		    _delta = delta;
		}

		/// <summary>
		/// Gets total number of word occurences over all categories
		/// </summary>
		private int CountTotalWordsInCategories()
		{
			var total = 0;

			foreach (var cat in Categories.Values)
			{
				total += cat.TotalWords;
			}

			return total;
		}

		/// <summary>
		/// Gets or creates a category
		/// </summary>
		ICategory GetOrCreateCategory(string categoryName)
		{
			ICategory category;

			if (!Categories.TryGetValue(categoryName, out category))
			{
				category = new Category(categoryName, _excludeWords);
				Categories.Add(categoryName, category);
			}

			return category;
		}

		/// <summary>
		/// Trains this Category from a word or phrase
		/// </summary>
		public void TeachPhrases(string cat, string[] phrases)
		{
			GetOrCreateCategory(cat).TeachPhrases(phrases);
		}

		/// <summary>
		/// Trains this Category from a word or phrase
		/// </summary>
		public void TeachCategory(string cat, System.IO.TextReader tr)
		{
			GetOrCreateCategory(cat).TeachCategory(tr);
		}

        /// <summary>
        /// Classifies a text
        /// </summary>
        /// <returns>
        /// returns classification values for the text, the higher, the better is the match.
        /// </returns>
        public ClassifierResult Classify(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text ?? string.Empty);

            using (var stream = new MemoryStream(bytes))
            {
                return Classify(new StreamReader(stream));
            }
        }

        /// <summary>
        /// Classifies a text
        /// </summary>
        /// <returns>
        /// returns classification values for the text, the higher, the better is the match.</returns>
        public ClassifierResult Classify(StreamReader reader)
		{
			var score = Categories.ToDictionary(cat => cat.Value.Name, cat => 0.0);

            var words = new EnumerableCategory("", _excludeWords);

			words.TeachCategory(reader);

			foreach (var wordPair in words)
			{
				var phraseCount = wordPair.Value;

			    foreach (var categoryPair in Categories)
			    {
			        var cat = categoryPair.Value;

			        if (phraseCount.RawPhrase.Length > cat.MinWordLength)
			        {
			            var count = cat.GetPhraseCount(phraseCount.RawPhrase);

			            if (0 < count)
			            {
			                score[cat.Name] += System.Math.Log((double) count/(double) cat.TotalWords);
			            }
			            else
			            {
			                score[cat.Name] += System.Math.Log(0.01/(double) cat.TotalWords);
			            }
			        }
			    }
			}

			foreach (var kvp in Categories)
			{
				var cat = kvp.Value;
				score[cat.Name] += System.Math.Log((double)cat.TotalWords / (double)this.CountTotalWordsInCategories());
			}

            // [ML] - If there is a delta greater than 0 add an unconfirmed category if min - max falls in this range

            if (_delta.HasValue && _delta > 0)
            {
                var min = score.Min(i => i.Value);
                var max = score.Max(i => i.Value);

                if ((decimal)(max - min) < _delta)
                {
                    //return new Dictionary<string, double>() { { "unconfirmed", 0.00 } };
                }
            }

            return new ClassifierResult(_delta, score);
		}
	}
}
