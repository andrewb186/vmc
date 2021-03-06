﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AxAXVLC;

namespace VMCplayer
{
    public partial class VLCControl : UserControl
    {
        public VLCControl()
        {
            InitializeComponent();
        }


        public AxVLCPlugin2 getVideoCoponent()
        {
            return this.axVLCPlugin21;
        }

        public void AddItemToPlaylist(string filename)
        {
            Uri uri = new Uri(filename);
            if (uri.IsAbsoluteUri)
            {
                axVLCPlugin21.playlist.add(uri.AbsoluteUri, filename, null);
            }
            else
            {
                string error = "";
            }
        }

        public void play()
        {
            if (!axVLCPlugin21.playlist.isPlaying)
            {
                axVLCPlugin21.playlist.play();
            }
        }


        public void stop()
        {
            if (axVLCPlugin21.playlist.isPlaying)
            {
                axVLCPlugin21.playlist.stop();
            }
        }


        public void next()
        {
            axVLCPlugin21.playlist.next();
        }

        public void prev()
        {
            axVLCPlugin21.playlist.prev();
        }

    }
}
