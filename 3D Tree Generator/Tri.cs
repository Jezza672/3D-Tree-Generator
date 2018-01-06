using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK;
using System.Drawing;

namespace _3D_Tree_Generator
{
    [Serializable]
    class Tri : IEnumerable<Vertex>
    {
        public Vertex Item1;
        public Vertex Item2;
        public Vertex Item3;

        public Tri()
        {
            Item1 = new Vertex();
            Item2 = new Vertex();
            Item3 = new Vertex();
        }

        public Tri(Vertex vertexOne, Vertex vertexTwo, Vertex vertexThree)
        {
            Item1 = vertexOne;
            Item2 = vertexTwo;
            Item3 = vertexThree;
        }

        public Tri(Tuple<Vertex, Vertex, Vertex> tuple)
        {
            Item1 = tuple.Item1;
            Item2 = tuple.Item2;
            Item3 = tuple.Item3;
        }

        public IEnumerator<Vertex> GetEnumerator()
        {
            yield return Item1;
            yield return Item2;
            yield return Item3;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return "Tri: " + Item1 +  ", " + Item2 + ", " + Item3;
        }

        public Tri Transformed(Matrix4 matrix)
        {
            Tri tri = new Tri();
            tri.Item1 = Item1.Transformed(matrix);
            tri.Item2 = Item2.Transformed(matrix);
            tri.Item3 = Item3.Transformed(matrix);
            return tri;
        }

        public Tri Transform(Matrix4 matrix)
        {
            Item1 = Item1.Transformed(matrix);
            Item2 = Item2.Transformed(matrix);
            Item3 = Item3.Transformed(matrix);
            return this;
        }
    }
}
