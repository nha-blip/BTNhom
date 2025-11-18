using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace BTLT5
{
    internal class Chidori
    {
        private static Bitmap _spriteSheet;
        private static List<Rectangle> _animationFrames;
        private static int _animationSpeed = 4; 

        public Point Position { get; private set; }
        public bool IsActive { get; set; } // Đánh dấu để xóa

        private int _speedMagnitude = 20; // Độ lớn tốc độ (luôn dương)
        private int _velocityX;           // Tốc độ di chuyển X (có thể âm)
        private int _velocityY;           // Tốc độ di chuyển Y (có thể âm)
        
        private int _directionRow; // 0=Xuống, 1=Trái, 2=Phải, 3=Lên

        private int _currentFrameIndex;
        private int _frameCount = 0;
        private Size _frameSize;

        public Chidori(Point startPosition, int directionRow)
        {
            this.Position = startPosition;
            this.IsActive = true;
            this._currentFrameIndex = 0;
            this._frameCount = 0;
            this._directionRow = directionRow; // Lưu lại hướng

            this._velocityX = 0;
            this._velocityY = 0;       

            switch (directionRow)
            {
                case 0: // Hướng xuống (Down)
                    _velocityY = _speedMagnitude;
                    break;
                case 1: // Hướng trái (Left)
                    _velocityX = -_speedMagnitude;
                    break;
                case 2: // Hướng phải (Right)
                    _velocityX = _speedMagnitude;
                    break;
                case 3: // Hướng lên (Up)
                    _velocityY = -_speedMagnitude;
                    break;
            }

            if (_animationFrames != null && _animationFrames.Count > 0)
            {
                _frameSize = _animationFrames[0].Size;
            }
        }

        public static void LoadContent(Bitmap sheet)
        {
            _spriteSheet = sheet;
            _animationFrames = new List<Rectangle>();

            // Kích thước frame bạn cung cấp
            int frameWidth = 106;
            int frameHeight = 48;

            // Hàng 4 (y = 3 * 125 = 375)
            int rowY = 420;

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
            // Lấy kích thước chuẩn (chưa xoay)
            int w = _frameSize.Width;  
            int h = _frameSize.Height; 

            if (_directionRow == 0 || _directionRow == 3) // Lên hoặc Xuống (đã xoay)
            {         
                int hitboxX = Position.X - h / 2;
                int hitboxY = Position.Y - w / 2;
                return new Rectangle(hitboxX, hitboxY, h, w); // Dùng kích thước đã đảo
            }
            else // Trái hoặc Phải (chưa xoay hoặc xoay 180)
            {
                // Kích thước (w, h)
                // Tọa độ top-left là: (Tâm X - w/2, Tâm Y - h/2)
                int hitboxX = Position.X - w / 2;
                int hitboxY = Position.Y - h / 2;
                return new Rectangle(hitboxX, hitboxY, w, h);
            }
        }

        public void Update(int screenWidth, int screenHeight)
        {
            // 1. Di chuyển bằng tốc độ có hướng (X và Y)
            Position = new Point(Position.X + _velocityX, Position.Y + _velocityY);

            // 2. Kiểm tra bay ra khỏi màn hình (cả 4 cạnh)
            int drawX = Position.X; 
            int drawY = Position.Y; 

            if (drawX > screenWidth || drawX < -_frameSize.Width ||
                drawY > screenHeight || drawY < -_frameSize.Height)
            {
                IsActive = false; // Đánh dấu để xóa
            }

            // 3. Cập nhật animation
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

            int w = sourceRect.Width;
            int h = sourceRect.Height;

            int centerX = Position.X;
            int centerY = Position.Y - h / 2;

            System.Drawing.Drawing2D.GraphicsState state = g.Save();
            try
            {
                // 2. Di chuyển gốc tọa độ (0,0) của Graphics
                // đến TÂM của sprite
                g.TranslateTransform(Position.X, Position.Y);

                // 3. Xoay Graphics dựa trên hướng
                switch (_directionRow)
                {
                    case 0: // Xuống
                        g.RotateTransform(90); // Xoay 90 độ
                        break;
                    case 1: // Trái
                        g.RotateTransform(180); // Xoay 180 độ (lật ngược)
                        break;
                    case 3: // Lên
                        g.RotateTransform(-90); // Xoay -90 độ
                        break;
                        // case 2 (Phải) không cần làm gì, vì sprite gốc đã quay phải
                }

                // 4. Vẽ sprite tại gốc tọa độ MỚI
                // (Tâm của sprite giờ là (0,0) nên ta phải vẽ từ (-w/2, -h/2))
                g.DrawImage(
                    _spriteSheet,
                    new Rectangle(-w / 2, -h / 2, w, h), // Vẽ tại tâm (0,0)
                    sourceRect,
                    GraphicsUnit.Pixel
                );

                //g.DrawRectangle(Pens.Red, GetBoundingBox());
                //g.DrawRectangle(Pens.Blue, new Rectangle(-w / 2, -h / 2, w, h));
            }
            finally
            {
                // 5. Khôi phục lại trạng thái Graphics như cũ
                // Dù có lỗi hay không, bước này CỰC KỲ QUAN TRỌNG
                g.Restore(state);
            }
        }
    }    
}
