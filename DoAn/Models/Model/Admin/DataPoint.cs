using System.Runtime.Serialization;

namespace DoAn.Models.Model.Admin
{
    [DataContract]
    public class DataPoint
    {
        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "label")] public string Label = $"";

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")] public double? Y;

        public DataPoint(string label, double y)
        {
            Label = label;
            Y = y;
        }

        public DataPoint()
        {
            Label = "";
            Y = 0;
        }
    }
}