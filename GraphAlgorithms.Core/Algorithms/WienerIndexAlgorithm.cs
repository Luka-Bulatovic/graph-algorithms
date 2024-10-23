using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Core.Algorithms
{
    public class WienerIndexAlgorithm : GraphAlgorithm
    {
        private readonly GraphAlgorithmManager graphAlgorithmManager;

        public int[,] Distances;
        public int WienerIndexValue = 0;
        
        private int diameter = 0;
        public int Diameter 
        {
            get { return diameter; }
        }

        private int radius = INF_DISTANCE;
        public int Radius
        {
            get { return radius; }
        }

        public WienerIndexAlgorithm(Graph g, GraphAlgorithmManager graphAlgorithmManager) : base(g)
        {
            Distances = new int[g.N, g.N];

            this.graphAlgorithmManager = graphAlgorithmManager;
        }

        public override void InitializeValues()
        {
        }

        public override void Run()
        {
            _isExecuted = false;
            int currDistance = 0;

            OutputDescription.AppendLine("------- Wiener Index Algorithm -------");
            foreach (Node fromNode in G.Nodes)
            {
                BreadthFirstSearchAlgorithm currNodeBFS =
                    graphAlgorithmManager.RunAlgorithm(G, fromNode, (g, n) => new BreadthFirstSearchAlgorithm(g, n), cacheResult: false);

                int currNodeEccentricity = 0;

                foreach (Node toNode in G.Nodes)
                {
                    currDistance = currNodeBFS.GetDistanceToNode(toNode);
                    diameter = Math.Max(diameter, currDistance);
                    currNodeEccentricity = Math.Max(currNodeEccentricity, currDistance);

                    Distances[fromNode.Index, toNode.Index] = currDistance;
                }

                radius = Math.Min(radius, currNodeEccentricity);
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
            
            _isExecuted = true;
        }
    }
}
