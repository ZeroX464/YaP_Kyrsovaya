namespace YaP_Kyrsovaya
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gameMenuStrip = new System.Windows.Forms.MenuStrip();
            this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BackToMenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPanel = new System.Windows.Forms.Panel();
            this.scoreboardButton = new System.Windows.Forms.Button();
            this.minesweeperLabel = new System.Windows.Forms.Label();
            this.exitButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.gameTimer = new System.Windows.Forms.Timer(this.components);
            this.gameMenuStrip.SuspendLayout();
            this.menuPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // gameMenuStrip
            // 
            this.gameMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restartToolStripMenuItem,
            this.TipToolStripMenuItem,
            this.BackToMenuToolStripMenuItem});
            this.gameMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.gameMenuStrip.Name = "gameMenuStrip";
            this.gameMenuStrip.Size = new System.Drawing.Size(320, 24);
            this.gameMenuStrip.TabIndex = 0;
            this.gameMenuStrip.Text = "gameMenuStrip";
            this.gameMenuStrip.Visible = false;
            // 
            // restartToolStripMenuItem
            // 
            this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
            this.restartToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.restartToolStripMenuItem.Text = "Restart";
            this.restartToolStripMenuItem.Click += new System.EventHandler(this.RestartToolStripMenuItem_Click);
            // 
            // TipToolStripMenuItem
            // 
            this.TipToolStripMenuItem.Name = "TipToolStripMenuItem";
            this.TipToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.TipToolStripMenuItem.Text = "Tip";
            this.TipToolStripMenuItem.Click += new System.EventHandler(this.TipToolStripMenuItem_Click);
            // 
            // BackToMenuToolStripMenuItem
            // 
            this.BackToMenuToolStripMenuItem.Name = "BackToMenuToolStripMenuItem";
            this.BackToMenuToolStripMenuItem.Size = new System.Drawing.Size(92, 20);
            this.BackToMenuToolStripMenuItem.Text = "Back to menu";
            this.BackToMenuToolStripMenuItem.Click += new System.EventHandler(this.BackToMenuToolStripMenuItem_Click);
            // 
            // menuPanel
            // 
            this.menuPanel.Controls.Add(this.scoreboardButton);
            this.menuPanel.Controls.Add(this.minesweeperLabel);
            this.menuPanel.Controls.Add(this.exitButton);
            this.menuPanel.Controls.Add(this.startButton);
            this.menuPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuPanel.Location = new System.Drawing.Point(0, 24);
            this.menuPanel.Name = "menuPanel";
            this.menuPanel.Size = new System.Drawing.Size(320, 327);
            this.menuPanel.TabIndex = 1;
            // 
            // scoreboardButton
            // 
            this.scoreboardButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.scoreboardButton.Font = new System.Drawing.Font("Arial Rounded MT Bold", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scoreboardButton.Location = new System.Drawing.Point(79, 169);
            this.scoreboardButton.Name = "scoreboardButton";
            this.scoreboardButton.Size = new System.Drawing.Size(159, 34);
            this.scoreboardButton.TabIndex = 4;
            this.scoreboardButton.Text = "Scoreboard";
            this.scoreboardButton.UseVisualStyleBackColor = true;
            this.scoreboardButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ScoreboardButton_MouseClick);
            // 
            // minesweeperLabel
            // 
            this.minesweeperLabel.AutoSize = true;
            this.minesweeperLabel.Font = new System.Drawing.Font("Impact", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.minesweeperLabel.Location = new System.Drawing.Point(73, 93);
            this.minesweeperLabel.Name = "minesweeperLabel";
            this.minesweeperLabel.Size = new System.Drawing.Size(165, 34);
            this.minesweeperLabel.TabIndex = 3;
            this.minesweeperLabel.Text = "Minesweeper";
            this.minesweeperLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // exitButton
            // 
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitButton.Font = new System.Drawing.Font("Arial Rounded MT Bold", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitButton.Location = new System.Drawing.Point(79, 209);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(159, 34);
            this.exitButton.TabIndex = 1;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ExitButton_MouseClick);
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startButton.Font = new System.Drawing.Font("Arial Rounded MT Bold", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startButton.Location = new System.Drawing.Point(79, 130);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(159, 33);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.StartButton_MouseClick);
            // 
            // gameTimer
            // 
            this.gameTimer.Interval = 1000;
            this.gameTimer.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 351);
            this.Controls.Add(this.menuPanel);
            this.Controls.Add(this.gameMenuStrip);
            this.KeyPreview = true;
            this.MainMenuStrip = this.gameMenuStrip;
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.gameMenuStrip.ResumeLayout(false);
            this.gameMenuStrip.PerformLayout();
            this.menuPanel.ResumeLayout(false);
            this.menuPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip gameMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem restartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TipToolStripMenuItem;
        private System.Windows.Forms.Panel menuPanel;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label minesweeperLabel;
        private System.Windows.Forms.ToolStripMenuItem BackToMenuToolStripMenuItem;
        private System.Windows.Forms.Button scoreboardButton;
        private System.Windows.Forms.Timer gameTimer;
    }
}

