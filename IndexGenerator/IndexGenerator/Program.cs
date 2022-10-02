using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace IndexGenerator
{
    public enum IndexError { OK, NoSpace, TooLong }
    class IndexGenerator
    {
        private SortedDictionary<string, List<int>> index;
        private StringBuilder sb; 

        public IndexGenerator()
        {
            index = new SortedDictionary<string, List<int>>();
            sb = new StringBuilder(255);
        }

        public IndexError AddToIndex(string s)
        {
            int i = 0;
            StringBuilder sb = new StringBuilder(255);

            while(i < s.Length)
            {
                if (char.IsLetter(s[i]))
                {
                    sb.Append(s[i]);
                }
                else
                {
                    if(sb.Length > 1)
                    {
                        if (index.ContainsKey(sb.ToString().ToLower()) == false)
                        {
                            index.Add(sb.ToString().ToLower(), new List<int>(new int[] { i-sb.Length }));
                        }
                        else
                        {
                            index[sb.ToString().ToLower()].Add(i-sb.Length);
                        }
                    }
                    sb.Clear();
                }
                i++;
            }
            return IndexError.OK;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            
            foreach(KeyValuePair<string, List<int>> kv in index)
            {
                sb.Append(kv.Key.PadRight(40));
                sb.AppendLine(string.Join(",", kv.Value));
            }
            return sb.ToString();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string txt = File.ReadAllText("Text.txt");
            IndexGenerator mainIndex = new IndexGenerator();

            Stopwatch sw = Stopwatch.StartNew();
            mainIndex.AddToIndex(txt);
            sw.Stop();

            Console.WriteLine(mainIndex);
            Console.WriteLine($"Čas zpracování: {sw.ElapsedMilliseconds} ms.");
        }
    }
}
