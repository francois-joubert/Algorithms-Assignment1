using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
  class Program
  {
    static void Main(string[] args)
    {
      var input = Read("input.txt");
      //var input = Read("inputTest.txt");

      var cmpCnt = SortAndCountCmp(ref input, 0, input.Length);

      input.ToList().ForEach(x => Console.WriteLine(x));

      Console.WriteLine("=============================");
      Console.WriteLine(cmpCnt);

      Console.ReadLine();
    }


    public static int[] Read(string fileName)
    {
      var f = File.OpenText(fileName);
      var input = new List<int>();

      while (true)
      {
        var ln = f.ReadLine();
        if (ln == null)
        {
          break;
        }

        input.Add(int.Parse(ln));
      }

      return input.ToArray();
    }

    public static int SortAndCountCmp(ref int[] input, int a, int b)
    {
      if (b <= a + 1)
      {
        return 0;
      }

      int p, cmpCnt;
      PartitionAndCountCmp(ref input, a, b, out p, out cmpCnt);

      var cmpCntA = SortAndCountCmp(ref input, a, p);
      var cmpCntB = SortAndCountCmp(ref input, p + 1, b);

      return cmpCnt + cmpCntA + cmpCntB;
    }

    public static void PartitionAndCountCmp(ref int[] input, int a, int b, out int p, out int cmpCnt)
    {
      // No swap for first.
      // Swap element with first for any other pivot choice.

      input = input.Swap(a, b - 1); // use last for pivot

      //var mIdx = FindMedianIndex(input, a, b - 1);
      //input = input.Swap(a, mIdx); // use median for pivot

      p = a;

      // swap pivot to 1st

      for (int i = a + 1; i < b; i++)
      {
        if (input[i] < input[a])
        {
          p++;
          input = input.Swap(p, i);
        }
      }

      input = input.Swap(p, a);

      cmpCnt = b - a - 1;
    }

    public static int FindMedianIndex(int[] input, int a, int b)
    {
      if (b == a + 1)
      {
        return a;
      }

      int mid = (int)Math.Floor((a + b) / 2.0);

      if (input[mid] > input[a] && input[mid] < input[b])
      {
        return mid;
      }
      else if (input[a] > input[mid] && input[a] < input[b])
      {
        return a;
      }
      else
      {
        return b;
      }
    }
  }

  public static class Ext
  {
    public static int[] Swap(this int[] array, int a, int b)
    {
      var x = array[a];
      array[a] = array[b];
      array[b] = x;

      return array;
    }
  }
}

//162085
//164123
//160133,145139,145139,
