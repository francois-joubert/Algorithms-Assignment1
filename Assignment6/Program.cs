using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment6
{
  class Program
  {
    static void Main(string[] args)
    {
      var list = ReadList(@"../../inputMedian.txt");
      var res = ComputeMedianSum(list);

      Console.WriteLine(res);

      Console.ReadKey();
    }

    public static void ComputeTwoSum()
    {
      var set = ReadSet(@"../../input.txt");
      var list = set.ToList();

      var cnt = 0;

      for (int i = -10000; i <= 10000; i++)
      {
        var res = TwoSum(set, list, i);
        if (res) { cnt++; }

        Console.WriteLine(i + " " + res);    // 427
      }

      Console.WriteLine(cnt);

      Console.ReadKey();
    }

    public static bool TwoSum(HashSet<long> set, List<long> list, int target)
    {
      for (int i = 0; i < set.Count; i++)
      {
        if (set.Contains(target - list[i]))
        {
          return true;
        }
      }

      return false;
    }

    public static HashSet<long> ReadSet(string fileName)
    {
      var f = File.OpenText(fileName);
      var list = new HashSet<long>();

      while (true)
      {
        var ln = f.ReadLine();
        if (ln == null)
        {
          break;
        }

        list.Add(long.Parse(ln));
      }

      return list;
    }

    public static int ComputeMedianSum(List<long> list)
    {
      var sortedList = new SortedSet<long>();
      long medianSum = 0;

      for (int k = 1; k <= list.Count; k++)
      {
        sortedList.Add(list[k - 1]);
        var index = 0;
        if (k % 2 == 0)
        {
          index = k / 2;
        }
        else
        {
          index = (k + 1) / 2;
        }

        //if (k == 0) { index = 0; }

        var m = sortedList.ElementAt(index - 1);

        medianSum += m;
      }

      return (int)(medianSum % 10000);
    }

    public static List<long> ReadList(string fileName)
    {
      var f = File.OpenText(fileName);
      var list = new List<long>();

      while (true)
      {
        var ln = f.ReadLine();
        if (ln == null)
        {
          break;
        }

        list.Add(long.Parse(ln));
      }

      return list;
    }
  }
}
