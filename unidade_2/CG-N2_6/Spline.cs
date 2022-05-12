#define CG_Debug

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

using System;
using System.Collections.Generic;

namespace gcgcg
{

    internal class Spline : ObjetoGeometria
    {
        //qtd de pontos para calcular da spline
        int qtdPontosCalculados;
        //lista de pontos calculados da spline
        protected List<Ponto4D> pontosCalculados2 = new List<Ponto4D>();
        //tabela usada no Bezier para encontrar valores da Spline
        double[,] tabelao;        
        //ponto de controle selecionado atualmente
        int indexPontoSelecionado = 0;  


        public Spline(char rotulo, Objeto paiRef) : base(rotulo, paiRef)
        {



            base.PontosAdicionar(new Ponto4D(-200, 200));   // -200,200     superior esquerdo
            base.PontosAdicionar(new Ponto4D(200, 200));    // 200,200      superior direito
            base.PontosAdicionar(new Ponto4D(-200, -200));  // -200,-200    inferior esquerdo
            base.PontosAdicionar(new Ponto4D(200, -200));    // 200 -200     inferior direito

            //base.PrimitivaTamanho = 10;


            //comecamos com o ponto inferiorDireito selecionado
            this.indexPontoSelecionado = 3;

            //inicializamos com valor que ja fica com aparencia de curva
            qtdPontosCalculados = 25;
            calculaPontosBerzier();
        }
        public void calculaTabelao()//metodo usado para criar/popular a tabela usada em Berzier
        {
            //criando a matriz (inicialmente (linhas)4x10(colunas))
            tabelao = new double[4, this.qtdPontosCalculados + 1];
            //o incremento de t vai variar conforme a qtd de pontos solicitada
            double t = 0;

            // TODO: parar na metade da tabela e preencher de traz pra frente tmb usando <= e /2

            //primeiro percorremos por coluna
            for (int coluna = 0; coluna < tabelao.GetLength(1); coluna++)
            {

                if (coluna == this.qtdPontosCalculados)
                {
                    t = 1;
                }
                //pre calculamos (1-t) pois sera usado em todas as linhas
                double umMenosT = 1 - t;

                //entao descemos as linhas
                for (int linha = 0; linha < tabelao.GetLength(0); linha++)
                {

                    //agora um if para cada linha  

                    if (linha == 0)     //SE FOR P0 = ((1-t)^3)
                    {
                        tabelao[linha, coluna] = Math.Pow(umMenosT, 3);
                        //Console.WriteLine(" tabelaoT[ " + linha + " , " + coluna + "] = " + tabelao[linha, coluna]); 
                    }
                    else if (linha == 1)//SE P1 = 3t((1-t)^2)
                    {
                        tabelao[linha, coluna] = (Math.Pow(umMenosT, 2)) * 3 * t;
                        //Console.WriteLine(" tabelaoT[ " + linha + " , " + coluna + "] = " + tabelao[linha, coluna]);
                    }
                    else if (linha == 2)//SE P2 = ((3(t^2))
                    {
                        tabelao[linha, coluna] = 3 * (Math.Pow(t, 2)) * umMenosT;
                        //Console.WriteLine(" tabelaoT[ " + linha + " , " + coluna + "] = " + tabelao[linha, coluna]);
                    }
                    else // se  P3 = (t^3)
                    {
                        tabelao[linha, coluna] = Math.Pow(t, 3);
                       // Console.WriteLine(" tabelaoT[ " + linha + " , " + coluna + "] = " + tabelao[linha, coluna]);
                    }
                }
                //incrementa t para proxima coluna ser processada
                t += (double)1 / this.qtdPontosCalculados;
            }

        }

        public void calculaPontosBerzier(){
            pontosCalculados2.Clear();
            calculaTabelao();

            //auxiliar para calcular de 0 a quantidade pontos for?
            int indexRef = 0;

            //como usamos a notacao de leitura de esquerda -> direita e de cima para baixo 
            //para alocar os pontos de controle, aqui vamos apenas colocalos num handle para facilitar
            // o entendimento do codigo
            //ex: ao apertar 3 ativa o ponto inferior esquerdo(indice 2), mas em Bezier ele corresponde ao P0
            Ponto4D p0 = pontosLista[2];
            Ponto4D p1 = pontosLista[0];
            Ponto4D p2 = pontosLista[1];
            Ponto4D p3 = pontosLista[3];

            //percorremos o this.tabelao para encontrar os valores para todos os this.qtdPontosCalculados serem criados
            while (indexRef <= qtdPontosCalculados)
            {
                //B(t) = P0 *((1-t)^3) + P1 * 3t((1-t)^2) + P2 * ((3(t^2))*(1-t)) + P3*(t^3)
                //usando o this.tabelao que ja fez pre calculos, encontramos o valor de X e Y para cada ponto da spline 
                double novoX = p0.X * tabelao[0, indexRef] + p1.X * tabelao[1, indexRef] + p2.X * tabelao[2, indexRef] + p3.X * tabelao[3, indexRef];
                double novoY = p0.Y * tabelao[0, indexRef] + p1.Y * tabelao[1, indexRef] + p2.Y * tabelao[2, indexRef] + p3.Y * tabelao[3, indexRef];

               // Console.WriteLine("index " + indexRef + " - NOVO = ( " + novoX + " , " + novoY + "  )");


                //add na tabela para plotar spline
                pontosCalculados2.Add(new Ponto4D(novoX, novoY, 0));

                indexRef++;
            }

        }
        public Ponto4D getPontoSuperiorEsquerdo()
        {
            return pontosLista[0];
        }
        public Ponto4D getPontoSuperiorDireito()
        {
            return pontosLista[1];
        }
        public Ponto4D getPontoInferiorEsquerdo()
        {
            return pontosLista[2];
        }
        public Ponto4D getPontoInferiorDireito()
        {
            return pontosLista[3];
        }
        public void setPontoEscolhido(int valor)
        {
            if (valor > 0 && valor < 5)
                this.indexPontoSelecionado = (valor - 1);

        }
        public void movePontoSelecionadoEsquerda()
        {
            //move ponto de controle selecionado e atualiza a spline
            pontosLista[this.indexPontoSelecionado].X -= 2;
            calculaPontosBerzier();

        }
        public void movePontoSelecionadoDireita()
        {
            //move ponto de controle selecionado e atualiza a spline
            pontosLista[this.indexPontoSelecionado].X += 2;
            calculaPontosBerzier();

        }
        public void movePontoSelecionadoCima()
        {
            //move ponto de controle selecionado e atualiza a spline
            pontosLista[this.indexPontoSelecionado].Y += 2;
            calculaPontosBerzier();

        }
        public void movePontoSelecionadoBaixo()
        {
            //move ponto de controle selecionado e atualiza a spline
            pontosLista[this.indexPontoSelecionado].Y -= 2;
            calculaPontosBerzier();

        }
        public void voltaEstadoInicial()
        {

            this.indexPontoSelecionado = 3;
            pontosLista[0].X = -200;// -200,200     superior esquerdo
            pontosLista[0].Y = 200;

            pontosLista[1].X = 200;// 200,200      superior direito
            pontosLista[1].Y = 200;

            pontosLista[2].X = -200;// -200,-200    inferior esquerdo
            pontosLista[2].Y = -200;

            pontosLista[3].X = 200;// -200,-200    inferior direito
            pontosLista[3].Y = -200;

            calculaPontosBerzier();


        }
        public void aumentaQtdPontosCalculados()
        {
            if (this.qtdPontosCalculados < 50)
            {
                this.qtdPontosCalculados++;
                calculaPontosBerzier();
            }
        }
        public void diminuiQtdPontosCalculados()
        {
            //TODO: talvez diminuir para >=
            if (this.qtdPontosCalculados > 1)
            {
                this.qtdPontosCalculados--;
                calculaPontosBerzier();
            }
        }

        protected override void DesenharObjeto()
        {
            //desenha pontos de controle
            GL.PointSize(10f);
            GL.Begin(PrimitiveType.Points);
            for (int index = 0; index < pontosLista.Count; index++)
            {
                //pinta todos de preto
                GL.Color3(Convert.ToByte(0), Convert.ToByte(0), Convert.ToByte(0));

                //se for o selecionado, pinta vermelho
                if (index == this.indexPontoSelecionado)
                    GL.Color3(Convert.ToByte(255), Convert.ToByte(0), Convert.ToByte(0));

                GL.Vertex2(pontosLista[index].X, pontosLista[index].Y);

            }
            GL.End();

            //agora desenha linha amarela spline
            GL.LineWidth(4);
            GL.Begin(PrimitiveType.LineStrip);
            GL.Color3(Convert.ToByte(255), Convert.ToByte(255), Convert.ToByte(0));

            foreach (Ponto4D pto in pontosCalculados2)
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