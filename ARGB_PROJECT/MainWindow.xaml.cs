using System;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Interop;
using System.Reflection;




namespace RGB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Bitmap bitmap;
        public BitmapImage image = new BitmapImage();
        public MainWindow()
        {
            InitializeComponent();
        }

        public void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff|All Files|*.*";
            if (open.ShowDialog() == true)
            {             
                string ImageSource = open.FileName;
                StatusText.Text = "Perfect we have a picture lets proceed";
                bitmap = new Bitmap(ImageSource);
                image.UriSource = new Uri(ImageSource);

                UploadedImage.Source = image;
                UploadedImage.Visibility = Visibility.Visible;
                convert.Visibility = Visibility.Visible;
                chooseImage.Visibility= Visibility.Collapsed;
            }
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            ColorPics();
            }
        public void ColorPics()
        {
            int height = (int)(image.Height);
            int width = (int)(image.Width);
            Bitmap RedBitmap= new Bitmap( 200, 200);
            Bitmap BlueBitmap= new Bitmap(width, height);
            Bitmap GreenBitmap= new Bitmap(width, height);

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    System.Drawing.Color pixelColor = bitmap.GetPixel(i, j);
                    int r = 0, g = 0, b = 0;
                    r = pixelColor.R;
                    g = pixelColor.G;
                    b = pixelColor.B;
                    GreenBitmap.SetPixel(i, j, System.Drawing.Color.FromArgb(0, g, 0));
                    BitmapImage GreenImage = ConvertBitmapToBitmapImage(GreenBitmap);
                    green.Source=GreenImage;
                    Pictures.Visibility = Visibility.Visible;
                }  
            }

        }

        static BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
        {
            // Step 1: Create a MemoryStream to hold the image data
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Step 2: Save the Bitmap to MemoryStream as a PNG image
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

                // Step 3: Create a BitmapImage
                memoryStream.Seek(0, SeekOrigin.Begin); // Move to the beginning of the stream
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream; // Set the stream as the source
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad; // Cache the image
                bitmapImage.EndInit(); // Finalize the initialization

                return bitmapImage;
            }
        }
    }
}