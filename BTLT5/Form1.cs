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
        private Timer _gameTimer; // <--- Game Loop DUY NHẤT
        private Random random;

        // Logic spawn quái vật
        private int spawnInterval = 50; // 50 * 16ms = 800ms (nửa giây)
        private int spawnCounter = 0;

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

            // 2. Tải tài nguyên
            _chidoriManager.LoadContent();
            Monster.LoadContent();

            // 3. Thiết lập Form
            // Kích hoạt Double Buffering (Rất quan trọng để chống giật)
            this.DoubleBuffered = true;
            this.KeyPreview = true;

            // 4. Thiết lập Game Loop DUY NHẤT
            _gameTimer = new Timer();
            _gameTimer.Interval = 100; // ~60 FPS
            _gameTimer.Tick += GameTimer_Tick; // <--- Hàm Tick DUY NHẤT
            _gameTimer.Start();

            // Bỏ tất cả code của 'timer' (100ms) cũ
            // Bỏ code 'backBuffer' và 'g = this.CreateGraphics()'
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
            spawnCounter++;
            if (spawnCounter >= spawnInterval)
            {
                monstersManager.spawnMonster(ClientSize.Width, ClientSize.Height);
                spawnCounter = 0;
            }
        }

        /// <summary>
        /// Logic xử lý tất cả va chạm
        /// </summary>
        private void HandleCollisions()
        {
            // a. Va chạm Đạn vs Quái vật (GỌI HÀM BỊ THIẾU)
            // Cần cho MonstersManager một cách để lấy List<Monster>
            _chidoriManager.CheckCollisions(monstersManager.Monsters);

            // b. Va chạm Người chơi vs Quái vật
            foreach (var monster in monstersManager.Monsters)
            {
                if (monster.IsAlive && player.GetBounding().IntersectsWith(monster.GetBounds()))
                {
                    // Trò chơi kết thúc
                    _gameTimer.Stop();
                    MessageBox.Show("GAME OVER!");
                    Application.Exit();
                    break;
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
    }
}