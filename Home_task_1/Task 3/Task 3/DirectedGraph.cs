using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Task_3
{
    internal class DirectedGraph
    {
        private List<GraphNode> _nodes;
        public List<GraphNode> Nodes { get { return _nodes; } }
        public DirectedGraph()
        {
            _nodes = new List<GraphNode>();
        }
        public void AddNode(NodeCoordinates coordinates, bool emptiness = true)
        {
            if (!FindNode(coordinates))
            {
                _nodes.Add(new GraphNode(coordinates.X, coordinates.Y, coordinates.Z, emptiness));
            }           
        }
        public bool FindNode(NodeCoordinates coordinates)
        {
            foreach(GraphNode item in _nodes)
            {
                if (item.Coordinates.Equals(coordinates))
                {
                    return true;
                }
            }
            return false;
        }
        public void AddEdge(NodeCoordinates begin, in NodeCoordinates end)
        {
            int indexBegin = _nodes.IndexOf(new GraphNode(begin.X, begin.Y, begin.Z));
            int indexEnd = _nodes.IndexOf(new GraphNode(end.X, end.Y, end.Z));
             _nodes[indexBegin].AddEdge(_nodes[indexEnd]);            
        }
        public LinkedList<int> BFS(in GraphNode head, uint depth) //Пошук ушир на n - 1 рівнів
        {
            int s = _nodes.IndexOf(head);
            if(s == -1)
            {
                return null;
            }
            bool[] visited = new bool[_nodes.Count];
            for (int i = 0; i < visited.Length; ++i)
            {
                visited[i] = !_nodes[i].Emptiness;
            }
            LinkedList<int> queue = new LinkedList<int>();
            visited[s] = true;
            queue.AddLast(s);
            uint level = 0;
            uint iterator = 1;
            int queueSize = 1;
            while (queue.Any()) 
            {
                s = queue.First();
                queue.RemoveFirst();
                LinkedList<int> list = new LinkedList<int>();
                foreach(var edge in _nodes[s].Edges)
                {
                    list.AddLast(_nodes.IndexOf(new GraphNode(edge.ConnectedNode.Coordinates.X, edge.ConnectedNode.Coordinates.Y, edge.ConnectedNode.Coordinates.Z)));
                }
                foreach (var val in list)
                {
                    if (!visited[val])
                    {
                        visited[val] = true;
                        if (_nodes[s].Coordinates.X <= _nodes[val].Coordinates.X &&
                            _nodes[s].Coordinates.Y <= _nodes[val].Coordinates.Y &&
                            _nodes[s].Coordinates.Z <= _nodes[val].Coordinates.Z)
                        {
                            queue.AddLast(val);
                        }
                        
                    }
                }
                if (iterator == queueSize)
                {
                    queueSize = queue.Count;
                    iterator = 0;
                    ++level;
                }
                if (level == depth)
                {
                    return queue;
                }              
                ++iterator;
            }
            return null;
        }
    }
}
