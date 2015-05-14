﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moving
{
    public class Ship
    {
        public static Ship Create(int velocity, int x = 0, int y = 0)
        {
            return new Ship(x, y) { Velocity = velocity };
        }

        public Ship(int posX, int posY)
        {
            this.PosX = posX;
            this.PosY = posY;
            this.moveDestination = new Vector();

            lastMove = DateTime.Now;
            moveDuration = 0;
        }

        protected int PosX { get; set; }
        protected int PosY { get; set; }
        public int ShipID { get; set; }

        /// <summary>
        /// Pixels/second
        /// </summary>
        public int Velocity { get; set; }

        /// <summary>
        /// Last time Move() was called
        /// </summary>
        DateTime lastMove;
        double moveDuration; // MILLISECONDS
        /// <summary>
        /// Destination
        /// </summary>
        Vector moveDestination;
        /// <summary>
        /// Delta
        /// </summary>
        Vector direction;
        bool Moving = false;

        public double DistanceTo(Vector otherdude)
        {
            return Math.Sqrt(((otherdude.X - PosX) * (otherdude.X - PosX)) + ((otherdude.Y - PosY) * (otherdude.Y - PosY)));
        }

        private Vector startMove;
        public double MoveDistance
        {
            get;
            private set;
        }

        public void Move(int x, int y)
        {
            Vector currPosition = Position;
            this.PosX = (int)currPosition.X;
            this.PosY = (int)currPosition.Y;
            startMove = currPosition;
            Moving = true;
            direction = new Vector(x - PosX, y - PosY);
            moveDestination = new Vector(x, y);

            double dist = Math.Sqrt((direction.X*direction.X + direction.Y*direction.Y));
            this.moveDuration = 
                dist / this.Velocity * 1000; // speed is in px/s, not px/ms
                /* t = d/v */

            this.MoveDistance = dist;

            lastMove = DateTime.Now;
        }

        public double MovedRatio
        {
            get
            {
                if (startMove != null)
                    return this.Position.DistanceTo(startMove) / this.MoveDistance;
                return 0;
            }
        }

        public bool IsMoving
        {
            get
            {
                return Moving;
            }
        }

        public Vector Position
        {
            get
            {
                if (Moving)
                {
                    double timeElapsed = (DateTime.Now - lastMove).TotalMilliseconds;
                    if (timeElapsed < moveDuration)
                    {
                        double movedRatio = timeElapsed / moveDuration;
                        return new Vector(
                            PosX + (direction.X * movedRatio),
                            PosY + (direction.Y * movedRatio)
                        );
                    }
                    else
                    {
                        this.Moving = false;
                        this.PosX = (int)(PosX + this.direction.X);
                        this.PosY = (int)(PosY + this.direction.Y);
                        return new Vector(PosX, PosY);
                    }
                }
                else
                {
                    return new Vector(PosX, PosY);
                }
            }
        }

        public Vector Destination
        {
            get
            {
                return moveDestination;
            } 
        }

        public void StopThere()
        {
            var pos = this.Position;
            this.UpdatePosition((int)pos.X, (int)pos.Y);
            // this ensures the ship stops moving
        }

        public void UpdatePosition(int x, int y)
        {
            PosX = x;
            PosY = y;
            Moving = false;
        }

        public void ChangeSpeed(int newV)
        {
            this.StopThere();
            this.Velocity = newV;
            this.Move((int)this.moveDestination.X, (int)this.moveDestination.Y);
        }
    }
    
}
