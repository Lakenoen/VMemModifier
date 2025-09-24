using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;

namespace VMemReaderCore;
public class SearchAlgorithms
{
    private static readonly short ALFSIZE = 256;
    public static HashSet<long> bmSearch(in List<byte> text, in List<byte> pattern)
    {
        HashSet<long> results = new HashSet<long>();
        try
        {
            if (pattern.Count == 0)
                return results;

            int[] map = new int[ALFSIZE];

            for (int i = 0; i < ALFSIZE; ++i)
                map[i] = -1;

            for (int i = 0; i < pattern.Count; ++i)
                map[pattern[i]] = i;

            int shift = 0;

            while (shift <= (text.Count - pattern.Count))
            {
                int j = pattern.Count - 1;

                while (j >= 0 && pattern[j] == text[shift + j])
                    --j;

                if (j < 0)
                {
                    results.Add((long)shift);
                    shift += (shift + pattern.Count < text.Count) ? pattern.Count - map[text[shift + pattern.Count]] : 1;
                }
                else
                {
                    shift += int.Max(1, map[text[shift + j]]);
                }

            }
        } catch (Exception e)
        {
            Log.Instance.Factory.CreateLogger<SearchAlgorithms>().LogDebug(e.Message, e.StackTrace);
            throw;
        }

        return results;
    }

    public static HashSet<long> bmSearchRegEx(in List<byte> text, in List<byte> pattern)
    {
        try
        {
            string textStr = Encoding.ASCII.GetString(text.ToArray());
            string patternStr = Encoding.ASCII.GetString(pattern.ToArray());
            Regex regex = new Regex(patternStr);
            return regex.Matches(textStr).Cast<Match>().ToList().Select((Match m) => (long)m.Index).ToHashSet();
        }
        catch (Exception ex)
        {
            Log.Instance.Factory.CreateLogger<SearchAlgorithms>().LogDebug(ex.Message);
            throw;
        }
    }

    public delegate HashSet<long> Search(in List<byte> text, in List<byte> pattern);

}
