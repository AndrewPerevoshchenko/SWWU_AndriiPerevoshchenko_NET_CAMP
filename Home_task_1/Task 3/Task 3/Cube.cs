using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Task_3
{
    //Помилки, які вже бачу: проблеми з перевірками діагоналей - не до кінця дороблено
    //Використано лісти, хоча, насправді, можна було простіше конструкцію (навіть з масивами)
    //A lot of memory!
    internal class Cube
    {
        private const uint SIZE_STANDART = 3;
        private readonly uint _size;
        private DirectedGraph _frame;
        public Cube(uint size = SIZE_STANDART)
        {
            _size = size;
            _frame = new DirectedGraph();
            GenerateCubeFrame();
        }
        private void GenerateCubeFrame() //Генерація каркаса
        {
            GenerateEdgeThree();
            GenerateEdgeTwo();
            GenerateEdgeOne();
            GenerateDiagonalEdges();
        }
        private void GenerateEdgeThree() //Вершини, які матимуть три ребра виходу + їхні ребра (на малюнку червоний колір)
        {
            NodeCoordinates begin = new NodeCoordinates(0, 0, 0);
            _frame.AddNode(begin);
            NodeCoordinates nodeX;
            NodeCoordinates nodeY;
            NodeCoordinates nodeZ;
            for (uint i = 0; i < _size - 1; ++i)
            {
                for (uint j = 0; j < _size - 1; ++j)
                {
                    for (uint k = 0; k < _size - 1; ++k)
                    {
                        nodeX = new NodeCoordinates(i + 1, j, k);
                        nodeY = new NodeCoordinates(i, j + 1, k);
                        nodeZ = new NodeCoordinates(i, j, k + 1);
                        _frame.AddNode(nodeX);
                        _frame.AddNode(nodeY);
                        _frame.AddNode(nodeZ);
                        _frame.AddEdge(begin, nodeX);
                        _frame.AddEdge(begin, nodeY);
                        _frame.AddEdge(begin, nodeZ);
                        begin = new NodeCoordinates(i, j, k + 1);
                    }
                    begin = new NodeCoordinates(i, j + 1, 0);
                }
                begin = new NodeCoordinates(i + 1, 0, 0);
            }
        }     
        private void GenerateEdgeTwo() //Аналогічно, але з двома (на малюнку бірюзовий, помаранчевий, зелений кольори)
        {
            uint maxIndex = _size - 1;
            for (uint i = 0; i < maxIndex; ++i)
            {
                _frame.AddNode(new NodeCoordinates(i, maxIndex, maxIndex));
                _frame.AddNode(new NodeCoordinates(maxIndex, i, maxIndex));
                _frame.AddNode(new NodeCoordinates(maxIndex, maxIndex, i));
            }           
            for (uint i = 0; i < maxIndex; ++i)
            {
                for (uint j = 0; j < maxIndex; ++j)
                {
                    NodeCoordinates begin = new NodeCoordinates(i, j, maxIndex);
                    NodeCoordinates endFirst = new NodeCoordinates(i + 1, j, maxIndex);
                    NodeCoordinates endSecond = new NodeCoordinates(i, j + 1, maxIndex);
                    _frame.AddEdge(begin, endFirst);
                    _frame.AddEdge(begin, endSecond);
                    begin = new NodeCoordinates(i, maxIndex, j);
                    endFirst = new NodeCoordinates(i + 1, maxIndex, j);
                    endSecond = new NodeCoordinates(i, maxIndex, j + 1);
                    _frame.AddEdge(begin, endFirst);
                    _frame.AddEdge(begin, endSecond);
                    begin = new NodeCoordinates(maxIndex, i, j);
                    endFirst = new NodeCoordinates(maxIndex, i + 1, j);
                    endSecond = new NodeCoordinates(maxIndex, i, j + 1);
                    _frame.AddEdge(begin, endFirst);
                    _frame.AddEdge(begin, endSecond);
                }
            }
        }
        private void GenerateEdgeOne() //Одиночні ребра (На малюнку блакитний, жовтий, фіолетовий кольори)
        {
            uint maxIndex = _size - 1;
            _frame.AddNode(new NodeCoordinates(maxIndex, maxIndex, maxIndex));
            for(uint i = 0; i < maxIndex; ++i)
            {
                NodeCoordinates begin = new NodeCoordinates(i, maxIndex, maxIndex);
                NodeCoordinates end = new NodeCoordinates(i + 1, maxIndex, maxIndex);
                _frame.AddEdge(begin, end);
                begin = new NodeCoordinates(maxIndex, i, maxIndex);
                end = new NodeCoordinates(maxIndex, i + 1, maxIndex);
                _frame.AddEdge(begin, end);
                begin = new NodeCoordinates(maxIndex, maxIndex, i);
                end = new NodeCoordinates(maxIndex, maxIndex, i + 1);
                _frame.AddEdge(begin, end);
            }
        }
        private void GenerateDiagonalEdges() //На малюнку не вказані, але розпочинаються з вісі Х в додатній і від'ємний боки
        {
            uint maxIndex = _size - 1;
            for (uint i = 0; i <= maxIndex; ++i)
            {
                for (uint j = 0; j < maxIndex; ++j)
                {
                    NodeCoordinates begin = new NodeCoordinates(j, j, i);
                    NodeCoordinates end = new NodeCoordinates(j + 1, j + 1, i);
                    _frame.AddEdge(begin, end);
                    begin = new NodeCoordinates(maxIndex - j, j, i);
                    end = new NodeCoordinates(maxIndex - j - 1, j + 1, i);
                    _frame.AddEdge(begin, end);
                }
            }
        }
        public void FillCubeRandomly() //Заповнення куба рандомним чином
        {
            Random random = new Random();
            foreach(GraphNode item in _frame.Nodes)
            {
                item.Emptiness = random.NextDouble() >= 0.5 ? true : false;
            }
        }
        public List<(NodeCoordinates, NodeCoordinates)> FindThroughHoles() //Пошук ліній
        {
            List<(NodeCoordinates, NodeCoordinates)> result = new List<(NodeCoordinates, NodeCoordinates)>(); 
            for (uint i = 0; i < _size; ++i) //Спочатку n * n вершин, що лежать на площині X0Z
            {
                for(uint j = 0; j < _size; ++j)
                {
                    int headIndex = _frame.Nodes.IndexOf(new GraphNode(i, 0, j));
                    LinkedList<int> nodsIndexes = _frame.BFS(_frame.Nodes[headIndex], _size - 1);
                    if (nodsIndexes != null)
                    {
                        foreach (var item in nodsIndexes)
                        {
                            if (_frame.Nodes[item].EqualsTwoCoordinates(_frame.Nodes[headIndex]))
                            {
                                result.Add((_frame.Nodes[headIndex].Coordinates, _frame.Nodes[item].Coordinates));
                            }
                        }
                    }
                }
            }
            for (uint i = 1; i < _size; ++i) //n*n вершин без тих, що вже перевірили, на площині Y0Z
            {
                for (uint j = 0; j < _size; ++j)
                {
                    int headIndex = _frame.Nodes.IndexOf(new GraphNode(i, j, 0));
                    LinkedList<int> nodsIndexes = _frame.BFS(_frame.Nodes[headIndex], _size - 1);
                    if (nodsIndexes != null)
                    {
                        foreach (var item in nodsIndexes)
                        {
                            if (_frame.Nodes[item].EqualsTwoCoordinates(_frame.Nodes[headIndex]))
                            {
                                result.Add((_frame.Nodes[headIndex].Coordinates, _frame.Nodes[item].Coordinates));
                            }
                        }
                    }
                }
            }
            for (uint i = 1; i < _size; ++i) //Залишок вершин на площині X0Y
            {
                for (uint j = 1; j < _size; ++j)
                {
                    int headIndex = _frame.Nodes.IndexOf(new GraphNode(0, i, j));
                    LinkedList<int> nodsIndexes = _frame.BFS(_frame.Nodes[headIndex], _size - 1);
                    if (nodsIndexes != null)
                    {
                        foreach (var item in nodsIndexes)
                        {
                            if (_frame.Nodes[item].EqualsTwoCoordinates(_frame.Nodes[headIndex]))
                            {
                                result.Add((_frame.Nodes[headIndex].Coordinates, _frame.Nodes[item].Coordinates));
                            }
                        }
                    }
                }
            }
            return result;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for(uint i = 0; i < _size; ++i)
            {
                for (uint j = 0; j < _size; ++j) 
                {
                    for(uint k = 0; k < _size; ++k)
                    {
                        sb.Append(_frame.Nodes[_frame.Nodes.IndexOf(new GraphNode(i, j, k))].Emptiness + " ");
                    }
                    sb.Append("\n");
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }
    }       
}

