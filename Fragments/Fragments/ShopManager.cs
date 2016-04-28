using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
namespace Fragments
{
    class ShopManager
    {
        private static ShopManager instance;
        private Shop current;
        private Texture2D scroll;
        private SpriteFont font;
        private TextObject message;
        private TextList options;
        private Message shop = new Message("scroll", false);

        public Texture2D Scroll
        {
            set { scroll = value; }
        }

        public SpriteFont Font
        {
            set { font = value; }
        }
        public static ShopManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ShopManager();

                }
                return instance;
            }
        }
        public TextList Options
        {
            get { return options; }
        }
        public Shop Current
        {
            get { return current; }
            set { current = value; }
        }

        //public List<Item> GetItems()
        //{
        //    return current.Items;
        //}
        
        public void UpdateShop()
        {
            Console.WriteLine("Entering " + GameManager.Instance.CurrentMap.MapName);
            message = new TextObject(font, "Hello, and welcome to my shop in " + GameManager.Instance.CurrentMap.MapName, new Vector2(shop.RectX + 150, shop.RectY + 50));
            options = new TextList(null, new Vector2(shop.RectX + 175, shop.RectY + 125));
            options.Font = font;
            foreach (Item i in ShopManager.Instance.Current.Items)
            {
                options.Add("Lvl " + i.Level + " " +i.Name.ToUpper() + "  Cost: " + i.Cost+ " gold");
            }
            shop.Texture = scroll;
            shop = new Message(message, options, scroll, shop.Rect);
            shop.Rect = new Rectangle(0, 0, 925, 600);
        }
        public void DrawItems(SpriteBatch sb)
        {          
            shop.Draw(sb);
        }
    }
}
