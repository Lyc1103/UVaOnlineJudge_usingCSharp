// See https://aka.ms/new-console-template for more information

using System;
using System.Linq;
using Tools;
using Algorithm;

class Program
{
	static int INFINITY = 1000000001;
	static void Main()
	{
		var fileStream = new System.IO.FileStream("10986.txt", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
		var file = new System.IO.StreamReader(fileStream, System.Text.Encoding.UTF8, true, 128);

		int N = Convert.ToInt32(file.ReadLine());
		for (int i = 1; i <= N; i++)
		{
			string? input = file.ReadLine();
			int n, m, S, T;
			if (input != null)
			{
				n = Convert.ToInt32(input.Split(' ')[0]);
				m = Convert.ToInt32(input.Split(' ')[1]);
				S = Convert.ToInt32(input.Split(' ')[2]);
				T = Convert.ToInt32(input.Split(' ')[3]);
			}
			else
			{
				Console.WriteLine("Completely read.");
				return;
			}

			AllEdges graph_edges = new AllEdges(m + 1);

			for (int j = 0; j < m; j++)
			{
				string? edge_string = file.ReadLine();
				if (edge_string != null)
				{
					Edge edge = new Edge(Convert.ToInt32(edge_string.Split(' ')[0]), Convert.ToInt32(edge_string.Split(' ')[1]), Convert.ToInt32(edge_string.Split(' ')[2]));
					graph_edges.edges.Add(edge);
				}
			}
			Graph graph = new Graph(n, graph_edges, m);
			if (graph.nodes[S] == null || graph.nodes[T] == null)
				Console.WriteLine("Case #" + i + ": unreachable");
			else
			{
				int send_cost = sendingEmail(graph, S, T);
				if (send_cost >= INFINITY) Console.WriteLine("Case #" + i + ": unreachable");
				else Console.WriteLine("Case #" + i + ": " + send_cost);
			}

		}
		return;
	}

	private static int sendingEmail(Graph graph, int source_index, int terminal_index)
	{
		int res = DijkstraAlgorithm.ExecDijkstraAlgo(graph, source_index, terminal_index);
		return res;
	}
}
