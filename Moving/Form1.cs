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
        //Thread redrawThread;
        Random random = new Random();

        Ship lockedShip;
        bool mouseDown = false;
        bool parkShip = false;
        bool moveShips = false;

        int generalSpeed = 50;

        public Form1()
        {
            CreateShips();
            CheckForIllegalCrossThreadCalls = true;

            InitializeComponent();
            MyTimer = new System.Windows.Forms.Timer();
            MyTimer.Interval = 50;

            nextRedraw = DateTime.Now.AddMilliseconds(500);
            MyTimer.Tick += MyTimer_Tick;
            MyTimer.Start();

            this.DoubleBuffered = true;
            this.mapBox.Paint += mapBox_Paint;
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyPress;
        }

        void Form1_KeyPress(object sender, KeyEventArgs e)
        {
            var key = e.KeyCode;
            switch (key)
            {
                case Keys.Escape:
                    loseLock();
                    break;
                case Keys.Space:
                    this.lockedShip = this.Ships[this.getRandom(0, this.Ships.Count)];
                    e.SuppressKeyPress = true;
                    break;
                case Keys.C:
                    AddShips(int.Parse(shpNmbTextBox.Text), getPosRelative().X, getPosRelative().Y);
                    break;
                case Keys.Back:
                    if (shpNmbTextBox.Focused == false)
                        RemoveShips(int.Parse(shpNmbTextBox.Text));
                    break;
            }
        }

        public void CreateShips()
        {
            this.Ships = new List<Ship>();
            for (int i = 0; i < 1; i++)
            {
                var ship = Ship.Create(velocity: generalSpeed);
                ship.ShipID = i;
                Ships.Add(ship);
            }
        }

        void MyTimer_Tick(object sender, EventArgs e)
        {
            lock (Ships)
                movingLabel.Text = string.Format("Moving: {0}/{1}", Ships.Count(s=>s.IsMoving), Ships.Count);

            shipIdLabel.Text = "Ship ID: ";
            locationLabel.Text = "Location: ";
            speedLabel.Text = "Speed: " + (lockedShip == null ? generalSpeed : lockedShip.Velocity) + "px/sec";
            destinationLabel.Text = "Destination: ";
            this.distanceLabel.Text = "Distance: ";

            if (lockedShip != null)
            {
                var pos = lockedShip.Position;
                shipIdLabel.Text += lockedShip.ShipID;
                locationLabel.Text += string.Format("{0} X | {1} Y", (int)pos.X, (int)pos.Y);
                destinationLabel.Text += string.Format("{0} X | {1} Y", (int)lockedShip.Destination.X, (int)lockedShip.Destination.Y);
                //directionProgressBar.Value = (int)((lockedShip.MovedRatio < 1 ? lockedShip.MovedRatio : 1) * 100);
                this.distanceLabel.Text += string.Format(
                    "{0} / {1}\n<{2}%>",
                    (lockedShip.MoveDistance * lockedShip.MovedRatio).ToString("F1"),
                    (lockedShip.MoveDistance).ToString("F1"), 
                    (lockedShip.MovedRatio*100).ToString("F1"));

                if (mouseDown && mapBox.Capture)
                {
                    var mousePos = getPosRelative();
                    lockedShip.Move(mousePos.X, mousePos.Y);
                }
            }

            if (nextRedraw <= DateTime.Now)
            {
                this.mapBox.Invalidate();
                nextRedraw = nextRedraw.AddMilliseconds(MyTimer.Interval);
            }
        }

        SolidBrush shipBrush = new SolidBrush(Color.Red);
        SolidBrush lockedShipBrush = new SolidBrush(Color.Blue);
        Pen pathPen = new Pen(Color.Violet, 2.5f);
        Font fpsFont = new Font("Arial", 10.5f, FontStyle.Bold);

        DateTime lastRedraw = DateTime.Now;
        int fpsCalcLimiter = 0;
        double lastFps = 24;
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
                if (ship.IsMoving == false && this.moveShips == true)
                {
                    if (!(ship == lockedShip && (mouseDown || parkShip)))
                        ship.Move(getRandom(0, maxX), getRandom(0, maxY));
                }
                if (ship == lockedShip)
                {
                    if (ship.IsMoving && (this.showPathCheckBox.Checked))
                    {
                        g.DrawLine(pathPen,
                            new PointF((float)pos.X, (float)pos.Y),
                            new PointF((float)ship.Destination.X, (float)ship.Destination.Y));
                    }
                    if (parkShip == true)
                    {
                        g.FillEllipse(
                            new SolidBrush(Color.Firebrick),
                            (float)ship.Destination.X - 3f, (float)ship.Destination.Y - 3f,
                            6f, 6f);
                    }
                    g.FillEllipse(this.lockedShipBrush, (float)pos.X - 5f, (float)pos.Y - 5f, 10f, 10f);
                    continue;
                }
                if (ship.IsMoving && this.showAllPaths)
                {
                    g.DrawLine(pathPen,
                        new PointF((float)pos.X, (float)pos.Y),
                        new PointF((float)ship.Destination.X, (float)ship.Destination.Y));
                }
                g.FillEllipse(this.shipBrush, (float)pos.X - 5f, (float)pos.Y - 5f, 10f, 10f);
            }
            double ms = (DateTime.Now - lastRedraw).TotalMilliseconds;
            if (fpsCalcLimiter % 5 == 0)
            {
                this.lastFps = 1000 / ms;
            }
            g.DrawString("FPS: " + (lastFps).ToString("F2"),
                fpsFont,
                lockedShipBrush,
                new PointF(this.mapBox.Width - 80, 5));
            lastRedraw = DateTime.Now;
            fpsCalcLimiter++;
        }


        int getRandom(int min, int max)
        {
            return this.random.Next(min, max);
        }


        private void mapBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Vector clickPos = new Vector(e.X, e.Y);
                lock (this.Ships)
                {
                    foreach (var ship in this.Ships)
                    {
                        if (ship.Position.DistanceTo(clickPos) <= 7.5)
                        {
                            lockedShip = ship;
                            return;
                        }
                    }
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                OnRightClick(sender, e);
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


        private void mapBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.mouseDown = true;
                this.parkShip = false;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                OnRightClick(sender, e);
            }
        }

        private void mapBox_MouseUp(object sender, MouseEventArgs e)
        {
            this.mouseDown = false;
        }
        
        private void startButton_Click(object sender, EventArgs e)
        {
            moveShips = true;
        }

        private void breakButton_Click(object sender, EventArgs e)
        {
            moveShips = false;
        }

        private void forceBreakButton_Click(object sender, EventArgs e)
        {
            moveShips = false;
            Ship[] ships = SafeShipArray();
            for (int i = 0; i < ships.Length; i++)
                ships[i].StopThere();
        }

        private Ship[] SafeShipArray()
        {
            Ship[] ships;
            lock (this.Ships)
                ships = this.Ships.ToArray();
            return ships;
        }

        private void lockspeedSlider_ValueChanged(object sender, EventArgs e)
        {
            if (lockedShip == null)
            {
                Ship[] ships = SafeShipArray();
                for (int i = 0; i < ships.Length; i++)
                {
                    ships[i].ChangeSpeed(lockspeedSlider.Value);
                }
                generalSpeed = lockspeedSlider.Value;
            }
            else
            {
                lockedShip.ChangeSpeed(lockspeedSlider.Value);
            }
        }

        private void adjButton_Click(object sender, EventArgs e)
        {
            int wantedAmount = int.Parse(shpNmbTextBox.Text);
            if (wantedAmount > Ships.Count)
            {
                AddShips(wantedAmount-Ships.Count, 0, 0);
            }
            else if (wantedAmount < Ships.Count)
            {
                RemoveShips(Ships.Count-wantedAmount);
            }
        }

        private void AddShips(int number, int x, int y)
        {
            lock (Ships)
            {
                for (int i = 0; i < number; i++)
                {
                    Ship s = Ship.Create(generalSpeed, x, y);
                    s.ShipID = Ships.Count;
                    Ships.Add(s);
                }
            }
        }

        private void RemoveShips(int number)
        {
            if (number > Ships.Count)
                number = Ships.Count;
            lock (Ships)
            {
                Ships.RemoveRange(Ships.Count - number, number);
            }
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            AddShips(int.Parse(shpNmbTextBox.Text), 0,0 );
        }

        private void remBtn_Click(object sender, EventArgs e)
        {
            RemoveShips(int.Parse(shpNmbTextBox.Text));
        }

        private void mapBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lockedShip != null)
            {
                lockedShip.Move(e.X, e.Y);
                parkShip = true;
            }
        }

        private void OnRightClick(object sender, MouseEventArgs e)
        {
            Point loc = getPosRelative();
            contextMenuStrip1.Show(this.mapBox, loc);
            contextMenuStrip1.Location = loc;
        }

        private void loseLockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loseLock();
        }

        private void loseLock()
        {
            this.lockedShip = null;
        }

#pragma warning disable
        private void allMoveHereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            moveShips = true;
            Ship[] ships = SafeShipArray();
            int x = contextMenuStrip1.Location.X, y = contextMenuStrip1.Location.Y;
            for (int i = 0; i < ships.Length; i++)
            {
                ships[i].Move(x, y);
            }
        }
#pragma warning enable


        private void allStopHereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            allMoveHereToolStripMenuItem_Click(sender, e);
            moveShips = false;
        }

        private void createShipsHereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int x = contextMenuStrip1.Location.X,
                y = contextMenuStrip1.Location.Y;
            AddShips(int.Parse(shpNmbTextBox.Text), x, y);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.MyTimer.Interval = trackBar1.Value;
        }

        bool showAllPaths = false;
        private void showPathsCheckboxk_CheckedChanged(object sender, EventArgs e)
        {
            showAllPaths = showAllPathsCheckboxk.Checked;
        }
    }
}