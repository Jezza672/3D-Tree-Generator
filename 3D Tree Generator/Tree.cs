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
    [Serializable]
    class Tree : Mesh
    {
        public float Height {get; set;}
        public float TrunkRadius { get; set; }
        public float Noise{get; set;}
        public float Branching {get; set;}
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
            Branching = 0.1f;
            Seed = 100;
            Quality = 3;
            MinHeight = 0.1f;
            MinRadius = 0.1f;
            Alpha = 0.1f;
            Beta = 0.05f;
            IsTextured = false;
        }

        public void GenerateTree()
        {
            Random rnd = new Random(Seed);
            Tris = GenerateBranch(
                TrunkRadius,
                Height,
                100,
                Quality,
                color: 0.2f,
                topColor: 0.8f,
                topRadius: 0.2f,
                minDist: Branching     
                );
        }

        /// <summary>
        /// Recursive function for creating a branch.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="height"></param>
        /// <param name="segments"></param>
        /// <param name="quality"></param>
        /// <param name="color">optional</param>
        /// <param name="topRadius">optional</param>
        /// <param name="topColor">optional</param>
        /// <returns></returns>
        public static Tri[] GenerateBranch(float radius, float height, int segments, int quality, 
                                                float color = 0.5f, 
                                                float topRadius = 0f, 
                                                float topColor = -1f, 
                                                bool branch = true,
                                                float minDist = 1f,
                                                float flare = 0f,
                                                float flareEnd = 0f,
                                                Func<double, float> shapeFunction = null,
                                                Func<double, float> inverseFunction = null
                                            
                                            )
        {
            List<Vertex> verts = new List<Vertex>();
            List<Tri> tris = new List<Tri>();
            //create the bottom ring of vertices
            verts.AddRange(CreateCrossSection(radius, quality, color, 1 + flare).Select(e => e.Transformed(Matrix4.CreateTranslation(0,0,-radius * (flare)/2f))));

            float distSinceBranch = 0; //so that branches are ot too close to eachother or the base of the branch

            for (float i = 1; i < segments + 1; i++)
            {
                float currentHeight = i.Lerp(0f, segments, 0, height); //Lerp (Linear interpolation -> see extension methods) to get current height
                float currentRadius;
                if (shapeFunction == null)
                {
                     currentRadius = i.Lerp(0f, segments, radius, topRadius);
                    //get currentRadius
                }
                else if (inverseFunction == null)
                {
                    currentRadius = i.InterpCust(0, segments, radius, topRadius, shapeFunction);
                }
                else
                {
                    currentRadius = i.CustInterp(0, segments, radius, topRadius, shapeFunction, inverseFunction);
                }
                
                Matrix4 matrix = Matrix4.CreateTranslation(0, currentHeight, 0);
                float delta = 0;
                if (flare > 0 && currentHeight <= flareEnd)
                {
                    delta = currentHeight.InterpRoot(0, flareEnd, flare, 0);
                    matrix = matrix * Matrix4.CreateTranslation(0,0, -currentRadius * (delta) / 2f);
                }
                float sliceColor = color;
                if (topColor >= 0)
                {
                    sliceColor = i.Lerp(0f, segments, color, topColor);
                }

                verts.AddRange(CreateCrossSection(currentRadius, quality, sliceColor, delta + 1).Select(e => e.Transformed(matrix)));

                int pos = verts.Count - quality;
                for (int j = 0; j < quality; j++)
                {
                    //Debug.WriteLine("VertsCount: " + verts.Count);
                    //Debug.WriteLine(String.Format("{0}, {1}, {2}", pos + j, pos + (j + 1) % quality, pos + j - quality));
                    //Debug.WriteLine(String.Format("{0}, {1}, {2}", pos + j - quality, pos + (j + 1) % quality, pos - quality + (j + 1) % quality));
                    tris.Add(new Tri(verts[pos + j], verts[pos + (j + 1) % quality], verts[pos + j - quality]));
                    tris.Add(new Tri(verts[pos + j - quality], verts[pos + (j + 1) % quality], verts[pos - quality + (j + 1) % quality]));
                }
                distSinceBranch += height / segments;
                if (distSinceBranch > minDist && branch)
                {
                    matrix = Matrix4.CreateRotationY(i) * Matrix4.CreateRotationX(1) * Matrix4.CreateTranslation(0, currentHeight, 0) ;
                    tris.AddRange(GenerateBranch(currentRadius/2f, i.Lerp(0f, segments, height, 0), 
                        segments, quality, branch: false, color: 0.2f, topColor: 0.8f, shapeFunction: shapeFunction, flare: 1f, flareEnd: i.Lerp(0f, segments, height, 0)/3f).Select(e => e.Transformed(matrix)));
                    distSinceBranch = 0;
                }
            }
            return tris.ToArray();


        }

        /// <summary>
        /// Created a regular <paramref name="horizontalSegments"/> sided shape
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="horizontalSegments"></param>
        /// <param name="num"></param>
        /// <returns>array ov vertices</returns>
        public static Vertex[] CreateCrossSection(float radius, int horizontalSegments, float color, float zScale = 1)
        {
            Vertex[] verts = new Vertex[horizontalSegments];
            double theta = 2 * Math.PI / horizontalSegments;

            for (int i = 0; i < horizontalSegments; i++)
            {
                verts[i] = new Vertex(new Vector3(radius * (float)Math.Sin(theta * i), 0, radius * (float)Math.Cos(theta * i) * zScale), Vector3.Zero, new Vector3(color));
                //Debug.WriteLine(verts[i].ToStringFull());
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
