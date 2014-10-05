using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net;
using log4net.Config;

namespace VMCplayer
{
    public partial class Form1 : Form
    {
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        public Form1()
        {
            log.Info("Initialising Form");
            InitializeComponent();           
        }

        private string fileToPlay = @"I:\Family Guy - Season 11\Family Guy S11E11 The Giggity Wife.mkv";

        private void Form1_Load(object sender, EventArgs e)
        {
            if (vlcControl1.getVideoCoponent() != null)
            {
                log.Info("File added to playlist");
                vlcControl1.AddItemToPlaylist(fileToPlay);
            }
            else
            {
                log.Error("Video component not intialised");
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            vlcControl1.prev();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            log.Debug("Playing File " + fileToPlay);
            log.Info("Playing Media");
            vlcControl1.play();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            log.Info("Media Stopped by user");
            vlcControl1.stop();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            vlcControl1.next();
        }

       




    }
}
