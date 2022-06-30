/**
  Autor: Dalton Solano dos Reis
**/

#define CG_Debug
#define CG_OpenGL
// #define CG_DirectX

using System;
using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{
    internal class Ponto : ObjetoGeometria
    {
        int tamanhoDoPonto;
        
        public Ponto(char rotulo, Objeto paiRef, Ponto4D pto) : base(rotulo, paiRef)
        {
            base.PontosAdicionar(pto);
            tamanhoDoPonto = 10;
            base.PrimitivaTipo = PrimitiveType.Points;
           

        }
        public double getX(){
            return base.PontosUltimo().X;
        }
          public double getY(){
            return base.PontosUltimo().Y;
        }
          public double getZ(){
            return base.PontosUltimo().Z;
        }
          public double getW(){
            return base.PontosUltimo().W;
        }
        public void setTamanhoPonto(int novoTamanho){
            this.tamanhoDoPonto = novoTamanho;
        }
        public Ponto4D getPonto(){
            return base.PontosUltimo();
        }
        public void setPonto(Ponto4D novo){
            base.PontosAlterar(novo, 0);
        }
        public void setPonto(double x , double y){
            base.PontosUltimo().X = x;
            base.PontosUltimo().Y = y;

        }

        protected override void DesenharObjeto()
        {
#if CG_OpenGL && !CG_DirectX
            GL.PointSize(tamanhoDoPonto);
            GL.Begin(base.PrimitivaTipo);
            //como um ponto só tem UM ponto mesmo, nao ha necessidade do foreach
            GL.Vertex2(pontosLista[0].X, pontosLista[0].Y);

            GL.End();
#elif CG_DirectX && !CG_OpenGL
    Console.WriteLine(" .. Coloque aqui o seu código em DirectX");
#elif (CG_DirectX && CG_OpenGL) || (!CG_DirectX && !CG_OpenGL)
    Console.WriteLine(" .. ERRO de Render - escolha OpenGL ou DirectX !!");
#endif
        }

        //TODO: melhorar para exibir não só a lista de pontos (geometria), mas também a topologia ... poderia ser listado estilo OBJ da Wavefrom
#if CG_Debug
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto Ponto: " + base.rotulo + "\n";
          
            retorno += "[" + pontosLista[0].X + "," + pontosLista[0].Y + "," + pontosLista[0].Z + "," + pontosLista[0].W + "]" + "\n";
            
            return (retorno);
        }
#endif

    }
}