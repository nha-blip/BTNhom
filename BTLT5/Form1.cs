using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTLT5
{
    public partial class Form1 : Form
    {
        public Bitmap sprite;
        public Bitmap backBuffer;
        public Graphics g;
        public Charactor player;
        public Timer timer;
        public Form1()
        {
            InitializeComponent();
            player = new Charactor();
            backBuffer= new Bitmap(ClientSize.Width, ClientSize.Height);
            g = this.CreateGraphics();
            timer = new Timer();
            timer.Interval = 100;
            timer.Enabled = true;
            timer.Tick += timer_Tick;
            Render();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Render();
            player.KeyUp(e.KeyCode);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            player.KeyDow(e.KeyCode);
        }

        

        public void Render()
        {
            Graphics g1= Graphics.FromImage(backBuffer);
            g1.Clear(Color.White);
            player.Draw(g1);
            g.DrawImageUnscaled(backBuffer, 0, 0);
            g1.Dispose();


        }
        private void timer_Tick(object sender, EventArgs e)
        {
            player.Update();
            Render();
            
        }
    }
    
}
