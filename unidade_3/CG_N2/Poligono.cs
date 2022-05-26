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
        public Poligono(char rotulo, Objeto paiRef) : base(rotulo, paiRef)
        {

        }

        public Ponto4D getPontoFinal()
        {
            return pontos[pontos.Count - 1].getPonto();
        }
        public void addPonto(Ponto novo)
        {
            pontos.Add(novo);

            if (pontos.Count.Equals(1))
                pontos.Add(novo);
        }


        protected override void DesenharObjeto()
        {
#if CG_OpenGL && !CG_DirectX

            foreach (Ponto pto in pontos)
            {
                pto.Desenhar();
            }

            GL.Begin(PrimitiveType.LineLoop);
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
            for (var i = 0; i < pontosLista.Count; i++)
            {
                retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
            }
            return (retorno);
        }
#endif

    }
}