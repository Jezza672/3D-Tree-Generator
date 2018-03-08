using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using _3D_Tree_Generator.Test_Classes;
using NCalc;

namespace _3D_Tree_Generator.Test_Classes
{
    /// <summary>
    /// Test Tree for debug purposes. Includes Unit Tests.
    /// </summary>
    class TestTree : Tree
    {
        public TestTree()
        {
            Debug.WriteLine("Initialising Test Parameters");
            Height = 10;
            TrunkRadius = 1;
            TopRadius = 0f;
            MinDist = 1f;
            MaxDist = 2f;
            Seed = 0;
            Quality = 8;
            Segments = 20;
            Depth = 2;
            Flare = 1;
            FlareEnd = 10 / 10;
            BendXStrength = 0.2f;
            BendYStrength = 0.2f;
            TrunkFunction = new Expression("x");
            BranchFunction = new Expression("x");
            BendXFunction = new Expression("Sin(x)");
            BendZFunction = new Expression("Cos(x)");
            LeafNum = 50;
            Leaf = null;
            Name = "Tree";
            Debug.WriteLine("Finished Intitalising Test paramaters");
        }

        public void TestConnectCrossSection(int quality = 4)
        {
            List<Tri> tris = new List<Tri>(); //use a temp list to avoid the resource heavy setter of the Tri property
            List<Vertex> verts = CreateCrossSection(1, quality, new Vector3(1, 0, 0)).ToList(); //create the bass layer of  vertices
            verts.AddRange(CreateCrossSection(1, quality, new Vector3(0, 1, 0)).Select(i => i.Transformed(Matrix4.CreateTranslation(0, 1, 0)))); //create the sceond layer, and move them up by 1 unit

            Vertex[] newVerts = CreateCrossSection(1, quality, new Vector3(0, 0, 1)).Select(i => i.Transformed(Matrix4.CreateTranslation(0, 2, 0))).ToArray(); //create the third layer
            tris.AddRange(StitchCrossSection(verts, newVerts)); //add these using the StitchCrossSection function, to show it works
            verts.AddRange(newVerts);

            newVerts = CreateCrossSection(1, quality, new Vector3(1, 0, 1)).Select(i => i.Transformed(Matrix4.CreateTranslation(0, 3, 0))).ToArray();
            tris.AddRange(StitchCrossSection(verts, newVerts)); //again, to show it works given triangles ordered by itself, i.e. in perpetuity.

            Tris = tris.ToArray();//send to the triangles of this mesh
        }
    }
}
