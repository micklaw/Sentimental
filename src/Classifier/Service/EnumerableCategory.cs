using System.Collections;
using System.Collections.Generic;

namespace Classifier.Service
{
    /// <summary>
    /// Represents a Enumerable Bayesian category - that is contains a list of phrases with their occurence counts 
    /// </summary>
    internal class EnumerableCategory : Category, IEnumerable<KeyValuePair<string, PhraseCount>>
    {
        public EnumerableCategory(string cat, ExcludedWords excludedWords) : base(cat, excludedWords)
        {
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public IEnumerator<KeyValuePair<string, PhraseCount>> GetEnumerator()
        {
            return _phrases.GetEnumerator();
        }

    }
}