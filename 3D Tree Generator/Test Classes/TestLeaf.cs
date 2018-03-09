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
    class TestLeaf : Leaf
    {
        public TestLeaf(Vector3 position) : base()
        {
            Position = position;
            Children.Add(new TestAxes(position));
        }

        public TestLeaf(TestLeaf leaf, Vector3 normal) : base(leaf, normal)
        {
            Children.Add(new TestAxes());
        }
    }
}
