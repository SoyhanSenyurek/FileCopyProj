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


namespace FileCopyProj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        

        private void SourceBrowseButton_Click(object sender, RoutedEventArgs e)
        {

            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    SourceFileNameTextBox.Text = dialog.SelectedPath;
                }
            }
        }

        private void TargetBrowseButton_Click(object sender, RoutedEventArgs e)
        {

            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    TargetFileNameTextBox.Text = dialog.SelectedPath;
                }
            }
        }


        public void GetDirectoriesFiles(string path,string targetPath)
        {
            DirectoryInfo d = new DirectoryInfo(path); //Assuming Test is your Folder

            var sadsadas = d.GetFileSystemInfos();


            foreach (var file in sadsadas)
            {
                if (file.Attributes == FileAttributes.Directory)
                {
                    DirectoryInfo targetDirectory = new DirectoryInfo(targetPath);
                    var exist = targetDirectory.GetFileSystemInfos().FirstOrDefault(x => x.Name == file.Name);
                    if (exist == null)
                        System.IO.Directory.CreateDirectory(targetDirectory.FullName + "\\" + file.Name);

                    GetDirectoriesFiles(file.FullName, targetDirectory.FullName + "\\" + file.Name);
                }
                else if (file.Attributes == FileAttributes.Archive)
                {
                    
                    DirectoryInfo targetDirectory = new DirectoryInfo(targetPath);
                    var exist = targetDirectory.GetFileSystemInfos().FirstOrDefault(x => x.Name == file.Name);
                    if (exist == null)
                    {
                        File.Create(targetDirectory.FullName + "\\" + file.Name).Dispose();
                        File.Copy(file.FullName, targetDirectory.FullName + "\\" + file.Name, true);


                    }
                    else
                        File.Copy(file.FullName, exist.FullName,true);
                }
            }
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            GetDirectoriesFiles(SourceFileNameTextBox.Text, TargetFileNameTextBox.Text);
        }
    }
}
