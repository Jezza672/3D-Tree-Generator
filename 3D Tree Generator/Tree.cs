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
        public float TopRadius { get; set; }
        public float Noise { get; set; }
        public float MinDist { get; set; }
        public float MaxDist { get; set; }
        public int Seed { get; set; }
        public int Quality { get; set; }
        public int Segments { get; set; }
        public int Depth { get; set; }
        public float Flare { get; set; }
        public float FlareEnd { get; set; }
        public float BendXStrength { get; set; }
        public float BendYStrength { get; set; }
        public Expression TrunkFunction { get; set; }
        public Expression BranchFunction { get; set; }
        public Expression BendXFunction { get; set; }
        public Expression BendZFunction { get; set; }
        public int LeafNum { get; set; }
        public Leaf Leaf { get; set; }

        public Tree()
        {

        }

        public Tree(float height, float radius, Matrix4 position)
        {
            Height = height;
            TrunkRadius = radius;
            TopRadius = 0f;
            MinDist = 1f;
            MaxDist = 2f;
            Seed = 0;
            Quality = 8;
            Segments = 20;
            Depth = 2;
            Flare = 1;
            FlareEnd = height / 10;
            BendXStrength = 0f;
            BendYStrength = 0f;
            TrunkFunction = new Expression("x");
            BranchFunction = new Expression("x");
            BendXFunction = new Expression("0");
            BendZFunction = new Expression("0");
            LeafNum = 50;
            Leaf = null;
            Name = "Tree";
        }

        public void GenerateTree()
        {
            Random random = new Random(Seed);
            Tris = GenerateBranch(
                TrunkRadius,
                Height,
                Segments,
                Quality,
                darkness: 0.2f,
                topDarkness: 0.8f,
                topRadius: TopRadius,
                depth: Depth,
                minDist: MinDist,
                maxDist: MaxDist,
                flare: Flare,
                flareEnd: FlareEnd,
                doFlare: false,
                xBendStrength: BendXStrength,
                zBendStrength: BendYStrength,
                trunkFunction: TrunkFunction,
                branchFunction: BranchFunction,
                xBendFunction: BendXFunction,
                zBendFunction: BendZFunction,
                rnd: random
                );
            if (Leaf != null)
            {
                AddLeaves(Leaf, random);
            }
        }

        /// <summary>
        /// Recursive function for creating a branch.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="height"></param>
        /// <param name="segments"></param>
        /// <param name="quality"></param>
        /// <param name="darkness">optional</param>
        /// <param name="topRadius">optional</param>
        /// <param name="topDarkness">optional</param>
        /// <returns></returns>
        public static Tri[] GenerateBranch(float radius, float height, int segments, int quality,
                                                float darkness = 0.5f,
                                                float topRadius = 0f,
                                                float branchTopRadiusFactor = 0.4f,
                                                float topDarkness = -1f,
                                                int depth = 0,
                                                float minDist = 1f,
                                                float maxDist = 2f,
                                                float flare = 0f,
                                                float flareEnd = 0f,
                                                bool doFlare = false,
                                                float xBendStrength = 0f,
                                                float zBendStrength = 0f,
                                                Expression trunkFunction = null,
                                                Expression branchFunction = null,
                                                Expression xBendFunction = null,
                                                Expression zBendFunction = null,
                                                Random rnd = null
                                              
                                            )
        {
            rnd = (rnd == null) ? new Random() : rnd;
            xBendFunction = (xBendFunction == null) ? new Expression("0") : xBendFunction;
            zBendFunction = (zBendFunction == null) ? new Expression("0") : zBendFunction;

            List<Vertex> verts = new List<Vertex>();
            List<Tri> tris = new List<Tri>();

            //create the bottom ring of vertices
            flare = !doFlare ? 0 : flare; //if doFlare is false, set the flare to 0, i.e. none
            //create a cross section and transform it according to the amount of flare
            verts.AddRange(CreateCrossSection(radius, quality, darkness, 1 + flare).Select(e => e.Transformed(Matrix4.CreateTranslation(0, 0, -radius * (flare) / 2f))));

            float distSinceBranch = 0; //so that branches are not too close to eachother or the base of the branch
            double Y = rnd.Range(0, 1.5);
            for (float i = 1; i < segments + 1; i++)
            {
                float currentHeight = i.Lerp(0, segments, 0, height); //Lerp (Linear interpolation -> see extension methods) to get current height
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

                float xOffset = xBendFunction.Eval(i) * xBendStrength;
                float zOffset = zBendFunction.Eval(i) * zBendStrength;

                //Debug.WriteLine(xOffset);
                //Debug.WriteLine(zOffset);

                Matrix4 matrix = Matrix4.CreateTranslation(xOffset, currentHeight, zOffset); //define the translation to move the vertices generated to the correct height
                float delta = 0;
                if (flare > 0 && currentHeight <= flareEnd) //cause flaring if less than the flareEnd amount
                {
                    delta = currentHeight.InterpRoot(0, flareEnd, flare, 0);
                    matrix = matrix * Matrix4.CreateTranslation(0, 0, -currentRadius * (delta) / 2f);
                }

                //if a colour gradient is wanted, create one, otherwise set to the default colour.
                float sliceColor = (topDarkness >= 0) ? i.Lerp(0f, segments, darkness, topDarkness) : darkness;

                //generate and add vertices to the vertex array, but apply the previously mentioned tranformations first
                Vertex[] newVerts = CreateCrossSection(currentRadius, quality, sliceColor, delta + 1).Select(e => e.Transformed(matrix)).ToArray();
                tris.AddRange(StitchCrossSection(verts, newVerts));
                verts.AddRange(newVerts);

                distSinceBranch += height / segments;

                //make the minimum distance be a random number between two decreasing values as you near the end of the branch
                double minimumDistance = rnd.Range(i.Lerp(0, segments, minDist * 2, minDist), i.Lerp(0, segments, maxDist, maxDist/2));
                if (distSinceBranch > minimumDistance && depth != 0) //if we are past the minimum distance since the previous branch, create a new one
                {            
                    matrix = Matrix4.CreateRotationY(currentHeight.CustInterp(0, height * (rnd.Range(0.9, 1.1)), 0, Math.PI * rnd.Range(10, 20), x => (float)(x*x))) * Matrix4.CreateRotationX(1) * Matrix4.CreateTranslation(0, currentHeight, 0);
                    Y += rnd.Range(i.Lerp(0, segments, 0.7, 0.2), i.Lerp(0, segments, 1.5, 0.5));

                    //if a seperate branch function is specified, use it, else use the trunk function
                    Expression function = (branchFunction != null) ? branchFunction : trunkFunction;

                    tris.AddRange(GenerateBranch(
                        currentRadius / 2f,
                        i.Lerp(0f, segments, height, 0) * (float)rnd.Range(0.8, 1.2),
                        segments,
                        quality,
                        depth: depth - 1,
                        darkness: 0.2f,
                        topRadius: branchTopRadiusFactor * (currentRadius / 2f),
                        branchTopRadiusFactor: branchTopRadiusFactor,
                        topDarkness: 0.8f,
                        trunkFunction: function,
                        flare: i.Lerp(0f, segments, flare, 0),
                        flareEnd: i.Lerp(0f, segments, flareEnd, 0) / 3f,
                        doFlare: true,
                        rnd: rnd,
                        xBendFunction: xBendFunction,
                        zBendFunction: zBendFunction,
                        xBendStrength : i.Lerp(0f, segments, xBendStrength, 0),
                        zBendStrength: i.Lerp(0f, segments, zBendStrength, 0)
                        ).Select(e => e.Transformed(matrix)));
                    distSinceBranch = 0;
                }
            }
            return tris.ToArray();
        }

        /// <summary>
        /// Create the geometry between the current triangles and the next layer of vertices
        /// </summary>
        /// <param name="tris">must be ordered correctly</param>
        /// <param name="newVerts"></param>
        /// <returns></returns>
        public static List<Tri> StitchCrossSection(List<Vertex> verts   , Vertex[] newVerts)
        {
            int offset =  verts.Count - newVerts.Length; //to access the last few vertices added to the list of vertices
            List<Tri> newTris = new List<Tri>(); 
            for (int i = 0; i < newVerts.Length; i++) //create the geometry
            {
                //Debug.WriteLine((end + 2 * i).ToString() + " " + (end + 2 * i).ToString() + " " + i.ToString());
                newTris.Add(new Tri(verts[offset + (i + 1) % newVerts.Length], verts[offset + i], newVerts[i]));
                //Debug.WriteLine((end + 2 * i).ToString() + " " + ((i + 1) % verts.Length).ToString() + " " + i.ToString());
                newTris.Add(new Tri(verts[offset + (i + 1) % newVerts.Length], newVerts[(i + 1) % newVerts.Length], newVerts[i]));
            }
            return newTris;
        }

        public void AddLeaves(Leaf leaf, Random rnd)
        {
            Children = new List<Mesh>();
            foreach (Tri tri in Tris)
            {
                if (rnd.NextDouble() > 0.99)
                {
                    Leaf temp = new Leaf(leaf);
                    temp.Position = tri.Item1.Position;
                    Children.Add(temp);
                }
            }
        }

        /// <summary>
        /// Creates a regular <paramref name="horizontalSegments"/> sided shape
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="horizontalSegments"></param>
        /// <param name="num"></param>
        /// <returns>array ov vertices</returns>
        public static Vertex[] CreateCrossSection(float radius, int horizontalSegments, Vector3 color, float zScale = 1)
        {
            Vertex[] verts = new Vertex[horizontalSegments];
            double theta = 2 * Math.PI / horizontalSegments;

            for (int i = 0; i < horizontalSegments; i++)
            {
                verts[i] = new Vertex(new Vector3(radius * (float)Math.Sin(theta * i), 0, radius * (float)Math.Cos(theta * i) * zScale), Vector3.Zero, color);
                //Debug.WriteLine(verts[i].ToStringFull());
            }

            // Debug.WriteLine("tris: " + String.Join(", \n", tris.Select(p => p.ToString()).ToArray()));
            return verts;
        }
        public static Vertex[] CreateCrossSection(float radius, int horizontalSegments, float color = 0.5f, float zScale = 1)
        {
            return CreateCrossSection(radius, horizontalSegments, new Vector3((float)139 / 160 * color, (float)69 / 160 * color, (float)19 / 160 * color), zScale);
        }
    }
}
