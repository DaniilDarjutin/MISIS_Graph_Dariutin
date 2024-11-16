using System;
using System.Collections.Generic;

namespace GraphRepresentation
{
    public class Edge
    {
        public int From { get; }
        public int To { get; }
        public int Weight { get; }

        public Edge(int from, int to, int weight)
        {
            From = from;
            To = to;
            Weight = weight;
        }
    }

    public abstract class Graph
    {
        public abstract void InsertVertex(int id);
        public abstract void InsertEdge(int from, int to, int weight);
        public abstract void RemoveVertex(int id);
        public abstract void RemoveEdge(int from, int to);
        public abstract bool FindVertex(int id);
        public abstract bool FindEdge(int from, int to);
    }

    public class EdgeListGraph : Graph
    {
        private readonly List<Edge> edges = new();
        private readonly HashSet<int> vertices = new();

        public override void InsertVertex(int id)
        {
            vertices.Add(id);
        }

        public override void InsertEdge(int from, int to, int weight)
        {
            edges.Add(new Edge(from, to, weight));
        }

        public override void RemoveVertex(int id)
        {
            vertices.Remove(id);
            edges.RemoveAll(e => e.From == id || e.To == id);
        }

        public override void RemoveEdge(int from, int to)
        {
            edges.RemoveAll(e => e.From == from && e.To == to);
        }

        public override bool FindVertex(int id)
        {
            return vertices.Contains(id);
        }

        public override bool FindEdge(int from, int to)
        {
            return edges.Exists(e => e.From == from && e.To == to);
        }
    }

    public class AdjacencyListGraph : Graph
    {
        private readonly Dictionary<int, List<(int to, int weight)>> adjacencyList = new();

        public override void InsertVertex(int id)
        {
            if (!adjacencyList.ContainsKey(id))
                adjacencyList[id] = new List<(int, int)>();
        }

        public override void InsertEdge(int from, int to, int weight)
        {
            InsertVertex(from);
            InsertVertex(to);
            adjacencyList[from].Add((to, weight));
        }

        public override void RemoveVertex(int id)
        {
            adjacencyList.Remove(id);
            foreach (var key in adjacencyList.Keys)
            {
                adjacencyList[key].RemoveAll(e => e.to == id);
            }
        }

        public override void RemoveEdge(int from, int to)
        {
            if (adjacencyList.ContainsKey(from))
                adjacencyList[from].RemoveAll(e => e.to == to);
        }

        public override bool FindVertex(int id)
        {
            return adjacencyList.ContainsKey(id);
        }

        public override bool FindEdge(int from, int to)
        {
            return adjacencyList.ContainsKey(from) && adjacencyList[from].Exists(e => e.to == to);
        }
    }

    public class ArcListGraph : Graph
    {
        private readonly List<Edge> arcs = new();
        private readonly HashSet<int> vertices = new();

        public override void InsertVertex(int id)
        {
            vertices.Add(id);
        }

        public override void InsertEdge(int from, int to, int weight)
        {
            arcs.Add(new Edge(from, to, weight));
        }

        public override void RemoveVertex(int id)
        {
            vertices.Remove(id);
            arcs.RemoveAll(a => a.From == id || a.To == id);
        }

        public override void RemoveEdge(int from, int to)
        {
            arcs.RemoveAll(a => a.From == from && a.To == to);
        }

        public override bool FindVertex(int id)
        {
            return vertices.Contains(id);
        }

        public override bool FindEdge(int from, int to)
        {
            return arcs.Exists(a => a.From == from && a.To == to);
        }
    }

    public class AdjacencyBundleGraph : Graph
    {
        private readonly Dictionary<int, List<Edge>> bundles = new();

        public override void InsertVertex(int id)
        {
            if (!bundles.ContainsKey(id))
                bundles[id] = new List<Edge>();
        }

        public override void InsertEdge(int from, int to, int weight)
        {
            InsertVertex(from);
            bundles[from].Add(new Edge(from, to, weight));
        }

        public override void RemoveVertex(int id)
        {
            bundles.Remove(id);
            foreach (var key in bundles.Keys)
            {
                bundles[key].RemoveAll(e => e.To == id);
            }
        }

        public override void RemoveEdge(int from, int to)
        {
            if (bundles.ContainsKey(from))
                bundles[from].RemoveAll(e => e.To == to);
        }

        public override bool FindVertex(int id)
        {
            return bundles.ContainsKey(id);
        }

        public override bool FindEdge(int from, int to)
        {
            return bundles.ContainsKey(from) && bundles[from].Exists(e => e.To == to);
        }
    }

    public static class GraphTest
    {
        public static void RunTests()
        {
            Graph graph = new AdjacencyListGraph();
            graph.InsertVertex(1);
            graph.InsertVertex(2);
            graph.InsertEdge(1, 2, 10);

            Console.WriteLine($"Вершина 1 найдена: {graph.FindVertex(1)}");
            Console.WriteLine($"Ребро (1, 2) найдено: {graph.FindEdge(1, 2)}");

            graph.RemoveEdge(1, 2);
            Console.WriteLine($"Ребро (1, 2) после удаления найдено: {graph.FindEdge(1, 2)}");

            graph.RemoveVertex(1);
            Console.WriteLine($"Вершина 1 после удаления найдена: {graph.FindVertex(1)}");
        }
    }

    class Program
    {
        static void Main()
        {
            GraphTest.RunTests();
        }
    }
}
