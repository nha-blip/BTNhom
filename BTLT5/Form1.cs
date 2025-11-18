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
        // Bỏ 'public Bitmap backBuffer' và 'public Graphics g'
        // Bỏ 'public Timer timer;'

        public Charactor player;
        private MonstersManager monstersManager;
        private ChidoriManager _chidoriManager;
        private ItemsManager itemsManager;
        private Timer _gameTimer; // <--- Game Loop DUY NHẤT
        private Random random;
        private Timer endgame;
        private int RemainTime;

        // Logic spawn quái vật
        private int monsterSpawnInterval = 10; // 50 * 16ms = 800ms (nửa giây)
        private int monsterSpawnCounter = 0;

        // Logic spawn item
        private int itemSpawnInterval = 100;
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
            // Kích hoạt Double Buffering (Rất quan trọng để chống giật)
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
            // Bỏ tất cả code của 'timer' (100ms) cũ
            // Bỏ code 'backBuffer' và 'g = this.CreateGraphics()'
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

        /// <summary>
        /// Logic sinh quái vật
        /// </summary>
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

        /// <summary>
        /// Logic xử lý tất cả va chạm
        /// </summary>
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
        }

        /// <summary>
        /// Hàm này tự động được gọi sau khi this.Invalidate()
        /// Đây là nơi VẼ MỌI THỨ
        /// </summary>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Dùng 'e.Graphics' thay vì 'g' hay 'g1' tự tạo
            Graphics g = e.Graphics;

            // Xóa màn hình
            g.Clear(Color.White);

            // Vẽ mọi thứ
            player.Draw(g);
            monstersManager.DrawAll(g);
            _chidoriManager.DrawAll(g);
            itemsManager.DrawAll(g);

            // Không cần 'g.DrawImageUnscaled(backBuffer, 0, 0)'
            // Vì DoubleBuffered=true đã tự làm việc đó
        }

        // --- SỰ KIỆN BÀN PHÍM ---
        // (Bỏ hàm Render() thủ công)

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            player.KeyUp(e.KeyCode);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            player.KeyDown(e.KeyCode);
            if (e.KeyCode == Keys.A)
            {
                // Logic bắn (đã đúng)
                _chidoriManager.Spawn(player.GetAttackSpawnPoint(), player.Row);
            }
        }

        // Bỏ hàm Render() cũ
        private void endgame_Tick(object sender, EventArgs e)
        {
            RemainTime--;
            lblTime.Text=RemainTime.ToString();
            
        }
    }
}