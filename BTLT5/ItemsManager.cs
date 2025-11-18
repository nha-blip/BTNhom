using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLT5
{
    internal class ItemsManager
    {
        private List<Item> items;
        private Random random;

        public List<Item> Items { get { return items; } }

        public ItemsManager(Random r)
        {
            items = new List<Item>();
            random = r;
        }

        public void AddItem(int x, int y)
        {
            Item item = new Item(x, y);
            items.Add(item);
        }

        public void spawnItem(int formWidth, int formHeight)
        {
            int x = random.Next(100, formWidth - 100);
            int y = random.Next(100, formHeight - 100);

            AddItem(x, y);
        }

        public void DrawAll(Graphics g)
        {
            foreach (Item item in items)
            {
                item.Draw(g);
            }
        }

        public void UpdateAll(int x, int y)
        {
            items.RemoveAll(i => !i.IsAlive);
            foreach (Item item in items)
            {
                item.Update();
            }
        }
    }
}
