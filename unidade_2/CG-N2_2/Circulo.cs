#define CG_Debug

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{
    internal class Circulo : ObjetoGeometria
    {
        private double raio;
        Ponto4D pontoCentral;
        public Circulo(char rotulo, Objeto paiRef, Ponto4D pontoCentro, double raio) : base(rotulo, paiRef)
        {
            base.PrimitivaTipo = PrimitiveType.Points;
            pontoCentral = pontoCentro;
            this.raio = raio;
            //se quiser, tranformar depois em metodo para escolha de qtd de pontos
            int qtdPontos = 72;
            Ponto4D ponto = new Ponto4D();
            for(int angulo = 0; angulo < 360; angulo+=(360/qtdPontos)){
                ponto = Matematica.GerarPtosCirculo(angulo,raio);
                ponto += pontoCentral;
                base.PontosAdicionar(ponto);
            }
                      // coisa linda
        }

        protected override void DesenharObjeto()
        {
           // GL.PointSize(3f);
            //GL.Begin(PrimitiveType.Points);
            
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
            retorno = "__ Objeto Circulo: " + base.rotulo + "\n";
            for (var i = 0; i < pontosLista.Count; i++)
            {
                retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
            }
            return (retorno);
        }
#endif

    }
}