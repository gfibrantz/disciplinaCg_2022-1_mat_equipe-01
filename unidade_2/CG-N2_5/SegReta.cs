using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{
  //famoso sr.palito
  internal class SegReta : ObjetoGeometria
  {

    int raio;
    int angulo;
    public SegReta(char rotulo, Objeto paiRef, Ponto4D ptoInicial, Ponto4D ptoFinal) : base(rotulo, paiRef)
    {
      base.PrimitivaTipo = PrimitiveType.Lines;
      base.PontosAdicionar(ptoInicial);
      this.raio = 100;
      this.angulo =45;
      Ponto4D ponto = Matematica.GerarPtosCirculo(angulo,raio);
      base.PontosAdicionar(ponto);
      
      
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
    public void aumentaRaio(){
     
      this.raio+=1;
      atualizaPontoFinal();
    }

    public void diminuiRaio(){
      if( raio >1){
        this.raio-=1;
        atualizaPontoFinal();     

      }
    }
    public void atualizaPontoFinal(){
        Ponto4D novoPonto = Matematica.GerarPtosCirculo(this.angulo,this.raio);
        novoPonto.X+=pontosLista[0].X;
        novoPonto.Y+= pontosLista[0].Y;
        pontosLista[1] = novoPonto;
    }
    public void aumentaAngulo(){
      if(angulo ==360)
        this.angulo = 0;
      this.angulo++;

      atualizaPontoFinal();

    }
    public void diminuiAngulo(){
      if(angulo ==0)
        this.angulo =359;
      this.angulo--;
      atualizaPontoFinal();
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