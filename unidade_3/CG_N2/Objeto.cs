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
        double radiansX =0;
        double radiansY =0;
        double radiansZ = 0;

        //testes
        private Transformacao4D matrizTemp = new Transformacao4D();


        public Objeto(char rotulo, Objeto paiRef)
        {
            this.rotulo = rotulo;
            

        }
        private void levarParaOrigem(bool naOrigem)
        {
            Ponto4D centro = bBox.obterCentro;

            //copio a situacao atual
            Transformacao4D agora = new Transformacao4D();
            agora.AtribuirDados(tTransformacao.ObterDados());

            //levo para origem
            tTransformacao.AtribuirIdentidade();
            tTransformacao.AtribuirTranslacao(-centro.X, -centro.Y, -centro.Z);


        }

        public double rotacionarHorario(){
             //copio a situacao atual
            Transformacao4D agora = new Transformacao4D();
            agora.AtribuirDados(tTransformacao.ObterDados());
            if(radiansZ < 0)
                radiansZ= 0;

            this.radiansZ+=0.01;
            
            tTransformacao.AtribuirRotacaoZ(radiansZ);
            tTransformacao = tTransformacao.MultiplicarMatriz(agora);
            return radiansZ;


        }
         public double rotacionarAntiHorario(){
             //copio a situacao atual
            Transformacao4D agora = new Transformacao4D();
            agora.AtribuirDados(tTransformacao.ObterDados());
            if(radiansZ > 0)
                radiansZ= 0;
            
            this.radiansZ -= 0.01;
            tTransformacao.AtribuirRotacaoZ(radiansZ);
            tTransformacao = tTransformacao.MultiplicarMatriz(agora);
            return radiansZ;


        }
        public void translacaoXPositivo()
        {
            tTransformacao.AtribuirTranslacao(tTransformacao.ObterElemento(12) + 3, tTransformacao.ObterElemento(13), tTransformacao.ObterElemento(14));


        }
        public void translacaoXNegativo()
        {
            tTransformacao.AtribuirTranslacao(tTransformacao.ObterElemento(12) - 3, tTransformacao.ObterElemento(13), tTransformacao.ObterElemento(14));

        }
        public void translacaoYPositivo()
        {
            tTransformacao.AtribuirTranslacao(tTransformacao.ObterElemento(12) , tTransformacao.ObterElemento(13)+ 3, tTransformacao.ObterElemento(14));

        }
        public void translacaoYNegativo()
        {
            tTransformacao.AtribuirTranslacao(tTransformacao.ObterElemento(12), tTransformacao.ObterElemento(13) - 3, tTransformacao.ObterElemento(14));

        }
        public void escalaDiminuiOrigem()
        {
           
            tTransformacao.AtribuirEscala(tTransformacao.ObterElemento(0)-0.1, tTransformacao.ObterElemento(5)-0.1,tTransformacao.ObterElemento(10)-0.1);
                
        }
          public void escalaAumentarOrigem()
        {
            
            tTransformacao.AtribuirEscala(tTransformacao.ObterElemento(0)+0.1, tTransformacao.ObterElemento(5)+0.1,tTransformacao.ObterElemento(10)+0.1);
            

        }

          public void escalaAumentarCentroBBox()
        {
            Ponto4D centro = bBox.obterCentro;
            tTransformacao.AtribuirEscala(tTransformacao.ObterElemento(0)+0.1, tTransformacao.ObterElemento(5)+0.1,tTransformacao.ObterElemento(10)+0.1);
            tTransformacao.AtribuirTranslacao(centro.X , centro.Y, centro.Z );
            

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