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
        public Charactor player;
        private MonstersManager monstersManager;
        private ChidoriManager _chidoriManager;
        private ItemsManager itemsManager;
        private Timer _gameTimer; 
        private Random random;
        private Timer endgame;
        private int RemainTime;

        // Logic spawn quái vật
        private int monsterSpawnInterval = 15; 
        private int monsterSpawnCounter = 0;

        // Logic spawn item
        private int itemSpawnInterval = 150;
        private int itemSpawnCounter = 0;

        public Form1()
        {
            InitializeComponent();

            int startX = (this.ClientSize.Width / 2) - (48 / 2); 
            int startY = (this.ClientSize.Height / 2) - (64 / 2);

            // 1. Khởi tạo đối tượng
            player = new Charactor(startX, startY);
            random = new Random();
            monstersManager = new MonstersManager(random);
            _chidoriManager = new ChidoriManager();
            itemsManager = new ItemsManager(random);

            // 2. Tải tài nguyên
            _chidoriManager.LoadContent();
            Monster.LoadContent();
            Item.LoadContent();

            // 3. Thiết lập Form
            // Kích hoạt Double Buffering 
            this.DoubleBuffered = true;
            this.KeyPreview = true;

            // 4. Thiết lập Game Loop DUY NHẤT
            _gameTimer = new Timer();
            _gameTimer.Interval = 50; // ~60 FPS
            _gameTimer.Tick += GameTimer_Tick; // <--- Hàm Tick DUY NHẤT
            _gameTimer.Start();

            // 5. Thiết lập thời gian endgame
            endgame = new Timer();
            endgame.Interval = 1000;
            endgame.Start();
            endgame.Tick += endgame_Tick;

            RemainTime = 120;
            lblTime.Text=RemainTime.ToString();
        }

        /// <summary>
        /// Đây là Game Loop chính, chạy ở 60 FPS
        /// </Gsummary>
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            // 1. CẬP NHẬT LOGIC (Update)
            HandleSpawning();
            player.Update(ClientSize.Width, ClientSize.Height);
            monstersManager.UpdateAll(player.x, player.y);
            _chidoriManager.UpdateAll(this.ClientSize.Width, this.ClientSize.Height);
            itemsManager.UpdateAll(this.ClientSize.Width, this.ClientSize.Height);

            // 2. KIỂM TRA VA CHẠM (Collision)
            HandleCollisions();

            // 3. VẼ (Draw)
            // Yêu cầu Form tự vẽ lại (sẽ gọi Form1_Paint)
            this.Invalidate();
        }

        private void HandleSpawning()
        {
            monsterSpawnCounter++;
            if (monsterSpawnCounter >= monsterSpawnInterval)
            {
                monstersManager.spawnMonster(ClientSize.Width, ClientSize.Height);
                monsterSpawnCounter = 0;
            }

            itemSpawnCounter++;
            if (itemSpawnCounter >= itemSpawnInterval)
            {
                itemsManager.spawnItem(ClientSize.Width, ClientSize.Height);
                itemSpawnCounter = 0;
            }
        }

        private void HandleCollisions()
        {
            // a. Va chạm Đạn vs Quái vật (GỌI HÀM BỊ THIẾU)
            // Cần cho MonstersManager một cách để lấy List<Monster>
            lblScore.Text =_chidoriManager.CheckCollisions(monstersManager.Monsters).ToString();
            int remainingTime = int.Parse(lblTime.Text);


            // b. Va chạm Người chơi vs Quái vật
            foreach (var monster in monstersManager.Monsters)
            {
                if (monster.IsAlive && player.GetBounding().IntersectsWith(monster.GetBounds()))
                {
                    // Trò chơi kết thúc
                    _gameTimer.Stop();
                    endgame.Stop();
                    MessageBox.Show("GAME OVER!");
                    Application.Exit();
                    break;
                }
                else if (lblTime.Text == "0")
                {
                    endgame.Stop();
                    _gameTimer.Stop();
                    MessageBox.Show("Điểm số của bạn là "+ lblTime.Text);
                    Application.Exit();
                }
            }

            foreach (var item in itemsManager.Items)
            {
                if (item.IsAlive && player.GetBounding().IntersectsWith(item.GetBounds()))
                {
                    // Thu thập item
                    item.Die();
                    RemainTime += 5; // Tăng thời gian thêm 5 giây
                    lblTime.Text = RemainTime.ToString();
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Xóa màn hình
            g.Clear(Color.White);

            // Vẽ mọi thứ
            player.Draw(g);
            itemsManager.DrawAll(g);
            monstersManager.DrawAll(g);
            _chidoriManager.DrawAll(g);            
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            player.KeyUp(e.KeyCode);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            player.KeyDown(e.KeyCode);
            if (e.KeyCode == Keys.A)
            {
                if (player.CanShoot())
                {
                    // 2. Bắn 
                    _chidoriManager.Spawn(player.GetAttackSpawnPoint(), player.Row);

                    // 3. Kích hoạt hồi chiêu (để chặn lần bắn tiếp theo ngay lập tức)
                    player.TriggerCooldown();
                }
            }
        }

        private void endgame_Tick(object sender, EventArgs e)
        {
            RemainTime--;
            lblTime.Text=RemainTime.ToString();           
        }
    }
}