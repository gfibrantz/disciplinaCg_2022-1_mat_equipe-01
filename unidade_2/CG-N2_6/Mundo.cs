﻿/**
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
        private bool bBoxDesenhar = false;
        int mouseX, mouseY;   //TODO: achar método MouseDown para não ter variável Global
        private bool mouseMoverPto = false;
        //private Retangulo obj_Retangulo;
        
       
        private Spline obj_spline;

        SegReta[] obj_poliedroDeControle = new SegReta[3];
#if CG_Privado
    private Privado_SegReta obj_SegReta;
    private Privado_Circulo obj_Circulo;
#endif

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            camera.xmin = -400; camera.xmax = 400; camera.ymin = -400; camera.ymax = 400;

            Console.WriteLine(" --- Ajuda / Teclas: ");
            Console.WriteLine(" [  H     ] mostra teclas usadas. ");

          

            objetoId = Utilitario.charProximo(objetoId);
            obj_spline = new Spline(objetoId, null);           
           
            objetosLista.Add(obj_spline);
            objetoSelecionado = obj_spline;

            objetoId = Utilitario.charProximo(objetoId);
            obj_poliedroDeControle[0] = new SegReta(objetoId, null, obj_spline.getPontoSuperiorEsquerdo(), obj_spline.getPontoSuperiorDireito());
            objetosLista.Add( obj_poliedroDeControle[0]);
            objetoSelecionado =  obj_poliedroDeControle[0];

            objetoId = Utilitario.charProximo(objetoId);
            obj_poliedroDeControle[1] = new SegReta(objetoId, null, obj_spline.getPontoSuperiorEsquerdo(), obj_spline.getPontoInferiorEsquerdo());
            objetosLista.Add( obj_poliedroDeControle[1]);
            objetoSelecionado =  obj_poliedroDeControle[1];

            objetoId = Utilitario.charProximo(objetoId);
            obj_poliedroDeControle[2] = new SegReta(objetoId, null, obj_spline.getPontoSuperiorDireito(), obj_spline.getPontoInferiorDireito());
            objetosLista.Add( obj_poliedroDeControle[2]);
            objetoSelecionado =  obj_poliedroDeControle[2];
                    



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
            GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
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
            else if (e.Key == Key.X)
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
            else if (e.Key == Key.Number1)
                obj_spline.setPontoEscolhido(1);
            else if (e.Key == Key.Number2)
                obj_spline.setPontoEscolhido(2);
            else if (e.Key == Key.Number3)
                obj_spline.setPontoEscolhido(3);    
            else if (e.Key == Key.Number4)
                obj_spline.setPontoEscolhido(4);
            else if (e.Key == Key.C)
                obj_spline.movePontoSelecionadoCima();
            else if (e.Key == Key.B)
                obj_spline.movePontoSelecionadoBaixo();
            else if (e.Key == Key.E){
                obj_spline.movePontoSelecionadoEsquerda();
            }else if (e.Key == Key.D){
                obj_spline.movePontoSelecionadoDireita();                           
            }else if (e.Key == Key.R){
                obj_spline.voltaEstadoInicial();                           
            }else if (e.Key == Key.KeypadPlus||e.Key ==  Key.Plus ||e.Key ==  Key.KeypadAdd ){
                obj_spline.aumentaQtdPontosCalculados();                           
            }else if (e.Key == Key.KeypadMinus ||e.Key ==  Key.Minus || e.Key ==  Key.KeypadSubtract ){
                obj_spline.diminuiQtdPontosCalculados();                           
            }else
                Console.WriteLine(" __ Tecla não implementada.");
        }

        //TODO: não está considerando o NDC
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            mouseX = e.Position.X; mouseY = 600 - e.Position.Y; // Inverti eixo Y
            if (mouseMoverPto && (objetoSelecionado != null))
            {
                objetoSelecionado.PontosUltimo().X = mouseX;
                objetoSelecionado.PontosUltimo().Y = mouseY;
            }
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


            
        }
#endif
    }
    class Program
    {
        static void Main(string[] args)
        {
            Mundo window = Mundo.GetInstance(600, 600);
            window.Title = "CG_N2_Splinezera";
            window.Run(1.0 / 60.0);
        }
    }
}
