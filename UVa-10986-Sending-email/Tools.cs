using System;
using System.Collections.Generic;

namespace Tools
{
	public class Node
	{
		public int index;
		public int costFromStartNode;
		public int adjNodeSize;
		public List<int> adjNodeWeights;
		public List<Node> adjNode;

		public Node(int size, int index)
		{
			this.index = index;
			this.costFromStartNode = 100000001;
			this.adjNodeSize = -1;
			this.adjNodeWeights = new List<int>(size);
			this.adjNode = new List<Node>(size);
			for (int i = 0; i < size; i++)
				this.adjNode.Add(null);
		}


		public static void linkTwoNodes(Node parent, Node child, int cost)
		{
			(parent.adjNodeSize)++;
			parent.adjNodeWeights.Insert(parent.adjNodeSize, cost);
			parent.adjNode.Insert(parent.adjNodeSize, child);
		}

		public static void swapTwoNodes(List<Node> nodes, int a, int b)
		{
			var tmp = nodes[a];
			nodes[a] = nodes[b];
			nodes[b] = tmp;
		}
	}



	public class Edge
	{
		public int index1, index2, weight;

		public Edge(int index1, int index2, int weight)
		{
			this.index1 = index1;
			this.index2 = index2;
			this.weight = weight;
		}
	}

	public class AllEdges
	{
		public int edgesSize;
		public List<Edge> edges;
		public AllEdges(int edgesSize)
		{
			this.edgesSize = edgesSize;
			this.edges = new List<Edge>(edgesSize);
		}
	}

	public class Graph
	{
		public int nodesSize;
		public List<Node> nodes;

		public Graph(int nodes_num, AllEdges all_edges, int edges_num)
		{
			this.nodesSize = nodes_num;
			this.nodes = new List<Node>(nodes_num);
			for (int i = 0; i < nodes_num; i++)
				this.nodes.Add(null);

			for (int i = 0; i < edges_num; i++)
			{
				int src = all_edges.edges[i].index1;
				int dst = all_edges.edges[i].index2;
				if (this.nodes[src] == null)
					this.nodes[src] = new Node(edges_num, src);
				if (this.nodes[dst] == null)
					this.nodes[dst] = new Node(edges_num, dst);
				// this.printGraph();
				Node.linkTwoNodes(this.nodes[src], this.nodes[dst], all_edges.edges[i].weight);
				Node.linkTwoNodes(this.nodes[dst], this.nodes[src], all_edges.edges[i].weight);
			}
		}

		public void printGraph()
		{
			for (int i = 0; i < this.nodesSize; i++)
			{
				Console.Write(i + ": [");
				if (this.nodes[i] == null)
					Console.WriteLine("null]");
				else
				{
					if (this.nodes[i].adjNode[0] == null)
						Console.WriteLine("null]");
					else
					{
						int j;
						for (j = 0; j < this.nodes[i].adjNodeSize; j++)
							Console.Write("index = " + this.nodes[i].adjNode[j].index + ", ");
						Console.WriteLine("index = " + this.nodes[i].adjNode[j].index + "]");
					}
				}
			}
		}
	}

	public class MinHeap
	{
		public int last_index;
		public List<int>? positions;
		public List<Node>? nodes;

		private int INFINITY = 1000000001;

		public MinHeap(int size)
		{
			this.last_index = 0;
			this.positions = new List<int>(size + 1);
			this.nodes = new List<Node>(size + 1);
			for (int i = 0; i < size; i++)
			{
				this.positions.Add(0);
				this.nodes.Add(null);
			}
		}

		public bool isEmptyMinHeap()
		{
			return this.last_index <= 0 ? true : false;
		}

		public void pushToMinHeap(Node node)
		{
			(this.last_index)++;
			this.positions[node.index] = this.last_index;
			this.nodes.Insert(this.last_index, node);
			return;
		}

		public void insertToMinHeap(Node node, int edges_num)
		{
			(this.last_index)++;
			this.positions[node.index] = this.last_index;
			this.nodes[this.last_index] = node;
			for (int pos = this.last_index; pos / 2 > 0 && this.nodes[pos].costFromStartNode < this.nodes[pos / 2].costFromStartNode; pos /= 2)
			{
				Node.swapTwoNodes(this.nodes, pos, pos / 2);
				this.positions[this.nodes[pos].index] = pos;
				this.positions[this.nodes[pos / 2].index] = pos / 2;
			}
			return;
		}

		public Node extractMinFromMinHeap()
		{
			if (this.isEmptyMinHeap())
				return null;

			Node ret = this.nodes[1];
			this.nodes[1] = this.nodes[this.last_index];
			this.positions[this.nodes[1].index] = 1;
			(this.last_index)--;
			int last_pos = this.last_index;
			for (int pos = 1; pos <= last_pos;)
			{
				int lchild_pos = pos * 2;
				int rchild_pos = pos * 2 + 1;
				if (rchild_pos <= last_pos)
				{
					if (this.nodes[lchild_pos].costFromStartNode > this.nodes[rchild_pos].costFromStartNode)
					{
						if (this.nodes[pos].costFromStartNode > this.nodes[rchild_pos].costFromStartNode)
						{
							Node.swapTwoNodes(this.nodes, pos, rchild_pos);
							this.positions[this.nodes[pos].index] = pos;
							this.positions[this.nodes[rchild_pos].index] = rchild_pos;
							pos = rchild_pos;
						}
						else return ret;
					}
					else
					{
						if (this.nodes[pos].costFromStartNode > this.nodes[lchild_pos].costFromStartNode)
						{
							Node.swapTwoNodes(this.nodes, pos, lchild_pos);
							this.positions[this.nodes[pos].index] = pos;
							this.positions[this.nodes[lchild_pos].index] = lchild_pos;
							pos = lchild_pos;
						}
						else return ret;
					}
				}
				else if (lchild_pos <= last_pos)
				{
					if (this.nodes[pos].costFromStartNode > this.nodes[lchild_pos].costFromStartNode)
					{
						Node.swapTwoNodes(this.nodes, pos, lchild_pos);
						this.positions[this.nodes[pos].index] = pos;
						this.positions[this.nodes[lchild_pos].index] = lchild_pos;
						pos = lchild_pos;
					}
					else return ret;
				}
				else return ret;
			}
			return ret;
		}

		public void bottomUpBuildMinHeap()
		{
			int last_pos = this.last_index;
			for (int i = last_index / 2; i > 0; i--)
			{
				for (int pos = i; pos <= last_pos;)
				{
					int lchild_pos = pos * 2;
					int rchild_pos = pos * 2 + 1;
					if (rchild_pos <= last_pos)
					{
						if (this.nodes[lchild_pos].costFromStartNode > this.nodes[rchild_pos].costFromStartNode)
						{
							if (this.nodes[pos].costFromStartNode > this.nodes[rchild_pos].costFromStartNode)
							{
								Node.swapTwoNodes(this.nodes, pos, rchild_pos);
								this.positions[this.nodes[pos].index] = pos;
								this.positions[this.nodes[rchild_pos].index] = rchild_pos;
								pos = rchild_pos;
							}
							else break;
						}
						else
						{
							if (this.nodes[pos].costFromStartNode > this.nodes[lchild_pos].costFromStartNode)
							{
								Node.swapTwoNodes(this.nodes, pos, lchild_pos);
								this.positions[this.nodes[pos].index] = pos;
								this.positions[this.nodes[lchild_pos].index] = lchild_pos;
								pos = lchild_pos;
							}
							else break;
						}
					}
					else if (lchild_pos <= last_pos)
					{
						if (this.nodes[pos].costFromStartNode > this.nodes[lchild_pos].costFromStartNode)
						{
							Node.swapTwoNodes(this.nodes, pos, lchild_pos);
							this.positions[this.nodes[pos].index] = pos;
							this.positions[this.nodes[lchild_pos].index] = lchild_pos;
							pos = lchild_pos;
						}
						else break;
					}
					else break;
				}
			}
		}

		public void printElementsInMinHeap()
		{
			if (this.isEmptyMinHeap())
			{
				Console.WriteLine("minheap = []");
				return;
			}
			Console.WriteLine("minheap = [");
			int i;
			for (i = 1; i < this.last_index; i++)
				Console.WriteLine("\t(index = " + this.nodes[i].index + ", cost = " + this.nodes[i].costFromStartNode + ", pos = " + this.positions[this.nodes[i].index] + "),");
			Console.WriteLine("\t(index = " + this.nodes[i].index + ", cost = " + this.nodes[i].costFromStartNode + ", pos = " + this.positions[this.nodes[i].index] + ")]");
		}

	}

}