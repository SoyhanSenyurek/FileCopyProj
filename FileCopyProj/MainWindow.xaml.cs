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
        private object dummyNode;

        public string PrjRootPath { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        public TreeViewItem GetTree(DirectoryInfo d)
        {


            var sadsadas = d.GetFileSystemInfos();

            TreeViewItem item = new TreeViewItem();
            item.Header = d.Name;
            item.FontWeight = FontWeights.Normal;



            foreach (DirectoryInfo s in d.GetDirectories())
            {
                item.Items.Add(GetTree(s));
            }
            foreach (FileInfo fi in d.GetFiles())
            {
                item.Items.Add(fi.Name);
            }
            return item;

        }



        // Get folders inside user's folder

        private void SourceBrowseButton_Click(object sender, RoutedEventArgs e)
        {

            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    SourceFileNameTextBox.Text = dialog.SelectedPath;
                    DirectoryInfo di = new DirectoryInfo(SourceFileNameTextBox.Text);
                    SourceTreeView.Items.Add(GetTree(di));

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
                    DirectoryInfo di = new DirectoryInfo(TargetFileNameTextBox.Text);
                    TargetTreeView.Items.Add(GetTree(di));

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
                        File.Copy(file.FullName, targetDirectory.FullName + "\\" + file.Name,true);


                    }
                    else
                        File.Copy(file.FullName, exist.FullName,true);
                }
            }
        }

        public void copy()
        {
            if (SourceFileNameTextBox.Text == "" || TargetFileNameTextBox.Text == "" || SourceFileNameTextBox.Text == "Source" || TargetFileNameTextBox.Text == "Target")
            {
                MessageBox.Show("Kaynak ve Hedef Klasör Belirtilmedi", "Softtech");
            }
            else if (!Directory.Exists(SourceFileNameTextBox.Text))
            {
                MessageBox.Show("Kaynak Klasör Hatalı", "Softtech");
            }
            else if (!Directory.Exists(TargetFileNameTextBox.Text))
            {
                MessageBox.Show("Hedef Klasör Hatalı", "Softtech");
            }
            else if (SourceFileNameTextBox.Text == TargetFileNameTextBox.Text)
            {
                MessageBox.Show("Kaynak ve Hedef Klasör Aynı", "Softtech");

            }
            else
            {
                GetDirectoriesFiles(SourceFileNameTextBox.Text, TargetFileNameTextBox.Text);
                MessageBox.Show("Kopyalama Başarılı", "Softtech");

            }
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {

            if (ComboBox1.Text == "Hedef klasörü tut")
            {

                copy();
            }
            else
            {
                if (Directory.Exists(TargetFileNameTextBox.Text))
                {
                    Directory.Delete(TargetFileNameTextBox.Text, true);
                    Directory.CreateDirectory(TargetFileNameTextBox.Text);
                    MessageBox.Show("Klasör Silindi", "Softtech");
                    copy();
                }
                else
                {
                    MessageBox.Show("Klasör Silinemedi", "Softtech");
                }
            }
        }

       
     
    }
}
