using GraphAlgorithms.Core;
using GraphAlgorithms.Core.Algorithms;
using GraphAlgorithms.Core.Factories;
using GraphAlgorithms.Core.Interfaces;

//List<WienerIndexAlgorithm> graphs = new List<WienerIndexAlgorithm>();

//IGraphFactory graphFactory = new RandomUnicyclicBipartiteGraphFactory(7, 7, 10);
//for (int i = 0; i < 30000; i++)
//{
//    Graph g = graphFactory.CreateGraph();
//    graphs.Add(new WienerIndexAlgorithm(g));
//    graphs[i].Run();
//}

//graphs.Sort((x, y) => { return y.WienerIndexValue - x.WienerIndexValue; });

//for (int i = 0; i < 5; i++)
//{
//    Console.WriteLine(string.Format("Graph {0} = {1}", i, graphs[i].WienerIndexValue));
//    Console.WriteLine(graphs[i].G);
//    Console.WriteLine();
//    Console.WriteLine();
//}

//BreadthFirstSearchAlgorithm bfs = new BreadthFirstSearchAlgorithm(graphs[0].G, graphs[0].G.Nodes[0]);
//bfs.Run();
//Console.WriteLine(bfs);



//Graph g = GraphFactory.GetGraphFromFileWithNodeNames("foreignkeys.txt");

//TopologicalSortAlgorithm topsort = new TopologicalSortAlgorithm(g);
//topsort.Run();

//File.WriteAllText("output.txt", topsort.ToString());



