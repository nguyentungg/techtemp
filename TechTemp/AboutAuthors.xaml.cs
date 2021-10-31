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

namespace TechTemp
{
    /// <summary>
    /// Interaction logic for AboutAuthors.xaml
    /// </summary>
    public partial class AboutAuthors : Window
    {
        public AboutAuthors()
        {
            InitializeComponent();
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FacebookBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.facebook.com/ryan3.nguyen");
        }
        
        private void MailBtn_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText("thanhtung4693@gmail.com");
            if (Clipboard.GetText() != null) MessageBox.Show("Email had been copy to your Clipboard: " + Clipboard.GetText(),"Mail",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        private void TwittersBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://twitter.com/Ryan_NguyenII");
        }
    }
}
