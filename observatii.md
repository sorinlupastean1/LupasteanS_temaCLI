1. GL.Ortho(-5.0, 5.0, -5.0, 1.0, 0.0, 4.0);

Observații: s-a mutat triunghiul pe centru sus și a fost redimensionat

3.

1 - Viewport-ul este zona din fereastră unde se afișează conținutul grafic, setată cu glViewport(x, y, width, height).

2 - FPS (Frames per Second) indică numărul de cadre afișate într-o secundă. Un FPS mai mare oferă animații mai fluide și performanță mai bună.

3 - OnUpdateFrame() actualizează logica aplicației în fiecare cadru, gestionând mișcarea obiectelor și intrările utilizatorului.

4 - Immediate Mode Rendering în OpenGL permite desenarea geometriei direct în timp real, dar este mai puțin eficient decât metodele moderne.

5 - OpenGL 3.3 este ultima versiune care acceptă modul imediat de randare, înlocuit ulterior de tehnici mai eficiente precum VBO.

6 - OnRenderFrame() gestionează desenarea scenei după actualizarea logicii aplicației.

7 - OnResize() ajustează viewport-ul și proiecțiile când dimensiunea ferestrei se schimbă, pentru a menține corect redimensionarea graficii.

8 - CreatePerspectiveFieldOfView() generează o matrice de proiecție în perspectivă, stabilind unghiul de vizualizare, raportul de aspect și distanțele dintre planurile apropiat și îndepărtat pentru a reda corect scena.
