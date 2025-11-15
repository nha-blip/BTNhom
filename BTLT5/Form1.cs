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
        public List<Monster> monsters;
        public Timer timer;

        private int spawnInterval = 50;
        private int spawnCounter = 0;
        Random random;

        private ChidoriManager _chidoriManager;
        private Timer _gameTimer;
        public Form1()
        {
            InitializeComponent();

            player = new Charactor();
            monsters = new List<Monster>();

            // Components
            backBuffer = new Bitmap(ClientSize.Width, ClientSize.Height);
            g = this.CreateGraphics();
            timer = new Timer();
            timer.Interval = 100;
            timer.Enabled = true;
            timer.Tick += timer_Tick;
            random = new Random();
            

            this.DoubleBuffered = true;
            _chidoriManager = new ChidoriManager();
            _chidoriManager.LoadContent();
            _gameTimer = new Timer();
            _gameTimer.Interval = 16; // ~60 FPS
            _gameTimer.Tick += GameTimer_Tick;
            _gameTimer.Start();

            this.KeyPreview = true;
            Render();

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Render();
            player.KeyUp(e.KeyCode);
        }
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            _chidoriManager.UpdateAll(this.ClientSize.Width, this.ClientSize.Height);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            player.KeyDown(e.KeyCode);
            if (e.KeyCode == Keys.A)
            {
                // Giả sử vị trí xuất hiện là (50, 100)
                _chidoriManager.Spawn(player.GetAttackSpawnPoint(), player.Row);
            }

        }

        // Monster spawn
        private void spawnMonster()
        {
            int edge = random.Next(4);
            int x, y;

            switch (edge)
            {
                // Top edge spawn
                case 0:
                    x = random.Next(ClientSize.Width);
                    y = -64;
                    break;
                // Left edge spawn
                case 1:
                    x = -48;
                    y = random.Next(ClientSize.Height);
                    break;
                // Right edge spawn
                case 2:
                    x = ClientSize.Width;
                    y = random.Next(ClientSize.Height);
                    break;
                // Bottom edge spawn
                case 3:
                    x = random.Next(ClientSize.Width);
                    y = ClientSize.Height;
                    break;
                default:
                    x = 0;
                    y = 0;
                    break;
            }

            Monster monster = new Monster(x, y);
            monsters.Add(monster);
        }

        public void Render()
        {
            Graphics g1 = Graphics.FromImage(backBuffer);
            g1.Clear(Color.White);


            player.Draw(g1);
            foreach (Monster monster in monsters)
            {
                monster.Draw(g1);
            }
            _chidoriManager.DrawAll(g1);
            g.DrawImageUnscaled(backBuffer, 0, 0);
            g1.Dispose();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            spawnCounter++;
            if (spawnCounter >= spawnInterval)
            {
                spawnMonster();
                spawnCounter = 0;
            }

            foreach (Monster monster in monsters)
            {
                monster.Update(player.x, player.y);
            }
            player.Update();
            Render();
        }

        
    }

}
