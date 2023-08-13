using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cem_Yazicioglu_PacMan_Uygulaması
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            altınlar = this.Controls.OfType<PictureBox>().ToList();
            altınlar.Remove(pcbAvatar);
            altınlar.Remove(pcbYellowAvatar);
            altınlar.Remove(pcbRedAvatar);
            altınlar.Remove(pcbPinkAvatar);
            enemyAvatars.Add(pcbPinkAvatar);
            enemyAvatars.Add(pcbRedAvatar);
            enemyAvatars.Add(pcbYellowAvatar);
            timer1.Start();

        }
        private List<PictureBox> altınlar;
        private List<PictureBox> enemyAvatars = new List<PictureBox>();

        Random random = new Random();
        int redAvatarYon = 1;
        int pinkAvatarYon = 1;
        int yellowAvatarYon = 1;

        void EnemyAvatarHareketEt(ref int yon, PictureBox pictureBox)
        {
            int x = pictureBox.Location.X;
            int y = pictureBox.Location.Y;
            bool carptiMi;
            int sayac = 0;

            switch (yon)
            {
                case 0:
                    y -= 5;
                    break;
                case 1:
                    y += 5;
                    break;
                case 2:
                    x -= 5;
                    break;
                case 3:
                    x += 5;
                    break;
            }
            Point gecici = new Point(x, y);
            Rectangle EnemyAvatarRectangle = new Rectangle(gecici, pictureBox.Size);
            var labels = this.Controls.OfType<Label>().ToList();

            foreach (var item in labels)
            {
                carptiMi = item.Bounds.IntersectsWith(EnemyAvatarRectangle);
                sayac = carptiMi ? sayac + 1 : sayac;
            }
            if (sayac > 0)
            {
                yon = random.Next(4);
            }
            else
            {
                pictureBox.Location = new Point(x, y);
            }
        }

        void AvatarHareketEt(object sender, KeyEventArgs e)
        {
            int x = pcbAvatar.Location.X;
            int y = pcbAvatar.Location.Y;
            bool carptiMi;
            int sayac = 0;
            switch (e.KeyCode)
            {
                case Keys.Up:
                    pcbAvatar.Image = Properties.Resources.Up;
                    y -= 5;
                    break;
                case Keys.Down:
                    pcbAvatar.Image = Properties.Resources.down;
                    y += 5;
                    break;
                case Keys.Left:
                    pcbAvatar.Image = Properties.Resources.left;
                    x -= 5;
                    break;
                case Keys.Right:
                    pcbAvatar.Image = Properties.Resources.right;
                    x += 5;
                    break;
            }
            Point gecici = new Point(x, y);
            Rectangle avatarRectangle = new Rectangle(gecici, pcbAvatar.Size);
            var labels = this.Controls.OfType<Label>().ToList();

            foreach (var item in labels)
            {
                carptiMi = item.Bounds.IntersectsWith(avatarRectangle);
                sayac = carptiMi ? sayac + 1 : sayac;
            }
            if (sayac > 0)
            {
                return;
            }
            else
            {
                pcbAvatar.Location = new Point(x, y);
            }
        }

        void AltınTopla()
        {
            bool degdiMi;
            int silinecek = -1;

            for (int i = 0; i < altınlar.Count; i++)
            {
                degdiMi = pcbAvatar.Bounds.IntersectsWith(altınlar[i].Bounds);
                if (degdiMi)
                {
                    altınlar[i].Visible = false;
                    silinecek = i;
                    lblPuan.Text = (int.Parse(lblPuan.Text) + 1).ToString();
                    break;
                }
            }
            if (silinecek != -1)
            {
                altınlar.Remove(altınlar[silinecek]);
            }
            if (altınlar.Count == 0)
            {
                GameOver("Kazandınız.");
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            AvatarHareketEt(sender, e);
            AltınTopla();
        }

        void GameOver(string durum)
        {
            timer1.Stop();
            DialogResult cevap = MessageBox.Show($"Oyun bitti.{durum}\nPuanınız:{lblPuan.Text}\n Tekrar oynamak istiyor musunuz?", "Soru", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (cevap == DialogResult.Yes)
            {
                Application.Restart();
            }
            else
            {
                Application.Exit();
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
           
            EnemyAvatarHareketEt(ref pinkAvatarYon, pcbPinkAvatar);
            EnemyAvatarHareketEt(ref redAvatarYon, pcbRedAvatar);
            EnemyAvatarHareketEt(ref yellowAvatarYon, pcbYellowAvatar);
            foreach (var item in enemyAvatars)
            {
                if (item.Bounds.IntersectsWith(pcbAvatar.Bounds))
                {
                    GameOver("Kaybettiniz.");
                }
            }


        }
    }
}
