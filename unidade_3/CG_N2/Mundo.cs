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
        private bool alterandoVertice = false;
        private Retangulo obj_Retangulo;
        private Poligono obj_PoligonoTemp;
        private Ponto obj_Ponto;
        bool estaDesenhandoPoligono = false;
        private double rotacaoX = 0;
        private double rotacaoY = 0;
        private double rotacaoZ = 0;
#if CG_Privado
    private Privado_SegReta obj_SegReta;
    private Privado_Circulo obj_Circulo;

#endif

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            camera.xmin = 0; camera.xmax = 600; camera.ymin = 0; camera.ymax = 600;

            Console.WriteLine(" --- Ajuda / Teclas: ");
            Console.WriteLine(" [  H     ] mostra teclas usadas. ");

            objetoId = Utilitario.charProximo(objetoId);
            obj_Retangulo = new Retangulo(objetoId, null, new Ponto4D(50, 50, 0), new Ponto4D(150, 150, 0));
            obj_Retangulo.ObjetoCor.CorR = 255; obj_Retangulo.ObjetoCor.CorG = 0; obj_Retangulo.ObjetoCor.CorB = 255;
            objetosLista.Add(obj_Retangulo);

            obj_Ponto =  new Ponto('@', null, new Ponto4D(0,0));
            

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
            //else if (e.Key == Key.V)
                //mouseMoverPto = !mouseMoverPto;  //TODO: falta atualizar a BBox do objeto
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
            else if (e.Key == Key.Space )
            {
                if (obj_PoligonoTemp != null && estaDesenhandoPoligono)
                {
                    obj_PoligonoTemp.addPonto( new Ponto4D(mouseX, mouseY));
                }
                else//se nao tem objeto selecionado, entao esta criando um novo
                    criaPoligono();
            }
            //Console.WriteLine(" [  S     ] N3-Exe07: alterna entre aberto e fechado o polígono selecionado. ");
            else if (e.Key == Key.S)
            {
                if (objetoSelecionado is Poligono)
                {
                    if (objetoSelecionado.PrimitivaTipo == PrimitiveType.LineStrip)
                        objetoSelecionado.PrimitivaTipo = PrimitiveType.LineLoop;
                    else
                        objetoSelecionado.PrimitivaTipo = PrimitiveType.LineStrip;
                }

            }
            //Console.WriteLine(" [  C     ] N3-Exe04: remove o polígono selecionado. ");
            else if (e.Key == Key.C)
            {
                if (objetoSelecionado is Poligono)
                {
                    objetosLista.Remove(objetoSelecionado);                   
                    estaDesenhandoPoligono = false;
                    objetoSelecionado = null;
                    obj_PoligonoTemp = null;
                }

            }
            // Console.WriteLine(" [  P     ]         : exibe os vértices do polígono selecionado. ");
            else if (e.Key == Key.P)//mostra as coordenadas do poligono selecionado
            {
                if (objetoSelecionado is Poligono )
                {
                    obj_PoligonoTemp = (Poligono)objetoSelecionado;

                    Console.WriteLine(objetoSelecionado.ToString());
                }

            }
            //Console.WriteLine(" [  D     ] N3-Exe05: remove o vértice do polígono selecionado que estiver mais perto do mouse. ");
            else if (e.Key == Key.D)
            {
                if(objetoSelecionado != null){
                    obj_PoligonoTemp = (Poligono)objetoSelecionado;
                   
                   //encontra o vertice mais proximo e remove
                    obj_PoligonoTemp.removePonto(obj_PoligonoTemp.getVerticeMaisProximo(mouseX,mouseY));
                }
            }
            //    Console.WriteLine(" [  V     ] N3-Exe05: move o vértice do polígono selecionado que estiver mais perto do mouse. ");
            else if (e.Key == Key.V)
            {
                
                if(objetoSelecionado != null){
                    obj_PoligonoTemp = (Poligono)objetoSelecionado;
                   
                   //encontra o vertice mais proximo e remove
                    
                    alterandoVertice= true;
                    //TODO: FUNCIONA?
                    obj_Ponto.setPonto(obj_PoligonoTemp.getVerticeMaisProximo(mouseX,mouseY));
                }
            }

            //TODO: remover, adicionado para testes
             else if (e.Key == Key.K)
            {
                objetoSelecionado = (Poligono)objetosLista[1];
            }
            // Console.WriteLine(" [  M     ]         : exibe matriz de transformação do polígono selecionado. ");
             else if (e.Key == Key.M)
            {
                  Console.WriteLine(objetoSelecionado.Transformacao4D.ToString());
            }


            else if (e.Key == Key.Enter)
            {
                //verifica se ainda estava desenhando com ponto auxiliar
                //se sim, remove o ponto auxiliar
                if (estaDesenhandoPoligono && obj_PoligonoTemp !=null)
                {
                   
                    //remove pontos auxiliares do desenho, o primeiro(para mostrar a linha) e o ultimo do rastro
                    obj_PoligonoTemp.finalizaDesenhoPoligono();
                   
                    
                    //deseleciona poligono
                    objetoSelecionado = null;
                    obj_PoligonoTemp = null;
                    estaDesenhandoPoligono = false;

                }
                else if(alterandoVertice)
                {
                    alterandoVertice = !alterandoVertice;

                }
            }
            //      Console.WriteLine(" [  X     ]         : rotação entorno do eixo X. ");
            else if (e.Key == Key.X)
            {
                if(objetoSelecionado!=null){
                    rotacaoX+=0.2;
                    objetoSelecionado.Transformacao4D.AtribuirRotacaoX(rotacaoX);

                }
            }
            //Console.WriteLine(" [  Y     ]         : rotação entorno do eixo Y. ");     
            else if (e.Key == Key.Y)
            {
                if(objetoSelecionado!=null){
                    rotacaoY+= 0.2;
                    objetoSelecionado.Transformacao4D.AtribuirRotacaoY(rotacaoY);
                    obj_PoligonoTemp = (Poligono)objetoSelecionado;
                    obj_PoligonoTemp.AtualizaBBox();

                }
            }
            // Console.WriteLine(" [  Z     ]         : rotação entorno do eixo Z. ");
            else if (e.Key == Key.Z)
            {
                if(objetoSelecionado!=null){
                    rotacaoZ+=0.2;

                    objetoSelecionado.Transformacao4D.AtribuirRotacaoZ(rotacaoZ);

                }
            }
            else if (e.Key == Key.A)
            {
                //quando muda a selecao, resseta os parametros
                
               zeraRotacoes();
            }
            // Console.WriteLine(" [  I     ]         : aplica a matriz Identidade no polígono selecionado. ");
            else if (e.Key == Key.I)
            {
                //matriz identidade volta "tudo pro lugar",entao tmb zera rotacoes
                objetoSelecionado.Transformacao4D.AtribuirIdentidade();
                zeraRotacoes();
               
            }
            else
                Console.WriteLine(" __ Tecla não implementada.");
        }

        //TODO: não está considerando o NDC
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            mouseX = e.Position.X; mouseY = 600 - e.Position.Y;

            if (estaDesenhandoPoligono && obj_PoligonoTemp != null)
            {
                          
                if (mouseMoverPto)
                {
                     
                    obj_PoligonoTemp.getPontoFinal().X = mouseX;
                    obj_PoligonoTemp.getPontoFinal().Y = mouseY;                    

                }
            }//se nao esta desenhando, esta ajustando um vertice especifico
            if (alterandoVertice)
            {
                obj_Ponto.setPonto(mouseX, mouseY);
                obj_PoligonoTemp.AtualizaBBox();
                     

            }
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {

            if (e.Button == MouseButton.Left && estaDesenhandoPoligono)//continua desenho do poligono
            {

                //ajuste copiado do N2
               // mouseX = e.Position.X; mouseY = 750 - e.Position.Y;

                
                //adiciona novo ponto ao poligono que ja esta sendo desenhado
                obj_PoligonoTemp.addPonto(new Ponto4D(mouseX, mouseY));

            }
            else if (e.Button == MouseButton.Left && !estaDesenhandoPoligono)
            {//se clicou e nao esta desenhando, é um novo poligono

                //cria o novo poligon e seta que agora comecou o novo desenho
                // obj_PoligonoTemp = new Poligono(Utilitario.charProximo(objetoId), null);
                // estaDesenhandoPoligono = true;

                // //ajuste copiado do N2
                // mouseX = e.Position.X; mouseY = 750 - e.Position.Y;

                // //cria objeto Ponto para fazer parte do Poligono
                // objetoId = Utilitario.charProximo(objetoId);
                // obj_Ponto = new Ponto(objetoId, null, new Ponto4D(mouseX, mouseY, 0));
                // obj_PoligonoTemp.addPonto(obj_Ponto);
                // objetoSelecionado = obj_PoligonoTemp;
                // objetosLista.Add(obj_PoligonoTemp);

                criaPoligono();

            }
            if (e.Button == MouseButton.Right)
            {
                estaDesenhandoPoligono = false;
                obj_PoligonoTemp.finalizaDesenhoPoligono();
                obj_PoligonoTemp = null;
                objetoSelecionado=null;
                alterandoVertice = false;


            }
            //base.OnMouseDown(e);
        }
        public void criaPoligono()
        {
             //cria o novo poligon e seta que agora comecou o novo desenho
            obj_PoligonoTemp = new Poligono(Utilitario.charProximo(objetoId), null);
            estaDesenhandoPoligono = true;
            if(objetoSelecionado!=null)
            {
                objetoSelecionado.FilhoAdicionar(obj_PoligonoTemp);
            }
           
            
            objetoSelecionado = obj_PoligonoTemp;            
            

          

            //cria objeto Ponto para fazer parte do Poligono
          
            obj_PoligonoTemp.addPonto(new Ponto4D(mouseX, mouseY));
            
            //TODO: adiciona esse novo poligono na lista? ou só quando encerrar desenho?
            objetosLista.Add(obj_PoligonoTemp);
        }
        public void zeraRotacoes(){
            rotacaoX = 0;
            rotacaoY = 0;
            rotacaoZ = 0;
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
