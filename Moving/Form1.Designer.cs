namespace Moving
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.startButton = new System.Windows.Forms.Button();
            this.breakButton = new System.Windows.Forms.Button();
            this.forceBreakButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.distanceLabel = new System.Windows.Forms.Label();
            this.lockspeedSlider = new System.Windows.Forms.TrackBar();
            this.shipIdLabel = new System.Windows.Forms.Label();
            this.destinationLabel = new System.Windows.Forms.Label();
            this.speedLabel = new System.Windows.Forms.Label();
            this.locationLabel = new System.Windows.Forms.Label();
            this.movingLabel = new System.Windows.Forms.Label();
            this.showPathCheckBox = new System.Windows.Forms.CheckBox();
            this.shpNmbTextBox = new System.Windows.Forms.TextBox();
            this.adjButton = new System.Windows.Forms.Button();
            this.addBtn = new System.Windows.Forms.Button();
            this.remBtn = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.showAllPathsCheckboxk = new System.Windows.Forms.CheckBox();
            this.mapBox = new Moving.MapBoxPanel();
            this.contextMenuStrip1 = new Moving.SpecialContextMenuStrip(this.components);
            this.loseLockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allMoveHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allStopHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createShipsHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lockspeedSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(919, 13);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(142, 23);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // breakButton
            // 
            this.breakButton.Location = new System.Drawing.Point(919, 42);
            this.breakButton.Name = "breakButton";
            this.breakButton.Size = new System.Drawing.Size(70, 23);
            this.breakButton.TabIndex = 2;
            this.breakButton.Text = "Break";
            this.breakButton.UseVisualStyleBackColor = true;
            this.breakButton.Click += new System.EventHandler(this.breakButton_Click);
            // 
            // forceBreakButton
            // 
            this.forceBreakButton.Location = new System.Drawing.Point(989, 42);
            this.forceBreakButton.Name = "forceBreakButton";
            this.forceBreakButton.Size = new System.Drawing.Size(72, 23);
            this.forceBreakButton.TabIndex = 3;
            this.forceBreakButton.Text = "ForceB";
            this.forceBreakButton.UseVisualStyleBackColor = true;
            this.forceBreakButton.Click += new System.EventHandler(this.forceBreakButton_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.distanceLabel);
            this.panel1.Controls.Add(this.lockspeedSlider);
            this.panel1.Controls.Add(this.shipIdLabel);
            this.panel1.Controls.Add(this.destinationLabel);
            this.panel1.Controls.Add(this.speedLabel);
            this.panel1.Controls.Add(this.locationLabel);
            this.panel1.Location = new System.Drawing.Point(919, 71);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(142, 159);
            this.panel1.TabIndex = 4;
            // 
            // distanceLabel
            // 
            this.distanceLabel.AutoSize = true;
            this.distanceLabel.Location = new System.Drawing.Point(-1, 96);
            this.distanceLabel.Name = "distanceLabel";
            this.distanceLabel.Size = new System.Drawing.Size(55, 13);
            this.distanceLabel.TabIndex = 6;
            this.distanceLabel.Text = "Distance: ";
            // 
            // lockspeedSlider
            // 
            this.lockspeedSlider.Location = new System.Drawing.Point(2, 116);
            this.lockspeedSlider.Maximum = 500;
            this.lockspeedSlider.Name = "lockspeedSlider";
            this.lockspeedSlider.Size = new System.Drawing.Size(134, 45);
            this.lockspeedSlider.TabIndex = 5;
            this.lockspeedSlider.Value = 50;
            this.lockspeedSlider.ValueChanged += new System.EventHandler(this.lockspeedSlider_ValueChanged);
            // 
            // shipIdLabel
            // 
            this.shipIdLabel.AutoSize = true;
            this.shipIdLabel.Location = new System.Drawing.Point(-1, 0);
            this.shipIdLabel.Name = "shipIdLabel";
            this.shipIdLabel.Size = new System.Drawing.Size(48, 13);
            this.shipIdLabel.TabIndex = 3;
            this.shipIdLabel.Text = "Ship ID: ";
            // 
            // destinationLabel
            // 
            this.destinationLabel.AutoSize = true;
            this.destinationLabel.Location = new System.Drawing.Point(-1, 71);
            this.destinationLabel.Name = "destinationLabel";
            this.destinationLabel.Size = new System.Drawing.Size(66, 13);
            this.destinationLabel.TabIndex = 2;
            this.destinationLabel.Text = "Destination: ";
            // 
            // speedLabel
            // 
            this.speedLabel.AutoSize = true;
            this.speedLabel.Location = new System.Drawing.Point(-1, 48);
            this.speedLabel.Name = "speedLabel";
            this.speedLabel.Size = new System.Drawing.Size(44, 13);
            this.speedLabel.TabIndex = 1;
            this.speedLabel.Text = "Speed: ";
            // 
            // locationLabel
            // 
            this.locationLabel.AutoSize = true;
            this.locationLabel.Location = new System.Drawing.Point(-1, 24);
            this.locationLabel.Name = "locationLabel";
            this.locationLabel.Size = new System.Drawing.Size(54, 13);
            this.locationLabel.TabIndex = 0;
            this.locationLabel.Text = "Location: ";
            // 
            // movingLabel
            // 
            this.movingLabel.AutoSize = true;
            this.movingLabel.Location = new System.Drawing.Point(919, 237);
            this.movingLabel.Name = "movingLabel";
            this.movingLabel.Size = new System.Drawing.Size(48, 13);
            this.movingLabel.TabIndex = 5;
            this.movingLabel.Text = "Moving: ";
            // 
            // showPathCheckBox
            // 
            this.showPathCheckBox.AutoSize = true;
            this.showPathCheckBox.Checked = true;
            this.showPathCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showPathCheckBox.Location = new System.Drawing.Point(923, 254);
            this.showPathCheckBox.Name = "showPathCheckBox";
            this.showPathCheckBox.Size = new System.Drawing.Size(78, 17);
            this.showPathCheckBox.TabIndex = 6;
            this.showPathCheckBox.Text = "Show Path";
            this.showPathCheckBox.UseVisualStyleBackColor = true;
            // 
            // shpNmbTextBox
            // 
            this.shpNmbTextBox.Location = new System.Drawing.Point(919, 278);
            this.shpNmbTextBox.Name = "shpNmbTextBox";
            this.shpNmbTextBox.Size = new System.Drawing.Size(142, 20);
            this.shpNmbTextBox.TabIndex = 7;
            this.shpNmbTextBox.Text = "150";
            // 
            // adjButton
            // 
            this.adjButton.Location = new System.Drawing.Point(919, 304);
            this.adjButton.Name = "adjButton";
            this.adjButton.Size = new System.Drawing.Size(48, 23);
            this.adjButton.TabIndex = 8;
            this.adjButton.Text = "Adjust";
            this.adjButton.UseVisualStyleBackColor = true;
            this.adjButton.Click += new System.EventHandler(this.adjButton_Click);
            // 
            // addBtn
            // 
            this.addBtn.Location = new System.Drawing.Point(973, 304);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(43, 23);
            this.addBtn.TabIndex = 9;
            this.addBtn.Text = "Add";
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // remBtn
            // 
            this.remBtn.Location = new System.Drawing.Point(1022, 304);
            this.remBtn.Name = "remBtn";
            this.remBtn.Size = new System.Drawing.Size(39, 23);
            this.remBtn.TabIndex = 10;
            this.remBtn.Text = "Remove";
            this.remBtn.UseVisualStyleBackColor = true;
            this.remBtn.Click += new System.EventHandler(this.remBtn_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(923, 334);
            this.trackBar1.Maximum = 250;
            this.trackBar1.Minimum = 10;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(133, 45);
            this.trackBar1.TabIndex = 11;
            this.trackBar1.Value = 46;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // showAllPathsCheckboxk
            // 
            this.showAllPathsCheckboxk.AutoSize = true;
            this.showAllPathsCheckboxk.Location = new System.Drawing.Point(923, 373);
            this.showAllPathsCheckboxk.Name = "showAllPathsCheckboxk";
            this.showAllPathsCheckboxk.Size = new System.Drawing.Size(97, 17);
            this.showAllPathsCheckboxk.TabIndex = 12;
            this.showAllPathsCheckboxk.Text = "Show All Paths";
            this.showAllPathsCheckboxk.UseVisualStyleBackColor = true;
            this.showAllPathsCheckboxk.CheckedChanged += new System.EventHandler(this.showPathsCheckboxk_CheckedChanged);
            // 
            // mapBox
            // 
            this.mapBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mapBox.Location = new System.Drawing.Point(13, 13);
            this.mapBox.Name = "mapBox";
            this.mapBox.Size = new System.Drawing.Size(900, 533);
            this.mapBox.TabIndex = 0;
            this.mapBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mapBox_MouseClick);
            this.mapBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.mapBox_MouseDoubleClick);
            this.mapBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mapBox_MouseDown);
            this.mapBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mapBox_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loseLockToolStripMenuItem,
            this.allMoveHereToolStripMenuItem,
            this.allStopHereToolStripMenuItem,
            this.createShipsHereToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(165, 92);
            // 
            // loseLockToolStripMenuItem
            // 
            this.loseLockToolStripMenuItem.Name = "loseLockToolStripMenuItem";
            this.loseLockToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.loseLockToolStripMenuItem.Text = "Lose lock";
            this.loseLockToolStripMenuItem.Click += new System.EventHandler(this.loseLockToolStripMenuItem_Click);
            // 
            // allMoveHereToolStripMenuItem
            // 
            this.allMoveHereToolStripMenuItem.Name = "allMoveHereToolStripMenuItem";
            this.allMoveHereToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.allMoveHereToolStripMenuItem.Text = "All come here";
            this.allMoveHereToolStripMenuItem.Click += new System.EventHandler(this.allMoveHereToolStripMenuItem_Click);
            // 
            // allStopHereToolStripMenuItem
            // 
            this.allStopHereToolStripMenuItem.Name = "allStopHereToolStripMenuItem";
            this.allStopHereToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.allStopHereToolStripMenuItem.Text = "All stop here";
            this.allStopHereToolStripMenuItem.Click += new System.EventHandler(this.allStopHereToolStripMenuItem_Click);
            // 
            // createShipsHereToolStripMenuItem
            // 
            this.createShipsHereToolStripMenuItem.Name = "createShipsHereToolStripMenuItem";
            this.createShipsHereToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.createShipsHereToolStripMenuItem.Text = "Create ships here";
            this.createShipsHereToolStripMenuItem.Click += new System.EventHandler(this.createShipsHereToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1073, 558);
            this.Controls.Add(this.showAllPathsCheckboxk);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.remBtn);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.adjButton);
            this.Controls.Add(this.shpNmbTextBox);
            this.Controls.Add(this.showPathCheckBox);
            this.Controls.Add(this.movingLabel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.forceBreakButton);
            this.Controls.Add(this.breakButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.mapBox);
            this.Name = "Form1";
            this.Text = "Movement Simulator";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lockspeedSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MapBoxPanel mapBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button breakButton;
        private System.Windows.Forms.Button forceBreakButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TrackBar lockspeedSlider;
        private System.Windows.Forms.Label shipIdLabel;
        private System.Windows.Forms.Label destinationLabel;
        private System.Windows.Forms.Label speedLabel;
        private System.Windows.Forms.Label locationLabel;
        private System.Windows.Forms.Label movingLabel;
        private System.Windows.Forms.CheckBox showPathCheckBox;
        private System.Windows.Forms.TextBox shpNmbTextBox;
        private System.Windows.Forms.Button adjButton;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.Button remBtn;
        private SpecialContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem allMoveHereToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createShipsHereToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loseLockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allStopHereToolStripMenuItem;
        private System.Windows.Forms.Label distanceLabel;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.CheckBox showAllPathsCheckboxk;
    }
}

