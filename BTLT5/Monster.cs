using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace BTLT5
{
    public class Monster
    {
        public static Bitmap sprite;
        public int Column, Row;
        public int x, y;
        public int index;
        public int speed {  get; private set; }
        public bool IsAlive { get; private set; } = true;

        private const int FRAME_WIDTH = 48;
        private const int FRAME_HEIGHT = 64;

        private const int HurtboxWidth = 24;
        private const int HurtboxHeight = 40;
        private const int HurtboxOffsetX = (FRAME_WIDTH - HurtboxWidth) / 2;
        private const int HurtboxOffsetY = FRAME_HEIGHT - HurtboxHeight;

        private const int ANIMATION_FRAMES = 8;
        private const int WALK_DOWN_ROW = 0;
        private const int WALK_LEFT_ROW = 1;
        private const int WALK_RIGHT_ROW = 2;
        private const int WALK_UP_ROW = 3;

        public static void LoadContent()
        {
            // Tải từ Resources
            sprite = Properties.Resources.Sakura;
            
        }
        public Monster(int initialX, int initialY, int initialSpeed = 3)
        {
            x = initialX;
            y = initialY;
            
            index = 0;
            speed = initialSpeed;
        }
        public void Die() { IsAlive = false; }
        public void Update(int targetX, int targetY)
        {
            int dx = x - targetX;
            int dy = y - targetY;

            if (dx > speed)
            {
                x -= speed;
                Row = WALK_LEFT_ROW;
            }
            else if (dx < speed)
            {
                x += speed;
                Row = WALK_RIGHT_ROW;
            }

            if (dy > speed)
            {
                y -= speed;
                Row = WALK_UP_ROW;
            }
            else if (dy < speed)
            {
                y += speed;
                Row = WALK_DOWN_ROW;
            }

            index++;
            if (index > ANIMATION_FRAMES)
            {
                index = 0;
            }
        }
        public void Draw(Graphics g)
        {
            Column = index % 4;
            g.DrawImage(sprite, x, y, new Rectangle(Column * FRAME_WIDTH, Row * FRAME_HEIGHT, FRAME_WIDTH, FRAME_HEIGHT), GraphicsUnit.Pixel);

            g.DrawRectangle(Pens.Red, GetBounds());
            g.DrawRectangle(Pens.Blue, this.x, this.y, FRAME_WIDTH, FRAME_HEIGHT);
        }
        public Rectangle GetBounds()
        {
            return new Rectangle(
                 this.x + HurtboxOffsetX,
                 this.y + HurtboxOffsetY,
                 HurtboxWidth,
                 HurtboxHeight
             );
        }
    }
}
