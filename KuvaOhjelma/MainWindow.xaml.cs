/*
 * Windows-ohjelmointi IIO11300 Harjoitustyö
 * Kuvien selaus ohjelma
 * 
 * Copyright: Sami Antila 2014 
 * Created: 8.3.2014
 *
 * Resize luokka hoitaa kuvien koon muutokset
 *
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KuvaOhjelma
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        
        List<int> selectedPhotos;
        ObservableCollection<Image> thumbnails;
        Folder folder;
        #region GUI
        public MainWindow()
        {
            InitializeComponent();
            InitMyStuff();

        }
        public void InitMyStuff()
        {
            try
            {
                selectedPhotos = new List<int>();
                thumbnails = new ObservableCollection<Image>();
                folder = new Folder();
                UpdateGUIContent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        public ImageSource ChangeImage(bool isForward)
        {
            try
            {
                if (isForward && lstThumbnails.Items.Count > lstThumbnails.SelectedIndex)
                {
                    lstThumbnails.SelectedIndex = lstThumbnails.SelectedIndex + 1;
                    lstThumbnails.Focus();
                    /*if (!lstThumbnails.Items.MoveCurrentToNext())
                    {
                        lstThumbnails.Items.MoveCurrentToLast();
                    }*/
                    return imgStage.Source;
                }
                else if (lstThumbnails.SelectedIndex - 1 >= 0)
                {
                    lstThumbnails.SelectedIndex = lstThumbnails.SelectedIndex - 1;
                    lstThumbnails.Focus();
                    /*if (!lstThumbnails.Items.MoveCurrentToPrevious())
                    {
                        lstThumbnails.Items.MoveCurrentToFirst();
                    }*/
                    return imgStage.Source;
                }
                return imgStage.Source;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void SelectPhoto()
        {
            //tarkista onko kuva valituissa
            if (selectedPhotos.Exists(i => i == lstThumbnails.SelectedIndex))
            {
                //poista kuva valituista
                selectedPhotos.Remove(lstThumbnails.SelectedIndex);
                btnSelectPhoto.IsEnabled = true;
                btnUnselectPhoto.IsEnabled = false;
                txtSelectedPhotosCount.Text = "Currently selected photos: " + selectedPhotos.Count.ToString();
            }
            else
            {
                //lisää kuva valittuihin
                selectedPhotos.Add(lstThumbnails.SelectedIndex);
                btnSelectPhoto.IsEnabled = false;
                btnUnselectPhoto.IsEnabled = true;
                txtSelectedPhotosCount.Text = "Currently selected photos: " + selectedPhotos.Count.ToString();
            }
        }
        public void ResizeAllPhotos(int p_width)
        {
            try
            {
                int fileCount = folder.resizeAllPhotos(p_width);
                lstThumbnails.SelectedIndex = 1;
                thumbnails.Clear();
                selectedPhotos.Clear();
                UpdateGUIContent();
                MessageBox.Show(fileCount + " Photo(s) renamed and moved to \n" + folder.FolderPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void ResizeSelectedPhotos(int p_width)
        {
            //tarkista onko yhtään valokuvaa valittuna
            if (selectedPhotos.Any())
            {
                try
                {
                    int fileCount = folder.resizeSelectedPhotos(selectedPhotos, p_width);
                    lstThumbnails.SelectedIndex = 1;
                    thumbnails.Clear();
                    selectedPhotos.Clear();
                    UpdateGUIContent();
                    MessageBox.Show(fileCount + " Photo(s) resized, renamed and copied to \n" + folder.FolderPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void OpenFileDialogAndGetData()
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".JPG";
                dlg.Filter = "JPEG (.JPG)|*.JPG";
                Nullable<bool> result = dlg.ShowDialog();
                //Tarkistus, jos painaa cancel -> skippaa loput metodin toiminnot, muuten error
                if (result == true) {
                    string filePath = dlg.FileName;
                    string folderPath = System.IO.Path.GetDirectoryName(dlg.FileName);
                    folder.updateCurrentFolder(folderPath, filePath);
                    UpdateGUIContent();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void UpdateGUIContent()
        {
            try
            {
                thumbnails.Clear();
                selectedPhotos.Clear();
                btnSelectPhoto.IsEnabled = true;
                btnUnselectPhoto.IsEnabled = false;
                txtSelectedPhotosCount.Text = "Currently selected photos: " + selectedPhotos.Count().ToString();

                foreach (Photo foto in folder.Photos)
                {
                    thumbnails.Add(foto.Thumbnail);
                }
                txtBoxFolder.Text = folder.FolderPath;
                lstThumbnails.DataContext = thumbnails;
                if (folder.Photos.Count > 0)
                {
                    ImageToStage(folder.Photos[folder.selectedPhotoIndex].FilePath);
                }
                lstThumbnails.SelectedIndex = folder.selectedPhotoIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ImageToStage(string filename)
        {
            try
            {
                BitmapImage BitmapImage = new BitmapImage();
                BitmapImage.BeginInit();
                BitmapImage.UriSource = new Uri(@filename);
                BitmapImage.EndInit();
                imgStage.Source = BitmapImage;
                txtFileNameBottom.Text = filename;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }      
        private void OpenFullScreenView()
        {
            var newW = new FullScreenView(imgStage.Source);
            newW.Owner = this; 
            newW.ShowDialog();
        }
        private void RenameAndCopySelectedPhotos()
        {
            if (selectedPhotos.Any()) {
                try
                {
                    int fileCount = folder.renameSelectedPhotos(selectedPhotos);
                    UpdateGUIContent();
                    MessageBox.Show(fileCount + " Photo(s) renamed and copied to \n" + folder.FolderPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void ConvertSelectedPhotosToPng()
        {
            //tarkista onko yhtään valokuvaa valittuna
            if (selectedPhotos.Any()) { 
                try
                {
                    int fileCount = folder.ConvertSelectedToPng(selectedPhotos);
                    selectedPhotos.Clear();
                    UpdateGUIContent();
                    MessageBox.Show(fileCount + " Photo(s) converted to PNG and moved to \n" + folder.FolderPath + "\\converted_to_PNG");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void OpenResizeDialog(string type)
        {
            var newW = new Resize(type);
            newW.Owner = this;
            newW.ShowDialog();
        }
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            //window width changed + limits, elementtien koko ei voi olla negatiivinen
            if (sizeInfo.WidthChanged && sizeInfo.NewSize.Width > 500)
            {
                txtBoxFolder.Width = sizeInfo.NewSize.Width - 200;
                imgStage.Width = sizeInfo.NewSize.Width - 400;
            }
            //window height changed + limits, elementtien koko ei voi olla negatiivinen
            if (sizeInfo.HeightChanged && sizeInfo.NewSize.Height > 200)
            {
                imgStage.Height = sizeInfo.NewSize.Height - 40 - 100;
                lstThumbnails.Height = sizeInfo.NewSize.Height - 100 - 100;
            }
        }
        #endregion
        #region COMMANDS
        private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialogAndGetData();
        }
        private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion
        #region ACTIONS
        private void imgStage_KeyDown(object sender, KeyEventArgs e)
        {
        }
        private void lstThumbnails_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            lstThumbnails.SelectedIndex = folder.selectedPhotoIndex;
        }
        private void btnChangeFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialogAndGetData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void lstThumbnails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //kansiota vaihtaessa selectedindex saattaa jäädä liian suureksi
                if (folder.Photos.Any() && lstThumbnails.SelectedIndex < folder.Photos.Count && lstThumbnails.SelectedItems.Count > 0)
                {
                    //hakee listbox valittu -> lataa ison kuvan isoon näkymään
                    int index = lstThumbnails.SelectedIndex;
                    if (selectedPhotos.Contains(index))
                    {
                        btnSelectPhoto.IsEnabled = false;
                        btnUnselectPhoto.IsEnabled = true;
                    }
                    else
                    {
                        btnSelectPhoto.IsEnabled = true;
                        btnUnselectPhoto.IsEnabled = false;
                    }
                    //MessageBox.Show(index + folder.Photos.Count.ToString() + lstThumbnails.SelectedIndex);
                    //MessageBox.Show(index.ToString());
                    //MessageBox.Show(filesInFolder.Count.ToString());
                    string filePath = folder.Photos[index].FilePath;
                    ImageToStage(filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void lstThumbnails_KeyDown(object sender, KeyEventArgs e)
        {
            //tarkista onko listbox tyhjä
            if (lstThumbnails.Items.Count > 0)
            {
                switch (e.Key)
                {
                    case Key.Right:
                        ChangeImage(true);
                        break;
                    case Key.Left:
                        ChangeImage(false);
                        break;
                    case Key.E:
                        SelectPhoto();
                        break;
                    default:
                        return;
                }
            }
            e.Handled = true;
        }
        private void imgStage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                OpenFullScreenView();
            }
            e.Handled = true;
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    OpenFullScreenView();
                    break;
                case Key.Right:
                    ChangeImage(true);
                    break;
                case Key.Left:
                    ChangeImage(false);
                    break;
                default:
                    return;
            }
            e.Handled = true;
        }
        private void menuFullScreen_Click(object sender, RoutedEventArgs e)
        {
            OpenFullScreenView();
        }
        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void menRenameSelected_Click(object sender, RoutedEventArgs e)
        {
            RenameAndCopySelectedPhotos();
        }
        private void btnSelectPhoto_Click(object sender, RoutedEventArgs e)
        {
            SelectPhoto();
        }
        private void menResizeAll_Click(object sender, RoutedEventArgs e)
        {
            OpenResizeDialog("all");
        }
        private void menResizeSelected_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPhotos.Any())
            {
                OpenResizeDialog("selected");
            }
        }
        private void menItemConvertToPng_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void menItemConvertSelectedToPNG_Click(object sender, RoutedEventArgs e)
        {
            ConvertSelectedPhotosToPng();
        }
        #endregion
    }
}