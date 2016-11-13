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
      var set = ReadList(@"../../input.txt");
      var list = set.ToList();

      var cnt = 0;

      for (int i = -10000; i <= 10000; i++)
      {
        var res = TwoSum(set, list, i);
        if (res) { cnt++; }

        Console.WriteLine(i + " " + res);
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

    public static HashSet<long> ReadList(string fileName)
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
  }
}
