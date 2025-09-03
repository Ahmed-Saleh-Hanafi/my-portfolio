using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Project_Game_CG
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            game.StartGame(bitmap);
            pictureBox1.Image = bitmap;
        }
        public Bitmap bitmap = new Bitmap(1001, 541);
        Game game = new Game();

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
           
            if (e.KeyCode == Keys.Up)
            {
                game.player2.TranslatePlayer(0, -12);
                game.DisplayGame(bitmap);
                pictureBox1.Image = bitmap;
            }
             if (e.KeyCode == Keys.Down)
            {

                game.player2.TranslatePlayer(0, 12);
                game.DisplayGame(bitmap);
                pictureBox1.Image = bitmap;
            }
             if (e.KeyCode == Keys.Right)
            {

                game.player2.TranslatePlayer(12, 0);
                game.DisplayGame(bitmap);
                pictureBox1.Image = bitmap;
            }
             if (e.KeyCode == Keys.Left)
            {

                game.player2.TranslatePlayer(-12, 0);
                game.DisplayGame(bitmap);
                pictureBox1.Image = bitmap;
            }
             if (e.KeyCode == Keys.W)
            {
                game.player1.TranslatePlayer(0, -12);
                game.DisplayGame(bitmap);
                pictureBox1.Image = bitmap;
            }
             if (e.KeyCode == Keys.S)
            {
                game.player1.TranslatePlayer(0, 12);
                game.DisplayGame(bitmap);
                pictureBox1.Image = bitmap;
            }
             if (e.KeyCode == Keys.D)
            {

                game.player1.TranslatePlayer(12, 0);
                game.DisplayGame(bitmap);
                pictureBox1.Image = bitmap;
            }
             if (e.KeyCode == Keys.A)
            {

                game.player1.TranslatePlayer(-12, 0);
                game.DisplayGame(bitmap);
                pictureBox1.Image = bitmap;
            }
            

        }
        static int tx = 10, ty = 1 ;

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            

            if (game.ball.pt.Y <= 17)
            {
               
                ty = Math.Abs(ty);
                
            }
            else if (game.ball.pd.Y >= 523)
            {
                ty = -1 * Math.Abs(ty);

            }
            else if (game.ball.pr.X >= 948 && (game.ball.pl.Y <= 182 || game.ball.orgin.Y > 360))
            {
                tx = -1 * Math.Abs(tx);

            }
            else if (game.ball.pl.X <= 52  && (game.ball.pl.Y <= 182 || game.ball.orgin.Y > 360))
            {
                tx = Math.Abs(tx);

            }
            else if (Utilities.IsInsideFace(game.ball.pr, game.player2.player.Faces[0]) )
            {
                tx = -1 * Math.Abs( tx);

            }
            else if (Utilities.IsInsideFace(game.ball.pl, game.player2.player.Faces[1]))
            {
                tx =  Math.Abs(tx);

            }
            else if (Utilities.IsInsideFace(game.ball.pl, game.player1.player.Faces[0]) )
            {
                tx =  Math.Abs(tx);

            }
            else if (Utilities.IsInsideFace(game.ball.pl, game.player1.player.Faces[0]) || Utilities.IsInsideFace(game.ball.pr, game.player1.player.Faces[1]))
            {
                tx = Math.Abs(tx);

            }
            else if (game.ball.orgin.X <= 50 && game.ball.orgin.Y >= 183 && game.ball.orgin.Y <= 360)
            {
                game.ngoalp2++;
                label4.Text = Convert.ToString(game.ngoalp2);
                game.ball.ResetBall(new Point(500, 270));
                tx = Math.Abs(tx) + 2;
                ty = 2;
                game.player2.ScalePlayer(1,0.9);
            }
            else if (game.ball.orgin.X >= 950 && game.ball.orgin.Y >= 183 && game.ball.orgin.Y <= 360)
            {
                game.ngoalp1++;
                label3.Text = Convert.ToString(game.ngoalp1);
                game.ball.ResetBall(new Point(500, 270));
                tx = -1 *(Math.Abs(tx)+ 2);
                ty = 2;
                game.player1.ScalePlayer(1, 0.9);
            }
            if (game.ngoalp2 == 5)
            {
                GameTimer.Stop();
                MessageBox.Show("Player 2 win");
            }
            if (game.ngoalp1 == 5)
            {
                GameTimer.Stop();
                MessageBox.Show("Player 1 win");
            }

            game.ball.TranslateBall(tx, ty);
            game.DisplayGame(bitmap);
            pictureBox1.Image = bitmap;



        }
    }
}
