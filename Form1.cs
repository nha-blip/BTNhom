using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTLT04
{
    public partial class Form1 : Form
    {
        private ChidoriManager _chidoriManager;
        private Timer _gameTimer;
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _chidoriManager = new ChidoriManager();
            _chidoriManager.LoadContent();
            _gameTimer = new Timer();
            _gameTimer.Interval = 16; // ~60 FPS
            _gameTimer.Tick += GameTimer_Tick;
            _gameTimer.Start();

            this.KeyPreview = true;
            //this.KeyDown += Form1_KeyDown;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            _chidoriManager.UpdateAll(this.ClientSize.Width);
            this.Invalidate(); // Yêu cầu vẽ lại form
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.Clear(Color.White);

            _chidoriManager.DrawAll(g);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                // Giả sử vị trí xuất hiện là (50, 100)
                _chidoriManager.Spawn(new Point(50, 100));
            }
        }
    }
}
