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
using System.Windows.Input;


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
            Position = Position - LookingAt;
            Position =  new Vector3(Matrix4.CreateRotationX(rotation.X) * Matrix4.CreateRotationY(rotation.Y) * Matrix4.CreateRotationZ(rotation.Z) * new Vector4(Position, 1));
            Position = Position + LookingAt;
        }

        public void Rotate(float x, float y, float z)
        {
            Rotate(new Vector3(x, y, z));
        }

        public void Move(Vector2 movement)
        {
            Vector3 vec = LookingAt - Position;
            //Vector3 movex = - Vector3.Cross(vec, new Vector3(0, 1, 0)) * movement.Y * Sensitivity * 100;

            Position = Position + new Vector3(vec.X, vec.Y, 0) * movement.Y ;
            LookingAt = vec + Position;
        }

        public void Move(float x, float y)
        {
            Move(new Vector2(x, y));
        }

        public void Update(Control control) {
            Point mousepos = control.PointToClient(Control.MousePosition);
            if (!control.ClientRectangle.Contains(mousepos))
            {
                return;
            } 
            switch (Control.MouseButtons)
            {
                case MouseButtons.Left:
                    if (nue) //do this if this is the start of the click
                    {
                        prevmouspos = mousepos;
                        nue = false;
                    } //this happens nomatter if just pressed of dragging.
                    Rotate((mousepos.Y - prevmouspos.Y) * Sensitivity, (mousepos.X - prevmouspos.X) * Sensitivity, 0);
                    prevmouspos = mousepos;
                    break;
                case MouseButtons.Right:
                    nue = true;
                    break;
                case MouseButtons.Middle:
                    /*
                    if (nue)
                    {
                        prevmouspos = mousepos;
                        nue = false;
                    }
                    Move((mousepos.Y - prevmouspos.Y) * Sensitivity, (mousepos.X - prevmouspos.X) * Sensitivity);
                    prevmouspos = mousepos;
                    */
                    break;
                default:
                    nue = true;
                    break;
            }

        } //https://msdn.microsoft.com/en-gb/library/system.windows.forms.control.pointtoclient.aspx
        //https://stackoverflow.com/questions/7795068/getting-the-mouse-window-coordinates-in-opentk-c-net

        public void Zoom(Control control, MouseEventArgs e)
        {
            if (!control.ClientRectangle.Contains(control.PointToClient(Control.MousePosition))) //ensures mouse is inside the preview pane
            {
                return;
            }
            if (Control.ModifierKeys.HasFlag(Keys.Shift)) //tests if shift is pressed
            {
                //Debug.WriteLine("control pressed");
                Position += new Vector3(0, 1, 0) * Sensitivity * e.Delta * 0.3f;
                LookingAt += new Vector3(0, 1, 0) * Sensitivity * e.Delta * 0.3f;
            }
            else
            {
                Position = Position - LookingAt;
                Position = Position * (1 - Sensitivity * e.Delta * 0.1f);
                Position = Position + LookingAt;
            }
        }
    }
}
