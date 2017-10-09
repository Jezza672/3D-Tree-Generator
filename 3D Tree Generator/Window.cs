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

        List<Mesh> objects = new List<Mesh>();

        Camera camera = new Camera();

        float time = 0.0f;
        const float fps = 20f;
        Timer timer;

        Matrix4 ProjectionMatrix;
        Matrix4 ViewProjectionMatrix;

        Random rand = new Random();

        public Window() //contructor for the window
        {
            InitializeComponent();
        }

        private void glControl1_Load(object sender, EventArgs e) //GLControl loaded all dlls
        {

            //Mesh mesh = new Mesh();
            //mesh = new TestCube();
            //mesh = Mesh.MeshFromFile("Resources/Objects/Car.obj");
            Tree tree = new Tree();
            tree.GenerateTree(2f);
            objects.Add(tree.Mesh);

            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, glControl1.Width / (float)glControl1.Height, 1.0f, 40.0f);

            //glControl1.KeyDown += new KeyEventHandler(glControl1_KeyDown);  //https://github.com/andykorth/opentk/blob/master/Source/Examples/OpenTK/GLControl/GLControlGameLoop.cs
            //glControl1.KeyUp += new KeyEventHandler(glControl1_KeyUp);
            glControl1.Resize += new EventHandler(glControl1_Resize);
            glControl1.Paint += new PaintEventHandler(glControl1_Paint);

            initProgram(); // initialise shaders and VBOs
            GL.ClearColor(Color.CornflowerBlue);
            GL.Enable(EnableCap.DepthTest);
            GL.PointSize(5f);

            timer = new Timer();
            timer.Tick += new EventHandler(TimerTick);
            timer.Interval = (int)(1000 / fps);

            timer.Start();

            update();
            glControl1_Resize(glControl1, EventArgs.Empty);   
        }

        private void TimerTick(object source, EventArgs e)
        {
            update();
            glControl1_Paint(glControl1, EventArgs.Empty);
        }

        private void glControl1_Resize(object sender, EventArgs e) //https://github.com/andykorth/opentk/blob/master/Source/Examples/OpenTK/GLControl/GLControlGameLoop.cs
        {
            glControl1.MakeCurrent();
            GLControl c = (GLControl) sender;
            GL.Viewport(glControl1.ClientRectangle); //https://stackoverflow.com/questions/5858713/how-to-change-the-viewport-resolution-in-opentk
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(1.3f, glControl1.Width / (float)glControl1.Height, 1.3f, 40.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);
            
        }


        private void initProgram()
        {
            pgmID = GL.CreateProgram(); //creates the program

            loadShader("vs.glsl", ShaderType.VertexShader, pgmID, out vsID); //loads shaders
            loadShader("fs.glsl", ShaderType.FragmentShader, pgmID, out fsID);

            GL.LinkProgram(pgmID); //links the program once created
            Debug.WriteLine(GL.GetProgramInfoLog(pgmID)); //debug dump from linking

            attribute_vpos = GL.GetAttribLocation(pgmID, "vPosition");  // finds locations of shader attributes in the shader
            attribute_vcol = GL.GetAttribLocation(pgmID, "vColor");
            uniform_mview = GL.GetUniformLocation(pgmID, "modelview");

            if (attribute_vpos == -1 || attribute_vcol == -1 || uniform_mview == -1) // check to make sure they exist
            {
                Debug.WriteLine("Error binding attributes");
            }

            GL.GenBuffers(1, out vbo_position);  //creates the VBOs
            GL.GenBuffers(1, out ibo_elements);
            GL.GenBuffers(1, out vbo_color);
            GL.GenBuffers(1, out vbo_mview);

            

        }

        private void update()
        {
            time += timer.Interval;

            //get data
            List<Vector3> verts = new List<Vector3>(); // create lists for adding all data from every object
            List<int> inds = new List<int>();
            List<Vector3> colors = new List<Vector3>();
            int vertcount = 0;
            foreach (Mesh v in objects) // add data from each object in turn
            {
                verts.AddRange(v.Vertices);

                colors.AddRange(v.Colors);

                Vector3[] tempcols = new Vector3[v.Vertices.Length-v.Colors.Count];
                for (int i = 0; i < tempcols.Length; i++)
                {
                    //tempcols[i] = new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
                    tempcols[i] = new Vector3((0f+(float)i) / 255f, 69f / 255f, 19f / 255f);
                }
                colors.AddRange(tempcols);

                foreach (int ind in v.Indices)
                {
                    inds.Add(ind + vertcount); // offset indices as verts go into a combined big list
                }
                vertcount += v.Vertices.Length;
            }
            vertdata = verts.ToArray(); // turn lists to arrays
            indicedata = inds.ToArray();
            coldata = colors.ToArray();

            Debug.WriteLine("Verts: " + String.Join(", ", vertdata.Select(p => p.ToString()).ToArray()));  //https://stackoverflow.com/questions/380708/shortest-method-to-convert-an-array-to-a-string-in-c-linq
            Debug.WriteLine("inds: " + String.Join(", ", indicedata.Select(p => p.ToString()).ToArray()));
            Debug.WriteLine("cols: " + String.Join(", ", coldata.Select(p => p.ToString()).ToArray()));

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position); //start writing to the position VBO
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw); //write data
            GL.VertexAttribPointer(attribute_vpos, 3, VertexAttribPointerType.Float, false, 0, 0); // tell it to use this vbo for vpositon

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_color); //colors
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(coldata.Length * Vector3.SizeInBytes), coldata, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_vcol, 3, VertexAttribPointerType.Float, true, 0, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements); //indices
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indicedata.Length * sizeof(int)), indicedata, BufferUsageHint.StaticDraw);

            //objects[0].Rotation = new Vector3(time * 0.0005f, time * 0.001f, time * 0.0015f);

            ViewProjectionMatrix = camera.ViewMatrix * ProjectionMatrix;

            foreach (Mesh ob in objects)
            {
                ob.CalculateModelViewProjectionMatrix(ViewProjectionMatrix);
            } 

            GL.UseProgram(pgmID); //use our shader program

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0); // unbind the buffers (bind null buffer)

            Debug.WriteLine("Update");
        }


        private void glControl1_Paint(object sender, EventArgs e)
        {
            //update(); //remove once updates are called by events

            //GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            


            GL.EnableVertexAttribArray(attribute_vpos); //enable position vbo
            GL.EnableVertexAttribArray(attribute_vcol); // enable color vbo

            int indiceat = 0;

            foreach (Mesh ob in objects)
            {
                Debug.WriteLine(ob.ToString());
                GL.UniformMatrix4(uniform_mview, false, ref ob.ModelViewProjectionMatrix);
                GL.DrawElements(BeginMode.Triangles, ob.Indices.Length, DrawElementsType.UnsignedInt, indiceat * sizeof(uint));
                indiceat += ob.Indices.Length;
            }


            GL.DisableVertexAttribArray(attribute_vpos);  // enable vertex vbo
            GL.DisableVertexAttribArray(attribute_vcol); // enable color vbo


            GL.Flush();
            glControl1.SwapBuffers();
            Debug.WriteLine("Paint");
        }

        void loadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Debug.WriteLine("Loading " + type.ToString() + ": " +  filename);
            Debug.WriteLine(GL.GetShaderInfoLog(address));
            Debug.WriteLine("Done");
        }

        //http://barcodebattler.co.uk/tutorials/csgl1.htm
        //http://barcodebattler.co.uk/tutorials/csgl2.htm
        //http://barcodebattler.co.uk/tutorials/csgl3.htm

        //http://neokabuto.blogspot.co.uk/2013/02/opentk-tutorial-1-opening-windows-and.html
        //http://neokabuto.blogspot.co.uk/2013/03/opentk-tutorial-2-drawing-triangle.html
        //http://neokabuto.blogspot.co.uk/2013/07/opentk-tutorial-3-enter-third-dimension.html
    }
}
