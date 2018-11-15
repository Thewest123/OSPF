using Newtonsoft.Json;

namespace OSPFCalculator
{
    public class Route
    {
        public string Destination;
        public int    Cost;
        public string NextHop;

        [JsonIgnore]
        public bool IsChanged;
        [JsonIgnore]
        public bool IsNew;

        public Route(string destination, int cost, string nextHop, bool isChanged = false, bool isNew = false)
        {
            Destination = destination;
            Cost = cost;
            NextHop = nextHop;
            IsChanged = isChanged;
            IsNew = isNew;
        }
    }
}
