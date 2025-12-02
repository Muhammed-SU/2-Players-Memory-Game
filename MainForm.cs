using HafzaOyunu;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HafzaOyunu
{
    public partial class MainForm : Form
    {
        GameManager game;
        private TableLayoutPanel tableLayoutPanel1;
        private Label labelPlayer1;
        private Label labelPlayer2;
        private Label labelTurn;
        private Panel panel1;
        Size tileSize = new Size(80, 100); // Mahjong taşına göre

        public MainForm()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            var images = LoadImages();
            var player1 = new Player("Oyuncu 1");
            var player2 = new Player("Oyuncu 2");
            game = new GameManager(images, player1, player2);

            CreateGrid(images);
            ShowAllImagesTemporarily();
            UpdateTurnLabel();
        }

        private List<GameImage> LoadImages()
        {
            var imagePaths = Directory.GetFiles("Images").ToList();
            var selectedPaths = imagePaths.Take(20).ToList();
            selectedPaths.AddRange(selectedPaths); // çift oluştur
            selectedPaths = selectedPaths.OrderBy(x => Guid.NewGuid()).ToList(); // karıştır

            var gameImages = new List<GameImage>();
            foreach (var path in selectedPaths)
            {
                var pb = new PictureBox
                {
                    Size = tileSize,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BorderStyle = BorderStyle.FixedSingle,
                    Dock = DockStyle.Fill
                };
                pb.Click += PictureBox_Click;
                gameImages.Add(new GameImage(path, pb));
            }
            return gameImages;
        }

        private void CreateGrid(List<GameImage> images)
        {
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.ColumnCount = 8;
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();

            for (int i = 0; i < 8; i++)
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, tileSize.Width));

            for (int i = 0; i < 5; i++)
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, tileSize.Height));

            int index = 0;
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    tableLayoutPanel1.Controls.Add(images[index].PictureBox, col, row);
                    index++;
                }
            }
        }

        private async void ShowAllImagesTemporarily()
        {
            foreach (var img in game.Images)
                img.PictureBox.Image = Image.FromFile(img.ImagePath);

            await Task.Delay(5000);

            foreach (var img in game.Images)
                img.PictureBox.Image = null;
        }

        private async void PictureBox_Click(object sender, EventArgs e)
        {
            var clickedPB = sender as PictureBox;
            var clickedImage = game.Images.FirstOrDefault(img => img.PictureBox == clickedPB);

            if (clickedImage == null || clickedImage.IsMatched || clickedPB.Image != null)
                return;

            // Aynı karta çift basma kontrolü
            if (game.FirstSelected == clickedImage)
                return;

            clickedPB.Image = Image.FromFile(clickedImage.ImagePath);

            if (game.FirstSelected == null)
            {
                game.FirstSelected = clickedImage;
                await Task.Delay(5000); // 5 saniye bekle
            }
            else
            {
                if (game.CheckMatch(clickedImage))
                {
                    game.FirstSelected = null;
                    UpdateScoreLabels();
                    if (game.IsGameOver())
                    {
                        MessageBox.Show($"{game.CurrentPlayer.Name} kazandı!");
                        DisableAllImages();
                    }
                }
                else
                {
                    await Task.Delay(1000);

                    if (game.FirstSelected?.PictureBox != null)
                        game.FirstSelected.PictureBox.Image = null;

                    clickedPB.Image = null;
                    game.FirstSelected = null;
                    game.SwitchPlayer();
                    UpdateTurnLabel();
                }
            }
        }

        private void UpdateScoreLabels()
        {
            labelPlayer1.Text = $"Oyuncu 1: {game.Player1.Score}";
            labelPlayer2.Text = $"Oyuncu 2: {game.Player2.Score}";
        }

        private void UpdateTurnLabel()
        {
            labelTurn.Text = $"Sıra: {game.CurrentPlayer.Name}";
        }

        private void DisableAllImages()
        {
            foreach (var img in game.Images)
                img.PictureBox.Enabled = false;
        }

        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelPlayer1 = new System.Windows.Forms.Label();
            this.labelPlayer2 = new System.Windows.Forms.Label();
            this.labelTurn = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(54, 74);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(640, 500);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // labelPlayer1
            // 
            this.labelPlayer1.AutoSize = true;
            this.labelPlayer1.Location = new System.Drawing.Point(30, 26);
            this.labelPlayer1.Name = "labelPlayer1";
            this.labelPlayer1.Size = new System.Drawing.Size(75, 16);
            this.labelPlayer1.TabIndex = 1;
            this.labelPlayer1.Text = "Oyuncu 1: 0";
            this.labelPlayer1.Click += new System.EventHandler(this.labelPlayer1_Click);
            // 
            // labelPlayer2
            // 
            this.labelPlayer2.AutoSize = true;
            this.labelPlayer2.Location = new System.Drawing.Point(119, 26);
            this.labelPlayer2.Name = "labelPlayer2";
            this.labelPlayer2.Size = new System.Drawing.Size(75, 16);
            this.labelPlayer2.TabIndex = 2;
            this.labelPlayer2.Text = "Oyuncu 2: 0";
            // 
            // labelTurn
            // 
            this.labelTurn.AutoSize = true;
            this.labelTurn.Location = new System.Drawing.Point(202, 26);
            this.labelTurn.Name = "labelTurn";
            this.labelTurn.Size = new System.Drawing.Size(95, 16);
            this.labelTurn.TabIndex = 3;
            this.labelTurn.Text = "Sira : Oyuncu 1";
            this.labelTurn.Click += new System.EventHandler(this.labelTurn_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelTurn);
            this.panel1.Controls.Add(this.labelPlayer1);
            this.panel1.Controls.Add(this.labelPlayer2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(882, 68);
            this.panel1.TabIndex = 4;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(882, 721);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void labelTurn_Click(object sender, EventArgs e)
        {

        }

        private void labelPlayer1_Click(object sender, EventArgs e)
        {

        }
    }
}