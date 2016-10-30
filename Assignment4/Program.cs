using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment4
{
  class Program
  {
    static void Main(string[] args)
    {
      var graph = ReadEdges("inputTest2.txt");
      List<int> list = null;

      var t = new Thread(() => {
        list = graph.FindScc();
      }, 10000000);

      t.Start();
      t.Join();

      foreach (var size in list)
      {
        Console.Write(size + ",");
      }

      Console.ReadKey();
    }

    public static Graph ReadEdges(string fileName)
    {
      var f = File.OpenText(fileName);
      var graph = new Graph();

      while (true)
      {
        var ln = f.ReadLine();
        if (ln == null)
        {
          break;
        }

        var vals = ln.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries)
          .ToList()
          .Select(x => int.Parse(x))
          .ToArray();

        graph.Add(vals[0] - 1, vals[1] - 1);
      }

      return graph;
    }
  }

  public class Graph
  {
    private Dictionary<int, Vertex> _VertexList { get; set; }
    private Dictionary<int, List<Vertex>> _AdjList { get; set; }
    private Dictionary<int, List<Vertex>> _RevAdjList { get; set; }

    public Vertex this[int id] => _VertexList[id];

    public Graph()
    {
      _VertexList = new Dictionary<int, Vertex>();
      _AdjList = new Dictionary<int, List<Vertex>>();
      _RevAdjList = new Dictionary<int, List<Vertex>>();
    }

    public void Add(int id, int connectedId)
    {
      Vertex vertex;
      if (_VertexList.ContainsKey(id))
      {
        vertex = _VertexList[id];
      }
      else
      {
        vertex = new Vertex { Id = id };
        _VertexList.Add(id, vertex);
      }

      Vertex connectedVertex;
      if (_VertexList.ContainsKey(connectedId))
      {
        connectedVertex = _VertexList[connectedId];
      }
      else
      {
        connectedVertex = new Vertex { Id = connectedId };
        _VertexList.Add(connectedId, connectedVertex);
      }

      if (_AdjList.ContainsKey(id))
      {
        _AdjList[id].Add(connectedVertex);
      }
      else
      {
        _AdjList.Add(id, new List<Vertex> { connectedVertex });
      }

      if (_RevAdjList.ContainsKey(connectedId))
      {
        _RevAdjList[connectedId].Add(vertex);
      }
      else
      {
        _RevAdjList.Add(connectedId, new List<Vertex> { vertex });
      }
    }

    public List<Vertex> GetConnected(int id)
    {
      return _AdjList[id];
    }

    public List<Vertex> GetUnexploredConnected(int id, Dictionary<int, List<Vertex>> adjList)
    {
      if (adjList.ContainsKey(id))
      {
        return adjList[id]
          .Where(v => !v.Explored)
          .ToList();
      }
      else
      {
        return new List<Vertex>();
      }
    }

    public void ResetExplored()
    {
      _VertexList.ToList().ForEach(v => v.Value.Explored = false);
    }

    public void Dfs(Vertex start, Action<Vertex> action, bool reverse = false)
    {
      var unexplored = GetUnexploredConnected(start.Id, reverse ? _RevAdjList : _AdjList);
      for (int i = 0; i < unexplored.Count; i++)
      {
        var vertex = unexplored[i];
        vertex.Explored = true;
        action(vertex);
        Dfs(vertex, action, reverse);
      }
    }

    public List<int> FindScc()
    {
      var list = new List<Vertex>();
      for (int i = 0; i < _VertexList.Count; i++)
      {
        var vertex = this[i];
        if (!vertex.Explored)
        {
          Dfs(vertex, v => list.Add(v), true);
        }
      }

      ResetExplored();

      var sccSizes = new List<int>();
      for (int i = 0; i < list.Count; i++)
      {
        var vertex = list[i];
        if (!vertex.Explored)
        {
          var scc = new List<Vertex>();
          Dfs(vertex, v => scc.Add(v));
          sccSizes.Add(scc.Count);
        }
      }

      sccSizes.Sort();

      return sccSizes;
    }
  }

  public class Vertex
  {
    public int Id { get; set; }
    public bool Explored { get; set; }

    public override string ToString()
    {
      return $"[{Id}] {(Explored ? "x" : "-")}";
    }
  }
}