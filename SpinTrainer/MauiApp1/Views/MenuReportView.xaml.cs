using ENTITYS;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SpinningTrainer.Views
{
    public partial class MenuReportView : ContentPage
    {
        public class SessionModel
        {
            public int ID { get; set; }
            public int IDEntrenador { get; set; }
            public string Descrip { get; set; }
            public DateTime FechaC { get; set; }
            public DateTime FechaI { get; set; }
            public int Duracion { get; set; }
            public int EsPlantilla { get; set; }

            public ObservableCollection<MovimientoModel> Movimientos { get; set; }
        }

        public class MovimientoModel
        {
            public int ID { get; set; }
            public string Descrip { get; set; }
            public int TipoMov { get; set; }
            public int RPMMin { get; set; }
            public int RPMMax { get; set; }
            public string PosicionesDeManos { get; set; }
        }

        ObservableCollection<SessionModel> infoSesiones/* = new ObservableCollection<SessionEntity>()*/;

        public MenuReportView()
        {
            InitializeComponent();

            // Agregar sesiones de ejemplo
            var today = DateTime.Today; // Fecha actual del dispositivo
            infoSesiones.Add(new SessionModel
            {
                ID = 1,
                IDEntrenador = 1,
                Descrip = "Sesión de Inicio",
                FechaC = today.AddDays(-5),
                FechaI = today.AddDays(-5).AddHours(9),
                Duracion = 60,
                EsPlantilla = 1,
                Movimientos = new ObservableCollection<MovimientoModel>
                {
                    new MovimientoModel { ID = 1, Descrip = "Movimiento 1", TipoMov = 1, RPMMin = 50, RPMMax = 100, PosicionesDeManos = "Posición 1" },
                    new MovimientoModel { ID = 2, Descrip = "Movimiento 2", TipoMov = 2, RPMMin = 60, RPMMax = 110, PosicionesDeManos = "Posición 2" }
                }
            });

            infoSesiones.Add(new SessionModel
            {
                ID = 2,
                IDEntrenador = 1,
                Descrip = "Sesión Intermedia",
                FechaC = today.AddDays(-3),
                FechaI = today.AddDays(-3).AddHours(10),
                Duracion = 45,
                EsPlantilla = 0,
                Movimientos = new ObservableCollection<MovimientoModel>
                {
                    new MovimientoModel { ID = 3, Descrip = "Movimiento 3", TipoMov = 1, RPMMin = 55, RPMMax = 105, PosicionesDeManos = "Posición 3" },
                    new MovimientoModel { ID = 4, Descrip = "Movimiento 4", TipoMov = 2, RPMMin = 65, RPMMax = 115, PosicionesDeManos = "Posición 4" }
                }
            });

            infoSesiones.Add(new SessionModel
            {
                ID = 3,
                IDEntrenador = 2,
                Descrip = "Sesión Avanzada",
                FechaC = today.AddDays(-1),
                FechaI = today.AddDays(-1).AddHours(8),
                Duracion = 75,
                EsPlantilla = 1,
                Movimientos = new ObservableCollection<MovimientoModel>
                {
                    new MovimientoModel { ID = 5, Descrip = "Movimiento 5", TipoMov = 1, RPMMin = 60, RPMMax = 110, PosicionesDeManos = "Posición 5" },
                    new MovimientoModel { ID = 6, Descrip = "Movimiento 6", TipoMov = 2, RPMMin = 70, RPMMax = 120, PosicionesDeManos = "Posición 6" }
                }
            });

            // Filtrar sesiones cuya fecha sea igual o anterior a la fecha actual
            infoSesiones = new ObservableCollection<SessionModel>(infoSesiones.Where(s => s.FechaC <= today));

            // Establecer el origen de datos para el ListView
            lvInfoSesiones.ItemsSource = infoSesiones;
        }

        private async void DownloadImage_Tapped(object sender, EventArgs e)
        {
            try
            {
                var image = (Image)sender;
                var session = (SessionModel)image.BindingContext;

                // Aquí va la lógica para descargar el PDF
                await DownloadPdfAsync(session);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Se produjo un error al generar el PDF: {ex.Message}", "OK");
            }
        }

        private async Task DownloadPdfAsync(SessionModel session)
        {
            //try
            //{
            //    var document = new PdfDocument();
            //    var page = document.Pages.AddUserLoggedToLocalBD();
            //    var graphics = page.Graphics;
            //    var font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);

            //    // Título del reporte
            //    var titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 18, PdfFontStyle.Bold);
            //    graphics.DrawString("Reporte por Sesión", titleFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(page.GetClientSize().Width / 2, 10), new PdfStringFormat(PdfTextAlignment.Center));

            //    // Información detallada
            //    float y = 60;
            //    graphics.DrawString($"Fecha: {session.FechaI:dd/MM/yyyy HH:mm}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(10, y));
            //    graphics.DrawString($"Entrenador: Nombre del Entrenador", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(200, y));
            //    y += 20;
            //    graphics.DrawString($"Nro. Sesión: {session.ID}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(10, y));
            //    y += 20;
            //    graphics.DrawString("Movimientos:", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(10, y));
            //    y += 20;

            //    // Listado de movimientos
            //    foreach (var movimiento in session.Movimientos)
            //    {
            //        graphics.DrawString($"- {movimiento.Descrip}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(20, y));
            //        y += 20;
            //    }

            //    graphics.DrawString($"Tiempo Total: {session.Duracion} minutos", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(10, y));
            //    y += 20;

            //    // RPM Min, Med y Max
            //    int rpmMin = session.Movimientos.Min(m => m.RPMMin);
            //    int rpmMax = session.Movimientos.Max(m => m.RPMMax);
            //    int rpmMedio = session.Movimientos.Sum(m => m.RPMMin + m.RPMMax) / (2 * session.Movimientos.Count);
            //    graphics.DrawString($"RPM Min: {rpmMin} | Med: {rpmMedio} | Max: {rpmMax}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(10, y));
            //    y += 20;

            //    // FCM Min y Max (Ejemplo ficticio)
            //    int fcmMin = 120;
            //    int fcmMax = 180;
            //    graphics.DrawString($"FCM Min: {fcmMin} | Max: {fcmMax}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(10, y));
            //    y += 20;

            //    // Zona de Energía (si aplica)
            //    graphics.DrawString($"Zona de Energía: Personalizar según el cálculo", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(10, y));
            //    y += 20;

            //    // Guardar documento PDF
            //    using (var stream = new MemoryStream())
            //    {
            //        document.Save(stream);
            //        document.Close();

            //        var fileName = $"Reporte_Sesion_{session.ID}.pdf";
            //        var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

            //        stream.Seek(0, SeekOrigin.Begin); // Asegurar que el stream esté al inicio

            //        using (var fileStream = File.Create(filePath))
            //        {
            //            await stream.CopyToAsync(fileStream);
            //        }

            //        await DisplayAlert("PDF Guardado", $"El archivo PDF se ha guardado en: {filePath}", "OK");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    await DisplayAlert("Error", $"Se produjo un error al generar el PDF: {ex.Message}", "OK");
            //}
        }
    }
}
