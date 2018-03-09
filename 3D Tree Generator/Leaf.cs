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
    class Leaf : Mesh
    {

        const float divisor = 100;
        public string AltImage;

        public Leaf()
        {
            Leaf_Creator form = new Leaf_Creator();
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
            {
                Matrix4 trans = Matrix4.CreateTranslation((form.centerx - form.sizex) / divisor, 0, (form.centery - form.sizey )/ divisor);
                Tris = GenerateRectangle(form.sizex / divisor, form.sizey / divisor).Select(i => i.Transformed(trans)).ToArray();
                Texture = new Texture(form.filename);
                Debug.WriteLine(String.Format("Created leaf with TexId: {0}", (Texture.TexID)));
                AltImage = form.tempfilename;
                Name = Path.GetFileName(form.filename);
            }
            else
            {
                throw new ArgumentException("User Cancelled");
            }
        }


        public Leaf(Leaf clone)
        {
            Tris = clone.Tris;
            Texture = clone.Texture;
            AltImage = clone.AltImage;
            Name = clone.Name;
        }

        public Leaf(Leaf clone, Vector3 normal) : this(clone)
        {
            Rotation = RotateTo(normal);
        }

        private Vector3 RotateTo(Vector3 target)
        {
            float x = (float)(Math.Atan(target.Y / (Math.Sqrt(target.X * target.X + target.Y * target.Y))));
            float y = (float)(Math.Atan(target.X / target.Z));
            return new Vector3(x, y, 0);
        }


        private Tri[] GenerateRectangle(float x, float y)
        {
            Debug.WriteLine(x.ToString() + " " + y.ToString());
            Tri tri1 = new Tri(
                    new Vertex(new Vector3(0, 0, 0), new Vector2(1, 0)),
                    new Vertex(new Vector3(x, 0, 0), new Vector2(0, 0)),
                    new Vertex(new Vector3(0, 0, y), new Vector2(1, 1))
                );
            Tri tri2 = new Tri(
                    new Vertex(new Vector3(x, 0, y), new Vector2(0, 1)),
                    new Vertex(new Vector3(x, 0, 0), new Vector2(0, 0)),
                    new Vertex(new Vector3(0, 0, y), new Vector2(1, 1))
                );
            return new Tri[]{tri1, tri2};
        }
    }
}
