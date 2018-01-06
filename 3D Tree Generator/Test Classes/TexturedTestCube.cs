using System;
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

namespace _3D_Tree_Generator.Test_Classes
{
    class TexturedTestCube : Mesh
    {
        public TexturedTestCube() : base("Resources/Objects/Cube.obj")
        {
            Debug.WriteLine(String.Join("\n", Tris.Select(i => i.ToStringFull()).ToArray()));
            Recalculate();
            Texture = new Texture("Resources/Textures/TestTexture.jpg");
            IsTextured = true;
            Name = "TexturedTestCube";
            Position = new Vector3(0, 0, 0);

        }

    }
}
