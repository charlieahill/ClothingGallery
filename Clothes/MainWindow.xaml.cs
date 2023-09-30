using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Clothes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The full file path including root, filename, and extension of the file that contains the serialized list of clothes
        /// </summary>
        /// <example>Documents/CHillSW/Clothes/clothes.not</example>
        public string FilePath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"/CHillSW/Clothes/clothes.not";
        public string OutputFilePath { get; set; } = @"/CHillSW/Clothes/images/";

        /// <summary>
        /// The list of clothes in the pgoram
        /// </summary>
        BindingList<ClothingModel> clothes = new BindingList<ClothingModel>();

        private int CurrentImageIndex { get; set; } = 0;

        public MainWindow()
        {
            InitializeComponent();
            clothes = SaveLoad.Load(FilePath);
            RefreshListBoxItemsSource();
            ClothesListBox.SelectedIndex = 0;
        }

        /// <summary>
        /// On close of window save program data
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            clothes.Save(FilePath);
        }

        /// <summary>
        /// Creates a new item of clothing when the user clicks the add button
        /// </summary>
        private void AddNewClothingButton_Click(object sender, RoutedEventArgs e)
        {
            clothes.Add(new ClothingModel());
            RefreshListBoxItemsSource();
        }

        private void RefreshListBoxItemsSource()
        {
            ClothesListBox.ItemsSource = null;
            ClothesListBox.ItemsSource = clothes;

            if (clothes.Count > 0)
                ClothesListBox.SelectedItem = ClothesListBox.Items[ClothesListBox.Items.Count - 1];
        }

        /// <summary>
        /// Save current clothing item, and load in next note on selection changed
        /// </summary>
        private void ClothesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataContext = (ClothingModel)ClothesListBox.SelectedItem;
            LoadAndDisplayImage(0);
        }

        private void ButtonPreviousImage_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext == null) return;
            if ((DataContext as ClothingModel).ImageFilepaths.Count == 0) return;

            if (CurrentImageIndex == 0)
                LoadAndDisplayImage((DataContext as ClothingModel).ImageFilepaths.Count - 1);
            else
                LoadAndDisplayImage(CurrentImageIndex - 1);
        }

        private void ImageFromClipboardButton_Click(object sender, RoutedEventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                SaveImage(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + OutputFilePath);
                LoadAndDisplayImage((DataContext as ClothingModel).ImageFilepaths.Count - 1);
                return;
            }

            if (Clipboard.ContainsFileDropList() && (Clipboard.GetFileDropList().Count > 0))
            {
                string filepathFromClipboard = Clipboard.GetFileDropList()[0];

                string newFilepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + OutputFilePath;
                newFilepath = newFilepath.Remove(0, (Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/").Length);
                try
                {
                    File.Copy(filepathFromClipboard, newFilepath);
                    (DataContext as ClothingModel).ImageFilepaths.Add(newFilepath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to copy file: " + ex.ToString());
                }
            }
            return;
        }

        private void SaveImage(string filesFolder)
        {
            string imageExtension = ".bmp";

            if (!Directory.Exists(filesFolder))
            {
                try
                {
                    Directory.CreateDirectory(filesFolder);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to create directory to save files.");
                    Console.WriteLine("Unable to create directory to save files. Error: " + ex.ToString());
                    return;
                }
            }

            filesFolder += DateTime.Now.ToString("yyyyMMddHHmmss");

            while (File.Exists(filesFolder + imageExtension))
                filesFolder += "1";

            filesFolder += imageExtension;

            Console.WriteLine("Saving file to: " + filesFolder);

            IDataObject clipboardData = Clipboard.GetDataObject();

            if (clipboardData == null) return;

            SaveClipboardImageToFile(filesFolder);

            (DataContext as ClothingModel).ImageFilepaths.Add(filesFolder.Remove(0, (Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/").Length));
        }

        /// <summary>
        /// from https://stackoverflow.com/questions/2900447/how-to-save-a-wpf-bitmapsource-image-to-a-file
        /// </summary>
        /// <param name="filePath"></param>
        public static void SaveClipboardImageToFile(string filePath)
        {
            var image = Clipboard.GetImage();
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
            }
        }

        private void ButtonNextImage_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext == null) return;
            if ((DataContext as ClothingModel).ImageFilepaths.Count == 0) return;

            if (CurrentImageIndex == (DataContext as ClothingModel).ImageFilepaths.Count - 1)
                LoadAndDisplayImage(0);
            else
                LoadAndDisplayImage(CurrentImageIndex + 1);
        }

        /// <summary>
        /// Loads and displays the given image for the day
        /// Ammends the currentImageIndex with the update
        /// </summary>
        /// <param name="indexToDisplay"></param>
        private void LoadAndDisplayImage(int indexToDisplay)
        {
            CurrentImageIndex = indexToDisplay;

            if (DataContext == null) return;

            //Load first image
            if ((DataContext as ClothingModel).ImageFilepaths.Count == 0)
                ImageBox.Source = null;
            else
                LoadImageFromFile(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/" + (DataContext as ClothingModel).ImageFilepaths[CurrentImageIndex]);
        }

        /// <summary>
        /// Loads a given image from file to the onscreen picturebox
        /// </summary>
        /// <param name="filePath">The path of the image to load</param>
        private void LoadImageFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                ImageBox.Source = null;
                return;
            }

            Uri UriSource = new Uri(filePath);
            ImageBox.Source = new BitmapImage(UriSource);
        }
    }
}
