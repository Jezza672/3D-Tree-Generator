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

        public Matrix4 ModelMatrix { get; set; }
        public Matrix4 ModelViewProjectionMatrix;

        public Vector3[] Vertices { get; set; }
        public int[] Indices { get; set; }
        public Vector3[] Normals { get; set; }
        public Vector2[] TexCoords { get; set; }
        public Texture Texture { get; set; }

        public bool IsTextured = false;

        private Vector3[] colors;
        public Vector3[] Colors
        {
            get
            {
                if (colors.Length < Indices.Length)
                {
                    return Enumerable.Repeat(new Vector3(1,0,0), Indices.Length).ToArray(); //https://stackoverflow.com/questions/3363940/fill-listint-with-default-values
                }
                else
                {
                    return colors;
                }
            }
            set
            {
                colors = value;
            }
        }

        private Tri[] tris;
        public Tri[] Tris
        {
            get
            {
                return tris;
            }
            set
            {
                tris = value;
                Vector3[] normals = new Vector3[tris.Length * 3];
                for (int i = 0; i < tris.Length; i++)
                {
                    //Debug.WriteLine(tris[i].Item1);
                    normals[i * 3] = tris[i].Item1.Normal;
                    normals[i * 3 + 1] = tris[i].Item2.Normal;
                    normals[i * 3 + 2] = tris[i].Item3.Normal;
                }
                Normals = normals;

                int[] indices = Enumerable.Range(0, Tris.Length * 3).ToArray();
                Indices = indices;

                Vector3[] vertices = new Vector3[tris.Length * 3];
                for (int i = 0; i < tris.Length; i++)
                {
                    vertices[i * 3] = tris[i].Item1.Position;
                    vertices[i * 3 + 1] = tris[i].Item2.Position;
                    vertices[i * 3 + 2] = tris[i].Item3.Position;
                }
                Vertices = vertices;

                Vector3[] colours = new Vector3[tris.Length * 3];
                for (int i = 0; i < tris.Length; i++)
                {
                    colours[i * 3] = tris[i].Item1.Color;
                    colours[i * 3 + 1] = tris[i].Item2.Color;
                    colours[i * 3 + 2] = tris[i].Item3.Color;
                }
                Colors = colours;

                Vector2[] texcoords = new Vector2[tris.Length * 3];
                for (int i = 0; i < tris.Length; i++)
                {
                    texcoords[i * 3] = tris[i].Item1.TextureCoord;
                    texcoords[i * 3 + 1] = tris[i].Item2.TextureCoord;
                    texcoords[i * 3 + 2] = tris[i].Item3.TextureCoord;
                }
                TexCoords = texcoords;
            }
        }

        public string Name;

        /// <summary>
        /// Empty Mesh. Properties not initialised.
        /// </summary>
        public Mesh()
        {
            position = Vector3.Zero;
            rotation = Vector3.Zero;
            scale = Vector3.One;
            CalculateModelMatrix();
            ModelMatrix = Matrix4.Identity;
            ModelViewProjectionMatrix = Matrix4.Identity;
            tris = new Tri[] { new Tri()};
        }

        public Mesh(Tri[] newFaces) : this()
        {
            Tris = newFaces;
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
            List<Tri> newFaces = new List<Tri>();
            if (normals.Length < vertices.Length)
            {
                List<Vector3> temp = new List<Vector3>(normals);
                temp.AddRange(new Vector3[vertices.Length-normals.Length]);
                normals = temp.ToArray();
            }
            for (int i = 0; i < indices.Length - 2; i += 3)
            {
                //Debug.WriteLine("");
                //Debug.WriteLine(i.ToString());
                //Debug.WriteLine(vertices.Length.ToString());
                //Debug.WriteLine(normals.Length.ToString());
                //Debug.WriteLine(indices[i].ToString());
                //Debug.WriteLine(indices[i + 1].ToString());
                //Debug.WriteLine(indices[i + 2].ToString());
                Tri face = new Tri();
                face.Item1 = new Vertex(vertices[indices[i]], normals[indices[i]]);
                face.Item2 = new Vertex(vertices[indices[i + 1]], normals[indices[i + 1]]);
                face.Item3 = new Vertex(vertices[indices[i + 2]], normals[indices[i + 2]]);
                newFaces.Add(face);
            }
            Tris = newFaces.ToArray();
        }

        public Mesh(Vector3[] vertices, int[] indices, Vector3[] normals, Vector2[] texs)
        {
            IsTextured = true;

            List<Tri> newFaces = new List<Tri>();
            if (normals.Length < vertices.Length)
            {
                List<Vector3> temp = new List<Vector3>(normals);
                temp.AddRange(new Vector3[vertices.Length - normals.Length]);
                normals = temp.ToArray();
            }
            for (int i = 0; i < indices.Length - 2; i += 3)
            {
                //Debug.WriteLine("");
                //Debug.WriteLine(i.ToString());
                //Debug.WriteLine(vertices.Length.ToString());
                //Debug.WriteLine(normals.Length.ToString());
                //Debug.WriteLine(indices[i].ToString());
                //Debug.WriteLine(indices[i + 1].ToString());
                //Debug.WriteLine(indices[i + 2].ToString());
                Tri face = new Tri();
                face.Item1 = new Vertex(vertices[indices[i]], normals[indices[i]], texs[indices[i]]);
                face.Item2 = new Vertex(vertices[indices[i + 1]], normals[indices[i + 1]], texs[indices[i + 1]]);
                face.Item3 = new Vertex(vertices[indices[i + 2]], normals[indices[i + 2]], texs[indices[i + 2]]);
                newFaces.Add(face);
            }
            Tris = newFaces.ToArray();

        }


        /// <summary>
        /// Import .obj into mesh format
        /// </summary>
        /// <param name="filename"></param>
        public Mesh(string filename)
        {
            var sr = new StreamReader(filename);
            string str = sr.ReadToEnd();
            //Debug.WriteLine(str);
            List<Vector3> verts = new List<Vector3>();
            List<Vector3> norms = new List<Vector3>();
            List<Vector2> texs = new List<Vector2>();
            List<Tuple<int, int, int>> inds = new List<Tuple<int, int, int>>();
            string name = "";

            string[] lines = str.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None); //https://stackoverflow.com/questions/1547476/easiest-way-to-split-a-string-on-newlines-in-net
            foreach (string line in lines)
            {
                Debug.WriteLine(line);
                List<string> parts = new List<String>(line.Split(' '));
                if (((parts[parts.Count - 1] == " ") || (parts[parts.Count - 1] == "\r") || (parts[parts.Count - 1] == "\n") || (parts[parts.Count - 1] == "\r\n") || (parts[parts.Count - 1] == "")) && (parts.Count != 1))
                {
                    Debug.WriteLine("Removed: '" + parts[parts.Count - 1].ToString() + "'");
                    parts.RemoveAt(parts.Count - 1); //https://stackoverflow.com/questions/23245569/how-to-remove-the-last-element-added-into-the-list
                }
                switch (parts[0])
                {
                    case "v":
                        Debug.WriteLine("vertex: " + parts[1].ToString() + parts[2].ToString() + parts[3].ToString());
                        verts.Add(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                        break;
                    case "f":
                        Debug.Write("Tri: ");
                        Debug.WriteLine(String.Join(", ", parts.Select(p => p.ToString()).ToArray()));
                        for (int i = 1; i < parts.Count - 2; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                Debug.WriteLine("Index: " + parts[i + j].ToString());
                                string[] bits = parts[i + j].Split('/');
                                if (bits.Length > 2)
                                {
                                    inds.Add(new Tuple<int, int, int>(int.Parse(bits[0]) - 1, int.Parse(bits[1]) - 1, int.Parse(bits[2]) - 1));
                                }
                                else if (bits.Length == 2)
                                {
                                    inds.Add(new Tuple<int, int, int>(int.Parse(bits[0]) - 1, int.Parse(bits[1]) - 1, 0));
                                }
                                else if (bits.Length == 1)
                                {
                                    Debug.WriteLine(bits[0]);
                                    inds.Add(new Tuple<int, int, int>(int.Parse(bits[0]) - 1, 0, 0));
                                }
                                Debug.WriteLine(inds[inds.Count - 1]);
                            }
                        }
                        break;
                    case "vn":
                        norms.Add(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                        break;
                    case "vt":
                        texs.Add(new Vector2(float.Parse(parts[1]), float.Parse(parts[2])));
                        break;
                    case "":
                        Debug.WriteLine("Empty line");
                        break;
                    case "#":
                        break;
                    case "g":
                        name = parts[1];
                        break;
                    default:
                        Debug.WriteLine("unrecognised character: '{0}'", parts[0]);
                        break;
                }
            }

            if (verts.Count == 0)
            {
                verts = new Vector3[] { new Vector3(0, 0, 0) }.ToList();
            }

            if (texs.Count == 0)
            {
                texs = new Vector2[] { new Vector2(0, 0) }.ToList();
            }

            if (norms.Count == 0)
            {
                norms = new Vector3[] { new Vector3(0, 0, 0) }.ToList();
            }

            List<Tri> newFaces = new List<Tri>();
            for (int i = 0; i < inds.Count -3; i += 3)
            {
                newFaces.Add(new Tri(
                    new Vertex(verts[inds[i].Item1], norms[inds[i].Item3], texs[inds[i].Item2], new Vector3(0, 1, 0)),
                    new Vertex(verts[inds[i+1].Item1], norms[inds[i+1].Item3], texs[inds[i+1].Item2], new Vector3(0, 1, 0)),
                    new Vertex(verts[inds[i+2].Item1], norms[inds[i+2].Item3], texs[inds[i+2].Item2], new Vector3(0, 1, 0))
                    ));
            } 

            Tris = newFaces.ToArray();
            Name = name;
            //IsTextured = true;
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
            ModelMatrix = Matrix4.CreateTranslation(position) * Matrix4.CreateRotationX(rotation.X) * Matrix4.CreateRotationY(rotation.Y) * Matrix4.CreateRotationZ(rotation.Z) * Matrix4.CreateScale(scale);
        }

        public void CalculateModelViewProjectionMatrix(Matrix4 ViewProjectionMatrix)
        {
            ModelViewProjectionMatrix = ModelMatrix * ViewProjectionMatrix;
            //Debug.WriteLine("Model View Projection Matrix:");
            //Debug.WriteLine(ModelViewProjectionMatrix.ToString());
            //Debug.WriteLine("");
        }

        public Mesh AddTris(Tri[] tris)
        {
            List<Tri> temp = Tris.ToList();
            temp.AddRange(tris);
            Tris = temp.ToArray();
            return this;
        }

        public Mesh Transform(Vector3 Positon, Vector3 Rotation, Vector3 Scale)
        {
            Matrix4 mat = Matrix4.CreateTranslation(position) * Matrix4.CreateRotationX(rotation.X) * Matrix4.CreateRotationY(rotation.Y) * Matrix4.CreateRotationZ(rotation.Z) * Matrix4.CreateScale(scale);
            return Transform(mat);
        }

        public Mesh Transform(Matrix4 mat)
        {
            Debug.WriteLine(mat);
            Vector3 translation = mat.ExtractTranslation();
            //Debug.WriteLine(translation);
            //Debug.WriteLine(new Vector3(mat * new Vector4(new Vector3(0f,1.0f,0f), 1.0f)) + translation);
            //mat = mat.ClearTranslation();
            //Debug.WriteLine(mat);

            Tri[] newtris = new Tri[Tris.Length];
            
            for (int i = 0; i < Tris.Length; i++)
            {
                newtris[i] = Tris[i].Transformed(mat);
            }

            Mesh Mesh = new Mesh(newtris);
            return Mesh;
        }

        public override string ToString()
        {
            return base.ToString() + ": " + Name;
        }

        public static Mesh operator +(Mesh left, Mesh right)
        {
            return new Mesh(left.Tris.Concat(right.Tris).ToArray());   //https://stackoverflow.com/questions/59217/merging-two-arrays-in-net
        }
    }
}
