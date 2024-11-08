using System;
using System.Drawing;
using System.IO;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

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

    private bool colorsPrinted = false; // Add this variable to check if colors are printed

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadIdentity();
        GL.Rotate(cameraAngleX, 0.0f, 1.0f, 0.0f);
        GL.Rotate(cameraAngleY, 1.0f, 0.0f, 0.0f);
        GL.Translate(0.0f, 0.0f, -5.0f);

        // Draw the triangle with different colors for each vertex
        GL.Begin(PrimitiveType.Triangles);
        for (int i = 0; i < triangleVertices.Length; i++)
        {
            GL.Color3(vertexColors[i]);

            // Print the color information only once
            if (!colorsPrinted)
            {
                Console.WriteLine($"Vertex {i + 1} Color - R: {vertexColors[i].R}, G: {vertexColors[i].G}, B: {vertexColors[i].B}");
            }

            GL.Vertex3(triangleVertices[i]);
        }
        GL.End();

        colorsPrinted = true; // Ensure it only prints once

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