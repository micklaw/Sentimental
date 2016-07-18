using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Classifier.Service;

namespace Classifier
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var resourceNames = Assembly.GetEntryAssembly().GetManifestResourceNames();

            Console.WriteLine("** Getting Data Resources **");

            foreach (var name in resourceNames)
            {
                Console.WriteLine(name);
            }

            var quit = false;

            Console.WriteLine("\r\n** Initialising Classifier, this could take a little time (patience) **");

            var classifier = new ClassifierService().Create(0.15M);

            Console.WriteLine("** Classifier loaded **");

            while (!quit)
            {
                Console.WriteLine("\r\nRant here please:");

                var message = Console.ReadLine();

                if (message.ToLower().Equals("quit"))
                {
                    quit = true;
                    continue;
                }

                var results = classifier.Classify(message);

                foreach (var key in results.Results.Keys)
                {
                    Console.WriteLine("Key: {0}, Value: {1}", key, results.Results[key]);
                }

                Console.WriteLine(results.Reckons + "\r\n");
            }
        }
    }
}
