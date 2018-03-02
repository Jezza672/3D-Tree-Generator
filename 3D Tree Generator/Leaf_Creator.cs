using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK;
using System.Drawing;
using System.Diagnostics;
using NCalc;
using System.Windows.Forms;

namespace _3D_Tree_Generator
{
    public partial class Leaf_Creator : Form
    {
        Bitmap image;

        public int sizex;
        public int sizey;
        public int centerx;
        public int centery;
        public string filename;
        public string tempfilename;

        public Leaf_Creator()
        {
            InitializeComponent();
            marker.Parent = pictureBox1;
            marker.BackColor = Color.Transparent;
            marker.ImageLocation = "Resources/Textures/Marker.png";
            marker.Location = new Point(0, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileName.Text = openFileDialog1.FileName;
                Debug.WriteLine("Filename.Text");
                tempfilename = CreateTempFile(openFileDialog1.FileName);
                pictureBox1.Image =  new Bitmap(tempfilename);
                image = new Bitmap(pictureBox1.Image);

                numericUpDown3.Maximum = pictureBox1.Image.Width;
                numericUpDown3.Value = (int)pictureBox1.Image.Width / 2;
                numericUpDown4.Maximum = pictureBox1.Image.Height;
                numericUpDown4.Value = (int)pictureBox1.Image.Height / 2;
                numericUpDown1.Maximum = pictureBox1.Image.Width;
                numericUpDown1.Value = (int)pictureBox1.Image.Width;
                numericUpDown2.Maximum = pictureBox1.Image.Height;
                numericUpDown2.Value = (int)pictureBox1.Image.Height;

                updateMarker((int)numericUpDown3.Value, (int)numericUpDown4.Value);
            }
        }

        private void updateMarker(int X, int Y)
        {
            int x;
            int y;
            if (image.Width/image.Height < 195/176)
            {
                Debug.WriteLine("Taller");
                y = (int)((float)Y).Lerp(0, (double)numericUpDown2.Value, 0, pictureBox1.Size.Height);
                double scalefactor = (double)image.Height / (double)pictureBox1.Height;
                double newwidth = image.Width / scalefactor;
                double delta = (pictureBox1.Width / 2) - (newwidth / 2);
                x = (int)((float)X).Lerp(0, image.Width, delta, pictureBox1.Size.Width - delta);
            }
            else
            {
                Debug.WriteLine("Wider");
                x = (int)((float)X).Lerp(0, (double)numericUpDown1.Value, 0, pictureBox1.Size.Width);
                double scalefactor = (double)image.Width / (double)pictureBox1.Width;
                double newheight = image.Height / scalefactor;
                double delta = (pictureBox1.Height / 2) - (newheight / 2);
                y = (int)((float)Y).Lerp(0, image.Height, delta, pictureBox1.Size.Height - delta);
            }
            x -= 5;
            y -= 5;
            marker.Location = new Point(x, y);
        }

        private void value_Changed(object sender, EventArgs e)
        {
            updateMarker((int)numericUpDown3.Value, (int)numericUpDown4.Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            sizex = (int) numericUpDown1.Value;
            sizey = (int)numericUpDown2.Value;
            centerx = (int)numericUpDown3.Value;
            centery = (int)numericUpDown4.Value;
            filename = FileName.Text;
            this.Close();
        }

        public static string CreateTempFile(string fileName) //https://stackoverflow.com/questions/10431868/unlocking-image-from-picturebox
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");
            if (!File.Exists(fileName))
                throw new ArgumentException("Specified file must exist!", "fileName");
            string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + Path.GetExtension(fileName));
            File.Copy(fileName, tempFile);

            return tempFile;
        }
    }
}

