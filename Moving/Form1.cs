using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moving
{
    public partial class Form1 : Form
    {
        public List<Ship> Ships;
        public System.Windows.Forms.Timer MyTimer;
        DateTime nextRedraw;
        Thread redrawThread;

        Random random = new Random();

        Ship lockedShip;

        public Form1()
        {
            CreateShips();
            CheckForIllegalCrossThreadCalls = true;

            InitializeComponent();
            MyTimer = new System.Windows.Forms.Timer();
            MyTimer.Interval = 40;

            nextRedraw = DateTime.Now.AddMilliseconds(500);
            MyTimer.Tick += MyTimer_Tick;
            MyTimer.Start();

            this.DoubleBuffered = true;
            this.mapBox.Paint += mapBox_Paint;
        }

        public void CreateShips()
        {
            this.Ships = new List<Ship>(5);
            for (int i = 0; i < 10; i++)
            {
                Ships.Add(new Ship(0, 0));
            }
        }

        void MyTimer_Tick(object sender, EventArgs e)
        {


            if (nextRedraw <= DateTime.Now)
            {
                this.mapBox.Invalidate();
                nextRedraw = nextRedraw.AddMilliseconds(30);
            }
        }



        void mapBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int maxX = this.mapBox.Width;
            int maxY = this.mapBox.Height;
            
            Ship[] ships;
            lock (this.Ships)
                ships = this.Ships.ToArray();

            foreach (var ship in ships)
            {
                var pos = ship.Position;
                if (ship.IsMoving == false)
                {
                    ship.Move(getRandom(0, maxX), getRandom(0, maxY));
                    //ship.Move((int)pos.X + 100, 0);
                }
                SolidBrush brush = new SolidBrush(ship == lockedShip ? Color.Red : Color.Blue);
                g.FillEllipse(brush, (float)pos.X - 5f,(float) pos.Y - 5f, 10f, 10f);
            }
        }


        int getRandom(int min, int max)
        {
            return this.random.Next(min, max);
        }


        private void mapBox_MouseClick(object sender, MouseEventArgs e)
        {
            Vector clickPos = new Vector(e.X, e.Y);
            lock (this.Ships)
            {
                foreach (var ship in this.Ships)
                {
                    if (ship.Position.DistanceTo(clickPos) <= 7f)
                    {
                        lockedShip = ship;
                        return;
                    }
                }
            }
        }

        Point getPosRelative(Control control)
        {
            return control.PointToClient(Cursor.Position);
        }
        Point getPosRelative()
        {
            return mapBox.PointToClient(Cursor.Position);
        }
    }
}