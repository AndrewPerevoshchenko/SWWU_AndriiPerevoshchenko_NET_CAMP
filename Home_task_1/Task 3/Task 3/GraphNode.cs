using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Task_3
{
    public struct NodeCoordinates
    {
        private uint _x;
        private uint _y;
        private uint _z;
        public uint X { get { return _x; } }
        public uint Y { get { return _y; } }
        public uint Z { get { return _z; } }
        public NodeCoordinates(uint x, uint y, uint z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
        public NodeCoordinates(in NodeCoordinates other)
        {
            _x = other._x;
            _y = other._y;
            _z = other._z;
        }
        public override string ToString()
        {
            return $"{X} {Y} {Z}";
        }
    }
    internal class GraphNode: IEquatable<GraphNode>
    {
        private NodeCoordinates _coordinates;
        private bool _emptiness;
        private List<GraphEdge> _edges;
        public bool Emptiness { get { return _emptiness; } set { _emptiness = value; } }
        public NodeCoordinates Coordinates { get { return _coordinates; } }
        public List<GraphEdge> Edges { get { return _edges; } }
        public GraphNode(uint x, uint y, uint z, bool emptiness = true)
        {
            _coordinates = new NodeCoordinates(x, y, z);
            _edges = new List<GraphEdge>();
            _emptiness = emptiness;
        }
        public GraphNode(in GraphNode other)
        {
            _coordinates = new NodeCoordinates(other._coordinates);
            _edges = new List<GraphEdge>();
            foreach(GraphEdge item in other._edges)
            {
                _edges.Add(item);
            }
        }
        public void AddEdge(GraphEdge edge)
        {
            _edges.Add(edge);
        }
        public void AddEdge(GraphNode node)
        {
            AddEdge(new GraphEdge(node));
        }

        public bool Equals(GraphNode? other)
        {
            return other != null ? Coordinates.Equals(other.Coordinates) : false;
        }
        public bool EqualsTwoCoordinates(GraphNode ? other)
        {
            if (other != null)
            {
                if ((Coordinates.X == other.Coordinates.X && Coordinates.Y == other.Coordinates.Y) ||
                    (Coordinates.X == other.Coordinates.X && Coordinates.Z == other.Coordinates.Z) ||
                    (Coordinates.Y == other.Coordinates.Y && Coordinates.Z == other.Coordinates.Z))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
