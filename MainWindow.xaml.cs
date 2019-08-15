using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;


namespace app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string selectedFileName;
        private Bitmap img;
        private int[,] pixelPositions;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void UploadImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();  
            dlg.InitialDirectory = "c:\\";  
            dlg.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";  
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog() == true)  
            {  
                selectedFileName = dlg.FileName;  
                labelImage.Content = selectedFileName;
                // get an image to draw on and convert it to our chosen format
                img = new Bitmap(selectedFileName);  
                imageViewer.Source = BitmapToImageSource(img);
            }  
        }
        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
        private void ColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
            if(findPixel.IsChecked == true)
            {
                pixelPositions = new int[img.Width, img.Height];
                FindPixel(sender, e);
            }else if(changePixel.IsChecked == true)
            {
                ChangePixel(sender, e);
            }
		}
        private void FindPixel(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int pixelsFound = 0;
            for(int i = 0; i < img.Width; i++)
            {
                for(int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);
                    if(pixel.R == slColorR.Value && pixel.G == slColorG.Value && pixel.B == slColorB.Value)
                    {
                        pixelPositions[i,j] = 1;
                        pixelsFound++;
                    }
                }
            }
            pixelAmountFound.Content = "Pixels found: " + pixelsFound;
        }
        private void ChangePixel(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            for(int i = 0; i < pixelPositions.GetLength(0); i++)
            {
                for(int j = 0; j < pixelPositions.GetLength(1); j++)
                {
                    if(pixelPositions[i,j] == 1)
                    {
                        img.SetPixel(i, j, Color.FromArgb((int)slColorR.Value, (int)slColorG.Value, (int)slColorB.Value));
                    }
                }
            }
            imageViewer.Source = BitmapToImageSource(img);
        }
    }
}