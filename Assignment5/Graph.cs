using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment5
{
  public class Graph
  {
    private Dictionary<int, Vertex> _VertexList { get; set; }
    private Dictionary<int, List<Edge>> _AdjList { get; set; }
    private Dictionary<int, List<Edge>> _RevAdjList { get; set; }

    public Vertex this[int id] => _VertexList[id];

    public Graph()
    {
      _VertexList = new Dictionary<int, Vertex>();
      _AdjList = new Dictionary<int, List<Edge>>();
      _RevAdjList = new Dictionary<int, List<Edge>>();
    }

    public static Graph ReadAdjWithLength(string fileName)
    {
      var f = File.OpenText(fileName);
      var graph = new Graph();

      while (true)
      {
        var ln = f.ReadLine();
        if (ln == "")
        {
          continue;
        }
        else if (ln == null)
        {
          break;
        }

        var vals = ln.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);

        var v = int.Parse(vals[0]);

        foreach (var edge in vals.Skip(1))
        {
          var con_len = edge.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => int.Parse(x))
            .ToList();

          graph.Add(v, con_len[0], con_len[1]);
        }
      }

      return graph;
    }

    public void Add(int id, int connectedId, double length = 0)
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
        _AdjList[id].Add(new Edge(id, connectedId, length));
      }
      else
      {
        _AdjList.Add(id, new List<Edge> { new Edge(id, connectedId, length) });
      }

      if (_RevAdjList.ContainsKey(connectedId))
      {
        _RevAdjList[connectedId].Add(new Edge(connectedId, id, length));
      }
      else
      {
        _RevAdjList.Add(connectedId, new List<Edge> { new Edge(connectedId, id, length) });
      }
    }

    public List<Edge> GetConnected(Vertex vertex, bool reverse = false)
    {
      if (!reverse && _AdjList.ContainsKey(vertex.Id))
      {
        return _AdjList[vertex.Id];
      }
      else if (reverse && _RevAdjList.ContainsKey(vertex.Id))
      {
        return _RevAdjList[vertex.Id];
      }
      else
      {
        return new List<Edge>();
      }
    }

    public void ResetExplored()
    {
      _VertexList.ToList().ForEach(v => v.Value.Explored = false);
    }

    public void Dfs(Vertex start, Action<Vertex> action, bool reverse = false)
    {
      start.Explored = true;

      var connected = GetConnected(start, reverse);
      for (int i = 0; i < connected.Count; i++)
      {
        var vertex = _VertexList[connected[i].ConnectedId];
        if (!vertex.Explored)
        {
          Dfs(vertex, action, reverse);
        }
      }

      action(start);
    }

    public List<int> FindScc()
    {
      Console.WriteLine("Finding sincs");

      var list = new List<Vertex>();
      for (int i = _VertexList.Count - 1; i >= 0; i--)
      {
        var vertex = _VertexList[i + 1];
        if (!vertex.Explored)
        {
          var lst = new List<Vertex>();
          Dfs(vertex, v => lst.Add(v), true);
          //  lst.Reverse();
          list.AddRange(lst);
        }
      }

      ResetExplored();
      Console.WriteLine("Finding scc's");

      var sccSizes = new List<int>();
      for (int i = list.Count - 1; i >= 0; i--)
      //for (int i = 0; i < list.Count; i++)
      {
        var vertex = list[i];
        if (!vertex.Explored)
        {
          var scc = new List<Vertex>();
          Dfs(vertex, v => scc.Add(v));
          sccSizes.Add(scc.Count);
        }
      }

      return sccSizes.OrderByDescending(x => x).ToList();
    }

    public double CalcSortestPath(int startId, int destId)
    {
      var startVertex = _VertexList[startId];

      var crossings = new List<Edge>();
      crossings.AddRange(GetConnected(startVertex));

      var nodes = new Dictionary<int, double>();
      nodes.Add(startId, 0);

      Edge minEdge = null;
      while (minEdge?.ConnectedId != destId)
      {
        double minLength = double.MaxValue;

        foreach (var crossing in crossings)
        {
          if (nodes.ContainsKey(crossing.ConnectedId))
          {
            continue;
          }

          var newLenth = nodes[crossing.SouceId] + crossing.Length;

          if (newLenth < minLength)
          {
            minLength = newLenth;
            minEdge = crossing;
          }
        }

        nodes.Add(minEdge.ConnectedId, minLength);
        crossings.Remove(minEdge);

        GetConnected(_VertexList[minEdge.ConnectedId])
          .ForEach(e => {
            if (!nodes.ContainsKey(e.ConnectedId) && !crossings.Contains(e))
            {
              crossings.Add(e);
            }
          });
      }

      return nodes[destId];
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

  public class Edge
  {
    public int SouceId { get; set; }
    public int ConnectedId { get; set; }
    public bool Explored { get; set; }
    public double Length { get; set; }

    public Edge(int sourceId, int connectedId, double length = 0)
    {
      SouceId = sourceId;
      ConnectedId = connectedId;
      Length = length;
    }

    public override string ToString()
    {
      return $"[{SouceId}-{ConnectedId}] {Length} {(Explored ? "x" : "-")}";
    }
  }
}
