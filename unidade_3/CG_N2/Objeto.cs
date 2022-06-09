/**
  Autor: Dalton Solano dos Reis
**/

#define CG_OpenGL

using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using CG_Biblioteca;

namespace gcgcg
{
    internal abstract class Objeto
    {
        protected char rotulo;
        private Cor objetoCor = new Cor(255, 255, 255, 255);
        public Cor ObjetoCor { get => objetoCor; set => objetoCor = value; }
        private PrimitiveType primitivaTipo = PrimitiveType.LineLoop;
        public PrimitiveType PrimitivaTipo { get => primitivaTipo; set => primitivaTipo = value; }
        private float primitivaTamanho = 1;
        public float PrimitivaTamanho { get => primitivaTamanho; set => primitivaTamanho = value; }
        private BBox bBox = new BBox();
        public BBox BBox { get => bBox; set => bBox = value; }

        //objeto com a matriz de transformacao
        private Transformacao4D tTransformacao = new Transformacao4D();
        public Transformacao4D Transformacao4D { get => tTransformacao; set => tTransformacao = value; }
        private List<Objeto> objetosLista = new List<Objeto>();
        double radiansX = 0;
        double radiansY = 0;
        double radiansZ = 0;


        //testes
        private Transformacao4D matrizTemp = new Transformacao4D();


        public Objeto(char rotulo, Objeto paiRef)
        {
            this.rotulo = rotulo;


        }
        public Objeto getFilhoAt(int index){
            return objetosLista[index];
        }
        public int getQtdFilhos(){
            return objetosLista.Count;
        }
      
        public double rotacionarHorario()
        {
            //copio a situacao atual
            Transformacao4D agora = new Transformacao4D();
            agora.AtribuirDados(tTransformacao.ObterDados());
            if (radiansZ < 0)
                radiansZ = 0;

            this.radiansZ += 0.01;

            tTransformacao.AtribuirRotacaoZ(radiansZ);
            tTransformacao = tTransformacao.MultiplicarMatriz(agora);
            return radiansZ;


        }
        public void  rotacionarOrigem(bool horario)
        {

            int grausRotacionar = -5;
            if(horario)
                grausRotacionar = 5;

            Transformacao4D rotacaoHoraria = new Transformacao4D();
            rotacaoHoraria.AtribuirRotacaoZ(grausRotacionar * Transformacao4D.DEG_TO_RAD);           
            
           
            tTransformacao = rotacaoHoraria.MultiplicarMatriz(tTransformacao);
        }

        public void translacaoX(bool positivo)
        {

            int valorX = -3;
            if (positivo)
                valorX = 3;

            Transformacao4D translacaoX = new Transformacao4D();
            translacaoX.AtribuirTranslacao(valorX, 0, 0);

            tTransformacao = translacaoX.MultiplicarMatriz(tTransformacao);

        }
        public void translacaoY(bool positivo)
        {

            int valorY = -3;
            if (positivo)
                valorY = 3;

            Transformacao4D translacaoY = new Transformacao4D();
            translacaoY.AtribuirTranslacao(0, valorY, 0);

            tTransformacao = translacaoY.MultiplicarMatriz(tTransformacao);

        }

        public void escalaOrigem(bool aumenta)
        {
            //define valor a ser aplicado
            double valorEscala = 0.9;
            if (aumenta)
                valorEscala = 1.1;

            //cria matriz para aplicar na do poligono
            Transformacao4D escala = new Transformacao4D();
            escala.AtribuirEscala(valorEscala, valorEscala, valorEscala);

            //agora aplica no poligono
            tTransformacao = escala.MultiplicarMatriz(tTransformacao);

        }
      

        public void escalaAumentarCentroBBox()
        {
            Ponto4D centro = BBox.obterCentro;
            //copia dados atuais
            Transformacao4D translacaoPraOrigem = new Transformacao4D();
            translacaoPraOrigem.AtribuirTranslacao(-centro.X, -centro.Y, -centro.Z);
            //leva para origem
            tTransformacao = translacaoPraOrigem.MultiplicarMatriz(tTransformacao);

            //faz a transformação
            Transformacao4D escalaReduzMetade = new Transformacao4D();
            escalaReduzMetade.AtribuirEscala(1.5, 1.5, 1.5);
            //aplica na matriz do poligono
            tTransformacao = escalaReduzMetade.MultiplicarMatriz(tTransformacao);


            //volta pra posicao inicial 
            Transformacao4D translacaoDeVolta = new Transformacao4D();
            translacaoDeVolta.AtribuirTranslacao(centro.X, centro.Y, centro.Z);
            //aplica na matriz do poligono          
            tTransformacao = translacaoDeVolta.MultiplicarMatriz(tTransformacao);

        }

        public void escalaDiminuiCentroBBox()
        {
            Ponto4D centro = BBox.obterCentro;
            //copia dados atuais
            Transformacao4D translacaoPraOrigem = new Transformacao4D();
            translacaoPraOrigem.AtribuirTranslacao(-centro.X, -centro.Y, -centro.Z);
            //leva para origem
            tTransformacao = translacaoPraOrigem.MultiplicarMatriz(tTransformacao);

            //faz a transformação
            Transformacao4D escalaReduzMetade = new Transformacao4D();
            escalaReduzMetade.AtribuirEscala(0.5, 0.5, 0.5);
            //aplica na matriz do poligono
            tTransformacao = escalaReduzMetade.MultiplicarMatriz(tTransformacao);


            //volta pra posicao inicial 
            Transformacao4D translacaoDeVolta = new Transformacao4D();
            translacaoDeVolta.AtribuirTranslacao(centro.X, centro.Y, centro.Z);
            //aplica na matriz do poligono          
            tTransformacao = translacaoDeVolta.MultiplicarMatriz(tTransformacao);


        }


        public void rotacaoHorariaBBox()
        {
            Ponto4D centro = BBox.obterCentro;
            //copia dados atuais
            Transformacao4D translacaoPraOrigem = new Transformacao4D();
            translacaoPraOrigem.AtribuirTranslacao(-centro.X, -centro.Y, -centro.Z);
            //leva para origem
            tTransformacao = translacaoPraOrigem.MultiplicarMatriz(tTransformacao);

            //faz a rotacao
            Transformacao4D rotacaoHoraria = new Transformacao4D();
            rotacaoHoraria.AtribuirRotacaoZ(5 * Transformacao4D.DEG_TO_RAD);
            //aplica na matriz do poligono
            tTransformacao = rotacaoHoraria.MultiplicarMatriz(tTransformacao);


            //volta pra posicao inicial 
            Transformacao4D translacaoDeVolta = new Transformacao4D();
            translacaoDeVolta.AtribuirTranslacao(centro.X, centro.Y, centro.Z);
            //aplica na matriz do poligono          
            tTransformacao = translacaoDeVolta.MultiplicarMatriz(tTransformacao);


        }
        public void rotacaoAntiHorariaBBox()
        {
            Ponto4D centro = BBox.obterCentro;
            //copia dados atuais
            Transformacao4D translacaoPraOrigem = new Transformacao4D();
            translacaoPraOrigem.AtribuirTranslacao(-centro.X, -centro.Y, -centro.Z);
            //leva para origem
            tTransformacao = translacaoPraOrigem.MultiplicarMatriz(tTransformacao);

            //faz a rotacao
            Transformacao4D rotacaoHoraria = new Transformacao4D();
            rotacaoHoraria.AtribuirRotacaoZ(-5 * Transformacao4D.DEG_TO_RAD);
            //aplica na matriz do poligono
            tTransformacao = rotacaoHoraria.MultiplicarMatriz(tTransformacao);


            //volta pra posicao inicial 
            Transformacao4D translacaoDeVolta = new Transformacao4D();
            translacaoDeVolta.AtribuirTranslacao(centro.X, centro.Y, centro.Z);
            //aplica na matriz do poligono          
            tTransformacao = translacaoDeVolta.MultiplicarMatriz(tTransformacao);


        }
        public void Desenhar()
        {
#if CG_OpenGL
            //push
            GL.PushMatrix();
            GL.MultMatrix(tTransformacao.ObterDados());
            GL.Color3(objetoCor.CorR, objetoCor.CorG, objetoCor.CorB);
            GL.LineWidth(primitivaTamanho);
            GL.PointSize(primitivaTamanho);
#endif
            DesenharGeometria();
            for (var i = 0; i < objetosLista.Count; i++)
            {
                objetosLista[i].Desenhar();
            }
            GL.PopMatrix();
        }

        protected abstract void DesenharGeometria();
        public void FilhoAdicionar(Objeto filho)
        {
            this.objetosLista.Add(filho);
        }
        public void FilhoRemover(Objeto filho)
        {
            this.objetosLista.Remove(filho);
        }
    }
}