using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Algorithms
{
    public class WienerIndexAlgorithm : GraphAlgorithm
    {
        public int[,] Distances;
        public int WienerIndexValue = 0;

        public WienerIndexAlgorithm(Graph g) : base(g)
        {
            Distances = new int[g.N, g.N];
        }

        public override void InitializeValues()
        {
        }

        public override void Run()
        {
            OutputDescription.AppendLine("------- Wiener Index Algorithm -------");
            foreach (Node fromNode in G.Nodes)
            {
                BreadthFirstSearchAlgorithm currNodeBFS = new BreadthFirstSearchAlgorithm(G, fromNode);
                currNodeBFS.Run();

                foreach (Node toNode in G.Nodes)
                    Distances[fromNode.Index, toNode.Index] = currNodeBFS.GetDistanceToNode(toNode);
            }

            WienerIndexValue = 0;

            for (int i = 0; i < G.N; i++)
                for (int j = i + 1; j < G.N; j++)
                    WienerIndexValue += Distances[i, j];

            OutputDescription.AppendLine(string.Format("Wiener Index Value = {0}", WienerIndexValue));
            OutputDescription.AppendLine("Distances Matrix:");


            OutputDescription.Append('\t');
            for (int i = 0; i < G.N; i++)
                OutputDescription.Append(i.ToString() + '\t');
            OutputDescription.AppendLine();

            for (int i = 0; i < G.N; i++)
            {
                OutputDescription.Append(i.ToString() + '\t');
                for (int j = 0; j < G.N; j++)
                    OutputDescription.Append(Distances[i, j].ToString() + '\t');

                OutputDescription.AppendLine();
            }

            OutputDescription.AppendLine("------- END Wiener Index Algorithm -------");
        }
    }
}
