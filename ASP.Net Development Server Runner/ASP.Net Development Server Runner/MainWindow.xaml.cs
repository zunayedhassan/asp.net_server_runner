using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace ASP.Net_Development_Server_Runner
{
    public partial class MainWindow : Window
    {
        enum BrowserList
        {
            InternetExplorer,
            MozillaFirefox,
            GoogleChrome,
            Opera,
            Safari
        }

        private Random _randomPort = new Random();
        private string _path;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void browseServerButton_Click(object sender, EventArgs eventArgs)
        {
            FolderBrowserDialog serverLocationBrowserDialog = new FolderBrowserDialog();
            DialogResult serverLocationDialogResult = serverLocationBrowserDialog.ShowDialog();

            if (serverLocationDialogResult == System.Windows.Forms.DialogResult.OK)
            {
                serverLocationTextBox.Text = serverLocationBrowserDialog.SelectedPath;
            }
        }

        private void randomPortButton_Click(object sender, EventArgs eventArgs)
        {
            portTextBox.Text = (_randomPort.Next(1, 65535).ToString());
        }

        private void browseStartPageButton_Click(object sender, EventArgs eventArgs)
        {
            OpenFileDialog startPageOpenFileDialog = new OpenFileDialog();
            startPageOpenFileDialog.Filter = "Active Server Pages (.aspx)|*.aspx|All files (*.*)|*.*";

            DialogResult response = startPageOpenFileDialog.ShowDialog();

            if (response == System.Windows.Forms.DialogResult.OK)
            {
                startPageTextBox.Text = startPageOpenFileDialog.FileName;
                _path = "\"" + new FileInfo(startPageOpenFileDialog.FileName).DirectoryName + "\"";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            serverLocationTextBox.ToolTip = new CustomizedTooltip(@"Example: C:\Program Files\Common Files\microsoft shared\DevServer\10.0");
            portTextBox.ToolTip = new CustomizedTooltip("Select port number from 1 to 65535");
            startPageTextBox.ToolTip = new CustomizedTooltip("Select any *.aspx file that you want run in server");
            virtualPathTextBox.ToolTip = new CustomizedTooltip("Type any name as your server's root directory");
            browserComboBox.ToolTip = new CustomizedTooltip("Select any browser to run your *.aspx file");
        }

        private void startButton_Click(object sender, EventArgs eventArgs)
        {
            try
            {
                string url = "http://localhost:" + portTextBox.Text + "/" + virtualPathTextBox.Text + "/";
                string selectedBrowser = string.Empty;
                string serverProgramName = "WebDev.WebServer20.EXE";

                if (File.Exists(serverLocationTextBox.Text + @"\WebDev.WebServer40.EXE"))
                    serverProgramName = "WebDev.WebServer40.EXE";
                else if (File.Exists(serverLocationTextBox.Text + @"\WebDev.WebServer20.EXE"))
                    serverProgramName = "WebDev.WebServer20.EXE";
                else
                    serverProgramName = "WebDev.WebServer.EXE";

                WriteBatchFile("run.bat",
                               "\"" + serverLocationTextBox.Text + "\\\"" + serverProgramName + " /port:" +
                               portTextBox.Text + " /path:" + _path + " /vpath:\"/" + virtualPathTextBox.Text + "\"");

                switch ((BrowserList) browserComboBox.SelectedIndex)
                {
                    case BrowserList.InternetExplorer:
                        selectedBrowser = "\"C:\\Program Files\\Internet Explorer\\iexplore.exe\"";
                        break;

                    case BrowserList.MozillaFirefox:
                        selectedBrowser = "\"C:\\Program Files\\Mozilla Firefox\\firefox.exe\"";
                        break;

                    case BrowserList.GoogleChrome:
                        selectedBrowser = "\"C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe\"";
                        break;

                    case BrowserList.Opera:
                        selectedBrowser = "\"C:\\Program Files\\Opera\\opera.exe\"";
                        break;

                    case BrowserList.Safari:
                        selectedBrowser = "\"C:\\Program Files\\Safari\\Safari.exe\"";
                        break;
                }

                WriteBatchFile("open_page.bat", selectedBrowser + " " + url);

                RunFromBatchFile("run.bat");
                RunFromBatchFile("open_page.bat");
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Please, enter correct input", "Error", MessageBoxButton.OK,
                                               MessageBoxImage.Error);
            }
        }

        private void defaultButton_Click(object sender, RoutedEventArgs e)
        {
            serverLocationTextBox.Text = @"C:\Program Files\Common Files\microsoft shared\DevServer\10.0";
            portTextBox.Text = "8080";
            virtualPathTextBox.Text = "root";
            browserComboBox.SelectedIndex = (int) BrowserList.InternetExplorer;
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("ASP.Net Development Server Runner\n" +
                                           "Version: 1.1\n\n" +
                                           "by Zunayed Hassan\n" +
                                           "Email: zunayed-hassan@live.com", "About", MessageBoxButton.OK,
                                           MessageBoxImage.Information);
        }

        private void WriteBatchFile(string fileName, string command)
        {
            TextWriter runFileTextWriter = new StreamWriter(fileName);
            runFileTextWriter.Write(command);
            runFileTextWriter.Close();
        }

        private void RunFromBatchFile(string fileName)
        {
            // Start the child process. 
            Process aspServerProcess = new Process();

            // Redirect the output stream of the child process. 
            aspServerProcess.StartInfo.UseShellExecute = false;
            aspServerProcess.StartInfo.RedirectStandardOutput = true;
            aspServerProcess.StartInfo.FileName = fileName;
            aspServerProcess.Start();
        }
    }
}
