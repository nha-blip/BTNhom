using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTLT5
{
    internal class ChidoriManager
    {
        private List<Chidori> _chidoris;
        private Bitmap _chidoriSheet; // Ảnh sheet
        public int score;
        public ChidoriManager()
        {
            score = 0;
            _chidoris = new List<Chidori>();
        }

        /// <summary>
        /// Tải tài nguyên cho đạn lửa (gọi 1 lần khi game load)
        /// </summary>
        public void LoadContent()
        {
            // Tải ảnh từ file (bạn có thể dùng Properties.Resources)
            _chidoriSheet = Properties.Resources.chidori;

            // Gọi hàm static của DanLua để nạp tài nguyên
            Chidori.LoadContent(_chidoriSheet);
        }

        /// <summary>
        /// Tạo 1 quả cầu lửa mới (gọi khi nhấn phím 'A')
        /// </summary>
        public void Spawn(Point startPosition, int row)
        {
            if (_chidoriSheet == null) return;
            Chidori newChidori = new Chidori(startPosition, row);
            _chidoris.Add(newChidori);
        }

        /// <summary>
        /// Cập nhật tất cả quả cầu lửa (gọi mỗi tick của game)
        /// </summary>
        public void UpdateAll(int screenWidth, int screenHeight)
        {
            // Cập nhật từng quả cầu lửa
            foreach (var chidori in _chidoris)
            {
                chidori.Update(screenWidth, screenHeight);
            }

            // Xóa tất cả các quả cầu lửa đã bị đánh dấu IsActive = false
            // (bay ra khỏi màn hình, hoặc trúng quái vật)
            _chidoris.RemoveAll(fb => !fb.IsActive);
        }

        /// <summary>
        /// Vẽ tất cả quả cầu lửa (gọi trong sự kiện Paint)
        /// </summary>
        public void DrawAll(Graphics g)
        {
            foreach (var chidori in _chidoris)
            {
                chidori.Draw(g);
            }
        }

        /// <summary>
        /// Hàm kiểm tra va chạm với quái vật
        /// </summary>
        public int CheckCollisions(List<Monster> monsters)
        {
            // Dùng vòng lặp for (thay vì foreach) 
            // để an toàn khi xóa đạn
            for (int i = _chidoris.Count - 1; i >= 0; i--)
            {
                Chidori chidori = _chidoris[i];
                if (!chidori.IsActive) continue; // Bỏ qua đạn đã trúng

                foreach (Monster monster in monsters)
                {
                    if (!monster.IsAlive) continue; // Bỏ qua quái đã chết

                    // (QUAN TRỌNG) Đây là lúc kiểm tra!
                    if (chidori.GetBoundingBox().IntersectsWith(monster.GetBounds()))
                    {
                        // Đã trúng!
                        chidori.IsActive = false; // Đạn biến mất
                        monster.Die();          // Quái vật chết
                        score++;
                        // (Tùy chọn) Thoát vòng lặp
                        // nếu 1 đạn chỉ giết 1 quái
                        break;
                    }
                }
            }
            return score;
        }
    }
}
