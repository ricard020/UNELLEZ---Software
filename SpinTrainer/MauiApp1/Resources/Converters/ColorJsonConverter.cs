using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpinTrainer.Resources.Converters
{
    public class ColorJsonConverter : JsonConverter<Microsoft.Maui.Graphics.Color>
    {
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string hex = reader.GetString();
            hex = hex.StartsWith("#") ? hex.Substring(1) : hex;
            int a = Convert.ToInt32(hex.Substring(0, 2), 16);
            int r = Convert.ToInt32(hex.Substring(2, 2), 16);
            int g = Convert.ToInt32(hex.Substring(4, 2), 16);
            int b = Convert.ToInt32(hex.Substring(6, 2), 16);


            if (r == 0 && g == 0 && b == 0)
                return Color.FromHex("#E18417");
            else
                return Color.FromRgba(r / 255.0, g / 255.0, b / 255.0, a / 255.0);

        }

        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
        {
            string hex = $"#{(int)(value.Alpha * 255):X2}{(int)(value.Red * 255):X2}{(int)(value.Green * 255):X2}{(int)(value.Blue * 255):X2}";

            writer.WriteStringValue(hex);
        }
    }
}
