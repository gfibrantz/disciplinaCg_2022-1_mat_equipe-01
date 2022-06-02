/**
  Autor: Dalton Solano dos Reis
**/

//#define CG_Privado // código do professor.
#define CG_Gizmo  // debugar gráfico.
//#define CG_Debug // debugar texto.
#define CG_OpenGL // render OpenGL.
//#define CG_DirectX // render DirectX.

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

        //teste
        //novo comentario
        //comentario3      
        //
        //bruxaria222221


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
        private bool bBoxDesenhar = false;
        int mouseX, mouseY;   //TODO: achar método MouseDown para não ter variável Global
        private bool mouseMoverPto = true;
        private Retangulo obj_Retangulo;
        private Poligono obj_Poligono;
        private Ponto obj_Ponto;
        bool estaDesenhandoPoligono = false;
#if CG_Privado
    private Privado_SegReta obj_SegReta;
    private Privado_Circulo obj_Circulo;

#endif

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            camera.xmin = 0; camera.xmax = 750; camera.ymin = 0; camera.ymax = 750;

            Console.WriteLine(" --- Ajuda / Teclas: ");
            Console.WriteLine(" [  H     ] mostra teclas usadas. ");

            objetoId = Utilitario.charProximo(objetoId);
            obj_Retangulo = new Retangulo(objetoId, null, new Ponto4D(50, 50, 0), new Ponto4D(150, 150, 0));
            obj_Retangulo.ObjetoCor.CorR = 255; obj_Retangulo.ObjetoCor.CorG = 0; obj_Retangulo.ObjetoCor.CorB = 255;
            objetosLista.Add(obj_Retangulo);
            objetoSelecionado = obj_Retangulo;

            //comecamos ja com um objeto Poligono criado
            // objetoId = Utilitario.charProximo(objetoId);
            // obj_Poligono = new Poligono(objetoId, null);
            // obj_Poligono.ObjetoCor.CorR = 255; obj_Poligono.ObjetoCor.CorG = 235; obj_Poligono.ObjetoCor.CorB = 51;
            // objetosLista.Add(obj_Poligono);
            // objetoSelecionado = obj_Poligono;




#if CG_Privado
      objetoId = Utilitario.charProximo(objetoId);
      obj_SegReta = new Privado_SegReta(objetoId, null, new Ponto4D(50, 150), new Ponto4D(150, 250));
      obj_SegReta.ObjetoCor.CorR = 255; obj_SegReta.ObjetoCor.CorG = 255; obj_SegReta.ObjetoCor.CorB = 0;
      objetosLista.Add(obj_SegReta);
      objetoSelecionado = obj_SegReta;

      objetoId = Utilitario.charProximo(objetoId);
      obj_Circulo = new Privado_Circulo(objetoId, null, new Ponto4D(100, 300), 50);
      obj_Circulo.ObjetoCor.CorR = 0; obj_Circulo.ObjetoCor.CorG = 255; obj_Circulo.ObjetoCor.CorB = 255;
      obj_Circulo.PrimitivaTipo = PrimitiveType.Points;
      obj_Circulo.PrimitivaTamanho = 5;
      objetosLista.Add(obj_Circulo);
      objetoSelecionado = obj_Circulo;
#endif
#if CG_OpenGL
            GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
#endif
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
#if CG_OpenGL
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(camera.xmin, camera.xmax, camera.ymin, camera.ymax, camera.zmin, camera.zmax);
#endif
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
#if CG_OpenGL
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
#endif
#if CG_Gizmo
            Sru3D();
#endif
            for (var i = 0; i < objetosLista.Count; i++)
                objetosLista[i].Desenhar();
#if CG_Gizmo
            if (bBoxDesenhar && (objetoSelecionado != null))
                objetoSelecionado.BBox.Desenhar();
#endif
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
#if CG_Gizmo
            else if (e.Key == Key.O)
                bBoxDesenhar = !bBoxDesenhar;
#endif
            else if (e.Key == Key.V)
                mouseMoverPto = !mouseMoverPto;  //TODO: falta atualizar a BBox do objeto
            else if (e.Key == Key.R)
            {
                if (objetoSelecionado != null)
                {
                    objetoSelecionado.ObjetoCor.CorR = 255;
                    objetoSelecionado.ObjetoCor.CorG = 51;
                    objetoSelecionado.ObjetoCor.CorB = 71;
                }
            }
            else if (e.Key == Key.G)
            {
                if (objetoSelecionado != null)
                {
                    objetoSelecionado.ObjetoCor.CorR = 71;
                    objetoSelecionado.ObjetoCor.CorG = 255;
                    objetoSelecionado.ObjetoCor.CorB = 51;
                }

            }
            else if (e.Key == Key.B)
            {
                if (objetoSelecionado != null)
                {
                    objetoSelecionado.ObjetoCor.CorR = 51;
                    objetoSelecionado.ObjetoCor.CorG = 71;
                    objetoSelecionado.ObjetoCor.CorB = 255;
                }
            }
            // Console.WriteLine(" [Espaço  ] N3-Exe06: adiciona vértice ao polígono. ");
            // como eh ADICIONA, so vai funcionar se ja tiver um poligono sendo desenhado
            else if (e.Key == Key.Space && objetoSelecionado is Poligono)
            {
                if (objetoSelecionado != null)
                {
                    Poligono aux = (Poligono)objetoSelecionado;
                    objetoId = Utilitario.charProximo(objetoId);
                    obj_Ponto = new Ponto(objetoId, null, new Ponto4D(mouseX, mouseY, 0));
                    aux.addPonto(obj_Ponto);
                }
            }
            //Console.WriteLine(" [  S     ] N3-Exe07: alterna entre aberto e fechado o polígono selecionado. ");
             else if (e.Key == Key.S)
            {
                if(objetoSelecionado is Poligono){
                    if( objetoSelecionado.PrimitivaTipo == PrimitiveType.LineStrip)
                        objetoSelecionado.PrimitivaTipo = PrimitiveType.LineLoop;
                    else
                        objetoSelecionado.PrimitivaTipo = PrimitiveType.LineStrip;
                }

            }
            //Console.WriteLine(" [  C     ] N3-Exe04: remove o polígono selecionado. ");
            else if (e.Key == Key.C)
            {
                if(objetoSelecionado is Poligono){
                   objetosLista.Remove(objetoSelecionado);
                }

            }
            else if (e.Key == Key.Enter)
            {
                //verifica se ainda estava desenhando com ponto auxiliar
                //se sim, remove o ponto auxiliar
                if (estaDesenhandoPoligono && objetoSelecionado is Poligono)
                {
                    Poligono aux = (Poligono)objetoSelecionado;
                    //remove pontos auxiliares do desenho, o primeiro(para mostrar a linha) e o ultimo do rastro
                    aux.removePontoFinal();
                    aux.removePrimeiroPontoAuxiliar();
                    //objetosLista.Add(aux);
                    //TODO: talvez remover esta linha
                    estaDesenhandoPoligono = false;
                }
            }
            else
                Console.WriteLine(" __ Tecla não implementada.");
        }

        //TODO: não está considerando o NDC
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {

            if (estaDesenhandoPoligono && objetoSelecionado != null)
            {
                mouseX = e.Position.X; mouseY = 750 - e.Position.Y;
                if (mouseMoverPto && (objetoSelecionado != null))
                {
                    if (objetoSelecionado is Poligono)
                    {
                        Poligono aux = (Poligono)objetoSelecionado;
                        aux.getPontoFinal().X = mouseX;
                        aux.getPontoFinal().Y = mouseY;
                    }

                }
            }
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {

            if (e.Button == MouseButton.Left && estaDesenhandoPoligono)//continua desenho do poligono
            {

                //ajuste copiado do N2
                mouseX = e.Position.X; mouseY = 750 - e.Position.Y;

                //cria objeto Ponto para fazer parte do Poligono
                objetoId = Utilitario.charProximo(objetoId);
                obj_Ponto = new Ponto(objetoId, null, new Ponto4D(mouseX, mouseY, 0));
               


                //adiciona novo ponto ao poligono que ja esta sendo desenhado
                obj_Poligono.addPonto(obj_Ponto);

            }
            else if (e.Button == MouseButton.Left && !estaDesenhandoPoligono)
            {//se clicou e nao esta desenhando, é um novo poligono

                //cria o novo poligon e seta que agora comecou o novo desenho
                obj_Poligono = new Poligono(Utilitario.charProximo(objetoId), null);
                estaDesenhandoPoligono = true;

                //ajuste copiado do N2
                mouseX = e.Position.X; mouseY = 750 - e.Position.Y;

                //cria objeto Ponto para fazer parte do Poligono
                objetoId = Utilitario.charProximo(objetoId);
                obj_Ponto = new Ponto(objetoId, null, new Ponto4D(mouseX, mouseY, 0));
                obj_Poligono.addPonto(obj_Ponto);
                objetoSelecionado = obj_Poligono;
                objetosLista.Add(obj_Poligono);

            }
            if (e.Button == MouseButton.Right)
            {
                estaDesenhandoPoligono = false;
                obj_Poligono.removePontoFinal();


            }
            //base.OnMouseDown(e);
        }

#if CG_Gizmo
        private void Sru3D()
        {
#if CG_OpenGL
            GL.LineWidth(1);
            GL.Begin(PrimitiveType.Lines);
            // GL.Color3(1.0f,0.0f,0.0f);
            GL.Color3(Convert.ToByte(255), Convert.ToByte(0), Convert.ToByte(0));
            GL.Vertex3(0, 0, 0); GL.Vertex3(200, 0, 0);
            // GL.Color3(0.0f,1.0f,0.0f);
            GL.Color3(Convert.ToByte(0), Convert.ToByte(255), Convert.ToByte(0));
            GL.Vertex3(0, 0, 0); GL.Vertex3(0, 200, 0);
            // GL.Color3(0.0f,0.0f,1.0f);
            GL.Color3(Convert.ToByte(0), Convert.ToByte(0), Convert.ToByte(255));
            GL.Vertex3(0, 0, 0); GL.Vertex3(0, 0, 200);
            GL.End();
#endif
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
