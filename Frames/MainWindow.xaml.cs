using Frames.Model;
using Microsoft.Win32;
using Newtonsoft.Json;
using OpenCvSharp;
using System;
using System.Collections.Generic;
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
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace Frames
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        List<Project> Projects = new List<Project>();
        public MainWindow()
        {
            InitializeComponent();
            ApplicationThemeManager.Apply(this);
            tabControl.Height = this.Height;
            
            Projects.Add(new Project { Name = "Add Project", Progress = 100, Status = "Create A New Project", Tag = "a7xHs0obPqr" });
            for (int i = 0; i < 80; i++)
            {
                String hash = "0xHasH77"+i;
                if (i % 2 != 0)
                {
                    Projects.Add(new Project { Name = "Random " + i, Progress = i, Status = "Processing...", Tag = hash });
                }
                else
                {
                    Projects.Add(new Project { Name = "Random " + i, Progress = i+3, Status = "Extracting...", Tag = hash });
                }
            }
            homeList.ItemsSource = Projects;           
        }

        private void WinResized(object sender, SizeChangedEventArgs e)
        {
            tabControl.Height = this.Height;
        }

        private void listChildClicked(object sender, RoutedEventArgs e)
        {
            //this.Title = Projects[]
            
        }

        /*private void GenerateThumbnail(string videoPath, string thumbnailPath)
        {
            using (var capture = new VideoCapture(videoPath))
            {
                // Read the first frame
                if (capture.IsOpened() && capture.Read(out new Mat frame))
                {
                    frame.SaveImage(thumbnailPath); // Save as an image
                }
            }
        }*/

        private void SaveProject(Project project)
        {
            string json = JsonConvert.SerializeObject(project, Formatting.Indented);
            string filePath = System.IO.Path.Combine(project.Path, $"{project.Name}.json");
            File.WriteAllText(filePath, json);
            Projects.Add(project);
        }

        public void CreateProject()
        {
            // Open file dialog to select media files
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Video Files|*.mp4;*.avi;*.mkv;*.mov",
                Title = "Frames - Select Video Files for the Project"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Get selected files
                string[] selectedFiles = openFileDialog.FileNames;

                if (selectedFiles.Length == 0) return;

                // Extract parent directory
                string parentFolder = System.IO.Path.GetDirectoryName(selectedFiles[0]);

                // Prompt for project name
                string projectName = System.IO.Path.GetFileName(parentFolder); // Default name
                                                                     // Use a custom dialog or input box to allow the user to edit the name.

                // Collect file information
                var selectedFileNames = selectedFiles.Select(f => System.IO.Path.GetFileName(f)).ToList();
                long totalSize = 0;
                TimeSpan totalDuration = TimeSpan.Zero;

                foreach (var file in selectedFiles)
                {
                    // Get file size
                    FileInfo fileInfo = new FileInfo(file);
                    totalSize += fileInfo.Length;

                    // Get duration using OpenCvSharp
                    using (var capture = new VideoCapture(file))
                    {
                        double fps = capture.Fps;
                        int frameCount = (int)capture.Get(VideoCaptureProperties.FrameCount);
                        totalDuration += TimeSpan.FromSeconds(frameCount / fps);
                    }
                }

                // Generate a thumbnail for the project
                string thumbnailPath = System.IO.Path.Combine(parentFolder, $"{projectName}_thumbnail.jpg");
               // GenerateThumbnail(selectedFiles[0], thumbnailPath);

                // Create project object
                var project = new Project
                {
                    Name = projectName,
                    Path = parentFolder,
                    ThumbnailPath = thumbnailPath,
                    TotalDuration = totalDuration,
                    FileCount = selectedFiles.Length,
                    TotalSize = totalSize,
                    SelectedFiles = selectedFileNames // Store relative file paths
                };

                // Create and show the dialog
                var newProjectWindow = new NewProject(project);
                {
                    Owner = this
                        // Set the owner to the main window for proper dialog behavior
                };

                // Show the dialog modally and check the result
                if (newProjectWindow.ShowDialog() == true)
                {
                    // Add the new project to the list
                    project.Name = newProjectWindow.ProjectName;
                    SaveProject(project);//save to json
                }
                else
                {
                    System.Windows.MessageBox.Show("Project creation succesfuly cancelled", "Frames - Cancel Creating Project", System.Windows.MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            CreateProject();
        }
    }
}
