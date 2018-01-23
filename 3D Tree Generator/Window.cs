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


/* TODO:
 * Fix Color rendering
 * add normal support
 * GenerateBranch
*/

namespace _3D_Tree_Generator
{
    public partial class Window : Form
    {
        int pgmID, vsID, fsID; // program, vertex shader and fragment shader IDs

        int attribute_vcol; //shader attribute locations
        int attribute_vpos;
        int uniform_mview;

        int vbo_position; //vertex buffer object pointer
        int ibo_elements; //integer buffer object pointer for indices
        int vbo_color; //colour vbo pointer
        int vbo_mview; // viewmatrix buffer pointer


        Vector3[] vertdata; // array of vertices to send to buffers
        int[] indicedata;
        Vector3[] coldata; // " colors
        Vector2[] texcoorddata;

        List<Mesh> objects = new List<Mesh>();

        Camera camera = new Camera();
        Tree tree = new Tree(10f, 1f, Matrix4.Identity);

        float time = 0.0f;
        const float fps = 20f;
        Timer timer;

        DateTime prevTime = DateTime.Now;

        Dictionary<string, Shader> shaders = new Dictionary<string, Shader>();
        string activeShader = "default";


        Dictionary<string, Texture> Textures = new Dictionary<string, Texture>();

        Matrix4 ProjectionMatrix;
        Matrix4 ViewProjectionMatrix;

        public Window() //contructor for the window
        {
            InitializeComponent();
        }

        /// <summary>
        /// Repaint every frame.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void TimerTick(object source, EventArgs e)
        {
            glControl1_Paint(glControl1, EventArgs.Empty);
        }

        /// <summary>
        /// GlControl Loaded in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void glControl1_Load(object sender, EventArgs e) //GLControl loaded all dlls
        {

            tree.GenerateTree();
            objects.Add(tree);

            //Expression exp = new Expression("x*x", EvaluateOptions.IgnoreCase);
            //Debug.WriteLine(exp.Eval(10));

            //Mesh mesh = new Mesh(Tree.GenerateBranch(1f, 10f, 10, 7, color: 0.2f, topColor: 0.8f, minDist: 1f, trunkFunction: exp));
            
            
            //Mesh mesh = new Mesh(Tree.GenerateBranch(5f, 10f, 10, 7, color: 0.2f,  topColor: 0.8f, branch: false, flare: 1, flareEnd: 2f));

            //mesh.Rotation = new Vector3((float)Math.PI, 0, 0);
            //objects.Add(mesh);

            //objects.Add(new TestCube());
            //objects.Add(new TexturedTestCube());
            //objects.Add(new Mesh("Resources/Objects/Car.obj"));

            //objects.Add(new TestAxes());
            
            
            Debug.WriteLine("Objects: \n" + String.Join("\n", objects));

            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, glControl1.Width / (float)glControl1.Height, 0.5f, 100.0f);

            initProgram(); // initialise shaders and VBOs
            GL.ClearColor(Color.White);
            GL.Enable(EnableCap.DepthTest);
            GL.PointSize(5f);

            timer = new Timer();
            timer.Tick += new EventHandler(TimerTick);
            timer.Interval = (int)(1000 / fps);

            

            timer.Start();
            autoRefreshToolStripMenuItem.Checked = true;

            update();
            glControl1_Resize(glControl1, EventArgs.Empty);

        }

        private void initProgram()
        {
            GL.GenBuffers(1, out ibo_elements);

            shaders.Add("default", new Shader("Resources/Shaders/vs.glsl", "Resources/Shaders/fs.glsl", true));
            shaders.Add("textured", new Shader("Resources/Shaders/vs_tex.glsl", "Resources/Shaders/fs_tex.glsl", true));
            activeShader = "default";

        }

        /// <summary>
        /// Call whenever anything is changed in the scene.
        /// </summary>
        private void update()
        {
            //Debug.WriteLine(1000 / interval);

            //get data
            List<Vector3> verts = new List<Vector3>(); // create lists for adding all data from every object
            List<int> inds = new List<int>();
            List<Vector3> colors = new List<Vector3>();
            List<Vector2> texcoords = new List<Vector2>();

            int vertcount = 0;
            foreach (Mesh v in objects) // add data from each object in turn
            {
                /*
                Debug.WriteLine(v.Vertices.Length + " Verts are: " + String.Join(", ", v.Vertices));
                Debug.WriteLine(v.Colors.Length + " Colors are: " + String.Join(", ", v.Colors));
                Debug.WriteLine(v.Indices.Length + " Indices are: " + String.Join(", ", v.Indices));
                Debug.WriteLine(v.TexCoords.Length + " Textcoords are: " + String.Join(", ", v.TexCoords));
                */
                verts.AddRange(v.Vertices);

                colors.AddRange(v.Colors);

                texcoords.AddRange(v.TexCoords);

                foreach (int ind in v.Indices)
                {
                    inds.Add(ind + vertcount); // offset indices as verts go into a combined big list
                }
                vertcount += v.Vertices.Length;

            }
            vertdata = verts.ToArray(); // turn lists to arrays
            indicedata = inds.ToArray();
            coldata = colors.ToArray();
            texcoorddata = texcoords.ToArray();


            //Debug.WriteLine("Verts: " + String.Join(", ", vertdata.Select(p => p.ToString()).ToArray()));  //https://stackoverflow.com/questions/380708/shortest-method-to-convert-an-array-to-a-string-in-c-linq
            //Debug.WriteLine("inds: " + String.Join(", ", indicedata.Select(p => p.ToString()).ToArray()));
            //Debug.WriteLine("cols: " + String.Join(", ", coldata.Select(p => p.ToString()).ToArray()));
            //Debug.WriteLine("texs: " + String.Join(", ", texcoorddata.Select(p => p.ToString()).ToArray()));

            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vPosition")); //writes all vertices to the vbo
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);

            if (shaders[activeShader].GetAttribute("vColor") != -1) //writes all colours to vbo if the shader has it
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vColor"));
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(coldata.Length * Vector3.SizeInBytes), coldata, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vColor"), 3, VertexAttribPointerType.Float, true, 0, 0);
            }

            if (shaders[activeShader].GetAttribute("texcoord") != -1) //writes texture coordinates to vbo if shader has them
            {
                //Debug.WriteLine("Binding TexCoords");
                GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("texcoord"));
                GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(texcoorddata.Length * Vector2.SizeInBytes), texcoorddata, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(shaders[activeShader].GetAttribute("texcoord"), 2, VertexAttribPointerType.Float, true, 0, 0);
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements); //indices
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indicedata.Length * sizeof(int)), indicedata, BufferUsageHint.StaticDraw);

            ViewProjectionMatrix = camera.ViewMatrix * ProjectionMatrix;

            foreach (Mesh ob in objects)
            {
                ob.CalculateModelViewProjectionMatrix(ViewProjectionMatrix);
            }

            GL.UseProgram(shaders[activeShader].ProgramID); //use our active shader program

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0); // unbind the buffers (bind null buffer)

            //Debug.WriteLine("Update");
        }

        private void glControl1_Resize(object sender, EventArgs e) //https://github.com/andykorth/opentk/blob/master/Source/Examples/OpenTK/GLControl/GLControlGameLoop.cs
        {
            glControl1.MakeCurrent();
            GLControl c = (GLControl)sender;
            GL.Viewport(glControl1.ClientRectangle); //https://stackoverflow.com/questions/5858713/how-to-change-the-viewport-resolution-in-opentk
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(1.3f, glControl1.Width / (float)glControl1.Height, 1.3f, 40.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);

        }

        private void glControl1_Move(object sender, EventArgs e)
        {
            glControl1.MakeCurrent();
            GLControl c = (GLControl)sender;
            GL.Viewport(glControl1.ClientRectangle); //https://stackoverflow.com/questions/5858713/how-to-change-the-viewport-resolution-in-opentk
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(1.3f, glControl1.Width / (float)glControl1.Height, 1.3f, 40.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);
        }

        private void glControl1_Paint(object sender, EventArgs e)
        {
            double timeTaken = (DateTime.Now - prevTime).TotalSeconds;
            prevTime = DateTime.Now;
            this.Controls["FPS_Counter"].Text = "FPS: " + (1/ timeTaken).ToString("000.00");
            //update(); //remove once updates are called by events

            //GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shaders[activeShader].EnableVertexAttribArrays();

            int indiceat = 0;

            foreach (Mesh v in objects)
            {

                GL.UniformMatrix4(shaders[activeShader].GetUniform("modelview"), false, ref v.ModelViewProjectionMatrix); //send modelview to shader
                //Debug.WriteLine(v.IsTextured);
                if (v.IsTextured)
                {
                    if (shaders[activeShader].GetUniform("maintexture") != -1) //send texture to shader if textured
                    {
                        GL.ActiveTexture(TextureUnit.Texture0);
                        GL.BindTexture(TextureTarget.Texture2D, v.Texture.TexID);
                        GL.Uniform1(shaders[activeShader].GetUniform("maintexture"), 0);
                        //Debug.WriteLine("Sent Texture");
                    }

                }
                GL.DrawElements(BeginMode.Triangles, v.Indices.Length, DrawElementsType.UnsignedInt, indiceat * sizeof(uint)); //draw this object
                indiceat += v.Indices.Length;
            }

            shaders[activeShader].DisableVertexAttribArrays();

            GL.Flush();
            glControl1.SwapBuffers();
            //Debug.WriteLine("Paint");
        }

        private void glControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            camera.Zoom(glControl1, e);
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            camera.Update(glControl1);
            update();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            tree.GenerateTree();
            update();
        }

        private void ValueChanged(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                NumericUpDown num = sender as NumericUpDown; //turn the generic sender into the specific NumericUpDown type
                //this extracts the property of the tree to be changed, based on the AccessibleName of the sender
                var property = tree.GetType().GetProperty(num.AccessibleName);  //https://stackoverflow.com/questions/737151/how-to-get-the-list-of-properties-of-a-class
                //this sets the afformentioned property to the value of the NumericUpDown
                property.SetValue(tree, Convert.ChangeType(num.Value, property.PropertyType));
                //then regenerate the tree with new property and update the scene, if auto-refresh is enabled.
                if (this.autoRefreshToolStripMenuItem.Checked)
                {
                    tree.GenerateTree();
                    update();
                }
            }
        }

        private void Function_Update(object sender, EventArgs e)
        {
            TextBox box = Shape_Functions.Controls[(sender as Control).AccessibleName] as TextBox;
            if (box.Text == "" || box.Text == null)
            {
                box.Text = " ";
            }
            Expression ex = new Expression(box.Text, EvaluateOptions.IgnoreCase);
            if (ex.HasErrors())
            {
                MessageBox.Show(ex.Error, "Invalid Function", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); //https://stackoverflow.com/questions/3803630/how-to-call-window-alertmessage-from-c
            }
            else
            {
                try
                {
                    var property = tree.GetType().GetProperty(box.AccessibleName);  //https://stackoverflow.com/questions/737151/how-to-get-the-list-of-properties-of-a-class
                    property.SetValue(tree, Convert.ChangeType(ex, property.PropertyType));
                    if (this.autoRefreshToolStripMenuItem.Checked)
                    {
                        tree.GenerateTree();
                        update();
                    }
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Must be a function in x!", "Invalid Function", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); //https://stackoverflow.com/questions/3803630/how-to-call-window-alertmessage-from-c
                }
            }
        }


        private void SaveAs(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            save.DefaultExt = ".tre";
            save.Filter = "Tree files (*.tre)|*.tre|All files (*.*)|*.*";
            save.FilterIndex = 1;
            save.ShowDialog();
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            Stream stream = save.OpenFile();
            formatter.Serialize(stream, tree);
            stream.Close();


            //https://msdn.microsoft.com/en-us/library/ms973893.aspx
            //https://msdn.microsoft.com/en-us/library/system.windows.forms.savefiledialog.openfile(v=vs.110).aspx
        }

        private void Open(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            open.DefaultExt = ".tre";
            open.Filter = "Tree files (*.tre)|*.tre|All files (*.*)|*.*";
            open.FilterIndex = 1;
            open.ShowDialog();
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            Stream stream = open.OpenFile();
            tree = (Tree)formatter.Deserialize(stream);
            stream.Close();
            tree.GenerateTree();
            update();
        }

        //http://barcodebattler.co.uk/tutorials/csgl1.htm
        //http://barcodebattler.co.uk/tutorials/csgl2.htm
        //http://barcodebattler.co.uk/tutorials/csgl3.htm

        //http://neokabuto.blogspot.co.uk/2013/02/opentk-tutorial-1-opening-windows-and.html
        //http://neokabuto.blogspot.co.uk/2013/03/opentk-tutorial-2-drawing-triangle.html
        //http://neokabuto.blogspot.co.uk/2013/07/opentk-tutorial-3-enter-third-dimension.html
        //http://neokabuto.blogspot.co.uk/2014/01/opentk-tutorial-5-basic-camera.html
    }
}
