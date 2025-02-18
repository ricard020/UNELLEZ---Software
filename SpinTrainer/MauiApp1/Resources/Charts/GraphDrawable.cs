namespace SpinTrainer.Resources.Charts
{
    internal class GraphDrawable : IDrawable
    {
        public List<DataPoint> DataPoints;
        public double Progress { get; set; }
        public int CurrentSegmentIndex { get; set; }

        public GraphDrawable(List<DataPoint> dataPoints)
        {
            DataPoints = dataPoints;
            Progress = 0.0;
            CurrentSegmentIndex = 0;
        }

        public double GetTotalTimeInSeconds()
        {
            double totalMinutes = 0;
            for (int i = 1; i < DataPoints.Count; i++)
                totalMinutes += DataPoints[i].Duration;

            return totalMinutes * 60;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.WhiteSmoke;
            canvas.StrokeSize = 4;

            // Dimensiones del área gráfica
            float graphWidth = dirtyRect.Width - 20;
            float graphHeight = dirtyRect.Height - 20;
            float originX = 10;
            float originY = graphHeight + 10;

            float maxTime = (float)DataPoints[^1].Time;
            float maxIntensity = 51;
            float minIntensity = 50;

            // Dibujar ejes
            canvas.DrawLine(originX, originY, originX + graphWidth, originY); // Eje X
            //canvas.DrawLine(originX, originY, originX, originY - graphHeight); // Eje Y

            // Dibujar segmentos progresivos
            for (int i = 0; i <= CurrentSegmentIndex && i < DataPoints.Count - 1; i++)
            {
                var start = DataPoints[i];
                var end = DataPoints[i + 1];
                float segmentProgress = i == CurrentSegmentIndex ? (float)Progress : 1.0f;

                float x1 = originX + (float)(start.Time / maxTime) * graphWidth;
                float y1 = originY - (float)((50 - minIntensity) / (maxIntensity - minIntensity) * graphHeight);
                float x2 = originX + (float)(end.Time / maxTime) * graphWidth;
                float y2 = originY - (float)((50 - minIntensity) / (maxIntensity - minIntensity) * graphHeight);

                canvas.StrokeColor = end.SegmentColor;

                float currentX = x1 + (x2 - x1) * segmentProgress;
                float currentY = y1 + (y2 - y1) * segmentProgress;

                canvas.DrawLine(x1, y1, currentX, currentY);

                // Indicador actual
                if (i == CurrentSegmentIndex)
                {
                    canvas.FillColor = Color.FromHex("#E18417");
                    canvas.FillCircle(currentX, currentY, 5);
                }
            }
        }
    }
}
