using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;

namespace LibrarySorter
{
    class CSorter
    {
        private Dictionary<string, int> sortTheList(string[] lines)
        {
            List<String> listOfWords = new List<string>();
            Dictionary<string, int> unsortedDictionary = new Dictionary<string, int>();

            for (int i = 0; i < lines.Count(); i++)
            {
                var line = lines[i];
                string[] buff = Regex.Split(line, @"\W").Where(s => s != string.Empty).ToArray<string>();
                foreach (string element in buff)
                {
                    listOfWords.Add(element.ToLower());
                }
            }

            foreach (string str in listOfWords)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    if (unsortedDictionary.Any(element => element.Key.Equals(str)))
                    {
                        int coutn = unsortedDictionary.First(element => element.Key.Equals(str)).Value + 1;
                        unsortedDictionary.Remove(str);
                        unsortedDictionary.Add(str, coutn);
                    }
                    else
                    {
                        unsortedDictionary.Add(str, 1);
                    }
                }
            }

            var sortedDictionary = unsortedDictionary.OrderBy(pair => pair.Key).OrderByDescending(pair => pair.Value).ToDictionary<KeyValuePair<string, int>, string, int>(pair => pair.Key, pair => pair.Value);

            return sortedDictionary;
        }

        public Dictionary<string, int> sortTheListParallel(string[] lines)
        {
            var stack = new ConcurrentStack<string>();
            var unsortedDictionary = new ConcurrentDictionary<string, int>();

            Parallel.For(0, lines.Count() - 1, i =>
            {
                var line = lines[i];
                string[] buff = Regex.Split(line, @"\W").Where(s => s != string.Empty).ToArray<string>();
                Parallel.ForEach(buff, element =>
                {
                    stack.Push(element.ToLower());
                });
            });

            Parallel.ForEach(stack, str =>
            {
                unsortedDictionary.AddOrUpdate(str, 1, (k, v) => v + 1);
            });

            var sortedDictionary = unsortedDictionary.OrderBy(pair => pair.Key).OrderByDescending(pair => pair.Value).ToDictionary<KeyValuePair<string, int>, string, int>(pair => pair.Key, pair => pair.Value);

            return sortedDictionary;
        }
    }
}