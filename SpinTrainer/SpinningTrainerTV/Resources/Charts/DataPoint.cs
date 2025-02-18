using SpinningTrainerTV.Resources.Converters;
using System.Text.Json.Serialization;

namespace SpinningTrainerTV.Resources.Charts
{
    public class DataPoint
    {
        public double Time { get; set; }
        public double Intensity { get; set; }
        public double Duration { get; set; }

        [JsonConverter(typeof(ColorJsonConverter))]
        public Microsoft.Maui.Graphics.Color SegmentColor { get; set; }

        public DataPoint(double time, double intensity, double duration, Microsoft.Maui.Graphics.Color segmentColor)
        {
            Time = time;
            Intensity = intensity;
            Duration = duration;
            SegmentColor = segmentColor;
        }
    }
}
