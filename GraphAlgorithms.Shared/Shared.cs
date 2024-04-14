namespace GraphAlgorithms.Shared
{
    public static class Shared
    {
        public enum GraphClassEnum { 
            ConnectedGraph = 1, 
            UnicyclicBipartiteGraph = 2, 
            Tree = 3, 
            UnicyclicGraph = 4,
            BipartiteGraph = 5,
            AcyclicGraphWithFixedDiameter = 6
        };
    }
}