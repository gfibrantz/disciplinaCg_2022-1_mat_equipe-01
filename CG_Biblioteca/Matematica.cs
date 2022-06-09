using System;
using System.Collections.Generic;

namespace CG_Biblioteca
{
    /// <summary>
    /// Classe com funções matemáticas.
    /// </summary>
    public abstract class Matematica
    {
        /// <summary>
        /// Função para calcular um ponto sobre o perímetro de um círculo informando um ângulo e raio.
        /// </summary>
        /// <param name="angulo"></param>
        /// <param name="raio"></param>
        /// <returns></returns>
        public static Ponto4D GerarPtosCirculo(double angulo, double raio)
        {
            Ponto4D pto = new Ponto4D();
            pto.X = (raio * Math.Cos(Math.PI * angulo / 180.0));
            pto.Y = (raio * Math.Sin(Math.PI * angulo / 180.0));
            pto.Z = 0;
            return (pto);
        }

        public static double GerarPtosCirculoSimetrico(double raio)
        {
            return (raio * Math.Cos(Math.PI * 45 / 180.0));
        }

        public static double DistanciaEntrePontos(double x1, double y1, double x2, double y2)
        {
            //aqui fazemos sem raiz para nao fazer calculos desnecessarios
            return Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2);
        }
        public static double calculaTi ( double yClick, double y1, double y2)
        {
            return ((yClick - y1) / (y2 - y1));
        }
        public static double calculaXi ( double ti, double x1, double x2)
        {
            return (x1 + ((x2-x1)*ti));
        }
        public static bool ScanLineTop(List<Ponto4D> pontosPoligono, double xClick, double yClick, bool ehLineLoop)
        {
            int qtdIntersec = 0;
            for (int i = 0; i < pontosPoligono.Count; i++)
            {
                double ti = 0;
                int indexP1 = i;
                int indexP2 = i+1;
                //se estamos no ultimo ponto, entao p2 é o primeiro ponto do poligono
                if(i == pontosPoligono.Count - 1 && ehLineLoop)
                    indexP2 = 0;
                else if(i == pontosPoligono.Count - 1 && !ehLineLoop)//se é o ultimo, mas nao é LineLoop cai fora
                    break;
                
               //se nao estamos no ultimo ponto, processa normal
               
                //1o calculamos ti
                ti = calculaTi(yClick, pontosPoligono[indexP1].Y,pontosPoligono[indexP2].Y);
                
                //2o com ti valido, procuramos xi
                if (ti >= 0 && ti <= 1)
                {
                    double xi = calculaXi(ti, pontosPoligono[indexP1].X,pontosPoligono[indexP2].X);

                    //3o ao encontrar o xi verifica se ele esta a direita do x do clique
                    //se estiver, ponto de interseccao que deve ser contado
                    if(xi != xClick){
                        if(xi > xClick && 
                        yClick > Math.Min(pontosPoligono[indexP1].Y,pontosPoligono[indexP2].Y) &&
                        yClick <= Math.Max(pontosPoligono[indexP1].Y,pontosPoligono[indexP2].Y))
                            qtdIntersec++;
                    }

                }              

            }
            //se qtd ptos de intersec par, esta fora
            if (qtdIntersec % 2 == 0)
                return false;

            return true;
        }


    }
}