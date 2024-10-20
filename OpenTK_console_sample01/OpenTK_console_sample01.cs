using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;

/**
    Aplicația utilizează biblioteca OpenTK v2.0.0 (stable) oficială și OpenTK. GLControl v2.0.0
    (unstable) neoficială. Aplicația fiind scrisă în modul consolă nu va utiliza controlul WinForms
    oferit de OpenTK!
    Tipul de ferestră utilizat: GAMEWINDOW. Se demmonstrează modul imediat de randare (vezi comentariu!)...
**/
namespace OpenTK_console_sample01
{
    class SimpleWindow : GameWindow
    {
        private float objectX = 0f; // Poziția pe axa X a obiectului
        private float objectY = 0f; // Poziția pe axa Y a obiectului

        // Constructor.
        public SimpleWindow() : base(800, 600)
        {
            KeyDown += Keyboard_KeyDown;
            MouseMove += Mouse_Move; // Adăugăm handler pentru mișcarea mouse-ului
        }

        // Tratează evenimentul generat de apăsarea unei taste.
        void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Exit();

            if (e.Key == Key.F11)
                if (this.WindowState == WindowState.Fullscreen)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Fullscreen;

            // Controlul obiectului cu săgețile sus și jos
            if (e.Key == Key.Up)
                objectY += 0.1f; // Deplasare în sus
            if (e.Key == Key.Down)
                objectY -= 0.1f; // Deplasare în jos
        }

        // Mișcarea obiectului cu mouse-ul
        void Mouse_Move(object sender, MouseMoveEventArgs e)
        {
            objectX = (float)e.X / Width * 2 - 1; // Normalizează poziția X a mouse-ului între -1 și 1
            objectY = 1 - (float)e.Y / Height * 2; // Normalizează poziția Y între -1 și 1
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(Color.MidnightBlue);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-5.0, 5.0, -5.0, 1.0, 0.0, 1.0); // Am ajustat viewport-ul pentru a permite mișcarea completă
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // Logica aplicatiei
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            // Render obiectul care se mișcă

            GL.Begin(PrimitiveType.Triangles);

            GL.Color3(Color.MidnightBlue);
            GL.Vertex2(objectX - 0.1f, objectY - 0.1f);
            GL.Color3(Color.SpringGreen);
            GL.Vertex2(objectX, objectY + 0.1f);
            GL.Color3(Color.Ivory);
            GL.Vertex2(objectX + 0.1f, objectY - 0.1f);

            GL.End();

            this.SwapBuffers();
        }

        [STAThread]
        static void Main(string[] args)
        {
            using (SimpleWindow example = new SimpleWindow())
            {
                example.Run(30.0, 0.0);
            }
        }
    }
}

