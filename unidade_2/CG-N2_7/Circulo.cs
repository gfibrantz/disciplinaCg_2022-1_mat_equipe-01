#define CG_Debug

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{
    internal class Circulo : ObjetoGeometria
    {
        private double raio;
        private Ponto4D pontoCentral;
        private bool plotaPontoCentral;
        public Circulo(char rotulo, Objeto paiRef, Ponto4D pontoCentro, double raio, bool plotaPtoCentral = false) : base(rotulo, paiRef)
        {
            base.PrimitivaTipo = PrimitiveType.LineLoop;
            pontoCentral = pontoCentro;
            this.raio = raio;
            plotaPontoCentral = plotaPtoCentral;
            //se quiser, tranformar depois em metodo para escolha de qtd de pontos
            atualizaPontos();
        }
        public Ponto4D getPontoCentral()
        {
            return this.pontoCentral;
        }
        public void setPontoCentral(double x, double y)
        {
           this.pontoCentral.X = x;
            this.pontoCentral.Y = y;
            atualizaPontos();
        }
        public void atualizaPontos()
        {
            int qtdPontos = 72;
            Ponto4D ponto = new Ponto4D();
            base.PontosRemoverTodos();
            for (int angulo = 0; angulo < 360; angulo += (360 / qtdPontos))
            {
                ponto = Matematica.GerarPtosCirculo(angulo, raio);
                ponto += pontoCentral;
                base.PontosAdicionar(ponto);
            }
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
            if (this.plotaPontoCentral)
            {
                GL.PointSize(10f);
                GL.Begin(PrimitiveType.Points);
               
                GL.Vertex2(this.pontoCentral.X, this.pontoCentral.Y);
                
                GL.End();
            }
        }

        public double getRaio(){
            return this.raio;
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