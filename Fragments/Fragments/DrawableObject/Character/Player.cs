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
        private const int movementSpeed = 4;

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

        //Constructor
        public Player(int x, int y, int w, int h) 
            : base(x, y, w, h)
        {
            //TODO
        }

        //MOVEMENT FUNCTIONS
        public void Move(KeyboardState kb, GameManager gameManager)
        {
            switch(gameManager.State)
            {
                case GameManager.GameState.Town:
                    switch(movementState)
                    {
                        case MovementState.StandingRight:
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

        public bool IsColliding(Layer l)
        {
            foreach(KeyValuePair<Texture2D, Vector2> t in l.Objects)
            {
                Rectangle objRect = new Rectangle(
                    (int)t.Value.X + (int)l.X, 
                    (int)t.Value.Y, 
                    t.Key.Width, 
                    t.Key.Height);

                if (this.rec.Intersects(objRect)
                    && (t.Key == GameManager.Instance.CurrentMap.Textures["wall"]))
                {
                    return true;
                }
            }

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (flipped)
            {
                spriteBatch.Draw(
                    texture,
                    null,
                    rec,
                    null,
                    null,
                    0,
                    null,
                    Color.White,
                    SpriteEffects.FlipHorizontally);
            } 
            else
            {
                base.Draw(spriteBatch);
            }

        }
        public int Movement { get { return movementSpeed; } }
        public MovementState MS { get { return movementState; } } 
    }
}
