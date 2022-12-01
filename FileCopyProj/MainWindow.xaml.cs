using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;
using WebBrowser = System.Windows.Controls.WebBrowser;

namespace FileCopyProj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 



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
                    SourceFolderView.Navigate(new Uri(SourceFileNameTextBox.Text));

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
                    TargetFolderView.Navigate(new Uri(TargetFileNameTextBox.Text));
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
            if (SourceFileNameTextBox.Text == "" || TargetFileNameTextBox.Text == "" || SourceFileNameTextBox.Text =="Source"|| TargetFileNameTextBox.Text=="Target")
            {
                MessageBox.Show("Kaynak ve Hedef Klasör Belirtilmedi", "Softtech");
            }
            else if(SourceFileNameTextBox.Text==TargetFileNameTextBox.Text)
            {
                MessageBox.Show("Kaynak ve Hedef Klasör Aynı", "Softtech");

            }
            else
            {
                GetDirectoriesFiles(SourceFileNameTextBox.Text, TargetFileNameTextBox.Text);
                MessageBox.Show("Kopyalama Başarılı", "Softtech");

            }
        }
    }
}
