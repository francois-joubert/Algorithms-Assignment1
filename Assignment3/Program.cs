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

      var minCut = CalcMinCut(input, 100);

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
      var minCntCnt = int.MaxValue;

      for (; itr > 0; itr--)
      {
        var inp = input.ToList();
        var lookup = new Dictionary<int, int>();
        var contr = 0;

        var r = new Random((int)DateTime.Now.Ticks);

        while (contr < inp.Count - 3)
        {
          var vertexA = r.Next(inp.Count - 1) + 1;

          var adjListA = inp[vertexA];
          if (adjListA == null) { continue; }

          Console.Write($"{vertexA}, ");

          var adjIdx = r.Next(adjListA.Length);
          var vertexB = adjListA[adjIdx];
          while (lookup.ContainsKey(vertexB))
          {
            vertexB = lookup[vertexB];
          }

          if (vertexB == vertexA) { continue; }

          var adjListB = inp[vertexB];
          inp[vertexA] = adjListA.Concat(adjListB).ToArray();
          inp[vertexB] = null;

          lookup.Add(vertexB, vertexA);

          foreach (var kv in lookup.Where(kv => kv.Value == vertexB).ToList())
          {
            lookup[kv.Key] = vertexA;
          }

          contr++;
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

        var currentMinCntCnt = 0;
        foreach (var p in inp[lastV1])
        {
          if (p != lastV1 && (p == lastV2 || lookup[p] == lastV2))
          {
            currentMinCntCnt++;
          }
          //else
          //{
          //  var p1 = p;
          //  while (lookup.ContainsKey(p1))
          //  {
          //    p1 = lookup[p1];
          //  }
          //  if (p1 == lastV2)
          //  {
          //    minCntCnt++;
          //  }
          //}
        }

        Console.WriteLine($"-> {currentMinCntCnt}");
        Console.WriteLine();

        if (currentMinCntCnt < minCntCnt)
        {
          minCntCnt = currentMinCntCnt;
        }
      }

      return minCntCnt;
    }
  }
}
