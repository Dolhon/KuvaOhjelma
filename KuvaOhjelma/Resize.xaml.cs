/*
 * Windows-ohjelmointi IIO11300 Harjoitustyö
 * Kuvien selaus ohjelma
 * 
 * Copyright: Sami Antila 2014 
 * Created: 8.4.2014
 *
 * Resize luokka hoitaa kuvien koon muutokset
 *
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
    /// Interaction logic for Resize.xaml
    /// </summary>
    public partial class Resize : Window
    {
        string type;
        public Resize(string p_type)
        {
            this.type = p_type;
            InitializeComponent();
        }
        private void btnAcceptResize_Click(object sender, RoutedEventArgs e)
        {
            int width = (int)sldWidth.Value;
            try
            {
                if (this.type == "all")
                {
                    ((MainWindow)this.Owner).ResizeAllPhotos(width);
                }
                else //selected
                {
                    ((MainWindow)this.Owner).ResizeSelectedPhotos(width);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnCancelResize_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void sldWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //slider value txtblockiin
            txtSliderValue.Text = ((int)sldWidth.Value).ToString();
        }
    }
}
