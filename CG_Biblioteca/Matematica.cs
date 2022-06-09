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
        public static bool ScanLineTop(List<Ponto4D> poli, double xClick, double yClick, bool ehLoop)
        {
            int qtdIntersec = 0;
            for (int i = 0; i < poli.Count - 1; i++)
            {
                double t = 0;
                if (i == poli.Count - 1)
                {
                    if (ehLoop)
                    {
                        t = ((yClick - poli[i].Y) / (poli[0].Y - poli[i].Y));
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    t = ((yClick - poli[i].Y) / (poli[i + 1].Y - poli[i].Y));
                }
                if (t >= 0 && t <= 1)
                {
                    qtdIntersec++;
                }

            }
            //se qtd ptos de intersec par, esta fora
            if (qtdIntersec % 2 == 0)
                return false;

            return true;
        }


    }
}