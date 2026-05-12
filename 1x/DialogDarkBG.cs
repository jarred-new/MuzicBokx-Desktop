using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Forms;
using System.Windows.Threading;

namespace MuzicBokx
{
    class DialogDarkBG
    {
        public void ShowClose()
        {
            DarkerOpacity dark = new DarkerOpacity();
            dark.Show();
            WindowLeave leavebox = new WindowLeave();
            leavebox.ShowInTaskbar = false;
            leavebox.ShowDialog();
            dark.Close();
        }

        public void ShowAbout()
        {
            DarkerOpacity dark = new DarkerOpacity();
            dark.Show();
            AboutWindow aboutbox = new AboutWindow();
            aboutbox.ShowInTaskbar = false;
            aboutbox.ShowDialog();
            dark.Close();
        }

        public void ShowReset()
        {
            DarkerOpacity dark = new DarkerOpacity();
            dark.Show();
            ResetWindowAlert resetalert = new ResetWindowAlert();
            resetalert.ShowInTaskbar = false;
            resetalert.ShowDialog();
            dark.Close();
        }
    }
}
