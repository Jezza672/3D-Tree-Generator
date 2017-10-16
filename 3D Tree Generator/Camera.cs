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


namespace _3D_Tree_Generator
{
    class Camera
    {

        public Vector3 Position { get; set; } 
        public Vector3 LookingAt { get; set; }
        public Vector3 Up { get; set; }

        public Matrix4 ViewMatrix
        {

            get
            {
                return Matrix4.LookAt(Position, LookingAt, Up);
            }

            set {}
        }


        public Camera()
        {
            Position = new Vector3(0, 0, 10);
            LookingAt = new Vector3(0, 0, 0);
            Up = new Vector3(0, 1, 0);
        }
    }
}
