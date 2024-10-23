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

        public enum GraphPropertyEnum
        {
            Order = 1,
            MinSizeCoef = 2,
            Diameter = 3,
            CycleLength = 4,
            FirstBipartitionSize = 5,
            SecondBipartitionSize = 6,
            Radius = 7,
            SizeToOrderRatio = 8,
            Size = 9,
            WienerIndex = 10
        }
    }
}