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

      int[] sorted;
      var res = SortAndCountInv(input, out sorted);

      Console.WriteLine(res);

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

    public static long SortAndCountInv(int[] input, out int[] sorted)
    {
      var l = input.Length;

      if (l == 1)
      {
        sorted = input;
        return 0;
      }

      int h1 = l / 2;

      var sub1 = input.Take(h1).ToArray();
      var sub2 = input.Skip(h1).ToArray();
      int[] sorted1, sorted2;

      var n1 = SortAndCountInv(sub1, out sorted1);
      var n2 = SortAndCountInv(sub2, out sorted2);

      var split = CountSplitInv(sorted1, sorted2, out sorted);

      return n1 + n2 + split;
    }

    public static long CountSplitInv(int[] sortedInput1, int[] sortedInput2, out int[] merged)
    {
      int i = 0, j = 0;
      var res = new List<int>();
      var l1 = sortedInput1.Length;
      var l2 = sortedInput2.Length;
      long count = 0;

      for (int k = 0; k < l1 + l2; k++)
      {
        if (i == l1)
        {
          res.AddRange(sortedInput2.Skip(j));
          break;
        }
        else if (j == l2)
        {
          res.AddRange(sortedInput1.Skip(i));
          break;
        }

        if (sortedInput1[i] < sortedInput2[j])
        {
          res.Add(sortedInput1[i]);
          i++;
        }
        else
        {
          res.Add(sortedInput2[j]);
          count += l1 - i;
          j++;
        }
      }
      merged = res.ToArray();

      return count;
    }
  }
}
