using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment5
{
  class Program
  {
    static void Main(string[] args)
    {
      var graph = Graph.ReadAdjWithLength("../../input.txt");

      foreach (var destId in new int[] { 7, 37, 59, 82, 99, 115, 133, 165, 188, 197 })
      {
        var sp = graph.CalcSortestPath(1, destId);
        Console.Write(sp + ",");
      }

      Console.ReadKey();
    }
  }
}
