/**
  Autor: Dalton Solano dos Reis
**/

#define CG_Gizmo
// #define CG_Privado

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK.Input;
using CG_Biblioteca;

namespace gcgcg
{
    class Mundo : GameWindow
    {
        private static Mundo instanciaMundo = null;

        private Mundo(int width, int height) : base(width, height) { }

        public static Mundo GetInstance(int width, int height)
        {
            if (instanciaMundo == null)
                instanciaMundo = new Mundo(width, height);
            return instanciaMundo;
        }

        private CameraOrtho camera = new CameraOrtho();
        protected List<Objeto> objetosLista = new List<Objeto>();
        private ObjetoGeometria objetoSelecionado = null;
        private char objetoId = '@';
        double euclidiana = 0;
        double raioAoQuadrado;
        private bool bBoxDesenhar = false;
        int mouseX, mouseY;   //TODO: achar método MouseDown para não ter variável Global
        private bool mouseMoverPto = false;
        //private Retangulo obj_Retangulo;
        private Circulo obj_CirculoMenor;
        private Circulo obj_CirculoMaior;

        private Retangulo obj_Retangulo;

        private BBox obj_BBox;

#if CG_Privado
    private Privado_SegReta obj_SegReta;
    private Privado_Circulo obj_Circulo;
    
#endif

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            camera.xmin = -50; camera.xmax = 400; camera.ymin = -50; camera.ymax = 400;

            Console.WriteLine(" --- Ajuda / Teclas: ");
            Console.WriteLine(" [  H     ] mostra teclas usadas. ");

            objetoId = Utilitario.charProximo(objetoId);
            obj_CirculoMenor = new Circulo(objetoId, null, new Ponto4D(200, 200, 0), 50, true);
            obj_CirculoMenor.ObjetoCor.CorR = 0; obj_CirculoMenor.ObjetoCor.CorG = 0; obj_CirculoMenor.ObjetoCor.CorB = 0;
            obj_CirculoMenor.PrimitivaTamanho = 2;

            objetosLista.Add(obj_CirculoMenor);

            objetoId = Utilitario.charProximo(objetoId);
            obj_CirculoMaior = new Circulo(objetoId, null, new Ponto4D(200, 200, 0), 150);
            obj_CirculoMaior.ObjetoCor.CorR = 0; obj_CirculoMaior.ObjetoCor.CorG = 0; obj_CirculoMaior.ObjetoCor.CorB = 0;
            obj_CirculoMaior.PrimitivaTamanho = 2;
            objetosLista.Add(obj_CirculoMaior);


            // raio ao quadrado para ser comparado a distancia euclidiana
            raioAoQuadrado = Math.Pow(obj_CirculoMaior.getRaio(), 2);

            // cria os pontos inferior esquerdo e superior direito

            Ponto4D ponto45 = Matematica.GerarPtosCirculo(45, 150);
            ponto45 += obj_CirculoMaior.getPontoCentral();
            Ponto4D ponto225 = Matematica.GerarPtosCirculo(225, 150);
            ponto225 += obj_CirculoMaior.getPontoCentral();

            
             // cria um retangulo utilizando os dois pontos 

            objetoId = Utilitario.charProximo(objetoId);
            obj_Retangulo = new Retangulo(objetoId, null, ponto225, ponto45);
            obj_Retangulo.ObjetoCor.CorR = 255; obj_Retangulo.ObjetoCor.CorG = 0; obj_Retangulo.ObjetoCor.CorB = 255;
            objetosLista.Add(obj_Retangulo);

            //obj_Retangulo.BBox.Desenhar();

            //obj_Circulo.PrimitivaTipo = PrimitiveType.Points; deixamos dentro da classe circulo, mas é possivel deixar aqui

            objetoSelecionado = obj_Retangulo;

#if CG_Privado
      objetoId = Utilitario.charProximo(objetoId);
      obj_SegReta = new Privado_SegReta(objetoId, null, new Ponto4D(50, 150), new Ponto4D(150, 250));
      obj_SegReta.ObjetoCor.CorR = 255; obj_SegReta.ObjetoCor.CorG = 255; obj_SegReta.ObjetoCor.CorB = 0;
      objetosLista.Add(obj_SegReta);
      objetoSelecionado = obj_SegReta;

      objetoId = Utilitario.charProximo(objetoId);
      obj_Circulo = new Privado_Circulo(objetoId, null, new Ponto4D(100, 300), 50);
      obj_Circulo.ObjetoCor.CorR = 0; obj_Circulo.ObjetoCor.CorG = 255; obj_Circulo.ObjetoCor.CorB = 255;
      
      objetosLista.Add(obj_Circulo);
      objetoSelecionado = obj_Circulo;
#endif
            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(camera.xmin, camera.xmax, camera.ymin, camera.ymax, camera.zmin, camera.zmax);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
#if CG_Gizmo
            Sru3D();
#endif
            for (var i = 0; i < objetosLista.Count; i++)
                objetosLista[i].Desenhar();
            if (bBoxDesenhar && (objetoSelecionado != null))
                objetoSelecionado.BBox.Desenhar();
            this.SwapBuffers();
        }

        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.H)
                Utilitario.AjudaTeclado();


            else if (e.Key == Key.Escape)
                Exit();
            else if (e.Key == Key.E)
            {
                Console.WriteLine("--- Objetos / Pontos: ");
                for (var i = 0; i < objetosLista.Count; i++)
                {
                    Console.WriteLine(objetosLista[i]);
                }
            }
            else if (e.Key == Key.O)
                bBoxDesenhar = !bBoxDesenhar;
            else if (e.Key == Key.V)
                mouseMoverPto = !mouseMoverPto;   //TODO: falta atualizar a BBox do objeto
            else
                Console.WriteLine(" __ Tecla não implementada.");
        }

        //TODO: não está considerando o NDC
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {

            mouseX = e.Position.X - 130; mouseY = 600 - e.Position.Y - 130; // Inverti eixo Y
            euclidiana = (Math.Pow((obj_CirculoMaior.getPontoCentral().X - obj_CirculoMenor.getPontoCentral().X), 2) +
            Math.Pow(((obj_CirculoMaior.getPontoCentral().Y - obj_CirculoMenor.getPontoCentral().Y)), 2));
            //Console.WriteLine(euclidiana);

            if (mouseMoverPto && (objetoSelecionado != null) && e.Mouse.LeftButton == ButtonState.Pressed)
            {

                if (obj_CirculoMenor.getPontoCentral().X >= objetoSelecionado.BBox.obterMaiorX)
                {
                    bBoxDesenhar = true;
                }
                else if (obj_CirculoMenor.getPontoCentral().X <= objetoSelecionado.BBox.obterMenorX)
                {
                    bBoxDesenhar = true;
                }
                else if (obj_CirculoMenor.getPontoCentral().Y >= objetoSelecionado.BBox.obterMaiorY)
                {
                    bBoxDesenhar = true;
                }
                else if (obj_CirculoMenor.getPontoCentral().Y <= objetoSelecionado.BBox.obterMenorY)
                {
                    bBoxDesenhar = true;
                }
                else
                {
                    bBoxDesenhar = false;
                }

                if (euclidiana > raioAoQuadrado)
                { // vai sair do circulo maior, bloqueia
                    bBoxDesenhar = false;
                    objetoSelecionado.ObjetoCor.CorR = 0; objetoSelecionado.ObjetoCor.CorG = 255; objetoSelecionado.ObjetoCor.CorB = 255; // ciano
                }
                else
                { // esta dentro do circulo maior, pode mover
                    obj_CirculoMenor.setPontoCentral(mouseX, mouseY);
                }
            }
            else if (mouseMoverPto && (objetoSelecionado != null) && e.Mouse.LeftButton == ButtonState.Released) // soltou o mouse volta a origem
            {
                voltaOrigem();
            }


        }
        private void voltaOrigem()
        {
            bBoxDesenhar = false;
            obj_CirculoMenor.setPontoCentral(200, 200);
            objetoSelecionado.ObjetoCor.CorR = 255; objetoSelecionado.ObjetoCor.CorG = 0; objetoSelecionado.ObjetoCor.CorB = 255;
        }

#if CG_Gizmo
        private void Sru3D()
        {
            GL.LineWidth(5);
            GL.Begin(PrimitiveType.Lines);
            // GL.Color3(1.0f,0.0f,0.0f);
            GL.Color3(Convert.ToByte(255), Convert.ToByte(0), Convert.ToByte(0));
            GL.Vertex3(0, 0, 0); GL.Vertex3(200, 0, 0);
            // GL.Color3(0.0f,1.0f,0.0f);
            GL.Color3(Convert.ToByte(0), Convert.ToByte(255), Convert.ToByte(0));
            GL.Vertex3(0, 0, 0); GL.Vertex3(0, 200, 0);
            // GL.Color3(0.0f,0.0f,1.0f);
            // GL.Color3(Convert.ToByte(0), Convert.ToByte(0), Convert.ToByte(255));
            //.Vertex3(0, 0, 0); GL.Vertex3(0, 0, 200);
            GL.End();
            // GL.PointSize(1);
            //GL.Begin(PrimitiveType.Points);  

            // GL.Color3(Convert.ToByte(255), Convert.ToByte(0), Convert.ToByte(255));
            // Ponto4D ponto45 = Matematica.GerarPtosCirculo(45,150);
            // ponto45 +=obj_CirculoMaior.getPontoCentral();

            //  GL.End();




        }
#endif
    }
    class Program
    {
        static void Main(string[] args)
        {
            Mundo window = Mundo.GetInstance(600, 600);
            window.Title = "CG_N2";
            window.Run(1.0 / 60.0);
        }
    }
}
