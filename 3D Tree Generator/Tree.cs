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
using NCalc;

namespace _3D_Tree_Generator
{
    [Serializable]
    class Tree : Mesh
    {
        public float Height { get; set; }
        public float TrunkRadius { get; set; }
        public float Noise { get; set; }
        public float MinDist { get; set; }
        public float MaxDist { get; set; }
        public int Seed { get; set; }
        public float MinHeight { get; set; }
        public float MinRadius { get; set; }
        public int Quality { get; set; }
        public int Segments { get; set; }
        public int Depth { get; set; }
        public Expression TrunkFunction { get; set; }
        public Expression BranchFunction { get; set; }

        public Tree(float height, float radius, Matrix4 position)
        {
            Height = height;
            TrunkRadius = radius;
            MinDist = 1f;
            MaxDist = 2f;
            Seed = 100;
            Quality = 8;
            Segments = 20;
            MinHeight = 0.1f;
            MinRadius = 0.1f;
            Depth = 2;
            IsTextured = false;
            TrunkFunction = new Expression("x");
            BranchFunction = new Expression("x");
        }

        public void GenerateTree()
        {
            Random rnd = new Random(Seed);
            Tris = GenerateBranch(
                TrunkRadius,
                Height,
                Segments,
                Quality,
                color: 0.2f,
                topColor: 0.8f,
                topRadius: 0.2f,
                depth: Depth,
                minDist: MinDist,
                maxDist: MaxDist,
                trunkFunction: TrunkFunction,
                branchFunction: BranchFunction,
                rnd: rnd
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
                                                int depth = 1,
                                                float minDist = 1f,
                                                float maxDist = 2f,
                                                float flare = 0f,
                                                float flareEnd = 0f,
                                                Expression trunkFunction = null,
                                                Expression branchFunction = null,
                                                Random rnd = null
                                            )
        {
            if (rnd == null)
            {
                rnd = new Random((int)System.DateTime.Now.ToBinary());
            }
            List<Vertex> verts = new List<Vertex>();
            List<Tri> tris = new List<Tri>();
            //create the bottom ring of vertices
            verts.AddRange(CreateCrossSection(radius, quality, color, 1 + flare).Select(e => e.Transformed(Matrix4.CreateTranslation(0, 0, -radius * (flare) / 2f))));

            float distSinceBranch = 0; //so that branches are ot too close to eachother or the base of the branch
            double Y = rnd.Range(0, 1.5);
            for (float i = 1; i < segments + 1; i++)
            {
                float currentHeight = i.Lerp(0f, segments, 0, height); //Lerp (Linear interpolation -> see extension methods) to get current height
                float currentRadius;
                if (trunkFunction == null) // linear interpolate for radius if no trunk function is defined
                {
                    currentRadius = i.Lerp(0f, segments, radius, topRadius);
                    //get currentRadius
                }
                else //if there is a trunk function, use that
                {
                    currentRadius = i.CustInterp(0, segments, radius, topRadius, trunkFunction);
                }

                Matrix4 matrix = Matrix4.CreateTranslation(0, currentHeight, 0); //define the translation to move the vertices generated to the current slice
                float delta = 0;
                if (flare > 0 && currentHeight <= flareEnd) //if part of the flare, cause flaring
                {
                    delta = currentHeight.InterpRoot(0, flareEnd, flare, 0);
                    matrix = matrix * Matrix4.CreateTranslation(0, 0, -currentRadius * (delta) / 2f);
                }
                float sliceColor = color;
                if (topColor >= 0)
                {
                    sliceColor = i.Lerp(0f, segments, color, topColor); //set color for current slice, if a gradient is wanted
                }


                //generate and add vertices to the vertex array, but apply the previously mentioned tranformations first
                verts.AddRange(CreateCrossSection(currentRadius, quality, sliceColor, delta + 1).Select(e => e.Transformed(matrix)));

                int pos = verts.Count - quality;
                for (int j = 0; j < quality; j++) //this creates the triangles needed to join the previous slice with the current one
                {
                    //Debug.WriteLine("VertsCount: " + verts.Count);
                    //Debug.WriteLine(String.Format("{0}, {1}, {2}", pos + j, pos + (j + 1) % quality, pos + j - quality));
                    //Debug.WriteLine(String.Format("{0}, {1}, {2}", pos + j - quality, pos + (j + 1) % quality, pos - quality + (j + 1) % quality));
                    tris.Add(new Tri(verts[pos + j], verts[pos + (j + 1) % quality], verts[pos + j - quality]));
                    tris.Add(new Tri(verts[pos + j - quality], verts[pos + (j + 1) % quality], verts[pos - quality + (j + 1) % quality]));
                }
                distSinceBranch += height / segments;
                //make the minimum distance be a random number between two decreasing values as you near the end of the branch
                double minimumDistance = rnd.Range(i.Lerp(0, segments, minDist * 2, minDist), i.Lerp(0, segments, maxDist, maxDist/2));
                if (distSinceBranch > minimumDistance && Convert.ToBoolean(depth)) //if we are past the minimum distance since the previous branch, create a new one
                {
                    
                    matrix = Matrix4.CreateRotationY(currentHeight.CustInterp(0, height * (rnd.Range(0.9, 1.1)), 0, Math.PI * rnd.Range(10, 20), x => (float)(x*x))) * Matrix4.CreateRotationX(1) * Matrix4.CreateTranslation(0, currentHeight, 0);
                    Y += rnd.Range(i.Lerp(0, segments, 0.7, 0.2), i.Lerp(0, segments, 1.5, 0.5));
                    Expression function = trunkFunction;
                    if (branchFunction != null) //if there is a branch function specified, use that for branches else use the trunk function
                    {
                        function = branchFunction;
                    }
                    tris.AddRange(GenerateBranch(currentRadius / 2f, i.Lerp(0f, segments, height, 0) * (float)rnd.Range(0.8, 1.2),
                        segments, quality, depth: depth-1, color: 0.2f, topColor: 0.8f, trunkFunction: function, flare: 1f, flareEnd: i.Lerp(0f, segments, height, 0) / 3f).Select(e => e.Transformed(matrix)));
                    distSinceBranch = 0;
                }
            }
            return tris.ToArray();
        }

        /// <summary>
        /// Creates a regular <paramref name="horizontalSegments"/> sided shape
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
                verts[i] = new Vertex(new Vector3(radius * (float)Math.Sin(theta * i), 0, radius * (float)Math.Cos(theta * i) * zScale), Vector3.Zero, new Vector3((float)139 / 160 * color, (float)69 / 160 * color, (float)19 / 160 * color));
                //Debug.WriteLine(verts[i].ToStringFull());
            }

            // Debug.WriteLine("tris: " + String.Join(", \n", tris.Select(p => p.ToString()).ToArray()));
            return verts;
        }
    }
}
