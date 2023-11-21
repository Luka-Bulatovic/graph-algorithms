using GraphAlgorithms.Core;
using GraphAlgorithms.Core.Algorithms;
using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Service.DTO;
using static System.Formats.Asn1.AsnWriter;

namespace GraphAlgorithms.Service
{
    public static class GraphDTOConverter
    {
        public static Graph GetGraphFromGraphDTO(GraphDTO graphDTO)
        {
            Graph graph = new Graph(graphDTO.nodes.Count);

            for (int i = 0; i < graphDTO.nodes.Count; i++)
                graph.AddNode(new Node(graphDTO.nodes[i].id, graphDTO.nodes[i].label));

            for (int i = 0; i < graphDTO.edges.Count; i++)
            {
                int fromIndex = graphDTO.edges[i].from;
                int toIndex = graphDTO.edges[i].to;

                Node fromNode = graph.GetNode(fromIndex);
                Node toNode = graph.GetNode(toIndex);

                graph.ConnectNodes(fromNode, toNode);
            }

            return graph;
        }

        public static GraphDTO GetGraphDTOFromGraph(Graph graph)
        {
            GraphDTO graphDTO = new GraphDTO();

            foreach (Node node in graph.Nodes)
                graphDTO.nodes.Add(new NodeDTO(node));

            foreach (Node node in graph.Nodes)
            {
                List<Edge> adjEdges = graph.GetAdjacentEdges(node);

                foreach (Edge e in adjEdges)
                {
                    if (node.Index < e.GetDestNodeIndex())
                        graphDTO.edges.Add(new EdgeDTO(e));
                }
            }

            return graphDTO;
        }

        public static GraphEntity GetGraphEntityFromGraphDTO(GraphDTO graphDTO)
        {
            Graph graph = GetGraphFromGraphDTO(graphDTO);
            
            WienerIndexAlgorithm wienerAlg = new WienerIndexAlgorithm(graph);
            wienerAlg.Run();

            return new GraphEntity()
            {
                //ID = ?,
                Name = "",
                Order = graphDTO.nodes.Count,
                Size = graphDTO.edges.Count,
                DataXML = GraphMLConverter.GetGraphMLForGraph(graph),
                WienerIndex = wienerAlg.WienerIndexValue
            };
        }
    }
}
