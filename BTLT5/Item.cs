using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLT5
{
    internal class Item
    {
        public static Bitmap sprite;
        public int Column, Row;
        public int x, y;
        public int index;

        public bool IsAlive { get; private set; } = true;

        private const int FRAME_WIDTH = 30;
        private const int FRAME_HEIGHT = 30;

        private const int HurtboxWidth = 30;
        private const int HurtboxHeight = 30;
        private const int HurtboxOffsetX = (FRAME_WIDTH - HurtboxWidth) / 2;
        private const int HurtboxOffsetY = FRAME_HEIGHT - HurtboxHeight;

        public static void LoadContent()
        {
            // Tải từ Resources
            sprite = Properties.Resources.Clock;
        }
        public Item(int initialX, int initialY)
        {
            x = initialX;
            y = initialY;

            index = 0;
        }

        public void Die() { IsAlive = false; }

        public void Update()
        {
            index++;
            if (index >= 6)
            {
                index = 0;
            }
        }

        public void Draw(Graphics g)
        {
            Column = index % 2;
            Row = index / 3;
            g.DrawImage(sprite, x, y, new Rectangle(Column * FRAME_WIDTH, Row * FRAME_HEIGHT, FRAME_WIDTH, FRAME_HEIGHT), GraphicsUnit.Pixel);

            // g.DrawRectangle(Pens.Red, GetBounds());
            // g.DrawRectangle(Pens.Blue, this.x, this.y, FRAME_WIDTH, FRAME_HEIGHT);
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
