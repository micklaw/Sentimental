using System;
using System.Collections.Generic;
using System.Linq;

namespace Classifier.Service
{
    public class ClassifierResult
    {
        public decimal? _delta { get; set; }
        public readonly Dictionary<string, double> Results;

        public ClassifierResult(decimal? delta, Dictionary<string, double> results)
        {
            _delta = delta;
            Results = results;
        }

        public string Reckons
        {
            get
            {
                var mibbies = string.Empty;

                var ordered = Results.OrderByDescending(i => Math.Abs(i.Value));
                var furthestCategory = ordered.First();
                var closestCategory = ordered.Last();

                var max = Math.Abs((decimal)furthestCategory.Value);
                var min = Math.Abs((decimal)closestCategory.Value);

                if (decimal.Equals(min, max))
                {
                    return "Lord knows! This could be either or... sorry.";
                }

                if (_delta.HasValue && _delta > 0)
                {
                    if (Math.Abs((decimal)furthestCategory.Value - (decimal)closestCategory.Value) < _delta)
                    {
                        mibbies = "ever so slightly ";
                    }
                }

                return $"I'd reckon this is {mibbies}more {closestCategory.Key} than {furthestCategory.Key}.";
            }
        }
    }
}
