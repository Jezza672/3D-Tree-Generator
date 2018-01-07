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
            Tris = GenerateTree(TrunkRadius, Height / (float)(TrunkRadius/0.02), rnd, 0).Item2.Tris;
        }

        /// <summary>
        /// returns the vertices of current slice, and the mesh of the slices after this one
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="segmentHeight"></param>
        /// <returns></returns>
        public Tuple<Vertex[], Mesh> GenerateTree(float radius, float segmentHeight, Random rnd, int col)
        {
            List<Vertex> verts = new List<Vertex>();
            verts.AddRange(CreateCrossSection(radius, Quality, col)); //add the current slice

            if (radius < 0.02) //if too small, stop recurtion
            {
                return new Tuple<Vertex[], Mesh>(verts.ToArray(), new Mesh());
            }

            List<Tri> branchTris = new List<Tri>();
            if (rnd.NextDouble() < Branching)
            {
                Mesh branch = GenerateTree(radius / 1.5f, segmentHeight, rnd, col + 1).Item2;
                Matrix4 branchMat = Matrix4.CreateRotationY((float)(rnd.NextDouble() * Math.PI * 2)) * Matrix4.CreateRotationX(1) * Matrix4.CreateRotationY((float) (rnd.NextDouble()));
                branchTris =  branch.Tris.Select(x => x.Transformed(branchMat)).ToList();
            }

            Tuple<Vertex[], Mesh> result = GenerateTree(radius-0.02f, segmentHeight, rnd, col + 1); //get the next bits of the tree.
                  
            Matrix4 matrix = Matrix4.CreateTranslation(new Vector3(0, segmentHeight, 0)) * Matrix4.CreateRotationZ(Beta) * Matrix4.CreateRotationY(0.1f); //translation matrix for bits after
            
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
            mesh.AddTris(branchTris.ToArray());
            return new Tuple<Vertex[], Mesh>(verts.Take(Quality).ToArray(), mesh);           //https://stackoverflow.com/questions/943635/getting-a-sub-array-from-an-existing-array
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
                                                float flareEnd = 0f
                                            )
        {
            List<Vertex> verts = new List<Vertex>();
            List<Tri> tris = new List<Tri>();
            Matrix4 matrix = Matrix4.CreateScale(1, 1, 1 + flare) * Matrix4.CreateTranslation(0, 0, -flare/2f);
            verts.AddRange(CreateCrossSection(radius, quality, color).Select(e => e.Transformed(matrix)));
            float distSinceBranch = 0;
            for (float i = 1; i < segments + 1; i++)
            {
                float currentHeight = i.Map(0f, segments, 0, height);
                matrix = Matrix4.CreateTranslation(0, currentHeight, 0);
                if (flare > 0 && currentHeight <= flareEnd)
                {
                    float delta = ((float)Math.Sqrt(currentHeight)).Map(0, (float)Math.Sqrt(flareEnd), flare, 0);
                    matrix = Matrix4.CreateScale(1, 1, 1 + delta) * Matrix4.CreateTranslation(0, 0, -delta/2) * matrix;
                }
                float sliceColor = color;
                if (topColor >= 0)
                {
                    sliceColor = i.Map(0f, segments, color, topColor);
                }

                verts.AddRange(CreateCrossSection(i.Map(0f, segments, radius, topRadius), quality, sliceColor).Select(e => e.Transformed(matrix)));

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
                    tris.AddRange(GenerateBranch(i.Map(0f, segments, radius, topRadius)/2f, i.Map(0f, segments, height, 0), 
                        segments, quality, branch: false, color: 0.2f, topColor: 0.8f, flare: 1f, flareEnd: 2f).Select(e => e.Transformed(matrix)));
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
        public static Vertex[] CreateCrossSection(float radius, int horizontalSegments, float color)
        {
            Vertex[] verts = new Vertex[horizontalSegments];
            double theta = 2 * Math.PI / horizontalSegments;

            for (int i = 0; i < horizontalSegments; i++)
            {
                verts[i] = new Vertex(new Vector3(radius * (float)Math.Sin(theta * i), 0, radius * (float)Math.Cos(theta * i)), Vector3.Zero, new Vector3(color));
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

    public static class ExtensionMethods //https://stackoverflow.com/questions/14353485/how-do-i-map-numbers-in-c-sharp-like-with-map-in-arduino
    {
        public static float Map(this float value, float fromSource, float toSource, float fromTarget, float toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }
    }
}
