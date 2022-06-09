/**
  Autor: Dalton Solano dos Reis
**/

#define CG_Debug

using System.Collections.Generic;
using CG_Biblioteca;
using System;

namespace gcgcg
{
    internal abstract class ObjetoGeometria : Objeto
    {
        protected List<Ponto4D> pontosLista = new List<Ponto4D>();

        public ObjetoGeometria(char rotulo, Objeto paiRef) : base(rotulo, paiRef) { }

        protected override void DesenharGeometria()
        {
            DesenharObjeto();
        }
        protected abstract void DesenharObjeto();
        public void PontosAdicionar(Ponto4D pto)
        {
            pontosLista.Add(pto);
            if (pontosLista.Count.Equals(1))
                base.BBox.Atribuir(pto);
            else
                base.BBox.Atualizar(pto);
            base.BBox.ProcessarCentro();
        }
        public void AtualizaBBox()
        {
            if (pontosLista.Count >= 1)
            {
                //primeiros limites com os do p0
                BBox.Atribuir(Transformacao4D.MultiplicarPonto(pontosLista[0]));
                foreach (Ponto4D pto in pontosLista)
                {
                    BBox.Atualizar(Transformacao4D.MultiplicarPonto(pto));
                }
                BBox.ProcessarCentro();
            }

        }

        public void PontosRemoverUltimo()
        {
            pontosLista.RemoveAt(pontosLista.Count - 1);
        }

        protected void PontosRemoverTodos()
        {
            pontosLista.Clear();
        }

        public Ponto4D PontosUltimo()
        {
            return pontosLista[pontosLista.Count - 1];
        }
    

        public void PontosAlterar(Ponto4D pto, int posicao)
        {
            pontosLista[posicao] = pto;
        }
        public void PontoRemover(Ponto4D pto)
        {
            pontosLista.Remove(pto);
        }
        public Ponto4D getVerticeMaisProximo(double xClick, double yClick)
        {
            Ponto4D maisProximo = PontosUltimo();
            double distMenor = Double.MaxValue;

            //analisa todos os pontos do poligono
            foreach (Ponto4D pto in pontosLista)
            {
                double distAtual = Matematica.DistanciaEntrePontos(pto.X, pto.Y, xClick, yClick);
                //se a distancia atual é menor que a salva
                if (distAtual < distMenor)
                {
                    distMenor = distAtual;
                    maisProximo = pto;
                }

            }

            return maisProximo;

        }
          public bool getClicouDentro(double xClick, double yClick){
           
            if(base.PrimitivaTipo == OpenTK.Graphics.OpenGL.PrimitiveType.LineLoop){
                return Matematica.ScanLineTop(pontosLista, xClick, yClick,true);
            }

             return Matematica.ScanLineTop(pontosLista, xClick, yClick,false);          

        }

#if CG_Debug
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto: " + base.rotulo + "\n";
            for (var i = 0; i < pontosLista.Count; i++)
            {
                retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
            }
            return (retorno);
        }
#endif

    }
}