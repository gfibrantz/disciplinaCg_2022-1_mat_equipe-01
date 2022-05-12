using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{
  internal class SegReta : ObjetoGeometria
  {
    public SegReta(char rotulo, Objeto paiRef, Ponto4D ptoInicial, Ponto4D ptoFinal) : base(rotulo, paiRef)
    {
      base.PrimitivaTipo = PrimitiveType.Lines;
      base.PontosAdicionar(ptoInicial);
     // base.PontosAdicionar(new Ponto4D(ptoInicial.X, ptoInicial.Y));
      base.PontosAdicionar(ptoFinal);
      //base.PontosAdicionar(new Ponto4D(ptoInicial.X, ptoInicial.Y));
    }

    protected override void DesenharObjeto()
    {
      GL.Begin(base.PrimitivaTipo);
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