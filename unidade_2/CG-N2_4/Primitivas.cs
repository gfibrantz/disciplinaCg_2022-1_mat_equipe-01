#define CG_Debug

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;
using System.Collections;
using System;

namespace gcgcg
{

    internal class Primitivas : ObjetoGeometria
    {
        int indexPrimitiva = 0;
        int[,] cores;
        // Points, Lines, LineLoop, LineStrip, Triangles, TriangleStrip, TriangleFan, Quads, QuadStrip e Polygon.
        public Primitivas(char rotulo, Objeto paiRef, Ponto4D ptoInfEsq, Ponto4D ptoSupDir) : base(rotulo, paiRef)
        {

            Ponto4D ptoInfDir = new Ponto4D(ptoSupDir.X, ptoInfEsq.Y); // 200 -200
            Ponto4D ptoSupEsq = new Ponto4D(ptoInfEsq.X, ptoSupDir.Y); // -200 200

            base.PontosAdicionar(ptoSupDir); // 200 200
            base.PontosAdicionar(ptoSupEsq); // -200 200
            base.PontosAdicionar(ptoInfEsq); // -200 -200
            base.PontosAdicionar(ptoInfDir); // 200 -200

            // int[] preto = { 0, 0, 0 };
            // int[] cyano = { 0, 255,255 };
            // int[] magenta = { 1, 0, 1 };
            // int[] amarelo = { 1, 1, 0 };
            
            //magenta, ciano, amarelo, preto
            this.cores =  new int[,]  {{ 255, 0, 255 }, { 0, 255, 255 }, { 255, 255, 0 },{ 0, 0, 0 }};

        }
        public void incrementaIndex()
        {
            this.indexPrimitiva++;

        }

        protected override void DesenharObjeto()
        {
            PrimitiveType davez = PrimitiveType.Points;
            switch (indexPrimitiva % 10)
            {
                case 0:
                    davez = PrimitiveType.Points;
                    break;
                case 1:
                    davez = PrimitiveType.Lines;
                    break;
                case 2:
                    davez = PrimitiveType.LineLoop;
                    break;
                case 3:
                    davez = PrimitiveType.LineStrip;
                    break;
                case 4:
                    davez = PrimitiveType.Triangles;
                    break;
                case 5:
                    davez = PrimitiveType.TriangleStrip;
                    break;
                case 6:
                    davez = PrimitiveType.TriangleFan;
                    break;
                case 7:
                    davez = PrimitiveType.Quads;
                    break;
                case 8:
                    davez = PrimitiveType.QuadStrip;
                    break;
                case 9:
                    davez = PrimitiveType.Polygon;
                    break;

            }
            GL.Begin(davez);

            for(int index = 0; index < pontosLista.Count; index++){
                //pinta os pontos
                GL.Color3(Convert.ToByte(cores[index,0]), Convert.ToByte(cores[index,1]), Convert.ToByte(cores[index,2]));
                GL.Vertex2(pontosLista[index].X, pontosLista[index].Y);

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