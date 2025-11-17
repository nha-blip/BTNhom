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
    public class Charactor
    {
        public Bitmap sprite;
        public int Column, Row;
        public bool leftPressed = false;
        public bool rightPressed = false;
        public bool upPressed = false;
        public bool downPressed = false;
        public int x, y;
        public bool move;
        public int index;

        private const int FrameWidth = 48;
        private const int FrameHeight = 64;

        private const int HurtboxWidth = 24;
        private const int HurtboxHeight = 40;
        private const int HurtboxOffsetX = 12;
        private const int HurtboxOffsetY = 24;
        public Charactor(int startX, int startY)
        {
            this.x = startX;
            this.y = startY;
            
            sprite = Properties.Resources.Sasuke;

            index = 0;
            move = true;
            Row = 0;
        }
        public void KeyUp(Keys key)
        {
            if (key == Keys.Left)
                leftPressed = false;
            if (key == Keys.Right)
                rightPressed = false;
            if (key == Keys.Up)
                upPressed = false;
            if (key == Keys.Down)
                downPressed = false;
        }
        public void KeyDown(Keys key)
        {
            if (key == Keys.Left)
            {
                leftPressed = true;
                Row = 1;
            }
            if (key == Keys.Right)
            {
                rightPressed = true;
                Row = 2;
            }
            if (key == Keys.Up)
            {
                upPressed = true;
                Row = 3;
            }
            if (key == Keys.Down)
            {
                downPressed = true;
                Row = 0;
            }
        }
        public void Draw(Graphics g)
        {
            move = leftPressed || rightPressed || upPressed || downPressed;
            if (move)
            {
                Column = index % 8;
                g.DrawImage(sprite, x, y, new Rectangle(Column * 48, Row * 64, 48, 64), GraphicsUnit.Pixel);
                index++;
                if (index >= 8)
                {
                    index = 1;
                }
            }
            else
            {
                index = 0;
                g.DrawImage(sprite, x, y, new Rectangle(0, Row * 64, 48, 64), GraphicsUnit.Pixel);
            }

            g.DrawRectangle(Pens.Blue, this.x, this.y, FrameWidth, FrameHeight);
            g.DrawRectangle(Pens.Red, GetBounding());
        }
        public void Update(int width,int height)
        {
            if (leftPressed && x>0) x -= 5;
            if (rightPressed && x<width-48) x += 5;
            if (upPressed && y>0) y -= 5;
            if (downPressed && y<height-64) y += 5;
        }
        public Rectangle GetBounding()
        {
            return new Rectangle(
                 this.x + HurtboxOffsetX,
                 this.y + HurtboxOffsetY,
                 HurtboxWidth,
                 HurtboxHeight
             );
        }

        public Point GetAttackSpawnPoint()
        {
            return new Point(x + (FrameWidth / 2), y + (FrameHeight / 2));
        }
    }
}
