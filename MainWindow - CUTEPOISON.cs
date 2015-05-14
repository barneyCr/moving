using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CutePoison.Helper.Classes;
using CutePoison.Languages;

namespace CutePoison
{
    public sealed partial class MainWindow : Form, ILocaleDependant
    {
        System.Windows.Forms.Timer MyTimer;
        Thread RedrawThread;

        Ship lockedOnMinimapShip;
        bool mouseDown, resizeMouseDown;
        bool stoppedMoving;
        Point mapResizeStart;

        DateTime nextRedraw;

        public MainWindow(string uid, string sid, string url)
        {
            CheckForIllegalCrossThreadCalls = true;
            Core.MainForm = this;
            Core.UserID = uid;
            Core.SessionID = sid;
            InitializeComponent();
            //RedrawThread = new Thread(() => {
            //    while (this.IsDisposed == false)
            //    {
            //        if (this.IsHandleCreated == false) { Thread.Sleep(200); continue; }
            //        try
            //        {
            //            this.Invoke(new MethodInvoker(this.mapBox.Invalidate));
            //            Thread.Sleep(50);
            //        }
            //        catch (ThreadAbortException)
            //        {
            //            throw;
            //        }
            //        catch
            //        {
            //            Thread.Sleep(100);
            //        }
            //    }
            //});
            //RedrawThread.IsBackground = true;
            //RedrawThread.Priority = ThreadPriority.Lowest;
            //RedrawThread.Start();

            nextRedraw = DateTime.Now.AddMilliseconds(500);

            previousLocations = new List<Point>(locationThreshold);

            MyTimer = new System.Windows.Forms.Timer() { Interval = 40 };
            MyTimer.Tick += MyTimer_Tick;
            MyTimer.Start();

            Core.Connect(url);

            this.mapBox.DoubleClick += mapBox_DoubleClick;
            this.Load += async delegate {
                ShowLanguages();
            };
            Core.LocaleManager.LocaleLoaded += LocaleManager_LocaleLoaded;
            Core.LocaleManager.CacheCleared += LocaleManager_CacheCleared;
            Program.LoadLanguagesTask.ContinueWith(delegate 
                {
                    this.languageCombobox.Enabled = true; 
                    //MessageBox.Show("Loaded languages!"); 
                }, TaskScheduler.FromCurrentSynchronizationContext());

            //this.resizeImage.MouseDown += (s, args) => { this.resizeMouseDown = true; this.mapResizeStart = getPosRelative(this); };
            //this.resizeImage.MouseUp += resizeMouseDownHandler;
            //this.resizeImage.MouseMove += this.resizeImage_MouseMove;
        }

        private void resizeMouseDownHandler(object sender, MouseEventArgs args)
        {
            this.resizeMouseDown = false;
        }

        #region Locale implementation
        void LocaleManager_CacheCleared()
        {
            lock (languageCombobox)
            {
                languageCombobox.Items.Clear();
            }
        }
        void LocaleManager_LocaleLoaded(LocaleLoadedEventArgs args)
        {
            lock (languageCombobox)
            {
                languageCombobox.Items.Add(args.LanguageName);
            }
        }
        void ShowLanguages()
        {
            this.languageCombobox.Items.Clear();
            foreach (var file in Directory.EnumerateFiles(Environment.CurrentDirectory + @"\Languages\Files\", "*.txt")){
                languageCombobox.Items.Add(file.Split('\\').Last().Split('.').FirstOrDefault());
            }
        }
        public void ChangeLocale(dynamic lang)
        {
            if (lang == null)
            {
                MessageBox.Show("Language file doesn't exist!");
                return;
            }
            this.Text = Core.Hero == null ? "CutePoison" : String.Format(lang.main.title, Core.Hero.Username);

            dynamic obj = lang.tabs.settings;

            this.settingsTabPage.Text = lang.tabs.settings.head;
            this.statsTabpage.Text = lang.tabs.stats.head;
            this.fightSettingsTab.Text = lang.tabs.fight.head;

            this.killMobsChk.Text = obj.npcs;
            this.killPlayersChk.Text = obj.players;
            this.collectBoxesChk.Text = obj.boxes;
            this.ammoLbl.Text = obj.ammo;
            this.useRsbChk.Text = obj.rsbchanger;
            this.useSabChk.Text = obj.sabchanger;

            obj = lang.tabs.stats.earn;

            this.uriCollectedLbl.Text = obj.uri;
            this.crCollectedLbl.Text = obj.credits;
            this.boxesCollectedLbl.Text = obj.boxes;
            this.oreCollectedLbl.Text = obj.ores;
            this.expCollectedLbl.Text = obj.exp;
            this.honCollectedLbl.Text = obj.hon;
            obj = lang.tabs.stats.killed;
            this.shipsDestroyedLbl.Text = obj.ships;
            this.npcsDestroyedLbl.Text = obj.npcs;

            obj = lang.tabs.fight;
            this.empCloakCheckbox.Text = obj.empcloak;
            this.useEmpChk.Text = obj.empfight;
            this.useIshChk.Text = obj.useish;
            this.useSmbChk.Text = obj.usesmb;

            this.mapLogo = lang.mini.copyright;
            // haha :P
        }
        private async void languageCombobox_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                await Core.LocaleManager.GetLocale(this, languageCombobox.SelectedItem.ToString());
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                MessageBox.Show("Invalid language file");
            }
            catch { MessageBox.Show("General error, please visit our website and re-download language files!"); }
        }

        #endregion

        List<Point> previousLocations;
        int locationThreshold = 20;
        void MyTimer_Tick(object sender, EventArgs e)
        {
            if (Core.Hero != null && Core.CurrentMap != null)
            {
                var heroPos = Core.Hero.Position;
                Point posAsPoint = new Point((int)(heroPos.x / Core.CurrentMap.ByX), (int)(heroPos.y / Core.CurrentMap.ByY));
                if (Core.Hero.IsMoving)
                {
                    //previousLocations[tailPrevLocations = (tailPrevLocations % 99) + 1] = posAsPoint;
                    lock (previousLocations)
                    {
                        while (previousLocations.Count > locationThreshold)
                        {
                            previousLocations.RemoveAt(0);
                        }
                        previousLocations.Add(posAsPoint);
                    }
                }
                if (lockedOnMinimapShip != null)
                {
                    // add code if needed
                }
                if (mouseDown && mapBox.Capture)
                {
                    var mousePos = getPosRelative();
                    Core.BeginManualOverride();
                    Core.MoveTo(mousePos.X * Core.CurrentMap.ByX, mousePos.Y * Core.CurrentMap.ByY);
                }
                else if (!stoppedMoving)
                {
                    Core.EndManualOverride();
                }
            }
            if (nextRedraw <= DateTime.Now) {
                this.mapBox.Invalidate();
                nextRedraw = nextRedraw.AddMilliseconds(100);
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

        string mapLogo = "By Barney, thanks to jD";

        public void LoadMap()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => LoadMap()));
                return;
            }
            if (Core.CurrentMap == null)
            {
                Image img = new Bitmap(mapBox.Size.Width, mapBox.Size.Height);
                Graphics g = Graphics.FromImage(img);
                g.FillRectangle(new SolidBrush(Color.Azure), 0, 0, mapBox.Size.Width, mapBox.Size.Height);
                string str = "Loading map...";
                Font f = bigFontSegoe;
                SizeF size = g.MeasureString(str, f);
                g.DrawString(str, f, new SolidBrush(Color.OrangeRed), (img.Width / 2) - (size.Width / 2), (img.Height / 2) - (size.Height / 2));
                mapBox.BackgroundImage = img;
                return;
            }


            try
            {
                mapBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                mapBox.BackgroundImage = System.Drawing.Image.FromFile(Environment.CurrentDirectory + @"\Resources\Images\" + Helpers.Random(1,29)  + ".jpg");
            }
            catch (FileNotFoundException)
            {
                try
                {
                    mapBox.BackgroundImage = System.Drawing.Image.FromFile(Environment.CurrentDirectory + @"\Resources\Images\0.jpg");
                    mapBox.BackgroundImageLayout = ImageLayout.Stretch;
                }
                catch (FileNotFoundException)
                {
                    Image img = new Bitmap(mapBox.Size.Width, mapBox.Size.Height);
                    var g = Graphics.FromImage(img);
                    g.FillRectangle(new SolidBrush(Color.Black), 0, 0, mapBox.Size.Width, mapBox.Size.Height);
                    mapBox.BackgroundImage = img;
                }
            }
        }   

        private void onRepaintMinimap(object sender, PaintEventArgs e)
        {
            try
            {
                Hero hero = Core.Hero;
                
                float byx = Core.CurrentMap.ByX;
                float byy = Core.CurrentMap.ByY;
                Graphics g = e.Graphics;

                Station[] stations;
                Ship[] ships;
                Box[] boxes;
                Ore[] ores;
                Portal[] portals;

                lock (Core.SpaceStations)
                {
                    stations = Core.SpaceStations.ToArray();
                }
                lock (Core.Ships)
                {
                    ships = Core.Ships.Values.ToArray();
                }
                lock (Core.Boxes)
                {
                    boxes = Core.Boxes.ToArray();
                }
                lock (Core.Ores)
                {
                    ores = Core.Ores.ToArray();
                }
                lock (Core.Portals)
                {
                    portals = Core.Portals.ToArray();
                }

                foreach (Station station in stations)
                {
                    Image img;
                    switch (station.Company)
                    {
                        //case 1:
                        //    img = SH_IT.Properties.Resources.mmo_station;
                        //    break;
                        //case 2:
                        //    img = SH_IT.Properties.Resources.eic_station;
                        //    break;
                        //case 3:
                        //    img = SH_IT.Properties.Resources.vru_station;
                        //    break;
                        default:
                            img = new Bitmap(50, 50);
                            break;
                    }
                    g.DrawImage(img, (float)((station.Position.x - 750) / byx), (float)((station.Position.y - 750) / byy), 35, 35);
                }

                foreach (Box box in boxes)
                {
                    Color box_color = Color.Aqua;
                    if (box.Type == 0)
                        continue;
                    else if (box.Type == 1)
                    {
                        box_color = Color.DarkOrange;
                    }
                    else if (box.Type == 2)
                    {
                        box_color = Color.Yellow;
                    }
                    else if (box.Type == 21)
                    {
                        box_color = Color.DarkCyan;
                    }
                    else if (box.Type == 22)
                    {
                        box_color = Color.DarkGoldenrod;
                    }
                    else if (box.Type == 31)
                    {
                        box_color = Color.Magenta;
                    }
                    else if (box.Type == 32)
                    {
                        box_color = Color.Firebrick;
                    }

                    g.FillRectangle(new SolidBrush(box_color), (float)(box.Position.x / byx), (float)(box.Position.y / byy), 2, 2);
                }

                foreach (Ore ore in ores)
                {
                    g.FillRectangle(new SolidBrush(Color.Gray), (float)(ore.Position.x / byx), (float)(ore.Position.y / byy), 2, 2);
                }

                Pen EnemyDot = new Pen(Color.Orange, 1.25f);
                EnemyDot.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                foreach (Ship ship in ships)
                {
                    var currPos = ship.Position;
                    Color color = Color.FromArgb(0, 127, 255);

                    if (hero!= null)
                    {
                        if (ship.IsNPC || ship.Faction != hero.Faction)
                            color = Color.Red;
                        if (ship.ClanDiplomacy == 1)
                            color = Color.Lime;
                        else if (ship.ClanDiplomacy == 2)
                            color = Color.DarkOrange;
                        else if (ship.ClanDiplomacy == 3)
                            color = Color.Red;
                    }

                    float x = (float)(currPos.x / byx);
                    float y = (float)(currPos.y / byy);

                    g.FillEllipse(new SolidBrush(color), x, y, 2.75f, 2.75f);
                    if (!ship.IsNPC && ship.Faction != hero.Faction && ship.ClanDiplomacy != 1 && ship.ClanDiplomacy != 2)
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawRectangle(EnemyDot, x - 3, y - 3, 8, 8);
                        g.SmoothingMode = SmoothingMode.Default;
                    }
                    if (ship.IsMoving)
                    {
                        g.DrawLine(ship.UserID == hero.UserID ? myPathPen : otherPlayerPathPen, x, y, (float)(ship.Destination.x / byx), (float)(ship.Destination.y / byy));
                    }
                }

                if (hero != null && Core.CurrentMap != null)
                {
                    var currPos = hero.Position;
                    float cx = (float)(currPos.x / byx);
                    float cy = (float)(currPos.y / byy);
                    g.DrawLine(mapAxisPen, 0, cy, mapBox.Width, cy);
                    g.DrawLine(mapAxisPen, cx, 0, cx, mapBox.Height);
                    if (hero.IsMoving)
                    {
                        float destX = (float)(hero.Destination.x / byx);
                        float destY = (float)(hero.Destination.y / byy);
                        g.DrawLine(myPathPen, cx, cy, destX, destY);

                        g.FillEllipse(destinationBrush, new RectangleF(destX - 2.25f, destY - 2.25f, 4.5f, 4.5f));
                    }
                    if (hero.SelectedShip != null && hero.Attacking)
                    {
                        var sel = hero.SelectedShip.Position;
                        g.DrawLine(attackPen, cx, cy, (float)(sel.x / byy), (float)(sel.y / byx));
                    }

                    Point[] prevLocs;
                    lock (previousLocations)
                    {
                        prevLocs = previousLocations.ToArray();
                    }
                    g.DrawLines(previousPathPen, prevLocs);
                }

                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                foreach (Portal port in portals)
                {
                    g.DrawEllipse(new Pen(Color.FromArgb(255, 255, 255, 255)), (float)((port.Position.x) / byx) - 5, (float)((port.Position.y) / byy) - 5, 10, 10);
                }

                if (hero != null && hero.SelectedShip != null)
                {
                    string HPText = String.Format("HP: {0}/{1}", hero.SelectedShip.Hp, hero.SelectedShip.Maxhp);
                    string ShieldText = String.Format("Shield: {0}/{1}", hero.SelectedShip.Shield, hero.SelectedShip.Maxshield);
                    string AlienText = hero.SelectedShip.Username;
                    SizeF HPlength = g.MeasureString(HPText, miniRegularText);
                    SizeF Shieldlength = g.MeasureString(ShieldText, miniRegularText);
                    SizeF Alienlength = g.MeasureString(AlienText, miniRegularText);

                    float width = Math.Max(HPlength.Width, Math.Max(Alienlength.Width, Shieldlength.Width));
                    g.DrawString(ShieldText, miniRegularText, shieldBrush, mapBox.Size.Width - width - 2, mapBox.Height - 1);
                    g.DrawString(HPText, miniRegularText, hpBrush, mapBox.Size.Width - width - 2, mapBox.Height - HPlength.Height - Shieldlength.Height);
                    g.DrawString(AlienText, miniRegularText, alienNameBrush, mapBox.Size.Width - width - 2, mapBox.Height - HPlength.Height - Shieldlength.Height - Alienlength.Height);
                }
                else
                {
                    SizeF size = g.MeasureString(mapLogo, miniRegularText);
                    g.DrawString(mapLogo, miniRegularText,logoBrush, mapBox.Width - 5 - size.Width, mapBox.Height - 5 - size.Height);
                }
                if ((DateTime.Now - msgPostMoment) <= msgDuration)
                {
                    g.DrawString(msgToPost, msgFontSegoe, postMessageBrush, msgPostLocation);
                }
                else
                {
                    // implementation for queued messages may be added here :)
                }
            }
            catch
            {
            }
        }

        DateTime msgPostMoment;
        TimeSpan msgDuration;
        string msgToPost="";
        PointF msgPostLocation;
        public void DisplayMessage(bool top, string str, TimeSpan duration)
        {
            if (false) {

                if (!IsDisposed) {
                    if (InvokeRequired) {
                        Invoke(new Action(() => DisplayMessage(top, str, duration)));
                        return;
                    }
                    SizeF size = mapBox.CreateGraphics().MeasureString(str, msgFontSegoe);
                    msgDuration = duration;
                    msgToPost = str;
                    msgPostMoment = DateTime.Now;
                    if (top == true) {
                        msgPostLocation = new PointF((mapBox.Size.Width - size.Width) / 2, 5);
                    }
                    else
                        msgPostLocation = new PointF((mapBox.Size.Width - size.Width) / 2, (mapBox.Size.Height - size.Height) / 2 - 5);
                }
            }
        }

        private void startPicture_Click(object sender, EventArgs e)
        {
            startPicture.Enabled = false;
            if (Core.ManualOverride == true)
            {
                Core.EndManualOverride();
                return;
            }
            Core.BotThread = new Thread(new ThreadStart(Core.RunLogic));
            Core.BotThread.IsBackground = true;
            Core.BotThread.Start();
            pausePicture.Enabled = true;
            stopPicture.Enabled = true;
        }
        private void pausePicture_Click(object sender, EventArgs e)
        {
            Core.BeginManualOverride();
            pausePicture.Enabled = false;
            startPicture.Enabled = true;
        }
        private void stopPicture_Click(object sender, EventArgs e)
        {
            Core.runLogic = false;
            startPicture.Enabled = true;
            pausePicture.Enabled = stopPicture.Enabled = false;
            Core.BotThread.Abort();
        }
        private void mapBox_Click(object sender, MouseEventArgs e)
        {
            if (Core.CurrentMap != null)
            {
                int x = (int)Math.Round(e.X * Core.CurrentMap.ByX);
                int y = (int)Math.Round(e.Y * Core.CurrentMap.ByY);
                Core.MoveTo(x, y);
            }
        }

        private void mapBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (Core.CurrentMap != null)
            {
                int x = (int)Math.Round(e.X * Core.CurrentMap.ByX);
                int y = (int)Math.Round(e.Y * Core.CurrentMap.ByY);
                Point clicked = new Point(x, y);
                Ship[] ships;
                lock (Core.Ships)
                {
                    ships = Core.Ships.Values.ToArray();
                }
                foreach (var ship in ships)
                {
                    if (lockedOnMinimapShip == ship)
                    {
                        loseMinimapLock();
                        break;
                    }
                    var shipLoc = ship.Position;
                    Point asPoint = new Point((int)shipLoc.x, (int)shipLoc.y);
                    Rectangle lockRect = new Rectangle(asPoint, lockMinimapSize);
                    if (lockRect.Contains(clicked))
                    {
                        lockedOnMinimapShip = ship;
                        return;
                    }
                }
            }
            mouseDown = true;
            stoppedMoving = false;
        }
        private void loseMinimapLock()
        {
            this.lockedOnMinimapShip = null;
        }
        private void mapBox_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.DisplayMessage(false, textBox1.Text, TimeSpan.FromSeconds(2));
        }
        void mapBox_DoubleClick(object sender, EventArgs e)
        {
            stoppedMoving = true;
            Core.BeginManualOverride();
        }
        private void trailLengthBarValueChanged(object sender, EventArgs e)
        {
            this.locationThreshold = this.trackBar1.Value;
        }


        const double NormalMapSizeRatio = 210 / 135;
        const double MiniX = 125,
                     MiniY = 125 / NormalMapSizeRatio;
        private void resizeImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (resizeMouseDown) {
                Point resizeDest = getPosRelative(this);
                Size newSize = new Size(
                    this.mapBox.Size.Width + resizeDest.X - mapResizeStart.X,
                    this.mapBox.Size.Height + resizeDest.Y - mapResizeStart.Y);
                const float ratio = 1.605f;
                int targetWidth, targetHei;
                targetWidth = targetHei = Math.Max(newSize.Width, newSize.Height);
                if (ratio < 1) {
                    targetWidth = (int)Math.Ceiling(targetHei * ratio);
                }
                else {
                    targetHei = (int)Math.Ceiling(targetWidth / ratio);
                }
                if (targetWidth < MiniX || targetHei < MiniY)
                    return; // too small

                mapBox.Size = new Size(targetWidth, targetHei);
                if (Core.CurrentMap != null) {
                    Core.CurrentMap.ByX = Core.CurrentMap.SizeX / targetWidth;
                    Core.CurrentMap.ByY = Core.CurrentMap.SizeY / targetHei;
                }
                this.resizeImage.Location = new Point(this.mapBox.Size.Width - 16, this.mapBox.Size.Height - 16);
            }
        }
    }
}