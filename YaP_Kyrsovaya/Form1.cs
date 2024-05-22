using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YaP_Kyrsovaya
{
    public partial class Form1 : Form
    {
        private bool gameOver = false;
        private const int CellSize = 30;
        private int MapWidth = 8;
        private int MapHeight = 8;
        private int minesToPlace;
        private Cell[,] cells;
        private Panel selectLevelPanel;
        private TextBox mapWidthTextBox;
        private TextBox mapHeightTextBox;
        private TextBox minesTextBox;
        private Panel gamePanel;
        private Size gamePanelSize;
        private Flag flag;
        public Form1()
        {
            InitializeComponent();
            InitializeSelectLevelPanel();
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
            CalculateFormSizeAndShowGame();
        }
        private void NormalLevelButton_Click(object sender, EventArgs e)
        {
            MapWidth = 16;
            MapHeight = 16;
            minesToPlace = 40;
            CalculateFormSizeAndShowGame();
        }
        private void HardLevelButton_Click(object sender, EventArgs e)
        {
            MapWidth = 30;
            MapHeight = 16;
            minesToPlace = 99;
            CalculateFormSizeAndShowGame();
        }
        private void CustomLevelButton_Click(object sender, EventArgs e)
        {
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
            gamePanelSize = new Size(MapWidth * CellSize + 80, MapHeight * CellSize + 110);
            InitializeGamePanel();
            
            this.Size = new Size(gamePanel.Size.Width + 16, gamePanel.Size.Height + 40);

            selectLevelPanel.Visible = false;
            gameMenuStrip.Visible = true;
        }
        private void InitializeGamePanel()
        {
            gameOver = false;
            if (gamePanel != null) { this.Controls.Remove(gamePanel); }
            gamePanel = new Panel()
            {
                Size = gamePanelSize,
                Location = new Point(0,0)
            };
            this.Controls.Add(gamePanel);

            flag = new Flag()
            {
                Size = new Size(CellSize, CellSize),
                Location = new Point(gamePanel.Size.Width / 2 - CellSize / 2, 25),
                BackColor = Color.LightGray,
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
                    Cell cell = new Cell()
                    {
                        Size = new Size(CellSize, CellSize),
                        Location = new Point(x * CellSize + 40, y * CellSize + 60),
                        Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold),
                        BackgroundImageLayout = ImageLayout.Stretch
                };
                    cell.x = x;
                    cell.y = y;
                    cell.Click += Cell_Click;
                    gamePanel.Controls.Add(cell);
                    cells[x, y] = cell;
                }
            }

            // Случайное размещение мин
            Random rand = new Random();
            int mines = minesToPlace;
            while (mines > 0)
            {
                int x = rand.Next(MapWidth);
                int y = rand.Next(MapHeight);

                if (!cells[x, y].IsMine)
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
            if (!gameOver)
            {
                Cell clickedCell = sender as Cell;
                if (!clickedCell.IsOpened)
                {
                    if (flag.Active && clickedCell.IsFlagged)
                    {
                        clickedCell.IsFlagged = false;
                        clickedCell.BackgroundImage = null;
                    }
                    else if (flag.Active && !clickedCell.IsFlagged)
                    {
                        clickedCell.IsFlagged = true;
                        clickedCell.BackgroundImage = Properties.Resources.flag;
                    }
                    else if (!flag.Active && clickedCell.IsFlagged) { }
                    else
                    {
                        if (clickedCell.IsMine)
                        {
                            clickedCell.BackColor = Color.Red;
                            gameOver = true;
                            OpenAllMines();
                            MessageBox.Show("Game Over!");
                            // Перезапуск игры или завершение
                        }
                        else
                        {
                            OpenCell(clickedCell);
                        }
                    }
                }
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
                        cells[x, y].BackColor = Color.Red;
                    }
                }
            }
        }
        private void OpenCell(Cell cell)
        {
            if (!cell.IsOpened)
            {
                cell.IsOpened = true;
                cell.BackColor = Color.LightGray;

                if (cell.MinesNearly > 0)
                {
                    cell.Text = cell.MinesNearly.ToString();
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
        private void Flag_Click(object sender, EventArgs e)
        {
            Flag clickedFlag = sender as Flag;
            if (clickedFlag.Active == false)
            {
                clickedFlag.Active = true;
                clickedFlag.BackgroundImage = Properties.Resources.flag;
            }
            else
            {
                clickedFlag.Active = false;
                clickedFlag.BackgroundImage = null;
            }
        }

        private void startButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (gamePanel != null)
            {
                bool gameWasStarted = false;
                for (int x = 0; x < MapWidth; x++) // Проверка что игра была начата
                {
                    for (int y = 0; y < MapHeight; y++)
                    {
                        if (cells[x, y].IsOpened == true)
                        {
                            gameWasStarted = true;
                            break;
                        }
                        if (gameWasStarted) { break; }
                    }
                }
                if (gameWasStarted)
                {
                    MessageBoxButtons yesNoButtons = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show("You have an unfinished game, do you want to continue?", "Unfinished Game", yesNoButtons);
                    if (result == DialogResult.Yes)
                    {
                        menuPanel.Visible = false;
                        this.Size = new Size(gamePanel.Size.Width + 16, gamePanel.Size.Height + 40);
                        gamePanel.Visible = true;
                        gameMenuStrip.Visible = true;
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
            else
            {
                menuPanel.Visible = false;
                selectLevelPanel.Visible = true;
            }
        }
        private void scoreboardButton_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void settingsButton_MouseClick(object sender, MouseEventArgs e)
        {

        }
        private void exitButton_MouseClick(object sender, MouseEventArgs e)
        {
            Application.Exit();
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Controls.Remove(gamePanel);
            cells = new Cell[MapWidth, MapHeight];
            InitializeGamePanel();
        }

        private void TipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To automatically select a flag, press F");
        }

        private void BackToMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Size = new Size(336, 390);
            gamePanel.Visible = false;
            gameMenuStrip.Visible = false;
            menuPanel.Visible = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F && flag != null) { Flag_Click(flag, null); }
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
    /*
    Перерисовать флаг с цветом фона SystemColors.Control
    Добавить победу
    Мина картинкой
    Дизайн клеток
    Табло мин и времени
    Таблица лидеров
    Начало игры с пустых клеток
    */
}
