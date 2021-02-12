using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Generator_pociągów
{
    public partial class Form1 : Form
    {
        string[] names;
        List<string> aWag = new List<string>();
        List<string> bWag = new List<string>();
        List<string> wWag = new List<string>();
        List<string> loco = new List<string>();

        int count = 0;
        int szerZest = 0;
        List<MiniImage> zestawienie = new List<MiniImage>();

        Bitmap sklad;
        Graphics grph;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
             sklad = new Bitmap(pictureBox3.Width, 58);
             names = Directory.GetFiles(Application.StartupPath + @"\res", "*.gif",
                                                     SearchOption.TopDirectoryOnly);
            foreach (string name in names)
            {
                string nameB = Path.GetFileName(name);
                nameB = nameB.Substring(0, nameB.Length - 6); // potem dodac -a.gif
                
                if (nameB.StartsWith("A"))
                {
                    aWag.Add(nameB);
                }
                else if (nameB.StartsWith("B"))
                {
                    bWag.Add(nameB);
                }
                else if (nameB.StartsWith("W"))
                {
                    wWag.Add(nameB);
                }
                else
                {
                    loco.Add(nameB);
                }
            }
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            posrList.Items.Add(addPosrTxt.Text);
            Globals.posrednie.Add(addPosrTxt.Text);
            addPosrTxt.Text = "";
            addPosrTxt.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int idx = posrList.SelectedIndex;
                posrList.Items.RemoveAt(idx);
                Globals.posrednie.RemoveAt(idx);
            }
            catch (Exception ex)
            {

            }
        }

        private void numberTxt_TextChanged(object sender, EventArgs e)
        {
            Globals.number = numberTxt.Text;
        }

        private void nameTxt_TextChanged(object sender, EventArgs e)
        {
            Globals.name = nameTxt.Text;
        }

        private void entTxt_TextChanged(object sender, EventArgs e)
        {
            Globals.finish = entTxt.Text.ToUpper();
        }

        private void startTxt_TextChanged(object sender, EventArgs e)
        {
            Globals.start = startTxt.Text.ToUpper();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dr = saveFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                pictureBox1.BackgroundImage.Save(saveFileDialog1.FileName);
            }

        }

        // Generator
        private void button4_Click(object sender, EventArgs e)
        {
            Bitmap bm = new Bitmap(1080, 795);
            Graphics graphics = Graphics.FromImage(bm);
            Pen pen = new Pen(Color.Black, 2f);
            SolidBrush bg = new SolidBrush(Color.White);
            Rectangle rect = new Rectangle(0 + 50, 60, 980, 100);


            // Background
            PointF[] pf = new PointF[]
            {
                new PointF(0, 0),
                new PointF(0, 795),
                new PointF(1080, 795),
                new PointF(1080, 0)
            };
            graphics.FillPolygon(bg, pf);

            // Separator
            graphics.DrawLine(pen, new PointF(0 + 50, 150), new PointF(1080 - 50, 150));

            // Number
            graphics.DrawString(Globals.number, new Font("Arial", 50), Brushes.Red, rect);

            // Name
            StringFormat formatB = new StringFormat(StringFormatFlags.DirectionRightToLeft);

            graphics.DrawString(Globals.name.ToUpper(), new Font("Arial", 50, FontStyle.Italic), Brushes.Red, rect, formatB);

            // Start station

            Rectangle rectStart = new Rectangle(0 + 50, 200, 1080 - 50, 150);
            graphics.DrawString(Globals.start, new Font("Arial", 50), Brushes.Black, rectStart);


            // Final station
            Rectangle rectEnd = new Rectangle(0 + 50, 645, 980, 150);
            graphics.DrawString(Globals.finish, new Font("Arial", 50), Brushes.Black, rectEnd, formatB);


            // Stacje posrendnie

            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;

            Rectangle posr = new Rectangle(50, 375, 980, 270);
            string posrednie = "";
            for (int i = 0; i < Globals.posrednie.Count; i++)
            {
                posrednie += Globals.posrednie[i] + " - "; 
            }

            if (posrednie.Length > 2)
            {
                posrednie = posrednie.Substring(0, posrednie.Length - 2);
            }
            graphics.DrawString(posrednie, new Font("Arial", 35), Brushes.Black, posr, format);


            // Show graphics
            pictureBox1.BackgroundImage = null;
            pictureBox1.BackgroundImage = bm;




        }

        private void button5_Click(object sender, EventArgs e)
        {
            Process.Start("https://skrj.plk-sa.pl/kalkulacja/");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Wagony A")
            {
                listBox1.DataSource = aWag;
            }
            else if (comboBox1.Text == "Wagony B")
            {
                listBox1.DataSource = bWag;
            }
            else if (comboBox1.Text == "Wagony W")
            {
                listBox1.DataSource = wWag;
            }
            else
            {
                listBox1.DataSource = loco;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pictureBox2.BackgroundImage = new Bitmap(Application.StartupPath + @"\res\" + listBox1.SelectedItem + "-a.gif");
            }
            catch (Exception ex)
            {

            }
        }

        // Generator zestawien
        private void button6_Click(object sender, EventArgs e)
        {
            Graphics g = Graphics.FromImage(sklad);
            if (pictureBox2.BackgroundImage.Height >= 40 && pictureBox2.BackgroundImage.Height <= 50)
            {
                g.DrawImage(pictureBox2.BackgroundImage,
                    new Rectangle(0 + szerZest, 58 - pictureBox2.BackgroundImage.Height, pictureBox2.BackgroundImage.Width, 41),
                    new Rectangle(0, 0, pictureBox2.BackgroundImage.Width, pictureBox2.BackgroundImage.Height),
                    GraphicsUnit.Pixel);
            }
            else if(pictureBox2.BackgroundImage.Height >= 52)
            {
                g.DrawImage(pictureBox2.BackgroundImage,
                    new Rectangle(0 + szerZest, 58 - pictureBox2.BackgroundImage.Height, pictureBox2.BackgroundImage.Width, 58),
                    new Rectangle(0, 0, pictureBox2.BackgroundImage.Width, pictureBox2.BackgroundImage.Height),
                    GraphicsUnit.Pixel);
            }
            else
            {
                MessageBox.Show(pictureBox2.BackgroundImage.Height.ToString());
            }
            szerZest += pictureBox2.BackgroundImage.Width;

            pictureBox3.Image = sklad;

            hScrollBar1.Maximum = szerZest + 7;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            szerZest = 0;
            sklad = new Bitmap(pictureBox3.Width, 58);
            pictureBox3.Image = null;
            hScrollBar1.Maximum = szerZest + 7;
            pictureBox3.Location = new Point(7, 321);
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            pictureBox3.Location = new Point(-hScrollBar1.Value,
                pictureBox3.Location.Y);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Bitmap map = new Bitmap(szerZest, 58);
            Rectangle rect = new Rectangle(0, 0, szerZest, 58);
            Graphics gr = Graphics.FromImage(map);
            gr.DrawImage(pictureBox3.Image, rect, rect, GraphicsUnit.Pixel);

            DialogResult dr = saveFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                map.Save(saveFileDialog1.FileName);
            }

        }
    }

    class MiniImage
    {
        int width;
        Image image;
        public MiniImage(Image image, int width)
        {
            this.image = image;
            this.width = width;
        }
    }
}
