using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace _3D_Tree_Generator
{
    class Object
    {
        private Vector3 position;
        public Vector3 Position {
            get
            {
                return position;
            }
            set
            {
                position = value;
                CalculateModelMatrix();
            }

        }
        private Vector3 rotation;
        public Vector3 Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
                CalculateModelMatrix();
            }

        }
        private Vector3 scale;
        public Vector3 Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
                CalculateModelMatrix();
            }

        }

        public Matrix4 ModelMatrix { get; set; } = Matrix4.Identity;
        public Matrix4 ViewProjectionMatrix { get; set; } = Matrix4.Identity;
        public Matrix4 ModelViewProjectionMatrix { get; set; } = Matrix4.Identity;

        public Mesh DisplayMesh { get; set; }

        public Mesh CollisionMesh { get; set; }

        public Object()
        {
            position = Vector3.Zero;
            rotation = Vector3.Zero;
            scale = Vector3.Zero;
            CalculateModelMatrix();
        }

        public Object(Mesh mesh) : base()
        {
            DisplayMesh = mesh;
        }

        private void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.CreateScale(scale) * Matrix4.CreateRotationX(rotation.X) * Matrix4.CreateRotationY(rotation.Y) * Matrix4.CreateRotationZ(rotation.Z) * Matrix4.CreateTranslation(position);
        }
    }
}
