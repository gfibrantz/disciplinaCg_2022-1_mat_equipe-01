/**
  Autor: Dalton Solano dos Reis
**/

#define CG_Debug
#define CG_OpenGL
// #define CG_DirectX

using System;
using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;
using System.Collections.Generic;

namespace gcgcg
{
    internal class Poligono : ObjetoGeometria
    {
        protected List<Ponto> pontos = new List<Ponto>();
        bool estaSendoDesenhado;
        public Poligono(char rotulo, Objeto paiRef) : base(rotulo, paiRef)
        {
            estaSendoDesenhado = true;
        }

        public Ponto4D getPontoFinal()
        {
            
            return pontos[pontos.Count - 1].getPonto();
        }
        private void removePontoFinal()
        {
            pontos.RemoveAt(pontos.Count - 1);

            //TODO: remover gambiarra
            //base.pontosLista.Add(novo.get);
            base.PontosRemoverUltimo();
        }
        public void addPonto(Ponto novo)
        {
            pontos.Add(novo);
            //base.pontosLista.Add(novo.get);
            base.PontosAdicionar(novo.getPonto());

            if (pontos.Count.Equals(1)){
                pontos.Add(novo);
                //gambi                 
                base.PontosAdicionar(novo.getPonto());

            }
        }
        private void removePrimeiroPontoAuxiliar()
        {
            pontos.RemoveAt(0);
            //TODO: isso é gambi
            base.pontosLista.RemoveAt(0);

        }
        public void finalizaDesenhoPoligono(){
            this.estaSendoDesenhado = false;
            removePontoFinal();
            removePrimeiroPontoAuxiliar();
        }
        public string imprimePontos()
        {
            string retorno;
            retorno = "__ Objeto Poligono: " + base.rotulo + "\n";
            for (var i = 0; i < pontos.Count; i++)
            {
                retorno += "P" + i + "[" + pontos[i].getX() + "," + pontos[i].getY() + "," + pontos[i].getZ() + "," + pontos[i].getW() + "]" + "\n";
            }
            return (retorno);
        }

        public void setColor(byte r, byte g, byte b)
        {
            base.ObjetoCor.CorR = r;
            base.ObjetoCor.CorG = g;
            base.ObjetoCor.CorB = b;


        }


        protected override void DesenharObjeto()
        {
#if CG_OpenGL && !CG_DirectX

            foreach (Ponto pto in pontos)
            {
                pto.Desenhar();
            }
            GL.Color3(base.ObjetoCor.CorR,base.ObjetoCor.CorG, base.ObjetoCor.CorB);
            GL.Begin(base.PrimitivaTipo);
            foreach (Ponto pto in pontos)
            {
                GL.Vertex2(pto.getPonto().X, pto.getPonto().Y);
            }
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
            retorno = "__ Objeto Poligono: " + base.rotulo + "\n";
            for (var i = 0; i < pontos.Count; i++)
            {
                retorno += "P" + i + "[" + pontos[i].getX() + "," + pontos[i].getY() + "," + pontos[i].getZ() + "," + pontos[i].getW() + "]" + "\n";
            }
            return (retorno);
        }
#endif

    }
}