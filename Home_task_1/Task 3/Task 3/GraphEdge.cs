using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_3
{
    internal class GraphEdge
    {
        private GraphNode _connectedNode;
        public GraphNode ConnectedNode { get { return _connectedNode; } }
        public GraphEdge(GraphNode connectedNode)
        {
            _connectedNode = connectedNode;
        }
    }
}
