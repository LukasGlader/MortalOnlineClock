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
using System.Threading;

namespace MOclock2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AnalogWindow : Window
    {

        private int REFRESH_RATE = 500;
        //Used to keep track of whether or not we should flip the visibility state of the window.
        private bool windowWasActive;
        
        private NaveTimeModel t;
        private Thread updateThread;
        private AnalogGUIBucket bucket;

        public AnalogWindow()
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
                bucket = new AnalogGUIBucket();
                AnalogBucketUpdater updater = new AnalogBucketUpdater(REFRESH_RATE, bucket, t);
                updateThread = new Thread(updater.DoWork);
                updateThread.IsBackground = true;

                this.DataContext = bucket;
                updateThread.Start();

                windowWasActive = false;
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


        /// <summary>
        /// The main window has been selected.
        /// </summary>
        protected override void OnActivated(EventArgs e)
        {
            updateHiddenButtonsState();
        }

        /// <summary>
        /// The main window has been deselected.
        /// </summary>
        protected override void OnDeactivated(EventArgs e)
        {
            updateHiddenButtonsState();
        }

        /// <summary>
        /// Mouse entered main window.
        /// </summary>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            updateHiddenButtonsState();
        }

        /// <summary>
        /// Mouse left main window.
        /// </summary>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            updateHiddenButtonsState();
        }

        /// <summary>
        /// Updates the state of the Close and Digital button as a response to an event.
        /// </summary>
        private void updateHiddenButtonsState()
        {
            if (!this.IsActive && windowWasActive)
            {
                windowWasActive = false;
                ((Button)this.button_close).Visibility = Visibility.Hidden;
                ((Button)this.button_switch).Visibility = Visibility.Hidden;
            }
            else
            {
                windowWasActive = true;
                ((Button)this.button_close).Visibility = Visibility.Visible;
                ((Button)this.button_switch).Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Closes the application.
        /// </summary>
        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Drag-listener. Makes sure the user can move the application.
        /// </summary>
        private void move_window(object sender, RoutedEventArgs e)
        {
            this.DragMove();
        }

        /// <summary>
        /// Switch to the Digital GUI mode.
        /// </summary>
        private void button_switch_gui(object sender, RoutedEventArgs e)
        {
            updateThread.Abort();

            WindowPreferences.window_x = Left;
            WindowPreferences.window_y = Top;

            DigitalWindow main = new DigitalWindow();
            App.Current.MainWindow = main;
            this.Close();
            main.Show();
        }
    }
}
