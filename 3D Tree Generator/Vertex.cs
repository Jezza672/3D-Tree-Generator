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
        public Vector3 Color;

        /// <summary>
        /// Empty Vertex.
        /// </summary>
        public Vertex()
        {
            Position = new Vector3(0, 0, 0);
            Normal = new Vector3(0, 0, 0);
            TextureCoord = new Vector2(0, 0);
            Color = new Vector3(0, 0, 0);
        }

        /// <summary>
        /// Vertex with position.
        /// </summary>
        /// <param name="pos"></param>
        public Vertex(Vector3 position)
        {
            Position = position;
            Normal = new Vector3(0, 0, 0);
            TextureCoord = new Vector2(0, 0);
            Color = new Vector3(0, 0, 0);
        }


        /// <summary>
        /// Vertex with position and normal.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="norm"></param>
        public Vertex(Vector3 position, Vector3 normal)
        {
            Position = position;
            Normal = normal;
            TextureCoord = new Vector2(0, 0);
            Color = new Vector3(0, 0, 0);
        }


        /// <summary>
        /// Vertex with position, normal and texture coordinate.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="norm"></param>
        /// <param name="texcoord"></param>
        public Vertex(Vector3 position, Vector3 normal, Vector2 texturecoordinate)
        {
            Position = position;
            Normal = normal;
            TextureCoord = texturecoordinate;
            Color = new Vector3(0, 0, 0);
        }

        /// <summary>
        /// Vertex with poisition, normal and color.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="normal"></param>
        /// <param name="color"></param>
        public Vertex(Vector3 position, Vector3 normal, Vector3 color)
        {
            Position = position;
            Normal = normal;
            TextureCoord = new Vector2(0, 0);
            Color = color;
        }

        /// <summary>
        /// Transform vertex position by a matrix
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public Vertex Transform(Matrix4 mat)
        {
            Vector3 translation = mat.ExtractTranslation();
            mat = mat.ClearTranslation();
            return Transform(mat, translation);
        }

        /// <summary>
        /// Transform vertex position by a matrix
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="translation"></param>
        /// <returns></returns>
        public Vertex Transform(Matrix4 mat, Vector3 translation)
        {
            Position = new Vector3(mat * new Vector4(Position, 1)) + translation;
            //Position = Vector3.Transform(Position, mat);
            return this;
        }

        /// <summary>
        /// Returns a translated, alernate version of the vertex
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="translation"></param>
        /// <returns></returns>
        public Vertex Transformed(Matrix4 mat, Vector3 translation)
        {
            return new Vertex(Position, Normal, TextureCoord).Transform(mat, translation);
        }

        /// <summary>
        /// Returns a translated, alernate version of the vertex
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public Vertex Transformed(Matrix4 mat)
        {
            return new Vertex(Position, Normal, TextureCoord).Transform(mat);
        }

        public override string ToString()
        {
            return Position.ToString();
        }
    }
}
