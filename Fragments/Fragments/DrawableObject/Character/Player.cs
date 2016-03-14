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
        public Player(int x, int y, int w, int h, Texture2D texture) 
            : base(x, y, w, h, texture)
        {
            //TODO
        }

        //MOVEMENT FUNCTIONS
        public void Move(KeyboardState kb)
        {
            switch(GameManager.Instance.State)
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

        /*
        public bool IsColliding(Layer l)
        {
            foreach(DrawableObject obj in l.Objects)
            {
                Rectangle objRect = new Rectangle(
                    obj.X + (int)l.X, 
                    obj.Y, 
                    obj.Texture.Width,
                    obj.Texture.Height);

                if (this.rec.Intersects(objRect)
                    && (obj.Texture == GameManager.Instance.CurrentMap.Textures["wall"]))
                {
                    return true;
                }
            }

            return false;
        }
        */

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
                    return true;
                }
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
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
