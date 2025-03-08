using Frames.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace Frames
{
    /// <summary>
    /// Interaction logic for NewProject.xaml
    /// </summary>
    public partial class NewProject : FluentWindow
    {
        public String ProjectName { get; set; }
        public String ProjectDetails { get; set; }
        public NewProject(Project project)
        {
            InitializeComponent();
            ApplicationThemeManager.Apply(this);
            prjNameTbx.Text = project.Name;
            prjDetails.Content = project.Path;
        }

        private void accept()
        {
            if (string.IsNullOrWhiteSpace(prjNameTbx.Text))
            {
                System.Windows.MessageBox.Show("Invalid Project Name", "Frames - Error", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ProjectName = prjNameTbx.Text;
            DialogResult = true;
            Close();
        }

        private void cancel()
        {
            DialogResult = false;
            Close();
        }

        private void acceptBtn_Click(object sender, RoutedEventArgs e)
        {
            accept();
        }

        private void noBtn_Click(object sender, RoutedEventArgs e)
        {
           cancel();
        }
    }
}
