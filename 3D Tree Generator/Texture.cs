﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using _3D_Tree_Generator.Test_Classes;

namespace _3D_Tree_Generator
{
    class Texture
    {
        public int TexID;

        public static Texture Default = new Texture("Resources/Textures/Default.jpg");

        public Texture(Bitmap image)
        {
            TexID = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, TexID);
            BitmapData data = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            image.UnlockBits(data);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            //Debug.WriteLine(String.Format("TexID: {0}", (TexID)));

        }

        public Texture(string filename, bool Alert = true) : this(ConvertToBitmap(filename, Alert))
        {
            Debug.WriteLine(String.Format("Loaded {0} with TexID {1}", filename, TexID));
        }

        public static Bitmap ConvertToBitmap(string fileName, bool alert = true) //https://stackoverflow.com/questions/24383256/how-can-i-convert-a-jpg-file-into-a-bitmap-using-c
        {
            Bitmap bitmap;
            try
            {
                using (Stream bmpStream = System.IO.File.Open(fileName, System.IO.FileMode.Open))
                {
                    Image image = Image.FromStream(bmpStream);

                    bitmap = new Bitmap(image);

                }
            }
            catch (FileNotFoundException)
            {
                if (alert)
                {
                    MessageBox.Show(String.Format("Texture \"{0}\" could not be found", fileName), "Texture Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                using (Stream bmpStream = System.IO.File.Open("Resources/Textures/Default.jpg", System.IO.FileMode.Open))
                {
                    Image image = Image.FromStream(bmpStream);

                    bitmap = new Bitmap(image);

                }
            }
            catch (DirectoryNotFoundException)
            {
                if (alert)
                {
                    MessageBox.Show(String.Format("Texture \"{0}\" could not be found", fileName), "Texture Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                using (Stream bmpStream = System.IO.File.Open("Resources/Textures/Default.jpg", System.IO.FileMode.Open))
                {
                    Image image = Image.FromStream(bmpStream);

                    bitmap = new Bitmap(image);

                }
            }
            return bitmap;
        }
    }
}
