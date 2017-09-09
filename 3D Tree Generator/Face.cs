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
    class Face : IEnumerable<Vertex>
    {
        public Vertex Item1;
        public Vertex Item2;
        public Vertex Item3;

        public Face()
        {

        }
        public Face(Vertex vertexOne, Vertex vertexTwo, Vertex vertexThree)
        {
            Item1 = vertexOne;
            Item2 = vertexTwo;
            Item3 = vertexThree;
        }
        public Face(Tuple<Vertex, Vertex, Vertex> tuple)
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
    }
}
