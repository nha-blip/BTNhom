using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLT04
{
    internal class ChidoriManager
    {
        private List<Chidori> _chidoris;
        private Bitmap _chidoriSheet; // Ảnh sheet

        public ChidoriManager()
        { 
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
        public void Spawn(Point startPosition)
        {
            if (_chidoriSheet == null) return;
            Chidori newChidori = new Chidori(startPosition);
            _chidoris.Add(newChidori);
        }

        /// <summary>
        /// Cập nhật tất cả quả cầu lửa (gọi mỗi tick của game)
        /// </summary>
        public void UpdateAll(int screenWidth)
        {
            // Cập nhật từng quả cầu lửa
            foreach (var chidori in _chidoris)
            {
                chidori.Update(screenWidth);
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
        //public void CheckCollisions(List<QuaiVat> quaiVats)
        //{
        //    foreach (var fireball in _fireballs.Where(fb => fb.IsActive))
        //    {
        //        foreach (var monster in quaiVats.Where(m => m.IsAlive))
        //        {
        //            // Sử dụng IntersectsWith để kiểm tra va chạm
        //            if (fireball.GetBoundingBox().IntersectsWith(monster.GetBoundingBox()))
        //            {
        //                // Nếu va chạm:
        //                fireball.IsActive = false; // Đạn biến mất
        //                monster.TakeDamage(1); // Quái vật mất máu (hoặc chết)
        //            }
        //        }
        //    }
        //}
    }
}
