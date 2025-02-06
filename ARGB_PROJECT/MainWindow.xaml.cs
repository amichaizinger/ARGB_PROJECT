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
                StatusText.Text = "press the button (it might take a few seconds)";
                bitmap = new Bitmap(ImageSource);
                image.BeginInit();
                image.UriSource = new Uri(ImageSource);
                image.EndInit();
                UploadedImage.Source = image;
                UploadedImage.Visibility = Visibility.Visible;
                convert.Visibility = Visibility.Visible;
                chooseImage.Visibility= Visibility.Collapsed;
            }
        }

        private async void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            UploadedImage.Visibility = Visibility.Collapsed;
            convert.Visibility = Visibility.Collapsed;
            ColorPics();
            StatusText.Text = "Conversion completed!";
            UploadedImage.Visibility = Visibility.Collapsed;
            original.Source = image;
            chooseImage.Visibility = Visibility.Visible;
        }

        public void ColorPics()
        {
            // Your heavy task (e.g., image processing)
            int height = bitmap.Height;  // Get height from System.Drawing.Bitmap
            int width = bitmap.Width;    // Get width from System.Drawing.Bitmap

            Bitmap RedBitmap = new Bitmap(width, height);
            Bitmap GreenBitmap = new Bitmap(width, height);
            Bitmap BlueBitmap = new Bitmap(width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    System.Drawing.Color pixelColor = bitmap.GetPixel(i, j);
                    int r = pixelColor.R;
                    int g = pixelColor.G;
                    int b = pixelColor.B;

                    // Set pixels for each color channel
                    RedBitmap.SetPixel(i, j, System.Drawing.Color.FromArgb(r, 0, 0));  // Red channel
                    GreenBitmap.SetPixel(i, j, System.Drawing.Color.FromArgb(0, g, 0));  // Green channel
                    BlueBitmap.SetPixel(i, j, System.Drawing.Color.FromArgb(0, 0, b));  // Blue channel
                }
            }

            // Convert Bitmaps to BitmapImages for display
            BitmapImage RedImage = ConvertBitmapToBitmapImage(RedBitmap);
            BitmapImage GreenImage = ConvertBitmapToBitmapImage(GreenBitmap);
            BitmapImage BlueImage = ConvertBitmapToBitmapImage(BlueBitmap);


            Dispatcher.Invoke(() =>
            {
                red.Source = RedImage;
                green.Source = GreenImage;
                blue.Source = BlueImage;
                Pictures.Visibility = Visibility.Visible;
            });
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