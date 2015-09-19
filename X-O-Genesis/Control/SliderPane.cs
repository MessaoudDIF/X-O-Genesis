﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PetvetPOS_Inventory_System
{
    public partial class SliderPane : MyUserControl
    {
        private enum VIEW_STATE
        {
            Hidden,
            Visible,
            Passive,
        }

        private VIEW_STATE viewState = VIEW_STATE.Passive;
        public int max_width = 300;
        private int min_width = 30;
        private int width;

        public SliderPane()
        {
            InitializeComponent();
        }

        public SliderPane(MasterController masterController, Panel panel, DockStyle dockStyle)
            : base(masterController, panel, dockStyle)
        {
            InitializeComponent();
            width = min_width;
        }

        public void toggle()
        {
            switch(viewState){
                case VIEW_STATE.Passive:
                case VIEW_STATE.Hidden:
                    show();
                    break;
                case VIEW_STATE.Visible:
                    hide();
                    break;
         }
        }

        public void show()
        {
            viewState = VIEW_STATE.Visible;
            timer1.Start();
        }

        public void hide()
        {
            viewState = VIEW_STATE.Hidden;
            timer1.Start();
        }


        private void SliderPane_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            contentPanel.Visible = false;

            switch (viewState)
            {
                case VIEW_STATE.Hidden:
                    width = width - 30;
                    if (width >= min_width)
                        Size = new Size(width, Height);
                    else
                        timer1.Stop();
                    contentPanel.Visible = true;
                    break;
                case VIEW_STATE.Visible:
                    width = width + 30;
                    if (width < max_width)
                        Size = new Size(width, Height);
                    else
                        timer1.Stop();
                        contentPanel.Visible = true;
                    break;
            }
        }

        private void SliderPane_Paint(object sender, PaintEventArgs e)
        {
            contentPanel.Location = new Point((this.Width - contentPanel.Width) / 2, contentPanel.Location.Y);
        }
    }
}
