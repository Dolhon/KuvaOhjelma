/*
 * Windows-ohjelmointi IIO11300 Harjoitustyö
 * Kuvien selaus ohjelma
 * 
 * Copyright: Sami Antila 2014 
 * Created: 8.3.2014
 * 
 * Description: Sisältää kansioiden ja kuvatiedostojen käsittelyyn liittyvät luokat
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace KuvaOhjelma
{
    class Folder
    {
        #region PARAMETERS
        public string FolderPath { get; set; }
        public List<Photo> Photos { get; set; }
        public int selectedPhotoIndex { get; set; }
        public List<int> selectedPhotosInd { get; set; }
        #endregion
        #region CONSTRUCTOR
        public Folder()
        {
            try
            {
                string defFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string defFilePath = defFolder + "default.JPG";
                this.Photos = new List<Photo>();
                updateCurrentFolder(defFolder, defFilePath);
                this.selectedPhotosInd = new List<int>();
            }
            catch (Exception)
            {
                throw;
            }
            
        }
        #endregion
        #region METHODS
        public void updateCurrentFolder(string p_folder, string p_filePath)
        {
            try
            {
                int i = 0;
                this.Photos.Clear();
                foreach (string file in getFilesFromFolder(p_folder))
                {
                    this.Photos.Add(new Photo(file));
                    if (file == p_filePath)
                    {
                        this.selectedPhotoIndex = i;
                    }
                    i++;
                }
                this.FolderPath = p_folder;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public string[] getFilesFromFolder(string folder)
        {
            try
            {
                string[] array = Directory.GetFiles(@folder, "*.JPG");
                return array;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int resizeSelectedPhotos(List<int> selectedPhotos, int p_width) 
        {
            try
            {
                //luodaan uusi kansio polku resized_yyyy-MM-dd_HH-mm-ss
                string dateTimeStamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string newDir = FolderPath + "\\resized_" + dateTimeStamp;

                //luodaan uusi kansio, jos ei ole olemassa
                bool dirExists = System.IO.Directory.Exists(newDir);
                if (!dirExists)
                {
                  System.IO.Directory.CreateDirectory(newDir);
                }

                //RenameAndCopy() jokaiselle valokuvalle selectedPhotos listassa
                int i = 0;
                foreach (var index in selectedPhotos)
	              {
                      Photos[index].ResizeAndCopyPhoto(newDir, p_width);
                    i++;
	              }
                selectedPhotoIndex = 0;
                updateCurrentFolder(newDir, Photos[0].FilePath);
                return i;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int resizeAllPhotos(int p_width)
        {
          try
          {
            //luodaan uusi kansio polku resized_yyyy-MM-dd_HH-mm-ss
            string dateTimeStamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string newDir = FolderPath + "\\resized_" + dateTimeStamp;

            //luodaan uusi kansio, jos ei ole olemassa
            bool dirExists = System.IO.Directory.Exists(newDir);
            if (!dirExists)
            {
              System.IO.Directory.CreateDirectory(newDir);
            }

            //RenameAndCopy() jokaiselle valokuvalle selectedPhotos listassa
            int i = 0;
            foreach (var photo in Photos)
            {
                photo.ResizeAndCopyPhoto(newDir, p_width);
                i++;
            }
            this.selectedPhotoIndex = 0;
            updateCurrentFolder(newDir, Photos[0].FilePath);
            return i;
          }
          catch (Exception)
          {

            throw;
          }
        }
        public int renameSelectedPhotos(List<int> selectedPhotos)
        {
            try
            {
                //luodaan uusi kansio polku selected_yyyy-MM-dd_HH-mm-ss
                string dateTimeStamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string newDir = FolderPath + "\\selected_" + dateTimeStamp;

                //luodaan uusi kansio, jos ei ole olemassa
                bool dirExists = System.IO.Directory.Exists(newDir);
                if (!dirExists)
                {
                    System.IO.Directory.CreateDirectory(newDir);
                }

                //RenameAndCopy() jokaiselle valokuvalle selectedPhotos listassa
                int i = 0;
                foreach (var index in selectedPhotos)
                {
                    Photos[index].RenameAndCopy(newDir);
                    i++;
                }
                this.selectedPhotoIndex = 0;
                updateCurrentFolder(newDir, Photos[0].FilePath);
                return i;
                
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int ConvertSelectedToPng(List<int> selectedPhotos)
        {
            try
            {
                //luodaan uusi kansio polku converted_to_png_yyyy-MM-dd_HH-mm-ss
                string dateTimeStamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string newDir = FolderPath + "\\converted_to_png_" + dateTimeStamp;

                //luodaan uusi kansio, jos ei ole olemassa
                bool dirExists = System.IO.Directory.Exists(newDir);
                if (!dirExists)
                {
                    System.IO.Directory.CreateDirectory(newDir);
                }

                //RenameAndCopy() jokaiselle valokuvalle selectedPhotos listassa
                int i = 0;
                foreach (var index in selectedPhotos)
                {
                    Photos[index].ConvertToPng(newDir);
                    i++;
                }
                return i;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
    
    class Photo
    {
        #region PARAMETERS
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public Image Thumbnail { get; set; }
        #endregion
        #region CONSTRUCTOR
        public Photo(string p_filePath)
        {
            try 
	        {	        
		        this.FilePath = p_filePath;
                this.Thumbnail = createThumbnail(p_filePath);
	        }
	        catch (Exception)
	        {
		        throw;
	        }
            
        }
        #endregion
        #region METHODS
        public Image createThumbnail(string filename)
        {
            try
            {
                //Image Elementti
                Image tnImage = new Image();
                tnImage.Width = 150;

                BitmapImage tnBitmapImage = new BitmapImage();

                tnBitmapImage.BeginInit();
                tnBitmapImage.UriSource = new Uri(@filename);
                tnBitmapImage.DecodePixelWidth = 150;
                tnBitmapImage.EndInit();
                tnImage.Source = tnBitmapImage;
                return tnImage;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public BitmapImage BitmapImageFromFile()
        {
            try
            {
                BitmapImage BitmapImage = new BitmapImage();
                BitmapImage.BeginInit();
                BitmapImage.UriSource = new Uri(@FilePath);
                BitmapImage.EndInit();
                return BitmapImage;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void RenameAndCopy(string newDir)
        {
            try
            {
                //kopioi tiedoston
                string dir = Path.GetDirectoryName(@FilePath);
                string newName = "selected_" + Path.GetFileName(@FilePath);
                string newPath = Path.Combine(newDir, newName);
                File.Copy(@FilePath, newPath);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void ResizeAndCopyPhoto(string newDir, int p_width)
        {
            try
            {
                // newDir\resized_tiedostonimi
                string dir = Path.GetDirectoryName(@FilePath);
                string newName = "resized_" + Path.GetFileName(@FilePath);
                string newPath = Path.Combine(newDir, newName);

                //luodaan BitmapImage
                var bitmap = new BitmapImage();

                //kopioi tiedosto
                File.Copy(@FilePath, newPath);

                //filestream avulla avataan tiedosto
                using (var stream = new FileStream(newPath, FileMode.Open))
                {
                    bitmap.BeginInit();

                    //muutetaan pelkkä leveys, jotta kuvasuhteet säilyvät
                    bitmap.DecodePixelWidth = p_width;
                    //bitmap.DecodePixelHeight = newHeight;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                }
                //enkoodataan jpeg tiedostoksi
                var encoder = new JpegBitmapEncoder();

                //tallenna tiedostoon
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    encoder.Save(stream);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void ConvertToPng(string newDir)
        {
          try
          {
                // newDir\resized_tiedostonimi
                string dir = Path.GetDirectoryName(@FilePath);
                string newName = "converted_to_png_" + Path.GetFileName(@FilePath);
                string newPath = Path.Combine(newDir, newName);
                var newPathPNG = Path.ChangeExtension(newPath, ".PNG");

                //luodaan BitmapImage
                var bitmap = new BitmapImage();

                //kopioi tiedosto
                File.Copy(@FilePath, newPath);

                //filestream avulla avataan tiedosto
                using (var stream = new FileStream(newPath, FileMode.Open))
                {
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                }
                //enkoodataan png tiedostoksi
                var encoder = new PngBitmapEncoder();

                //tallenna tiedostoon
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                using (var stream = new FileStream(newPathPNG, FileMode.Create))
                {
                  encoder.Save(stream);
                }
                //poistetaan väliaikainen tiedosto
                File.Delete(newPath);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
    
}
