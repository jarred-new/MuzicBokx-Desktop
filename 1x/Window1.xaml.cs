using System;
using System.IO;
using System.Net;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Windows.Input;

namespace MuzicBokx
{
    public partial class Window1
    {
        private System.Timers.Timer mouseIdleTimer;
        private const int IdleTimeoutMs = 3000;
        private bool cursorHidden = false;

        public Window1()
        {
            this.InitializeComponent();

            InitializeMouseAutoHide();
        }

        private void InitializeMouseAutoHide()
        {
            mouseIdleTimer = new System.Timers.Timer(IdleTimeoutMs);
            mouseIdleTimer.Elapsed += MouseIdleTimer_Elapsed;
            mouseIdleTimer.AutoReset = false; // Ensures the timer only runs once

            // Start the initial timer
            mouseIdleTimer.Start();
        }

        string musicPath;
        bool isStopped;
        private void Play_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(musicPath))
            {
                System.Windows.Forms.MessageBox.Show("No Music is loaded", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                isStopped = false;
                PauseText.Visibility = Visibility.Hidden;
                positionText.Visibility = Visibility.Visible;
                MusicName.Visibility = Visibility.Visible;
                Music.Play();
                gradient.Play();
            }
        }

        private void close_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DialogDarkBG ddbg = new DialogDarkBG();
            ddbg.ShowClose();
        }

        private void Pause_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(musicPath))
            {
                System.Windows.Forms.MessageBox.Show("You paused when the music is not loaded", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (isStopped == true) {
                System.Windows.Forms.MessageBox.Show("You paused when the music has stopped", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                PauseText.Visibility = Visibility.Visible;
                Music.Pause();
                gradient.Pause();
            }
        }

        private void Rewind_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Music.Position -= TimeSpan.FromSeconds(5);
        }

        private void Stop_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isStopped = true;
            PauseText.Visibility = Visibility.Hidden;
            Music.Stop();
            gradient.Stop();
            positionText.Visibility = Visibility.Hidden;
            MusicName.Visibility = Visibility.Hidden;
        }

        private void FastForward_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Music.Position += TimeSpan.FromSeconds(5);
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MP3 Files|*.mp3|WAV Files|*.wav|Midi files|*.mid; *.midi|All Files|*.*";
            try
            {
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    isStopped = false;
                    musicPath = ofd.FileName;
                    string musicPathName = ofd.SafeFileName;
                    Music.Source = new Uri(musicPath);
                    Music.Play();
                    gradient.Play();

                    positionText.Visibility = Visibility.Visible;
                    MusicName.Visibility = Visibility.Visible;

                    MusicName.Text = musicPathName;
                  
                    DispatcherTimer posTime = new DispatcherTimer();
                    posTime.Interval = TimeSpan.FromSeconds(1);
                    posTime.Tick += posTime_Tick;
                    posTime.IsEnabled = true;
                    posTime.Start();
                }
            }
            catch (Exception err)
            {
                string errormessage = "An error occurred: " + err.Message + "\nPress OK to quit...";
                if (System.Windows.MessageBox.Show(errormessage, "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    App.Current.Shutdown();
                }
            }
        }

        private void posTime_Tick(object sender, EventArgs e)
        {
            if (Music.Source != null && Music.NaturalDuration.HasTimeSpan)
            {
                positionText.Text = Music.Position.ToString();
            }
        }

        private void gradient_MediaEnded(object sender, RoutedEventArgs e)
        {
            gradient.Position = new TimeSpan(0, 0, 0);
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            DialogDarkBG ddbg = new DialogDarkBG();
            ddbg.ShowAbout();
        }

        private void Music_MediaEnded(object sender, RoutedEventArgs e)
        {
            bool isOnLoop = MuzicBokx.SavedSettings.Default.isLoop;
            if (isOnLoop == true)
            {
                Music.Position = new TimeSpan(0, 0, 0);
            }
            else if (isOnLoop == false)
            {
                Music.Stop();
                gradient.Stop();
            }
        }

        
        private void loadBgButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MP4 Files|*.mp4|AVI Files|*.avi|PNG Files|*.png|JPEG Files|*.jpg; *.jpeg|All Files|*.*";
            try
            {
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    MuzicBokx.SavedSettings.Default.bgUri = ofd.FileName;
                    MuzicBokx.SavedSettings.Default.Save();

                    gradient.Source = new Uri(MuzicBokx.SavedSettings.Default.bgUri);
                }
            }
            catch (Exception err)
            {
                string errormessage = "An error occurred: " + err.Message + "\nPress OK to quit...";
                if (System.Windows.MessageBox.Show(errormessage, "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    App.Current.Shutdown();
                }
            }
        }

        private void loopMusic_Checked(object sender, RoutedEventArgs e)
        {
            MuzicBokx.SavedSettings.Default.isLoop = loopMusic.IsChecked.Value;
            MuzicBokx.SavedSettings.Default.Save();
        }
        
        /*
        private void openButton_Copy_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MP4 Files|*.mp4|AVI Files|*.avi|PNG Files|*.png|JPEG Files|*.jpg; *.jpeg|All Files|*.*";
            try
            {
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    MuzicBokx.SavedSettings.Default.bgUri = ofd.FileName;
                    MuzicBokx.SavedSettings.Default.Save();

                    gradient.Source = new Uri(MuzicBokx.SavedSettings.Default.bgUri);
                }
            }
            catch (Exception err)
            {
                string errormessage = "An error occurred: " + err.Message + "\nPress OK to quit...";
                if (System.Windows.MessageBox.Show(errormessage, "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    App.Current.Shutdown();
                }
            }
        } */

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string bgpath = MuzicBokx.SavedSettings.Default.bgUri;
            bool loop = MuzicBokx.SavedSettings.Default.isLoop;
            isStopped = true;
            
            //Debug
            /* if (bgpath == "")
            {
                System.Windows.Forms.MessageBox.Show("Empty");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Not Empty: " + bgpath);
            } */
            //Debug

            gradient.Source = new Uri(bgpath, UriKind.Relative);
            loopMusic.IsChecked = loop;
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            DialogDarkBG ddbg = new DialogDarkBG();
            ddbg.ShowReset();
        }

        private void spaceRemind()
        {
            SpaceRemind.Visibility = Visibility.Visible;
            // delay
            System.Windows.Forms.Timer delay = new System.Windows.Forms.Timer();
            delay.Interval = 2000;
            delay.Enabled = true;
            delay.Tick += delegate
            {
                SpaceRemind.Visibility = Visibility.Hidden;
                delay.Enabled = false;
            };
        }

        private void hideControls()
        {
            border1.Visibility = Visibility.Hidden;
            close.Visibility = Visibility.Hidden;
            spaceRemind();
        }

        private void showControls()
        {
            border1.Visibility = Visibility.Visible;
            close.Visibility = Visibility.Visible;
        }

        private void hideButton_Click(object sender, RoutedEventArgs e)
        {
            hideControls();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
            {
                showControls();
            }
        }

        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // When the mouse moves, show the cursor and reset the timer
            Mouse.OverrideCursor = null; // Show the cursor
            mouseIdleTimer.Stop();
            mouseIdleTimer.Start();
        }

        private void MouseIdleTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // When the timer elapses (mouse is idle), hide the cursor
            // Must use Dispatcher to update UI elements from a different thread
            System.Windows.Application.Current.Dispatcher.Invoke(Delegate());
        }

        private Delegate Delegate()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.None; // Hide the cursor
            throw new NotImplementedException();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
           
        }
    }
}