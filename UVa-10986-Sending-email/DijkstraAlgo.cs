using Tools;

namespace Algorithm
{
	public class DijkstraAlgorithm
	{
		private static MinHeap initialSingleSource(Graph graph, int start_index)
		{
			graph.nodes[start_index].costFromStartNode = 0;
			MinHeap minheap = new MinHeap(graph.nodesSize);
			for (int i = 0; i < graph.nodesSize; i++)
				if (graph.nodes[i] != null)
					minheap.pushToMinHeap(graph.nodes[i]);

			// minheap.printElementsInMinHeap();
			minheap.bottomUpBuildMinHeap();
			// minheap.printElementsInMinHeap();
			return minheap;
		}

		private static void decreaseKeyInMinHeap(MinHeap minheap, Node node)
		{
			int pos = minheap.positions[node.index];
			for (int i = pos; i / 2 > 0 && minheap.nodes[i].costFromStartNode < minheap.nodes[i / 2].costFromStartNode; i /= 2)
			{
				Node.swapTwoNodes(minheap.nodes, i, i / 2);
				minheap.positions[minheap.nodes[i].index] = i;
				minheap.positions[minheap.nodes[i / 2].index] = i / 2;
			}
			return;
		}

		private static void relaxNodesCostFromStartNode(Node src, Node dst, int weight, MinHeap minheap)
		{
			if (dst.costFromStartNode > src.costFromStartNode + weight)
			{
				dst.costFromStartNode = src.costFromStartNode + weight;
				// minheap.printElementsInMinHeap();
				decreaseKeyInMinHeap(minheap, dst);
				// minheap.printElementsInMinHeap();
			}
			return;
		}

		public static int ExecDijkstraAlgo(Graph graph, int source_index, int terminal_index)
		{
			MinHeap minheap = initialSingleSource(graph, source_index);

			Node cur_node;
			while (!minheap.isEmptyMinHeap())
			{
				cur_node = minheap.extractMinFromMinHeap();
				if (cur_node.index == terminal_index)
					return cur_node.costFromStartNode;
				for (int i = 0; i <= cur_node.adjNodeSize; i++)
					relaxNodesCostFromStartNode(cur_node, cur_node.adjNode[i], cur_node.adjNodeWeights[i], minheap);
			}
			return -1;
		}
	}
}