using System;
using System.Collections.Generic;

namespace Graphe
{
    class Program
    {
        static void PrintEdge(object sc, GraphEventArgs<Person> gevent)
        {
            Console.WriteLine($"Edge has been added: {gevent.from.ToString()} ==> {gevent.to.ToString()}");
        }
        static void OutputPrint(string output)
        {
            Console.WriteLine(output);
        }
        static void Main(string[] args)
        {
            Graph<Person> graphe = new Graph<Person>();
            graphe.Event += PrintEdge;
            Person[] humanoid = new Person[10];
            humanoid[0] = new Person("Albatar");
            humanoid[1] = new Person("Zack");
            humanoid[2] = new Person("Danis");
            humanoid[3] = new Person("Erick");
            humanoid[4] = new Person("Sandor");
            humanoid[5] = new Person("Peter");
            humanoid[6] = new Person("Abel");
            humanoid[7] = new Person("Linus");
            humanoid[8] = new Person("Luke");
            humanoid[9] = new Person("Sheldon");
            foreach (var option in humanoid)
            {
                graphe.AddNode(option);
            }
            for (int i = 1; i <= 9; i++)
            {
                graphe.AddEdge(humanoid[i-1],humanoid[i]);

            }
            Console.Write("\r\n");
            graphe.BFS(humanoid[6],OutputPrint);
            Console.Write("\r\n");
            graphe.DFS(humanoid[3],OutputPrint);
        }
    }
    public delegate void GraphEventHandler<T>(object source, GraphEventArgs<T> geargs);
    public delegate void ExternalProcessor(string item);
    public class GraphEventArgs<T> : EventArgs
    {
        public T to { get; }
        public T from { get; }
        public GraphEventArgs(T to, T from)
        {
            this.to = to;
            this.from = from;
        }
    }
    class Graph<T>
    {
        public event GraphEventHandler<T> Event;
        protected class Point
        {
            public T point { get; set; }
            public List<T> neighbours { get; set; } = new List<T>(); 
            //public List<T> Neighbours { get {return neighbours;} set {neighbours=value; } }
            public Point(T point)
            {
                this.point = point;
            }
            //public double Weight;
        }
        List<Point> Nodes = new List<Point>();
        public void AddNode(T node)
        {
            Nodes.Add(new Point(node));
        }
        public void AddEdge(T from, T to)
        {
            foreach (var alpha in Nodes)
            {
                if (alpha.point.Equals(from))
                {
                    alpha.neighbours.Add(to);
                }
                else if (alpha.point.Equals(to))
                {
                    alpha.neighbours.Add(from);
                }
            }
            Event?.Invoke(this, new GraphEventArgs<T>(from, to));
        }
        public bool HasEdge(T from, T to)
        {
            foreach (var beta in Nodes)
            {
                if (beta.point.Equals(from))
                {
                    if (beta.neighbours.Equals(to))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public List<T> Neighbours(T node)
        {
            foreach (var charlie in Nodes)
            {
                if (charlie.point.Equals(node))
                {
                    return charlie.neighbours;
                }
            }
            return null;
        }
        class NodeNeighbours
        {
            public T Nod { get; set; }
            public int Distance { get; set; }

            public NodeNeighbours(T nod, int distance)
            {
                Nod = nod;
                Distance = distance;
            }
        }
        private void DFS(T from, List<T> processed, ExternalProcessor method)
        {
            processed.Add(from);
            method?.Invoke(from.ToString());
            foreach (var delta in Neighbours(from))
            {
                if (processed.Contains(delta)==false)
                {
                    DFS(delta, processed, method);
                }
            }
        }
        public void DFS(T from, ExternalProcessor method)
        {
            List<T> list = new List<T>();
            DFS(from, list, method);
        }
        public void BFS(T from, ExternalProcessor method)
        {
            Queue<T> S = new Queue<T>();
            List<T> F = new List<T>();
            List<NodeNeighbours> KnownNodes = new List<NodeNeighbours>();
            KnownNodes.Add(new NodeNeighbours(from, 0));
            S.Enqueue(from);
            F.Add(from);
            while (S.Count > 0)
            {
                T k = S.Dequeue();
                method?.Invoke(k.ToString());
                foreach (var x in Neighbours(k))
                {
                    if (F.Contains(x) == false)
                    {
                        S.Enqueue(x);
                        F.Add(x);
                        NodeNeighbours last = null;
                        foreach (var gama in KnownNodes)
                        {
                            if (gama.Nod.Equals(k))
                            {
                                last = gama;
                            }
                        }
                        KnownNodes.Add(new NodeNeighbours(x, last.Distance - 1));
                    }
                }
            }
        }
    }
    class Person
    {
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
        public Person(string name)
        {
            Name = name;
        }
    }
}
