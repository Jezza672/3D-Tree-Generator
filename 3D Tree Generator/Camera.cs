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
using System.Windows.Forms;


namespace _3D_Tree_Generator
{
    class Camera
    {

        public Vector3 Position { get; set; } 
        public Vector3 LookingAt { get; set; }
        public Vector3 Up { get; set; }
        public float Sensitivity { get; set; }

        private Point prevmouspos;
        private Boolean nue;
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
            Position = new Vector3(0, 5, 10);
            LookingAt = new Vector3(0, 0, 0);
            Up = new Vector3(0, 1, 0);
            prevmouspos = new Point(0,0);
            Sensitivity = 0.005f;
        }

        public void Rotate(Vector3 rotation)
        {
            Position =  new Vector3(Matrix4.CreateRotationX(rotation.X) * Matrix4.CreateRotationY(rotation.Y) * Matrix4.CreateRotationZ(rotation.Z) * new Vector4(Position, 1));
        }

        public void Rotate(float x, float y, float z)
        {
            Rotate(new Vector3(x, y, z));
        }

        public void Update(Control control) { 

            if (Control.MouseButtons == MouseButtons.Left)
            {
                Point mousepos = control.PointToClient(Control.MousePosition);
                if (nue)
                {
                    prevmouspos = mousepos;
                    nue = false;
                }               
                Rotate((mousepos.Y - prevmouspos.Y) * Sensitivity, (mousepos.X - prevmouspos.X) * Sensitivity, 0);
                prevmouspos = mousepos;
            } else if (Control.MouseButtons == MouseButtons.None)
            {
                nue = true;
            }

        } //https://msdn.microsoft.com/en-gb/library/system.windows.forms.control.pointtoclient.aspx
        //https://stackoverflow.com/questions/7795068/getting-the-mouse-window-coordinates-in-opentk-c-net
    }
}
