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
    class Vertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TextureCoord;


        /// <summary>
        /// Empty Vertex. Properties not initialised
        /// </summary>
        public Vertex()
        {
        }

        /// <summary>
        /// Vertex with position. Otheres not initialised
        /// </summary>
        /// <param name="pos"></param>
        public Vertex(Vector3 position)
        {
            Position = position;

        }


        /// <summary>
        /// Vertex with position and normal. Texture Coordinate not initialised
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="norm"></param>
        public Vertex(Vector3 position, Vector3 normal)
        {
            Position = position;
            Normal = normal;
        }


        /// <summary>
        /// Vertex with position, normal and texture coordinate
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="norm"></param>
        /// <param name="texcoord"></param>
        public Vertex(Vector3 position, Vector3 normal, Vector2 texturecoordinate)
        {
            Position = position;
            Normal = normal;
            TextureCoord = texturecoordinate;
        }
    }
}
