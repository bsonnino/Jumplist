using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Shell;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;

namespace Jumplist
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var args = Environment.GetCommandLineArgs();
            Loaded += (s, e) =>
                {
                    if (args.Length > 1)
                        AbreArquivo(args[1]);
                };
        }


        private void AbreArquivo(string nomeArquivo)
        {
            txtArquivo.Text = File.ReadAllText(nomeArquivo);
        }

        void Button_Click(object sender, RoutedEventArgs e)
        {
            //var dialog = new OpenFileDialog();
            //if (dialog.ShowDialog() == true)
            //    AbreArquivo(dialog.FileName);
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                StringBuilder sb = new StringBuilder();
                dialog.AllowNonFileSystemItems = true;
                dialog.IsFolderPicker = true;

                dialog.Multiselect = true;
                var result = dialog.ShowDialog();
                if (result == CommonFileDialogResult.OK)
                {
                    ICollection<ShellObject> items = dialog.FilesAsShellObject;
                    foreach (ShellObject item in items)
                    {
                        if (item is ShellLibrary)
                        {
                            foreach (ShellFileSystemFolder folder in ((ShellLibrary)item))
                            {
                                sb.AppendLine(string.Format(" **** {0} ****", folder.Path));

                                foreach (var file in Directory.GetFiles(folder.Path))
                                {
                                    sb.AppendLine(file);
                                }
                            }
                        }
                        else if (item is ShellFileSystemFolder)
                        {
                            foreach (var file in Directory.GetFiles(item.ParsingName))
                            {
                                sb.AppendLine(file);
                            }
                        }
                    }
                    txtArquivo.Text = sb.ToString();
                }
            }

        }
    }
}
