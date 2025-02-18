namespace SpinningTrainerTV.Resources.Charts
{
    public class GraphDrawable : IDrawable
    {
        private List<DataPoint> _dataPoints;
        public double Progress { get; set; }
        public int CurrentSegmentIndex { get; set; }

        public GraphDrawable(List<DataPoint> dataPoints)
        {
            _dataPoints = dataPoints;
            Progress = 0.0;
            CurrentSegmentIndex = 0;
        }

        private readonly (int min, int max, Microsoft.Maui.Graphics.Color color)[] intensityRanges = new[]
        {
                (50, 60, Microsoft.Maui.Graphics.Color.FromRgb(27, 128, 170)),  // 50-60 -> RGB(27,128,170)
                (60, 70, Microsoft.Maui.Graphics.Color.FromRgb(107, 170, 30)),  // 60-70 -> RGB(107,170,30)
                (70, 80, Microsoft.Maui.Graphics.Color.FromRgb(224, 193, 0)),   // 70-80 -> RGB(224,193,0)
                (80, 90, Microsoft.Maui.Graphics.Color.FromRgb(225, 118, 0)),   // 80-90 -> RGB(225,118,0)
                (90, 100, Microsoft.Maui.Graphics.Color.FromRgb(219, 17, 28)),  // 90-100 -> RGB(219,17,28)
            };

        public double GetTotalTimeInSeconds()
        {
            double totalMinutes = 0;
            for (int i = 1; i < _dataPoints.Count; i++)
                totalMinutes += _dataPoints[i].Duration;

            return totalMinutes * 60;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 4;

            // Dimensiones del área gráfica
            float graphWidth = dirtyRect.Width - 20;
            float graphHeight = dirtyRect.Height - 20;
            float originX = 10;
            float originY = graphHeight + 10;

            float maxTime = (float)_dataPoints[^1].Time;
            float maxIntensity = 100;
            float minIntensity = 50;

            // Dibujar los colores de fondo según los rangos
            foreach (var (min, max, color) in intensityRanges)
            {
                // Calcular la posición correspondiente en el eje Y según el rango de intensidad
                float topY = originY - (float)((max - minIntensity) / (maxIntensity - minIntensity) * graphHeight);
                float bottomY = originY - (float)((min - minIntensity) / (maxIntensity - minIntensity) * graphHeight);

                // Dibujar el rectángulo para ese rango de color
                canvas.FillColor = color;
                canvas.FillRectangle(originX, bottomY, graphWidth, topY - bottomY);
            }

            // Dibujar ejes
            canvas.DrawLine(originX, originY, originX + graphWidth, originY); // Eje X
            canvas.DrawLine(originX, originY, originX, originY - graphHeight); // Eje Y

            // Dibujar segmentos progresivos
            for (int i = 0; i <= CurrentSegmentIndex && i < _dataPoints.Count - 1; i++)
            {
                var start = _dataPoints[i];
                var end = _dataPoints[i + 1];
                float segmentProgress = i == CurrentSegmentIndex ? (float)Progress : 1.0f;

                float x1 = originX + (float)(start.Time / maxTime) * graphWidth;
                float y1 = originY - (float)((start.Intensity - minIntensity) / (maxIntensity - minIntensity) * graphHeight);
                float x2 = originX + (float)(end.Time / maxTime) * graphWidth;
                float y2 = originY - (float)((end.Intensity - minIntensity) / (maxIntensity - minIntensity) * graphHeight);

                canvas.StrokeColor = end.SegmentColor;

                float currentX = x1 + (x2 - x1) * segmentProgress;
                float currentY = y1 + (y2 - y1) * segmentProgress;

                canvas.DrawLine(x1, y1, currentX, currentY);

                // Indicador actual
                if (i == CurrentSegmentIndex)
                {
                    canvas.FillColor = Colors.Red;
                    canvas.FillCircle(currentX, currentY, 5);

                    // Valor de intensidad
                    canvas.FontSize = 20;
                    canvas.FontColor = Colors.White;
                    canvas.DrawString($"{(int)(start.Intensity + (end.Intensity - start.Intensity) * segmentProgress)}%",
                                      currentX + 10, currentY - 10, HorizontalAlignment.Left);
                }
            }
        }
    }
}
