namespace GraphAlgorithms.Shared
{
    public static class Shared
    {
        public enum GraphClassEnum { 
            Connected = 1, 
            UnicyclicBipartite = 2, 
            Tree = 3, 
            Unicyclic = 4,
            Bipartite = 5,
            AcyclicWithFixedDiameter = 6
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
            WienerIndex = 10,
            MinNodeDegree = 11,
            MaxNodeDegree = 12,
        }

        public enum RandomGraphCriteria
        {
//            None = 0,
            MinWienerIndex = 1,
            MaxWienerIndex = 2,
        }
    }
}