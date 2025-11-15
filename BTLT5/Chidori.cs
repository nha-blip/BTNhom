using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTLT5
{
    internal class Chidori
    {
        // --- Tài nguyên dùng chung cho tất cả các quả cầu lửa ---
        // (Static để tiết kiệm bộ nhớ, chỉ tải 1 lần)
        private static Bitmap _spriteSheet;
        private static List<Rectangle> _animationFrames;
        private static int _animationSpeed = 4; // Cứ 3 tick game thì mới đổi frame

        // --- Thuộc tính của riêng quả cầu lửa này ---
        public Point Position { get; private set; }
        public int Speed { get; private set; }
        public bool IsActive { get; set; } // Đánh dấu để xóa

        private int _currentFrameIndex;
        private int _frameCount = 0;
        private Size _frameSize;

        public Chidori(Point startPosition)
        {
            this.Position = startPosition;
            this.Speed = 15; // Tốc độ di chuyển (pixel / tick)
            this.IsActive = true;
            this._currentFrameIndex = 0;

            if (_animationFrames != null)
            {
                this._frameSize = _animationFrames[0].Size;
            }
        }

        public static void LoadContent(Bitmap sheet)
        {
            _spriteSheet = sheet;
            _animationFrames = new List<Rectangle>();

            // Kích thước frame bạn cung cấp
            int frameWidth = 106;
            int frameHeight = 125;

            // Hàng 4 (y = 3 * 125 = 375)
            int rowY = 375;

            // Frame 28 (Cột 1, x = 0)
            _animationFrames.Add(new Rectangle(0, rowY, frameWidth, frameHeight));
            // Frame 29 (Cột 2, x = 106)
            _animationFrames.Add(new Rectangle(106, rowY, frameWidth, frameHeight));
            // Frame 30 (Cột 3, x = 212)
            _animationFrames.Add(new Rectangle(212, rowY, frameWidth, frameHeight));
            // Frame 31 (Cột 4, x = 318)
            _animationFrames.Add(new Rectangle(318, rowY, frameWidth, frameHeight));
            // Frame 32 (Cột 5, x = 424)
            _animationFrames.Add(new Rectangle(424, rowY, frameWidth, frameHeight));
        }

        public Rectangle GetBoundingBox()
        {
            return new Rectangle(this.Position, this._frameSize);
        }

        public void Update(int screenWidth)
        {
            // 1. Di chuyển quả cầu lửa
            // Giả sử player luôn quay mặt sang phải
            Position = new Point(Position.X + Speed, Position.Y);

            // 2. Kiểm tra xem có bay ra khỏi màn hình không
            if (Position.X > screenWidth)
            {
                IsActive = false; // Đánh dấu để xóa
            }

            // 3. Cập nhật animation
            // Dùng 1 bộ đếm static chung để tất cả quả cầu lửa
            // có animation đồng bộ và tiết kiệm tài nguyên
            _frameCount++;
            if (_frameCount >= _animationSpeed)
            {
                _frameCount = 0;
                _currentFrameIndex++;
                if (_currentFrameIndex >= _animationFrames.Count)
                {
                    _currentFrameIndex = 0; // Quay lại frame đầu
                }
            }
        }

        public void Draw(Graphics g)
        {
            if (!IsActive || _spriteSheet == null || _animationFrames.Count == 0)
                return;

            // Lấy frame animation hiện tại
            Rectangle sourceRect = _animationFrames[_currentFrameIndex];

            // Vị trí vẽ
            Rectangle destRect = new Rectangle(Position.X, Position.Y, sourceRect.Width, sourceRect.Height);

            // Vẽ frame đó từ sprite sheet lên màn hình
            g.DrawImage(
                _spriteSheet,   // Ảnh nguồn (lớn)
                destRect,       // Vị trí đích (trên Form)
                sourceRect,     // Vị trí nguồn (trên file sheet)
                GraphicsUnit.Pixel
            );
        }
    }
}
