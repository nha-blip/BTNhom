using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public int x = 10, y = 50;
        public bool move;
        public int index;
        public Charactor()
        {
            sprite = new Bitmap("Sprite/Sasuke.png");
            index = 0;
            move = true;
        }
        public void KeyUp(Keys key)
        {
            leftPressed = false;
            rightPressed = false;
            upPressed = false;
            downPressed = false;
        }
        public void KeyDow(Keys key)
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
                Column = index % 4;
                g.DrawImage(sprite, x, y, new Rectangle(Column * 48, Row * 64, 48, 64), GraphicsUnit.Pixel);
                index++;
                if (index >= 4)
                {
                    index = 1;
                }
            }
            else
            {
                index = 0;
                g.DrawImage(sprite, x, y, new Rectangle(0, Row * 64, 48, 64), GraphicsUnit.Pixel);
            }
        }
        public void Update()
        {
            if (leftPressed) x -= 5;
            if (rightPressed) x += 5;
            if (upPressed) y -= 5;
            if (downPressed) y += 5;
        }
    }
}
