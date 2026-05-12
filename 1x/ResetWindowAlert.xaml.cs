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

namespace MuzicBokx
{
    /// <summary>
    /// Interaction logic for ResetWindowAlert.xaml
    /// </summary>
    public partial class ResetWindowAlert : Window
    {
        public ResetWindowAlert()
        {
            InitializeComponent();
        }

        private void Yes_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MuzicBokx.SavedSettings.Default.bgUri = @".\Media\gradient1.mp4";
            MuzicBokx.SavedSettings.Default.isLoop = false;
            MuzicBokx.SavedSettings.Default.Save();
            MuzicBokx.SavedSettings.Default.Reload();
            System.Windows.Forms.MessageBox.Show("Settings Reseted Successfully!\nWe will close for a while and you can return again...", 
                "Success", 
                System.Windows.Forms.MessageBoxButtons.OK, 
                System.Windows.Forms.MessageBoxIcon.Asterisk);
            Application.Current.Shutdown();
        }

        private void No_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
