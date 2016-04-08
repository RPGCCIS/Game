using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Fragments
{
    class Shop
    {
        private List<Item> items;

        public List<Item> Items
        {
            get { return items; }
        }

        public Shop()
        {
            items = new List<Item>();
            LoadItems();
        }
        public void LoadItems()
        {
            try
            {
                using (Stream inStream = File.OpenRead("../../../Shops/" + GameManager.Instance.CurrentMap.MapName + ".txt"))
                using (StreamReader input = new StreamReader(inStream))
                {
                    int count = int.Parse(input.ReadLine());
                    for(int i = 0; i < count; i++)
                    {
                        items.Add(new Item(input.ReadLine(), int.Parse(input.ReadLine()), int.Parse(input.ReadLine()), int.Parse(input.ReadLine()), int.Parse(input.ReadLine()), int.Parse(input.ReadLine()), int.Parse(input.ReadLine())));
                    }
                    input.Close();
                }
            }catch(Exception e)
            {

            }
        }
        
    }
}
