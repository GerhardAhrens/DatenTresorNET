//-----------------------------------------------------------------------
// <copyright file="GeometryTools.cs" company="Lifeprojects.de">
//     Class: GeometryTools
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>04.12.2023 13:48:01</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace Mainova.Tools.Grafik
{
    using System.IO;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Shapes = System.Windows.Shapes;

    public static class GeometryTools
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryTools"/> class.
        /// </summary>
        static GeometryTools()
        {
        }

        public static Shapes.Path GetPathGeometry(string iconString, Color iconColor)
        {
            var path = new Shapes.Path
            {
                Height = 24,
                Width = 24,
                Fill = new SolidColorBrush(iconColor),
                Data = Geometry.Parse(iconString)
            };
            return path;
        }

        public static Shapes.Path GetPathGeometry(string iconString)
        {
            return GetPathGeometry(iconString, Colors.Blue);
        }

        public static ImageSource GeometryToImageSource(Geometry geometry, int TargetSize, Brush foregroundColor)
        {
            var rect = geometry.GetRenderBounds(new Pen(Brushes.Black, 0));

            var bigger = rect.Width > rect.Height ? rect.Width : rect.Height;
            var scale = TargetSize / bigger;

            Geometry scaledGeometry = Geometry.Combine(geometry, geometry, GeometryCombineMode.Intersect, new ScaleTransform(scale, scale));
            rect = scaledGeometry.GetRenderBounds(new Pen(Brushes.Black, 0));

            Geometry transformedGeometry = Geometry.Combine(scaledGeometry, scaledGeometry, GeometryCombineMode.Intersect, new TranslateTransform(((TargetSize - rect.Width) / 2) - rect.Left, ((TargetSize - rect.Height) / 2) - rect.Top));

            RenderTargetBitmap bmp = new RenderTargetBitmap(TargetSize, TargetSize, 96, 96, PixelFormats.Pbgra32);

            DrawingVisual viz = new DrawingVisual();
            using (DrawingContext dc = viz.RenderOpen())
            {
                dc.DrawGeometry(foregroundColor, null, transformedGeometry);
            }

            bmp.Render(viz);

            var mem = new MemoryStream();
            PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(bmp));
            pngEncoder.Save(mem);
            var itm = GetImg(mem);
            return itm;
        }

        public static ImageSource GeometryToImageSource(Geometry geometry)
        {
            return GeometryToImageSource(geometry, 16, Brushes.Black);
        }

        public static ImageSource GeometryToImageSource(Geometry geometry, Brush foregroundColor)
        {
            return GeometryToImageSource(geometry, 16, foregroundColor);
        }

        private static BitmapImage GetImg(MemoryStream ms)
        {
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.StreamSource = ms;
            bmp.EndInit();

            return bmp;
        }
    }
}
