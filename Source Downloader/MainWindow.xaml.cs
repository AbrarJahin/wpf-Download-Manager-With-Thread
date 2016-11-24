using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;

namespace Source_Downloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string downloadFileLocation = AppDomain.CurrentDomain.BaseDirectory + "download.txt";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void bDownloadClick(object sender, RoutedEventArgs e)
        {
            bDownload.IsEnabled = false;
            String downloadedSourceString = string.Empty;

            // Create a New 'HttpWebRequest' object .
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(downloadUrl.Text);
            //myHttpWebRequest.AddRange(50, 51);

            // Assign the response object of 'HttpWebRequest' to a 'HttpWebResponse' variable.
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

            MessageBox.Show(myHttpWebResponse.ContentLength.ToString());

            // Display the contents of the page to the console.
            Stream streamResponse = myHttpWebResponse.GetResponseStream();
            StreamReader streamRead = new StreamReader(streamResponse);
            Char[] readBuffer = new Char[256];
            int count = streamRead.Read(readBuffer, 0, 256);

            while (count > 0)
            {
                String outputData = new String(readBuffer, 0, count);
                //Console.WriteLine(outputData);
                downloadedSourceString += outputData;
                count = streamRead.Read(readBuffer, 0, 256);
            }

            // Release the response object resources.
            streamRead.Close();
            streamResponse.Close();
            myHttpWebResponse.Close();

            // Store the result
            File.WriteAllText(downloadFileLocation, downloadedSourceString);

            //Now calculate total no of divs
            var matches = Regex.Matches(downloadedSourceString.ToLower(), "</div>");
            calculationResultValue.Text = matches.Count.ToString();

            bDownload.IsEnabled = true;
            MessageBox.Show("Download Completed");
        }
    }
}
