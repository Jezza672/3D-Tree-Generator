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

namespace _3D_Tree_Generator
{
    class Mesh
    {
        private Vector3 position;
        public Vector3 Position
        {
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
        private Matrix4 modelViewProjectionMatrix;
        public Matrix4 ModelViewProjectionMatrix
        {
            get
            {
                return modelViewProjectionMatrix;
            }
        }

        public Vector3[] Vertices { get; set; }
        public int[] Indices { get; set; }
        public Vector3[] Normals { get; set; }

        private Face[] faces;
        public Face[] Faces
        {
            get
            {
                return faces;
            }
            set
            {
                faces = value;
                Vector3[] normals = new Vector3[faces.Length * 3];
                for (int i = 0; i < faces.Length; i++)
                {
                    normals[i * 3] = faces[i].Item1.Normal;
                    normals[i * 3 + 1] = faces[i].Item2.Normal;
                    normals[i * 3 + 2] = faces[i].Item3.Normal;
                }

                Normals = normals;
                int[] indices = new int[faces.Length * 3];
                for (int i = 0; i < faces.Length * 3; i++)
                {
                    indices[i] = i;
                }
                Indices = indices;

                Vector3[] vertices = new Vector3[faces.Length * 3];
                for (int i = 0; i < faces.Length; i++)
                {
                    vertices[i * 3] = faces[i].Item1.Position;
                    vertices[i * 3 + 1] = faces[i].Item2.Position;
                    vertices[i * 3 + 2] = faces[i].Item3.Position;
                }
                Vertices = vertices;
            }
        }



        /// <summary>
        /// Empty Mesh. Properties not initialised.
        /// </summary>
        public Mesh()
        {
            position = Vector3.Zero;
            rotation = Vector3.Zero;
            scale = Vector3.Zero;
            CalculateModelMatrix();
        }

        public Mesh(Face[] newFaces) : this()
        {
            Faces = newFaces;
        }

        /// <summary>
        /// Create Mesh. Normals will be calculated.
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="indices"></param>
        public Mesh(Vector3[] vertices, int[] indices) : this(vertices, indices, Mesh.CalculateNormals(vertices, indices))
        {

        }

        /// <summary>
        /// Create Mesh with normals.
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="indices"></param>
        /// <param name="normals"></param>
        public Mesh(Vector3[] vertices, int[] indices, Vector3[] normals) : this()
        {
            List<Face> newFaces = new List<Face>();
            for (int i = 0; i < indices.Length; i += 3)
            {
                Face face = new Face();
                face.Item1 = new Vertex(vertices[indices[i]], normals[indices[i]]);
                face.Item2 = new Vertex(vertices[indices[i + 1]], normals[indices[i + 1]]);
                face.Item3 = new Vertex(vertices[indices[i + 2]], normals[indices[i + 2]]);
                newFaces.Add(face);
            }
            Faces = newFaces.ToArray();
        }

        
        /// <summary>
        /// Generate Mesh from .obj compliant strings. Textures will not be loaded.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Mesh MeshFromString(string str)
        {
            
            List<Vector3> verts = new List<Vector3>();
            List<Vector3> norms = new List<Vector3>();
            List<Vector2> texs = new List<Vector2>();
            List<Tuple<int, int, int>> inds = new List<Tuple<int, int, int>>();

            string[] lines = Regex.Split(str, "/r/n");
            foreach (string line in lines)
            {
                string[] parts = line.Split(' ');
                switch (parts[0])
                {
                    case "v":
                        verts.Add(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                        break;
                    case "f":
                        for (int i = 1; i < parts.Length - 2; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                string[] bits = parts[i + j].Split('/');
                                inds.Add(new Tuple<int, int, int>(int.Parse(bits[0]), int.Parse(bits[1]), int.Parse(bits[2])));
                            }
                        }
                        break;
                    case "vn":
                        norms.Add(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                        break;
                    default:
                        Console.WriteLine("unrecognised character: '{0}'", parts[0]);
                        break;
                }
            }

            Vector3[] orderedNorms = new Vector3[verts.Count];
            Vector2[] orderedTexs = new Vector2[verts.Count];

            for (int i = 0; i < verts.Count; i++)
            {
                orderedNorms[inds[i].Item1] = norms[inds[i].Item2];
                orderedTexs[inds[i].Item1] = texs[inds[i].Item3];
            }
            int[] vertOnlyInds = new int[inds.Count];

            for (int i = 0; i < inds.Count; i++)
            {
                vertOnlyInds[i] = inds[i].Item1;
            }

            Mesh mesh = new Mesh(verts.ToArray(), vertOnlyInds, orderedNorms);
            
            return mesh;
        }
        

        /// <summary>
        /// Generate Mesh from .obj files
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Mesh MeshFromFile(String filename)
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                return MeshFromString(sr.ReadToEnd());
            }
        }

        /// <summary>
        /// Calculate the normals of a mesh. Mesh must be in Tris form
        /// </summary>
        /// <param name="Verts"></param>
        /// <param name="Inds">Tris only, no N-Gons</param>
        /// <returns></returns>
        private static Vector3[] CalculateNormals(Vector3[] Verts, int[] Inds)
        {
            Vector3[] normals = new Vector3[Verts.Length];

            // Compute normals for each face
            for (int i = 0; i < Inds.Length; i += 3)
            {
                Vector3 v1 = Verts[Inds[i]];
                Vector3 v2 = Verts[Inds[i + 1]];
                Vector3 v3 = Verts[Inds[i + 2]];

                // The normal is the cross-product of two sides of the triangle
                normals[Inds[i]] += Vector3.Cross(v2 - v1, v3 - v1);
                normals[Inds[i + 1]] += Vector3.Cross(v2 - v1, v3 - v1);
                normals[Inds[i + 2]] += Vector3.Cross(v2 - v1, v3 - v1);
            }

            return normals;
        }

        private void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.CreateScale(scale) * Matrix4.CreateRotationX(rotation.X) * Matrix4.CreateRotationY(rotation.Y) * Matrix4.CreateRotationZ(rotation.Z) * Matrix4.CreateTranslation(position);
        }

        public void CalculateModelViewProjectionMatrix(Matrix4 ViewProjectionMatrix)
        {
            modelViewProjectionMatrix = ModelMatrix * ViewProjectionMatrix;
        }
    }
}
