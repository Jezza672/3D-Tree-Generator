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
    class Tree : Mesh
    {
        public float Height {get; set;}
        public float TrunkRadius { get; set; }
        public float Noise{get; set;}
        public int Branching {get; set;}
        public int Seed { get; set; }
        public float MinHeight { get; set; }
        public float MinRadius { get; set; }
        public int Quality { get; set; }
        public float Alpha { get; set; }
        public float Beta { get; set; }

        public Tree(float height, float radius, Matrix4 position)
        {
            Height = height;
            TrunkRadius = radius;
            Noise = 0;
            Branching = 1;
            Seed = 100;
            Quality = 3;
            MinHeight = 0.1f;
            MinRadius = 0.1f;
            Alpha = 0.1f;
            Beta = 0.05f;
        }

        /// <summary>
        /// unfinished
        /// </summary>
        /// <param name="height"></param>
        /// <param name="radius"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public Mesh GenerateBranch(float height, float radius, Matrix4 matrix, int depth)
        {
            float Smallradius = radius - 0.2f;
            if (Smallradius < MinRadius){
                Smallradius = MinRadius;
            }
            Mesh geometry = CreateFrustum(height, radius, Smallradius, 1, Quality).Transform(matrix);

            if (height < MinHeight) //if the branch is too short, stop generating more extentions
            {
                return geometry;
            }

            Matrix4 newMatrix = matrix * Matrix4.CreateTranslation(new Vector3(0, height, 0)) * Matrix4.CreateRotationZ(Alpha) * matrix.Inverted();
            geometry += GenerateBranch(height - 0.2f, Smallradius, newMatrix, depth + 1);
            //if (depth > 2) // make a branch
            //{
            //    newMatrix = matrix * Matrix4.CreateTranslation(new Vector3(0, height, 0)) * Matrix4.CreateRotationY(2f) * Matrix4.CreateRotationX(Alpha * 10f);
            //    geometry += GenerateBranch(height - 0.2f, Smallradius, newMatrix, 0).Transform(newMatrix);
            //}
            return geometry;
        }

        public void GenerateTree()
        {
            Tris = GenerateTree(TrunkRadius, Height / (float)(TrunkRadius/0.02)).Item2.Tris;
        }

        /// <summary>
        /// returns the vertices of current slice, and the mesh of the slices after this one
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="segmentHeight"></param>
        /// <returns></returns>
        public Tuple<Vertex[], Mesh> GenerateTree(float radius, float segmentHeight)
        {
            List<Vertex> verts = new List<Vertex>();
            verts.AddRange(CreateCrossSection(radius, Quality)); //add the current slice

            if (radius < 0.02) //if too small, stop recurtion
            {
                return new Tuple<Vertex[], Mesh>(verts.ToArray(), new Mesh());
            }

            Tuple<Vertex[], Mesh> result = GenerateTree(radius-0.02f, segmentHeight); //get the next bits of the tree.
                  
            Matrix4 matrix = Matrix4.CreateTranslation(new Vector3(0, segmentHeight, 0)) * Matrix4.CreateRotationZ(Beta); //translation matrix for bits after
            
            verts.AddRange(result.Item1.Select(i => i.Transformed(matrix))); //add the new vertices

            Tri[] tris = new Tri[Quality * 2]; //creates the new triangles
            for (int i = 0; i < Quality; i++)
            {
                tris[i] = new Tri(verts[i], verts[(i + 1) % Quality], verts[i + Quality]);
                tris[i + Quality] = new Tri(verts[(i + 1) % Quality], verts[i + Quality], verts[((i + 1) % Quality) + Quality]);
            }

            Mesh mesh = result.Item2;
            mesh = mesh.Transform(matrix);
            mesh.AddTris(tris);
            return new Tuple<Vertex[], Mesh>(verts.Take(Quality).ToArray(), mesh);           //https://stackoverflow.com/questions/943635/getting-a-sub-array-from-an-existing-array
        }

        public static Vertex[] CreateCrossSection(float radius, int horizontalSegments)
        {
            Vertex[] verts = new Vertex[horizontalSegments];
            double theta = 2 * Math.PI / horizontalSegments;

            for (int i = 0; i < horizontalSegments; i++)
            {
                verts[i] = new Vertex(new Vector3(radius * (float)Math.Sin(theta * i), 0, radius * (float)Math.Cos(theta * i)));
            }

            // Debug.WriteLine("tris: " + String.Join(", \n", tris.Select(p => p.ToString()).ToArray()));
            return verts;
        }

        /// <summary>
        /// Depreciated
        /// </summary>
        /// <param name="length"></param>
        /// <param name="radiusBottom"></param>
        /// <param name="radiusTop"></param>
        /// <param name="verticalSegments"></param>
        /// <param name="horizontalSegments"></param>
        /// <param name="matrix">Global position and rotation of the frustum</param>
        /// <returns></returns>
        public static Mesh CreateFrustum(float length, float radiusBottom, float radiusTop, int verticalSegments, int horizontalSegments)
        {
            Tri[] tris = new Tri[verticalSegments * horizontalSegments * 2];

            //Vector3 brown = new Vector3(139f / 255f, 69f / 255f, 19f / 255f);

            double theta = 2 * Math.PI / horizontalSegments;

            float segmentLength = (float)length / (float)verticalSegments;

            float rdiff = (radiusBottom - radiusTop) / (float)verticalSegments;

            //Vector3 translation = matrix.ExtractTranslation();
            //matrix.ClearTranslation();

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

                    Vertex vert1 = new Vertex(new Vector3(r1 * (float)Math.Sin(theta * j), segmentLength * i, r1 * (float)Math.Cos(theta * j)));
                    Vertex vert2 = new Vertex(new Vector3(r2 * (float)Math.Sin(theta * j), segmentLength * (i + 1), r2 * (float)Math.Cos(theta * j)));
                    Vertex vert3 = new Vertex(new Vector3(r1 * (float)Math.Sin(theta * (j + 1)), segmentLength * i, r1 * (float)Math.Cos(theta * (j + 1))));
                    Vertex vert4 = new Vertex(new Vector3(r2 * (float)Math.Sin(theta * (j + 1)), segmentLength * (i + 1), r2 * (float)Math.Cos(theta * (j + 1))));

                    //vert1.Position = new Vector3(matrix * new Vector4(vert1.Position, 1));// + translation;
                    //vert2.Position = new Vector3(matrix * new Vector4(vert2.Position, 1));// + translation;
                    //vert3.Position = new Vector3(matrix * new Vector4(vert3.Position, 1));// + translation;
                    //vert4.Position = new Vector3(matrix * new Vector4(vert4.Position, 1));// + translation;

                    //Debug.WriteLine(String.Format("i: {0}, j: {1}", i.ToString(), j.ToString()));
                    //Debug.WriteLine((i * horizontalSegments + j));
                    //Debug.WriteLine((i * horizontalSegments + j) + tris.Length / 2);

                    tris[(i * horizontalSegments + j)] = new Tri(vert1, vert2, vert3);
                    tris[(i * horizontalSegments + j) + tris.Length / 2] = new Tri(vert2, vert3, vert4);



                }
            }

            // Debug.WriteLine("tris: " + String.Join(", \n", tris.Select(p => p.ToString()).ToArray()));
            Mesh mesh = new Mesh(tris);
            return mesh;
        }
    }
}
