using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;

namespace YaP_Kyrsovaya
{
    public partial class Form1 : Form
    {
        private bool gameOver = false;
        private bool gameWin = false;
        private bool gameWasStarted = false;
        private const int CellSize = 30;
        private int MapWidth = 8;
        private int MapHeight = 8;
        private int minesToPlace;
        private int flagsCount;
        private int time;
        private Cell[,] cells;
        private TableLayoutPanel scoreboardTable;
        private Panel selectLevelPanel;
        private TextBox mapWidthTextBox;
        private TextBox mapHeightTextBox;
        private TextBox minesTextBox;
        private Panel gamePanel;
        private Panel gameWinPanel;
        private Size gamePanelSize;
        private Flag flag;
        private Label mineCountLabel;
        private Label timeCountLabel;
        private string level;
        public Form1()
        {
            InitializeComponent();
            InitializeSelectLevelPanel();
            InitializeScoreboardTable();
            InitializeGameWinPanel();
        }
        private (string, int)[,] ReadDataFromScoreboard()
        {
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = @"..\..\Resources\Scoreboard.xlsx";
            string filePath = Path.Combine(projectDirectory, relativePath);
            string fullPath = Path.GetFullPath(filePath);

            FileInfo fileInfo = new FileInfo(fullPath);
            using (var package = new ExcelPackage(fileInfo))
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial; // Лицензия
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Первый лист

                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                (string, int)[,] scoreboard = new (string, int)[3, 10];
                for (int row = 2; row <= rowCount; row++)
                {
                    string nickname = worksheet.Cells[row, 1].Text;
                    int levelIndex = (row - 2)/ 10;
                    int scoreIndex = (row - 2) % 10;
                    if (int.TryParse(worksheet.Cells[row, 2].Text, out int parsedScore))
                    {
                        scoreboard[levelIndex, scoreIndex] = (nickname, parsedScore);
                    }
                    else
                    {
                        throw new Exception("In score exist NaN");
                    }
                }
                return scoreboard;
            }
        }
        private void ChangeDataInScoreboard((string, int)[,] scoreboard, string winNickname, int winTime)
        {
            int i;
            switch (level)
            {
                case "Easy":
                    i = 0;
                    break;
                case "Normal":
                    i = 1;
                    break;
                case "Hard":
                    i = 2;
                    break;
                default:
                    throw new Exception("Level difficulty not selected");
            }
            for (int j = 0; j < scoreboard.GetLength(1); j++)
            {
                if (winTime < scoreboard[i, j].Item2)
                {
                    // Получение полного пути к файлу
                    string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string relativePath = @"..\..\Resources\Scoreboard.xlsx";
                    string filePath = Path.Combine(projectDirectory, relativePath);
                    string fullPath = Path.GetFullPath(filePath);

                    FileInfo fileInfo = new FileInfo(fullPath);
                    using (var package = new ExcelPackage(fileInfo))
                    {
                        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial; // Лицензия
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Первый лист

                        worksheet.Cells[10 * i + j + 2, 1].Value = winNickname;
                        worksheet.Cells[10 * i + j + 2, 2].Value = winTime;
                        package.Save();
                        InitializeScoreboardTable(); // Обновление таблицы
                        break;
                    }
                }
            }
        }
        private void InitializeGameWinPanel()
        {
            gameWinPanel = new Panel()
            {
                Size = new Size(336, 390),
                Location = new Point(0, 0),
                Visible = false
            };
            this.Controls.Add(gameWinPanel);

            Label winLabel = new Label()
            {
                Size = new Size(330, 34),
                Location = new Point(3, 80),
                Font = new Font("Impact", 24),
                ForeColor = Color.DarkRed,
                Text = "You win!!!",
                TextAlign = ContentAlignment.MiddleCenter
            };
            gameWinPanel.Controls.Add(winLabel);

            Label enterNicknameLabel = new Label()
            {
                Size = new Size(330, 34),
                Location = new Point(3, 120),
                Font = new Font("Impact", 20),
                Text = "Enter your nickname",
                TextAlign = ContentAlignment.MiddleCenter
            };
            gameWinPanel.Controls.Add(enterNicknameLabel);

            TextBox enterNicknameTextBox = new TextBox()
            {
                Size = new Size(160, 34),
                Location = new Point(85, 152),
                Font = new Font("Times New Roman", 14)
            };
            enterNicknameTextBox.KeyDown += EnterNicknameTextBox_KeyDown;
            gameWinPanel.Controls.Add(enterNicknameTextBox);
        }
        private void EnterNicknameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBox textBox = sender as TextBox;
                string winNickname = textBox.Text;
                AddWinnerToTheScoreboard(winNickname);
            }
        }
        private void AddWinnerToTheScoreboard(string winNickname)
        {
            (string, int)[,] scoreboard = ReadDataFromScoreboard();
            ChangeDataInScoreboard(scoreboard, winNickname, time);
            gameWinPanel.Visible = false;
            scoreboardTable.Visible = true;
            this.Size = new Size(200, 540);
        }

        private void InitializeScoreboardTable()
        {
            if (scoreboardTable != null) { this.Controls.Remove(scoreboardTable); }
            scoreboardTable = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                AutoSize = true
            };
            (string, int)[,] scoreboard = ReadDataFromScoreboard();
            Font titleFont = new Font("Impact", 16);
            scoreboardTable.Controls.Add(new Label() { Text = "Name", Font = titleFont }, 0, 0);
            scoreboardTable.Controls.Add(new Label() { Text = "Time", Font = titleFont }, 1, 0);

            for (int i = 0; i < scoreboard.GetLength(0); i++)
            {
                string level = i == 0 ? "Easy" : i == 1 ? "Normal" : "Hard";
                Label levelLabel = new Label() { Text = level, AutoSize = true, Font = new Font("Arial", 12, FontStyle.Bold) };
                scoreboardTable.RowCount += 1;
                scoreboardTable.Controls.Add(levelLabel, 0, scoreboardTable.RowCount - 1);
                scoreboardTable.SetColumnSpan(levelLabel, 2); // Объединяем две колонки для заголовка уровня

                for (int j = 0; j < scoreboard.GetLength(1); j++)
                {
                    scoreboardTable.RowCount += 1;
                    scoreboardTable.Controls.Add(new Label() { Text = scoreboard[i, j].Item1, AutoSize = true }, 0, scoreboardTable.RowCount - 1);
                    scoreboardTable.Controls.Add(new Label() { Text = scoreboard[i, j].Item2.ToString(), AutoSize = true }, 1, scoreboardTable.RowCount - 1);
                }
            }
            scoreboardTable.Visible = false;
            this.Controls.Add(scoreboardTable);
        }
        private void InitializeSelectLevelPanel()
        {
            selectLevelPanel = new Panel()
            {
                Size = new Size(350,351),
                Location = new Point(0, 0)
            };
            this.Controls.Add(selectLevelPanel);
            selectLevelPanel.Visible = false;

            Label selectLevelLabel = new Label()
            {
                Size = new Size(165, 34),
                Location = new Point(71, 52),
                Font = new Font("Impact", 20),
                Text = "Select level",
                TextAlign = ContentAlignment.MiddleCenter
            };
            selectLevelPanel.Controls.Add(selectLevelLabel);

            Button easyLevelButton = new Button()
            {
                Size = new Size(159, 33),
                Location = new Point(77, 89),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial Rounded MT Bold", 16),
                Text = "Easy",
            };
            easyLevelButton.Click += EasyLevelButton_Click;
            selectLevelPanel.Controls.Add(easyLevelButton);

            Button normalLevelButton = new Button()
            {
                Size = new Size(159, 33),
                Location = new Point(77, easyLevelButton.Location.Y + easyLevelButton.Height + 3),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial Rounded MT Bold", 16),
                Text = "Normal"
            };
            normalLevelButton.Click += NormalLevelButton_Click;
            selectLevelPanel.Controls.Add(normalLevelButton);

            Button hardLevelButton = new Button()
            {
                Size = new Size(159, 33),
                Location = new Point(77, normalLevelButton.Location.Y + normalLevelButton.Height + 3),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial Rounded MT Bold", 16),
                Text = "Hard"
            };
            hardLevelButton.Click += HardLevelButton_Click;
            selectLevelPanel.Controls.Add(hardLevelButton);

            Button customLevelButton = new Button()
            {
                Size = new Size(159, 33),
                Location = new Point(77, hardLevelButton.Location.Y + hardLevelButton.Height + 3),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial Rounded MT Bold", 16),
                Text = "Custom"
            };
            customLevelButton.Click += CustomLevelButton_Click;
            selectLevelPanel.Controls.Add(customLevelButton);

            Label mapWidthLabel = new Label()
            {
                Size = new Size(85, 24),
                Location = new Point(70, customLevelButton.Location.Y + customLevelButton.Height + 10),
                Font = new Font("Arial Rounded MT Bold", 14),
                Text = "Width",
                TextAlign = ContentAlignment.TopCenter
            };
            selectLevelPanel.Controls.Add(mapWidthLabel);

            Label mapHeightLabel = new Label()
            {
                Size = new Size(85, 24),
                Location = new Point(70, mapWidthLabel.Location.Y + mapWidthLabel.Height + 10),
                Font = new Font("Arial Rounded MT Bold", 14),
                Text = "Height",
                TextAlign = ContentAlignment.TopCenter
            };
            selectLevelPanel.Controls.Add(mapHeightLabel);

            Label minesLabel = new Label()
            {
                Size = new Size(85, 24),
                Location = new Point(70, mapHeightLabel.Location.Y + mapHeightLabel.Height + 10),
                Font = new Font("Arial Rounded MT Bold", 14),
                Text = "Mines",
                TextAlign = ContentAlignment.TopCenter
            };
            selectLevelPanel.Controls.Add(minesLabel);

            mapWidthTextBox = new TextBox()
            {
                Size = new Size(70, 30),
                Location = new Point(this.Width / 2 - 10, mapWidthLabel.Location.Y),
                Text = "8"
            };
            selectLevelPanel.Controls.Add(mapWidthTextBox);

            mapHeightTextBox = new TextBox()
            {
                Size = new Size(70, 30),
                Location = new Point(this.Width / 2 - 10, mapHeightLabel.Location.Y),
                Text = "8"
            };
            selectLevelPanel.Controls.Add(mapHeightTextBox);

            minesTextBox = new TextBox()
            {
                Size = new Size(70, 30),
                Location = new Point(this.Width / 2 - 10, minesLabel.Location.Y),
                Text = "10"
            };
            selectLevelPanel.Controls.Add(minesTextBox);
        }
        private void EasyLevelButton_Click(object sender, EventArgs e)
        {
            MapWidth = 8;
            MapHeight = 8;
            minesToPlace = 10;
            level = "Easy";
            CalculateFormSizeAndShowGame();
        }
        private void NormalLevelButton_Click(object sender, EventArgs e)
        {
            MapWidth = 16;
            MapHeight = 16;
            minesToPlace = 40;
            level = "Normal";
            CalculateFormSizeAndShowGame();
        }
        private void HardLevelButton_Click(object sender, EventArgs e)
        {
            MapWidth = 30;
            MapHeight = 16;
            minesToPlace = 99;
            level = "Hard";
            CalculateFormSizeAndShowGame();
        }
        private void CustomLevelButton_Click(object sender, EventArgs e)
        {
            level = null;
            MessageBoxIcon icon = MessageBoxIcon.Error;
            if (int.TryParse(mapWidthTextBox.Text, out int parsedWidth) &&
                int.TryParse(mapHeightTextBox.Text, out int parsedHeight) &&
                int.TryParse(minesTextBox.Text, out int parsedMines) &&
                parsedWidth > 0 && parsedHeight > 0 && parsedMines > 0)
            {
                if (parsedWidth > 30)
                {
                    MessageBox.Show("Width must be less than 30", "Error", MessageBoxButtons.OK, icon);
                }
                else if (parsedHeight > 30)
                {
                    MessageBox.Show("Height must be less than 30", "Error", MessageBoxButtons.OK, icon);
                }
                else if (parsedWidth > 3 && parsedHeight > 3 && parsedMines > parsedWidth * parsedHeight - 9)
                {
                    
                    MessageBox.Show($"For this map size the maximum number of mines {parsedWidth * parsedHeight - 9}", "Error", MessageBoxButtons.OK, icon);
                }
                else if (parsedMines > parsedWidth * parsedHeight)
                {
                    MessageBox.Show($"For this map size the maximum number of mines {parsedWidth * parsedHeight}", "Error", MessageBoxButtons.OK, icon);
                }
                else
                {
                    MapWidth = parsedWidth;
                    MapHeight = parsedHeight;
                    minesToPlace = parsedMines;
                    CalculateFormSizeAndShowGame();
                }
            }
            else // Были введены не целые положительные числа
            {
                MessageBox.Show("Parameters for custom level must be non-zero positive integers", "Error", MessageBoxButtons.OK, icon);
            }
        }
        private void CalculateFormSizeAndShowGame()
        {
            cells = new Cell[MapWidth, MapHeight];
            if (MapWidth * CellSize + 80 > 320 && MapHeight * CellSize + 110 > 330)
            {
                gamePanelSize = new Size(MapWidth * CellSize + 80, MapHeight * CellSize + 110);
            }
            else if (MapWidth * CellSize + 80 > 320)
            {
                gamePanelSize = new Size(MapWidth * CellSize + 80, 330);
            }
            else if (MapHeight * CellSize + 110 > 330)
            {
                gamePanelSize = new Size(320, MapHeight * CellSize + 110);
            }
            else
            {
                gamePanelSize = new Size(320, 330);
            }
            InitializeGamePanel();
            
            this.Size = new Size(gamePanel.Size.Width + 16, gamePanel.Size.Height + 40);

            selectLevelPanel.Visible = false;
            flagsCount = 0;
            gameMenuStrip.Visible = true;
        }
        private void InitializeGamePanel()
        {
            gameOver = false;
            gameWin = false;
            time = 0;
            flagsCount = 0;
            if (gamePanel != null) { this.Controls.Remove(gamePanel); }
            gamePanel = new Panel()
            {
                Size = gamePanelSize,
                Location = new Point(0,0)
            };
            this.Controls.Add(gamePanel);

            mineCountLabel = new Label()
            {
                Size = new Size(110, 30),
                Location = new Point(20, 25),
                Font = new Font("Arial", 14),
                Text = "Mines: " + minesToPlace.ToString(),
            };
            gamePanel.Controls.Add(mineCountLabel);

            timeCountLabel = new Label()
            {
                Size = new Size(100, 30),
                Location = new Point(gamePanel.Width - 110, 25),
                Font = new Font("Arial", 14),
                Text = "Time: 0",
            };
            gamePanel.Controls.Add(timeCountLabel);

            flag = new Flag()
            {
                Size = new Size(CellSize, CellSize),
                Location = new Point(gamePanel.Size.Width / 2 - CellSize / 2, 25),
                BackColor = SystemColors.Control,
                BackgroundImageLayout = ImageLayout.Stretch,
                FlatStyle = FlatStyle.Flat,
            };
            flag.Click += Flag_Click;
            gamePanel.Controls.Add(flag);

            // Инициализация ячеек игрового поля
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    int shiftToAlign_x = (gamePanel.Width - MapWidth * CellSize) / 2;
                    int shiftToAlign_y = (gamePanel.Height - 60 - MapHeight * CellSize) / 2 + 60;
                    Cell cell = new Cell()
                    {
                        Size = new Size(CellSize, CellSize),
                        Location = new Point(x * CellSize + shiftToAlign_x, y * CellSize + shiftToAlign_y),
                        Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        TextImageRelation = TextImageRelation.ImageBeforeText,
                        x = x,
                        y = y
                    };
                    cell.Click += Cell_Click;
                    gamePanel.Controls.Add(cell);
                    cells[x, y] = cell;
                }
            }
        }
        private void AddMinesToTheGamePanel(int ClickedCell_x, int ClickedCell_y)
        {
            // Случайное размещение мин
            Random rand = new Random();
            int mines = minesToPlace;
            while (mines > 0)
            {
                int x = rand.Next(MapWidth);
                int y = rand.Next(MapHeight);
                bool mineInStartLocation = false;

                if (MapWidth > 3 && MapHeight > 3)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (x == ClickedCell_x + i && y == ClickedCell_y + j) { mineInStartLocation = true; }
                        }
                    }
                }

                if (!cells[x, y].IsMine && !mineInStartLocation)
                {
                    cells[x, y].IsMine = true;
                    mines--;
                }
            }

            // Вычисление соседних мин для каждой ячейки
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    if (!cells[x, y].IsMine)
                    {
                        int minesNearly = 0;
                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                if (x + i >= 0 && x + i < MapWidth && y + j >= 0 && y + j < MapHeight)
                                {
                                    if (cells[x + i, y + j].IsMine)
                                    {
                                        minesNearly++;
                                    }
                                }
                            }
                        }
                        cells[x, y].MinesNearly = minesNearly;
                    }
                }
            }
        }
        private void Cell_Click(object sender, EventArgs e)
        {
            if (!gameOver && !gameWin)
            {
                Cell clickedCell = sender as Cell;
                if (!gameWasStarted)
                {
                    AddMinesToTheGamePanel(clickedCell.x, clickedCell.y);
                    gameWasStarted = true;
                    gameTimer.Start();
                }
                if (!clickedCell.IsOpened)
                {
                    if (flag.Active && clickedCell.IsFlagged)
                    {
                        flagsCount -= 1;
                        mineCountLabel.Text = "Mines: " + (minesToPlace - flagsCount).ToString();
                        clickedCell.IsFlagged = false;
                        clickedCell.BackgroundImage = null;
                    }
                    else if (flag.Active && !clickedCell.IsFlagged)
                    {
                        if (minesToPlace - flagsCount > 0)
                        {
                            flagsCount += 1;
                            mineCountLabel.Text = "Mines: " + (minesToPlace - flagsCount).ToString();
                            clickedCell.IsFlagged = true;
                            clickedCell.BackgroundImage = Properties.Resources.flag2;
                        }
                    }
                    else if (!flag.Active && clickedCell.IsFlagged) { }
                    else
                    {
                        if (clickedCell.IsMine)
                        {
                            gameTimer.Stop();
                            gameWasStarted = false;
                            clickedCell.BackColor = Color.Red;
                            clickedCell.BackgroundImage = Properties.Resources.mine;
                            gameOver = true;
                            OpenAllMines();
                            MessageBox.Show("Game Over!");
                        }
                        else
                        {
                            OpenCell(clickedCell);
                        }
                    }
                }
            }
            gameWin = true;
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    if (!cells[x, y].IsOpened && !cells[x, y].IsMine)
                    {
                        gameWin = false;
                        break;
                    }
                }
                if (!gameWin) { break; }
            }
            if (gameWin && level != null)
            {
                gameTimer.Stop();
                gamePanel.Visible = false;
                gameMenuStrip.Visible = false;
                gameWinPanel.Visible = true;
                this.Size = new Size(336, 390);
            }
            else if (gameWin)
            {
                gameTimer.Stop();
                MessageBox.Show("You win!!! You time: " + time.ToString());
            }
        }
        private void OpenAllMines()
        {
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    if (cells[x, y].IsMine)
                    {
                        cells[x, y].IsOpened = true;
                        if (!cells[x,y].IsFlagged)
                        {
                            cells[x, y].BackgroundImage = Properties.Resources.mine;
                        }
                    }
                }
            }
        }
        private void OpenCell(Cell cell)
        {
            if (!cell.IsOpened)
            {
                if (!cell.IsFlagged)
                {
                    cell.IsOpened = true;
                    cell.BackColor = SystemColors.Control;
                    if (cell.MinesNearly > 0)
                    {
                        int minesNearly = cell.MinesNearly;
                        cell.Text = minesNearly.ToString();
                    }
                    else // Открытие соседних ячеек, если мин поблизости нет
                    {
                        int x = cell.x;
                        int y = cell.y;
                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                if (x + i >= 0 && x + i < MapWidth && y + j >= 0 && y + j < MapHeight)
                                {
                                    if (!cells[x + i, y + j].IsMine)
                                    {
                                        OpenCell(cells[x + i, y + j]);
                                    }
                                }
                            }
                        }
                    }
                }
                
            }
        }
        private void Flag_Click(object sender, EventArgs e)
        {
            Flag clickedFlag = sender as Flag;
            if (clickedFlag.Active == false)
            {
                clickedFlag.Active = true;
                clickedFlag.BackgroundImage = Properties.Resources.flag2;
            }
            else
            {
                clickedFlag.Active = false;
                clickedFlag.BackgroundImage = null;
            }
        }
        private void StartButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (gamePanel != null)
            {
                if (gameWasStarted) // Проверка, что игра была начата
                {
                    MessageBoxButtons yesNoButtons = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show("You have an unfinished game, do you want to continue?", "Unfinished Game", yesNoButtons);
                    if (result == DialogResult.Yes)
                    {
                        menuPanel.Visible = false;
                        this.Size = new Size(gamePanel.Size.Width + 16, gamePanel.Size.Height + 40);
                        gamePanel.Visible = true;
                        gameMenuStrip.Visible = true;
                        gameTimer.Start();
                    }
                    else
                    {
                        gameWasStarted = false;
                        menuPanel.Visible = false;
                        selectLevelPanel.Visible = true;
                    }
                }
                else
                {
                    menuPanel.Visible = false;
                    selectLevelPanel.Visible = true;
                }
            }
            else
            {
                menuPanel.Visible = false;
                selectLevelPanel.Visible = true;
            }
        }
        private void ScoreboardButton_MouseClick(object sender, MouseEventArgs e)
        {
            menuPanel.Visible = false;
            scoreboardTable.Visible = true;
            this.Size = new Size(200, 540);
        }
        private void ExitButton_MouseClick(object sender, MouseEventArgs e)
        {
            Application.Exit();
        }

        private void RestartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gameTimer.Stop();
            this.Controls.Remove(gamePanel);
            cells = new Cell[MapWidth, MapHeight];
            InitializeGamePanel();
            gameWasStarted = false;
        }

        private void TipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To automatically select a flag, press F");
        }

        private void BackToMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gameTimer.Stop();
            this.Size = new Size(336, 390);
            gamePanel.Visible = false;
            gameMenuStrip.Visible = false;
            menuPanel.Visible = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F && flag != null && gamePanel.Visible == true) { Flag_Click(flag, null); }
            else if (e.KeyCode == Keys.Escape && selectLevelPanel.Visible == true) // Из выбора уровня в меню
            {
                selectLevelPanel.Visible = false;
                menuPanel.Visible = true;
            }
            else if (e.KeyCode == Keys.Escape && gamePanel != null && gamePanel.Visible == true) // Из игры в меню
            {
                BackToMenuToolStripMenuItem_Click(sender, null);
            }
            else if (e.KeyCode == Keys.Escape && scoreboardTable != null && scoreboardTable.Visible == true) // Из таблицы лидеров в меню
            {
                scoreboardTable.Visible = false;
                menuPanel.Visible = true;
                this.Size = new Size(336, 390);
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            time += 1;
            timeCountLabel.Text = "Time: " + time.ToString();
        }
    }
    public class Cell : Button
    {
        public bool IsMine;
        public bool IsOpened;
        public bool IsFlagged;
        public int MinesNearly;
        public int x;
        public int y;

        public Cell()
        {
            x = 0;
            y = 0;
            IsMine = false;
            IsOpened = false;
            IsFlagged = false;
            MinesNearly = 0;
        }
    }
    public class Flag : Button
    {
        public bool Active;
        public Flag()
        {
            Active = false;
        }
    }
}
