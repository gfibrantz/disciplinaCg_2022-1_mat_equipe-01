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
  internal class Chao : ObjetoGeometria
  {
    public Chao(char rotulo, Objeto paiRef) : base(rotulo, paiRef)
    {
        base.PontosAdicionar(new Ponto4D(0, 0, 0));
        base.PontosAdicionar(new Ponto4D(16, 0, 0));
            base.PontosAdicionar(new Ponto4D(16, 0, 20));
            base.PontosAdicionar(new Ponto4D(0, 0, 20));

    }

    protected override void DesenharObjeto()
    {
#if CG_OpenGL && !CG_DirectX
GL.LineWidth(5);  
      GL.Begin(base.PrimitivaTipo);
      foreach (Ponto4D pto in pontosLista)
      {
        GL.Vertex3(pto.X, pto.Y,pto.Z);
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
      retorno = "__ Objeto Retangulo: " + base.rotulo + "\n";
      for (var i = 0; i < pontosLista.Count; i++)
      {
        retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
      }
      return (retorno);
    }
#endif

  }
}