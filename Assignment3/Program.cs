using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3
{
  class Program
  {
    static void Main(string[] args)
    {
      var input = ReadAdj("input.txt");
      //var input = Read("inputTest.txt");

      var minCut = CalcMinCut(input, 1);

      //input.ToList().ForEach(x => Console.WriteLine(x));

      Console.WriteLine("=============================");
      Console.WriteLine(minCut);

      Console.ReadLine();
    }

    public static int[][] ReadAdj(string fileName)
    {
      var f = File.OpenText(fileName);
      var list = new List<int[]> { null };

      while (true)
      {
        var ln = f.ReadLine();
        if (ln == null)
        {
          break;
        }

        var vals = ln.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries)
          .ToList()
          .Select(x => int.Parse(x));

        list.Add(vals.Skip(1).ToArray());
      }

      return list.ToArray();
    }

    public static int CalcMinCut(int[][] input, int itr)
    {
      var inp = input.ToList();
      var lookup = new Dictionary<int, int>();
      var contr = 0;

      for (; itr > 0; itr--)
      {
        while (contr < inp.Count - 3)
        {
          var r = new Random((int)DateTime.Now.Ticks);

          var rvi = r.Next(inp.Count) + 1;
          var v1 = inp[rvi];
          if (v1 == null)
          {
            continue;
          }

          var rei = r.Next(v1.Length);
          var avi = v1[rei];
          if (lookup.ContainsKey(avi))
          {
            avi = lookup[avi];
          }

          var v2 = inp[avi];
          inp[rvi] = v1.Concat(v2).ToArray();
          inp[avi] = null;
          lookup.Add(avi, rvi);

          contr++;
        }
      }

      var lastV1 = 0;
      var lastV2 = 0;
      for (int i = 1; i <= inp.Count; i++)
      {
        if (inp[i] != null)
        {
          if (lastV1 == 0)
          {
            lastV1 = i;
          }
          else
          {
            lastV2 = i;
            break;
          }
        }
      }

      var minCntCnt = 0;
      foreach (var p in inp[lastV1])
      {
        if (p == lastV2 || lookup[p] == lastV2)
        {
          minCntCnt++;
        }
      }

      return minCntCnt;
    }
  }
}
