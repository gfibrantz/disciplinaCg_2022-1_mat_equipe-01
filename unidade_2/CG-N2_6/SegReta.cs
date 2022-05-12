using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;
using System;

namespace gcgcg
{
  //famoso sr.palito
  internal class SegReta : ObjetoGeometria
  {

    public SegReta(char rotulo, Objeto paiRef, Ponto4D ptoInicial, Ponto4D ptoFinal) : base(rotulo, paiRef)
    {
      base.PrimitivaTipo = PrimitiveType.Lines;
      base.PontosAdicionar(ptoInicial);      
      base.PontosAdicionar(ptoFinal);
      base.PrimitivaTamanho = 3;
      
      
    }


    public void deslocaEsquerda(){
     
      pontosLista[0].X-=2;
      pontosLista[0].X-=2;
      pontosLista[1].X-=2;
      pontosLista[1].X-=2;

    }
    public void deslocaDireita(){
     
      pontosLista[0].X+=2;
      pontosLista[0].X+=2;
      pontosLista[1].X+=2;
      pontosLista[1].X+=2;

    }
    
    protected override void DesenharObjeto()
    {
      
      GL.Begin(base.PrimitivaTipo);
      GL.Color3(Convert.ToByte(0), Convert.ToByte(255), Convert.ToByte(255));
      foreach (Ponto4D pto in pontosLista)
      {
        GL.Vertex2(pto.X, pto.Y);
      }
      GL.End();
    }
    
    //TODO: melhorar para exibir não só a lista de pontos (geometria), mas também a topologia ... poderia ser listado estilo OBJ da Wavefrom
#if CG_Debug
    public override string ToString()
    {
      string retorno;
      retorno = "__ Segmento de Reta: " + base.rotulo + "\n";
      for (var i = 0; i < pontosLista.Count; i++)
      {
        retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
      }
      return (retorno);
    }
#endif

  }
}