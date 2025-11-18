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

        public void LoadContent()
        {
            // Tải ảnh 
            _chidoriSheet = Properties.Resources.chidori;

            // Gọi hàm static để nạp tài nguyên
            Chidori.LoadContent(_chidoriSheet);
        }

        public void Spawn(Point startPosition, int row)
        {
            if (_chidoriSheet == null) return;
            Chidori newChidori = new Chidori(startPosition, row);
            _chidoris.Add(newChidori);
        }

        public void UpdateAll(int screenWidth, int screenHeight)
        {
            // Cập nhật 
            foreach (var chidori in _chidoris)
            {
                chidori.Update(screenWidth, screenHeight);
            }

            _chidoris.RemoveAll(fb => !fb.IsActive);
        }

        public void DrawAll(Graphics g)
        {
            foreach (var chidori in _chidoris)
            {
                chidori.Draw(g);
            }
        }

        public int CheckCollisions(List<Monster> monsters)
        {
            // Dùng vòng lặp for (thay vì foreach) 
            // để an toàn khi xóa 
            for (int i = _chidoris.Count - 1; i >= 0; i--)
            {
                Chidori chidori = _chidoris[i];
                if (!chidori.IsActive) continue; // Bỏ qua đạn đã trúng

                foreach (Monster monster in monsters)
                {
                    if (!monster.IsAlive) continue; // Bỏ qua quái đã chết

                    if (chidori.GetBoundingBox().IntersectsWith(monster.GetBounds()))
                    {
                        // Đã trúng!
                        chidori.IsActive = false; // Đạn biến mất
                        monster.Die();          // Quái vật chết
                        score++;
                        // Thoát vòng lặp
                        // nếu 1 đạn chỉ giết 1 quái
                        break;
                    }
                }
            }
            return score;
        }
    }
}
