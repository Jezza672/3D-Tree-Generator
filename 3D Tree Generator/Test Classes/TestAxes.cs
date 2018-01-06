using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace _3D_Tree_Generator.Test_Classes
{
    class TestAxes : Mesh
    {
        public TestAxes() : this(Vector3.Zero)
        {

        }

        public TestAxes(Vector3 pos) : base(
                new Vector3[] {
                new Vector3(0f, 0.1f,  0.1f),
                new Vector3(0f, -0.1f,  0.1f),
                new Vector3(0f, -0.1f,  -0.1f),
                new Vector3(0f, 0.1f,  -0.1f),
                new Vector3(5f, 0.1f,  0.1f),
                new Vector3(5f, -0.1f,  0.1f),
                new Vector3(5f, -0.1f,  -0.1f),
                new Vector3(5f, 0.1f,  -0.1f),

                new Vector3(0.1f, 0f, 0.1f),
                new Vector3(-0.1f, 0f,  0.1f),
                new Vector3(-0.1f, 0f,  -0.1f),
                new Vector3(0.1f,  0f, -0.1f),
                new Vector3(0.1f,  5f, 0.1f),
                new Vector3(-0.1f,  5f, 0.1f),
                new Vector3(-0.1f,  5f, -0.1f),
                new Vector3(0.1f, 5f, -0.1f),

                new Vector3(0.1f, 0.1f, 0f),
                new Vector3(-0.1f,0.1f, 0f),
                new Vector3(-0.1f, -0.1f, 0f),
                new Vector3(0.1f, -0.1f, 0f),
                new Vector3(0.1f, 0.1f, 5f),
                new Vector3(-0.1f, 0.1f, 5f),
                new Vector3(-0.1f,-0.1f, 5f),
                new Vector3(0.1f, -0.1f, 5f),
            }, new int[] {
                //left
                0, 2, 1,
                0, 3, 2,
                //back
                1, 2, 6,
                6, 5, 1,
                //right
                4, 5, 6,
                6, 7, 4,
                //top
                2, 3, 6,
                6, 3, 7,
                //front
                0, 7, 3,
                0, 4, 7,
                //bottom
                0, 1, 5,
                0, 5, 4,

                8, 10, 9, 8, 11, 10, 9, 10, 14, 14, 13, 9, 12, 13, 14, 14, 15, 12, 10, 11, 14, 14, 11, 15, 8, 15, 11, 8, 12, 15, 8, 9, 13, 8, 13, 12,

                16, 18, 17, 16, 19, 18, 17, 18, 22, 22, 21, 17, 20, 21, 22, 22, 23, 20, 18, 19, 22, 22, 19, 23, 16, 23, 19, 16, 20, 23, 16, 17, 21, 16, 21, 20
            }
        )
        {
            Name = "TestAxes";

            Position = pos;

            Colors = new Vector3[] {
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 0),

                

                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 0),

                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),

            };

            Debug.WriteLine(String.Format("Created Test Axis at {0}", Position.ToString()));
        }
    }
}
