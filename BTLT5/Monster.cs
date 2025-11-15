using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BTLT5
{
    public class Monster
    {
        public Bitmap sprite;
        public int Column, Row;
        public int x, y;
        public int index;
        public int speed {  get; private set; }

        private const int FRAME_WIDTH = 48;
        private const int FRAME_HEIGHT = 64;
        private const int ANIMATION_FRAMES = 8;
        private const int WALK_DOWN_ROW = 0;
        private const int WALK_LEFT_ROW = 1;
        private const int WALK_RIGHT_ROW = 2;
        private const int WALK_UP_ROW = 3;

        public Monster(int initialX, int initialY, int initialSpeed = 3)
        {
            x = initialX;
            y = initialY;
            sprite = new Bitmap("Sprite/Sakura.png");
            index = 0;
            speed = initialSpeed;
        }
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
        }
        public Rectangle GetBounds()
        {
            return new Rectangle(x, y, FRAME_WIDTH, FRAME_HEIGHT);
        }
    }
}
