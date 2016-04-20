using System.Linq;
using System.Windows;
using System.Drawing;
using System.IO;

namespace RGB_Decomposer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files.Count() != 1)
            {
                MessageBox.Show("Поддерживается обработка только одного файла.");
                return;
            }

            var filename = Path.GetFileNameWithoutExtension(files[0]);

            Bitmap image;

            try
            {
                image = (Bitmap)Image.FromFile(files[0]);
            }
            catch
            {
                MessageBox.Show("Не поддерживаемый тип файла.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            GetColorLayerOfImage(image, ColorLayer.Red).Save(filename + "_R.bmp");
            GetColorLayerOfImage(image, ColorLayer.Green).Save(filename + "_G.bmp");
            GetColorLayerOfImage(image, ColorLayer.Blue).Save(filename + "_B.bmp");

            MessageBox.Show("Готово! Слои в папке с программой.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        static Bitmap GetColorLayerOfImage(Bitmap image, ColorLayer whichLayer)
        {
            var layer = new Bitmap(image.Width, image.Height);

            for(int y = 0; y < image.Height; y++)
                for(int x = 0; x < image.Width; x++)
                {
                    var originalPixelColor = image.GetPixel(x, y);

                    var colorScale = 0;

                    switch(whichLayer)
                    {
                        case ColorLayer.Red: colorScale = originalPixelColor.R; break;
                        case ColorLayer.Green: colorScale = originalPixelColor.G; break;
                        case ColorLayer.Blue: colorScale = originalPixelColor.B; break;
                    }

                    colorScale = 255 - colorScale;

                    layer.SetPixel(x, y, Color.FromArgb(colorScale, colorScale, colorScale));

                    //var newR = whichLayer == ColorLayer.Red ?  0 : (255 - originalPixelColor.R);
                    //var newG = whichLayer == ColorLayer.Green ? 0 : (255 - originalPixelColor.G);
                    //var newB = whichLayer == ColorLayer.Blue ? 0 : (255 - originalPixelColor.B);

                    //layer.SetPixel(x, y, Color.FromArgb(newR, newG, newB));
                }

            return layer;
        }

        enum ColorLayer
        {
            Red,
            Green,
            Blue
        }
    }
}
