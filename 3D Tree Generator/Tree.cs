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

        public Mesh Mesh { get; set; }

        public Tree()
        {

        }

        public void GenerateTree()
        {
            Mesh = CreateBranch(height, (float) 0.5, 1, 5, new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        }

        public Mesh CreateBranch(float length, float radius, int verticalSegments, int horizontalSegments, Vector3 position, Vector3 Direction)
        {
            Tri[] tris = new Tri[verticalSegments * horizontalSegments * 2];

            Vector3 brown = new Vector3(139f / 255f, 69f / 255f, 19f / 255f);

            Direction.Normalize();
            double theta = 2 * Math.PI / horizontalSegments;
            float segmentLength = length / verticalSegments;

            for (int i = 0; i < verticalSegments; i++)
            {
                for (int j = 0; j < horizontalSegments; j++)
                {
                    Vertex vert1 = new Vertex(position);
                    Vertex vert2 = new Vertex(position);
                    Vertex vert3 = new Vertex(position);
                    Vertex vert4 = new Vertex(position);

                    vert1.Position += new Vector3((float )Math.Sin(theta * j), segmentLength * i, (float) Math.Cos(theta * j)) * Direction;
                    vert2.Position += new Vector3((float)Math.Sin(theta * j), segmentLength * (i + 1), (float)Math.Cos(theta * j)) * Direction;
                    vert3.Position += new Vector3((float)Math.Sin(theta * j + 1), segmentLength * i, (float)Math.Cos(theta * j + 1)) * Direction;
                    vert4.Position += new Vector3((float)Math.Sin(theta * j + 1), segmentLength * (i + 1), (float)Math.Cos(theta * j + 1)) * Direction;

                    vert1.Color = brown;
                    vert2.Color = brown;
                    vert3.Color = brown;
                    vert4.Color = brown;

                    tris[i*j*2] = new Tri(vert1, vert2, vert3);
                    tris[i * j * 2 + 1] = new Tri(vert1, vert3, vert4);

                }
            }
            for (int i = 0; i < tris.Length; i++)
            {
                if (tris[i] != null)
                {
                    Debug.WriteLine("index: " + i.ToString() + " is " + tris[i].ToString());
                }
                else
                {
                    Debug.WriteLine("index: " + i.ToString() + " is null");
                }
            }

            //Debug.WriteLine("tris: " + String.Join(", ", tris.Select(p => p.ToString()).ToArray()));
            Mesh mesh = new Mesh(tris);
            return mesh;
        }
    }
}
