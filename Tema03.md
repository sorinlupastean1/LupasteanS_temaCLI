1. În OpenGL, ordinea poate fi orară sau anti-orară dar e în funcție de modul în care dorim să vedem fața „frontală” a poligonului. De obicei, ordinea anti-orară este utilizată pentru a indica fața frontală.

// Desenează axa Ox (cu roșu).
GL.Begin(PrimitiveType.Lines);
GL.Color3(Color.Red);
GL.Vertex3(0, 0, 0);
GL.Vertex3(XYZ_SIZE, 0, 0);
GL.End();

// Desenează axa Oy (cu galben).
GL.Begin(PrimitiveType.Lines);
GL.Color3(Color.Yellow);
GL.Vertex3(0, 0, 0);
GL.Vertex3(0, XYZ_SIZE, 0); ;
GL.End();

// Desenează axa Oz (cu verde).
GL.Begin(PrimitiveType.Lines);
GL.Color3(Color.Green);
GL.Vertex3(0, 0, 0);
GL.Vertex3(0, 0, XYZ_SIZE);
GL.End();

2. Anti-aliasing este o tehnică prin care marginile zimțate ale obiectelor într-o imagine sunt făcute să pară mai netede. Funcționează prin adăugarea de pixeli de culoare intermediară la margini, ca să pară mai puțin colțuroase și mai naturale.

3. Comenzile GL.LineWidth(float) și GL.PointSize(float) sunt folosite pentru a seta grosimea liniilor și, respectiv, dimensiunea punctelor în OpenGL:

GL.LineWidth(float): Setează grosimea liniilor desenate. Valoarea float specificată determină cât de groasă va fi linia. De exemplu, GL.LineWidth(3.0f); va desena linii cu o grosime de 3 pixeli.

GL.PointSize(float): Setează dimensiunea punctelor desenate. Valoarea float specificată determină diametrul punctului. De exemplu, GL.PointSize(5.0f); va desena puncte cu un diametru de 5 pixeli.

Ambele funcții nu trebuie să fie plasate în interiorul unei zone GL.Begin()...GL.End(). Ele trebuie apelate înainte de blocul GL.Begin(), pentru a seta proprietățile obiectelor ce urmează să fie desenate.

4.  a - LineLoop: Desenează o linie care leagă o serie de puncte și, la final, conectează ultimul punct înapoi la primul, formând o buclă închisă.

b - LineStrip: Desenează o linie continuă care leagă fiecare punct cu următorul, dar nu închide bucla (nu leagă ultimul punct cu primul).

c - TriangleFan: Desenează triunghiuri având un punct de referință comun (primul punct), iar fiecare triunghi se formează între acest punct și fiecare pereche de puncte succesive din listă. Este util pentru a desena forme circulare sau „evantai”.

d - TriangleStrip: Desenează o serie de triunghiuri conectate, unde fiecare triunghi folosește ultimele două puncte anterioare plus unul nou. Produce o bandă de triunghiuri continuă.

5.  using OpenTK;
    using OpenTK.Graphics.OpenGL;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

namespace lab3
{
internal class ImmediateMode:GameWindow
{

        public ImmediateMode():base(800,600)
        {
            VSync = VSyncMode.On;



        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color.Green);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);



        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width / 2, Height);


        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
        }


    }

}

6. Utilizarea culorilor diferite, fie în gradient, fie per suprafață, este importantă în desenarea obiectelor 3D pentru că ajută la evidențierea formelor și la crearea unui efect de profunzime. Când folosești culori diferite, poți să vezi mai clar conturul și orientarea fiecărei suprafețe, ceea ce face obiectul să pară mai tridimensional și mai realist.

Avantajul este că permite privitorului să înțeleagă mai bine structura și poziția obiectului în spațiu, ceea ce îmbunătățește experiența vizuală și înțelegerea formei.

7. Un gradient de culoare este o tranziție treptată între două sau mai multe culori. În loc să fie o singură culoare pe o suprafață, un gradient schimbă culoarea treptat, de exemplu, de la roșu la albastru, creând un efect de estompare între ele.

Pentru a crea un gradient în OpenGL, trebuie să setezi culori diferite pentru fiecare vârf (vertex) al unei forme. OpenGL va amesteca automat culorile între vârfuri, generând astfel tranziția de culoare.

8-9.
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace tema
{
class Program : GameWindow
{
private Vector3[] triangleVertices;
private Color[] vertexColors; // Array pentru culorile fiecărui vârf
private float cameraAngleX = 0.0f;
private float cameraAngleY = 0.0f;
private int previousMouseX, previousMouseY;

        public Program() : base(800, 600, new GraphicsMode(32, 24, 0, 8))
        {
            VSync = VSyncMode.On;
            LoadTriangleVertices("triangle.txt"); // Încarcă coordonatele triunghiului din fișier
            InitializeVertexColors(); // Inițializează culorile pentru fiecare vârf
        }

        private void LoadTriangleVertices(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            triangleVertices = lines.Select(line =>
            {
                var parts = line.Split(' ');
                return new Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
            }).ToArray();
        }

        private void InitializeVertexColors()
        {
            // Inițializare culori pentru fiecare vârf
            vertexColors = new Color[]
            {
                Color.Red,     // Primul vârf - roșu
                Color.Green,   // Al doilea vârf - verde
                Color.Blue     // Al treilea vârf - albastru
            };
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.DepthTest);

            previousMouseX = Mouse.GetState().X;
            previousMouseY = Mouse.GetState().Y;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);

            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Width / (float)Height, 1.0f, 100.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var keyboard = Keyboard.GetState();
            var mouse = Mouse.GetState();

            if (mouse[MouseButton.Left])
            {
                int deltaX = mouse.X - previousMouseX;
                int deltaY = mouse.Y - previousMouseY;

                cameraAngleX += deltaX * 0.05f;
                cameraAngleY += deltaY * 0.05f;

                previousMouseX = mouse.X;
                previousMouseY = mouse.Y;
            }

            if (keyboard[Key.Escape]) Exit();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Rotate(cameraAngleX, 0.0f, 1.0f, 0.0f);
            GL.Rotate(cameraAngleY, 1.0f, 0.0f, 0.0f);
            GL.Translate(0.0f, 0.0f, -5.0f);

            // Desenează triunghiul cu culori diferite pentru fiecare vârf
            GL.Begin(PrimitiveType.Triangles);
            for (int i = 0; i < triangleVertices.Length; i++)
            {
                GL.Color3(vertexColors[i]); // Setează culoarea pentru fiecare vârf
                Console.WriteLine($"Vertex {i + 1} Color - R: {vertexColors[i].R}, G: {vertexColors[i].G}, B: {vertexColors[i].B}");
                GL.Vertex3(triangleVertices[i]);
            }
            GL.End();

            SwapBuffers();
        }

        [STAThread]
        static void Main(string[] args)
        {
            using (Program program = new Program())
            {
                program.Run(60.0);
            }
        }
    }

}

10. Când folosești o culoare diferită pentru fiecare vârf la desenarea unei linii sau a unui triunghi în modul strip, OpenGL creează un efect de gradient între culori. Practic, OpenGL amestecă treptat culorile între vârfuri, făcând tranziția lină de la o culoare la alta de-a lungul liniei sau suprafeței triunghiului.

Acest efect ajută la evidențierea formei și adaugă un aspect vizual mai plăcut și mai natural.
