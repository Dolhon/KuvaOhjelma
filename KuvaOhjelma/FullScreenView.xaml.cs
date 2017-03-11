/*
 * Windows-ohjelmointi IIO11300 Harjoitustyö
 * Kuvien selaus ohjelma
 * 
 * Copyright: Sami Antila 2014 
 * Created: 14.3.2014
 */

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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KuvaOhjelma
{
    /// <summary>
    /// Interaction logic for FullScreenView.xaml
    /// </summary>
    public partial class FullScreenView : Window
    {
        public FullScreenView()
        {
            InitializeComponent();
        }
        public FullScreenView(ImageSource p_photoSource):this()
        {
          InitializeComponent();
          this.imgStageFullScreen.Source = p_photoSource;
        }

        public void ChangeImage(bool isForward)
        {
            try
            {
                ImageSource source = ((MainWindow)this.Owner).ChangeImage(isForward);
                imgStageFullScreen.Source = source;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SelectPhoto()
        {
            try
            {
                ((MainWindow)this.Owner).SelectPhoto();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void imgStageFullScreen_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    this.Close();
                    break;
                case Key.Escape:
                    this.Close();
                    break;
                case Key.Left:
                    ChangeImage(false);
                    break;
                case Key.Right:
                    ChangeImage(true);
                    break;
                case Key.E:
                    SelectPhoto();
                    break;
                default:
                    return;
            }
            e.Handled = true;
        }

        private void imgStageFullScreen_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            {
                if (e.ClickCount == 2)
                {
                    this.Close();
                }
                e.Handled = true;
            }
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
        }
    }
}
