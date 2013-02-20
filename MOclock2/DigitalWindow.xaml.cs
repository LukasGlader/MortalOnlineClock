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
using System.Windows.Shapes;
using System.Threading;
using System.ComponentModel;
using System.Windows.Threading;

namespace MOclock2
{
    /// <summary>
    /// Interaction logic for DigitalWindow.xaml
    /// </summary>
    public partial class DigitalWindow : Window
    {

        private const int REFRESH_RATE = 100;
        private NaveTimeModel t;
        private DigitalGUIBucket bucket;
        private Thread updateThread;

        public DigitalWindow()
        {
            InitializeComponent();

            if (WindowPreferences.window_x != 0)
            {
                Left = WindowPreferences.window_x;
                Top = WindowPreferences.window_y;
            }

            try
            {
                t = new NaveTimeModel();
                bucket = new DigitalGUIBucket();
                DigitalBucketUpdater updater = new DigitalBucketUpdater(REFRESH_RATE, bucket, t);
                updateThread = new Thread(updater.DoWork);
                updateThread.IsBackground = true;

                this.DataContext = bucket;
                updateThread.Start();
            }
            catch (Exception e)
            {
                exitPrompt(e.Message);
            }
        }

        /// <summary>
        /// Displays a prompt with the given message and an OK button that will exit the application.
        /// </summary>
        private void exitPrompt(string s)
        {
            MessageBox.Show(s);
            Application.Current.Shutdown();
        }

        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Switch to the Analog GUI mode.
        /// </summary>
        private void analogSwitch_Click(object sender, RoutedEventArgs e)
        {
            updateThread.Abort();

            WindowPreferences.window_x = Left;
            WindowPreferences.window_y = Top;

            AnalogWindow main = new AnalogWindow();
            App.Current.MainWindow = main;
            this.Close();
            main.Show();
        }
    }
}

