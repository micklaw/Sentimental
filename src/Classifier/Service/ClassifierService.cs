using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Classifier.Service.Abstract;

namespace Classifier.Service
{
    /// <summary>
    /// Singleton wrapper for the classifier service, not a nice pattern, but startup is (reasonably) expensive
    /// </summary>
    public class ClassifierService
    {
        public Classifier Create(decimal? delta)
        {
            var classifier = new Classifier(delta);

            if (classifier.Categories == null || !classifier.Categories.Any())
            {
                var assembly = Assembly.GetEntryAssembly();
                var categories = Assembly.GetEntryAssembly().GetManifestResourceNames();

                Action<string> getFromFile = (resource) =>
                {
                    var splits = resource.Split('.');
                    var category = splits[splits.Length - 2];

                    using (var reader = new StreamReader(assembly.GetManifestResourceStream(resource)))
                    {
                        classifier.TeachCategory(category, reader);
                    }
                };

                foreach (var category in categories)
                {
                    getFromFile(category);
                }
            }

            return classifier;
        }
    }
}
