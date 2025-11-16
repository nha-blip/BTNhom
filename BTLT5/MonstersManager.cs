using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLT5
{
    internal class MonstersManager
    {
        private List<Monster> monsters;
        private Random random;

        public MonstersManager(Random r)
        {
            monsters = new List<Monster>();
            random = r;
        }

        public void AddMonster(int x, int y, int speed = 3)
        {
            Monster monster = new Monster(x, y);
            monsters.Add(monster);
        }

        public void spawnMonster(int formWidth, int formHeight)
        {
            int edge = random.Next(4);
            int x, y;

            switch (edge)
            {
                // Top edge spawn
                case 0:
                    x = random.Next(formWidth);
                    y = -64;
                    break;
                // Left edge spawn
                case 1:
                    x = -48;
                    y = random.Next(formHeight);
                    break;
                // Right edge spawn
                case 2:
                    x = formWidth;
                    y = random.Next(formHeight);
                    break;
                // Bottom edge spawn
                case 3:
                    x = random.Next(formWidth);
                    y = formHeight;
                    break;
                default:
                    x = 0;
                    y = 0;
                    break;
            }

            AddMonster(x, y);
        }

        public void DrawAll(Graphics g)
        {
            foreach(Monster monster in monsters)
            {
                monster.Draw(g);
            }
        }

        public void UpdateAll(int x, int y)
        {
            monsters.RemoveAll(m => m.isDead);
            foreach(Monster monster in monsters)
            {
                monster.Update(x, y);
            }
        }
    }
}
