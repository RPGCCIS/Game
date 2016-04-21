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
    class Player : Character
    {
        //private List<Equipment> equippedEquippment;

        public enum MovementState
        {
            StandingRight,
            WalkingRight,
            StandingLeft,
            WalkingLeft
        }
        private MovementState movementState;
        bool flipped; //SpriteEffect
        private Vector2 mapPos = new Vector2(9, 12);
        // Texture and drawing
        Texture2D spriteSheet;
        Vector2 playerLoc;      

        // Animation
        int frame;              
        double timeCounter;     
        double fps;             
        double timePerFrame;

        // Constants for "source" rectangle (inside the image)
        const int WALK_FRAME_COUNT = 7;
                 
        const int PLAYER_RECT_Y_OFFSET = 65;
            
        const int PLAYER_RECT_HEIGHT = 31;       
        const int PLAYER_RECT_WIDTH = 18;

        const int PLAYER_FRAME_OFFSET = 6;
        const float PLAYER_SIZE = 4.5f;

        public Texture2D SpriteSheet
        {
            set { spriteSheet = value; }
        }
        public int PlayerWidth
        {
            get { return (int)(PLAYER_RECT_WIDTH * PLAYER_SIZE); }
        }
        public int PlayerHeight
        {
            get { return (int)(PLAYER_RECT_HEIGHT * PLAYER_SIZE); }
        }
        public Vector2 MapPos
        {
            get { return mapPos; }
            set { mapPos = value; }
        }
        public float mapX
        {
            get { return mapPos.X; }
            set { mapPos.X = value; }
        }
        public float mapY
        {
            get { return mapPos.Y; }
            set { mapPos.Y = value; }
        }
        //Constructor
        public Player(int x, int y, int w, int h, Texture2D texture) 
            : base(x, y, (int)(PLAYER_RECT_WIDTH * PLAYER_SIZE), (int)(PLAYER_RECT_HEIGHT * PLAYER_SIZE), texture)
        {
            playerLoc = new Vector2(x, y);
            fps = 10.0;
            timePerFrame = 1.0 / fps;
            //TODO
        }

        //MOVEMENT FUNCTIONS
        public void Move(KeyboardState kb,GameTime gameTime)
        {
            switch(GameManager.Instance.State)
            {
                case GameManager.GameState.Town:
                    
                    switch (movementState)
                    {
                        case MovementState.StandingRight:
                            frame = 0;
                            if(kb.IsKeyDown(Keys.Right))
                            {
                                movementState = MovementState.WalkingRight;
                            }
                            else if (kb.IsKeyDown(Keys.Left))
                            {
                                movementState = MovementState.WalkingLeft;
                            }
                            break;

                        case MovementState.WalkingRight:
                            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
                            if (timeCounter >= timePerFrame)
                            {
                                frame += 1;

                                if (frame > WALK_FRAME_COUNT)
                                    frame = 1;

                                timeCounter -= timePerFrame;
                            }
                            if (kb.IsKeyUp(Keys.Right))
                            {
                                movementState = MovementState.StandingRight;
                            }
                            else if (kb.IsKeyDown(Keys.Left))
                            {
                                movementState = MovementState.WalkingLeft;
                            }
                            break;

                        case MovementState.StandingLeft:
                            frame = 0;
                            if (kb.IsKeyDown(Keys.Right))
                            {
                                movementState = MovementState.WalkingRight;
                            }
                            else if (kb.IsKeyDown(Keys.Left))
                            {
                                movementState = MovementState.WalkingLeft;
                            }
                            break;

                        case MovementState.WalkingLeft:
                            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
                            if (timeCounter >= timePerFrame)
                            {
                                frame += 1;

                                if (frame > WALK_FRAME_COUNT)
                                    frame = 1;

                                timeCounter -= timePerFrame;
                            }
                            if (kb.IsKeyDown(Keys.Right))
                            {
                                movementState = MovementState.WalkingRight;
                            }
                            else if (kb.IsKeyUp(Keys.Left))
                            {
                                movementState = MovementState.StandingLeft;
                            }
                            break;
                    }

                    //For graphics
                    flipped = (movementState == MovementState.StandingLeft 
                        || movementState == MovementState.WalkingLeft);
                    break;
            }
        }

        public bool IsColliding(Layer l, TypeOfObject type)
        {
            foreach (DrawableObject obj in l.Objects)
            {
                Rectangle objRect = new Rectangle(
                    obj.X + (int)l.X,
                    obj.Y,
                    obj.Rec.Width,
                    obj.Rec.Height);

                if (this.rec.Intersects(objRect)
                    && (obj.Type == type))
                {
                    if(type == TypeOfObject.Interactable)
                    {
                        InteractableObject iObj = (InteractableObject)(obj);
                        if (iObj.Destination == "Shop")
                        {
                            ShopManager.Instance.UpdateShop();
                            GameManager.Instance.State = GameManager.GameState.Shop;
                        }
                        else if(iObj.Destination == "Inn")
                        {
                            GameManager.Instance.Player.Hp = MaxHp;
                            GameManager.Instance.Player.Sp = MaxSp;
                            GameManager.Instance.Save();
                            
                        }
                        else 
                        {
                            
                            MapManager.Instance.LoadMap(iObj.Destination);
                        }

                    }

                    if (type == TypeOfObject.Gate)
                    {
                        GameManager.Instance.State = GameManager.GameState.Map;
                    }

                    if (type == TypeOfObject.NPC)
                    {
                        Console.WriteLine("ASDASDSADASDADS");
                        //TODO
                        NPC npc = (NPC)(obj);
                        GameManager.Instance.CT = npc.Conversation;
                        GameManager.Instance.Conversation = true;
                    }

                    return true;
                }
            }

            return false;
        }

        public new void Draw(SpriteBatch spriteBatch,Color c)
        {
            if (flipped)
            {
                //spriteBatch.Draw(
                //    texture,
                //    null,
                //    rec,
                //    null,
                //    null,
                //    0,
                //    null,
                //    Color.White,
                //    SpriteEffects.FlipHorizontally);
                if(movementState==MovementState.StandingRight)
                {
                    spriteBatch.Draw(
                    spriteSheet,                    // - The texture to draw
                    playerLoc,                       // - The location to draw on the screen
                    new Rectangle(                  // - The "source" rectangle
                    0,                          //   - This rectangle specifies
                    PLAYER_RECT_Y_OFFSET,        //	   where "inside" the texture
                    PLAYER_RECT_WIDTH,           //     to get pixels (We don't want to
                    PLAYER_RECT_HEIGHT),         //     draw the whole thing)
                    c,                    // - The color
                    0,                              // - Rotation (none currently)
                    Vector2.Zero,                   // - Origin inside the image (top left)
                    PLAYER_SIZE,                           // - Scale (100% - no change)
                    SpriteEffects.None,                     // - Can be used to flip the image
                    0);
                }
                else
                {
                    spriteBatch.Draw(
                spriteSheet,                    // - The texture to draw
                playerLoc,                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    frame * (PLAYER_RECT_WIDTH + PLAYER_FRAME_OFFSET),   //   - This rectangle specifies
                    PLAYER_RECT_Y_OFFSET,        //	   where "inside" the texture
                    PLAYER_RECT_WIDTH,           //     to get pixels (We don't want to
                    PLAYER_RECT_HEIGHT),         //     draw the whole thing)
                c,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                PLAYER_SIZE,                           // - Scale (100% - no change)
                SpriteEffects.None,                     // - Can be used to flip the image
                0);
                }
                
            } 
            else
            {
                if (movementState == MovementState.StandingLeft)
                {
                    spriteBatch.Draw(
                    spriteSheet,                    // - The texture to draw
                    playerLoc,                       // - The location to draw on the screen
                    new Rectangle(                  // - The "source" rectangle
                    0,                          //   - This rectangle specifies
                    PLAYER_RECT_Y_OFFSET,        //	   where "inside" the texture
                    PLAYER_RECT_WIDTH,           //     to get pixels (We don't want to
                    PLAYER_RECT_HEIGHT),         //     draw the whole thing)
                    c,                    // - The color
                    0,                              // - Rotation (none currently)
                    Vector2.Zero,                   // - Origin inside the image (top left)
                    PLAYER_SIZE,                           // - Scale (100% - no change)
                    SpriteEffects.FlipHorizontally,                     // - Can be used to flip the image
                    0);
                }
                else
                {
                    spriteBatch.Draw(
                spriteSheet,                    // - The texture to draw
                playerLoc,                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    frame * (PLAYER_RECT_WIDTH + PLAYER_FRAME_OFFSET),   //   - This rectangle specifies
                    PLAYER_RECT_Y_OFFSET,        //	   where "inside" the texture
                    PLAYER_RECT_WIDTH,           //     to get pixels (We don't want to
                    PLAYER_RECT_HEIGHT),         //     draw the whole thing)
                c,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                PLAYER_SIZE,                           // - Scale (100% - no change)
                SpriteEffects.FlipHorizontally,                     // - Can be used to flip the image
                0);
                }
            }

        }
        public MovementState MS { get { return movementState; } }

        public void DrawOverworld(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                GameManager.Instance.CurrentMap.GetTexture("playerOverworld"), 
                new Rectangle((900 / 14)*(int)mapPos.X, (750 / 14) *(int)mapPos.Y, 
                900 / 14, 750 / 14), Color.White);
        }
    }
}
