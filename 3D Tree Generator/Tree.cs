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
    class Tree
    {
        public float height {get; set;}
        public float noise{get; set;}
        public int branching {get; set;}
        public int seed { get; set; }
        public int minheight { get; set; }
        public int quality { get; set; }

        public Mesh Mesh { get; set; }

        public Tree()
        {

        }

        /// <summary>
        /// generate the tree, having set up the parameters
        /// </summary>
        /// <param name="height"></param>
        public void GenerateTree(float height)
        {
            Mesh = CreateFrustum(height, 0.5f, 0.1f, 2, 5, Matrix4.CreateTranslation(new Vector3(0f, 0f, 0f)) * Matrix4.CreateRotationZ(0f));
        }

        /// <summary>
        /// unfinished
        /// </summary>
        /// <param name="height"></param>
        /// <param name="radius"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public Mesh GenerateBranch(float height, float radius, Matrix4 matrix)
        {
            Mesh geometry = CreateFrustum(height, radius, 0f, 1, quality, matrix);

            if (height < minheight)
            {
                return geometry;
            }
            geometry += GenerateBranch(height - 1, radius - 1, matrix*Matrix4.CreateTranslation(new Vector3()));
            return geometry;
        }

        /// <summary>
        /// Create Frustum, LERP between bottom and top radius
        /// </summary>
        /// <param name="length"></param>
        /// <param name="radiusBottom"></param>
        /// <param name="radiusTop"></param>
        /// <param name="verticalSegments"></param>
        /// <param name="horizontalSegments"></param>
        /// <param name="matrix">Global position and rotation of the frustum</param>
        /// <returns></returns>
        public Mesh CreateFrustum(float length, float radiusBottom, float radiusTop, int verticalSegments, int horizontalSegments, Matrix4 matrix)
        {
            Tri[] tris = new Tri[verticalSegments * horizontalSegments * 2];

            //Vector3 brown = new Vector3(139f / 255f, 69f / 255f, 19f / 255f);

            double theta = 2 * Math.PI / horizontalSegments;

            float segmentLength = (float)length / (float)verticalSegments;

            float rdiff = (radiusBottom - radiusTop) / (float)verticalSegments;

            Vector3 translation = matrix.ExtractTranslation();
            matrix.ClearTranslation();

            //Debug.WriteLine(brown.ToString());
            //Debug.WriteLine(position + Direction);
            //Debug.WriteLine(theta.ToString());
            //Debug.WriteLine(segmentLength);
            //Debug.WriteLine(rdiff);


            for (int i = 0; i < verticalSegments; i++)
            {
                for (int j = 0; j < horizontalSegments; j++)
                {
                    //Debug.WriteLine((float)Math.Sin(theta * j));

                    float r1 = radiusBottom - rdiff * i;
                    float r2 = radiusBottom - rdiff * (i + 1);

                    Vertex vert1 = new Vertex(new Vector3( r1 * (float)Math.Sin(theta * j)      , segmentLength * i      , r1 * (float)Math.Cos(theta * j)       ));
                    Vertex vert2 = new Vertex(new Vector3( r2 * (float)Math.Sin(theta * j)     ,  segmentLength * (i + 1), r2 * (float)Math.Cos(theta * j)       ));
                    Vertex vert3 = new Vertex(new Vector3( r1 * (float)Math.Sin(theta * (j + 1)), segmentLength * i      , r1 * (float)Math.Cos(theta * (j + 1)) ));
                    Vertex vert4 = new Vertex(new Vector3( r2 * (float)Math.Sin(theta * (j + 1)), segmentLength * (i + 1), r2 * (float)Math.Cos(theta * (j + 1)) ));

                    vert1.Position = new Vector3(matrix * new Vector4(vert1.Position)) + translation;
                    vert2.Position = new Vector3(matrix * new Vector4(vert2.Position)) + translation;
                    vert3.Position = new Vector3(matrix * new Vector4(vert3.Position)) + translation;
                    vert4.Position = new Vector3(matrix * new Vector4(vert4.Position)) + translation;

                    //Debug.WriteLine(String.Format("i: {0}, j: {1}", i.ToString(), j.ToString()));
                    //Debug.WriteLine((i * horizontalSegments + j));
                    //Debug.WriteLine((i * horizontalSegments + j) + tris.Length / 2);

                    tris[(i * horizontalSegments + j)] = new Tri(vert1, vert2, vert3);
                    tris[(i * horizontalSegments + j) + tris.Length/2] = new Tri(vert2, vert3, vert4);

                    

                }
            }

            Debug.WriteLine("tris: " + String.Join(", \n", tris.Select(p => p.ToString()).ToArray()));
            Mesh mesh = new Mesh(tris);
            return mesh;
        }
    }
}
