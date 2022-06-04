/**
  Autor: Dalton Solano dos Reis
**/

#define CG_Debug
#define CG_OpenGL
// #define CG_DirectX

using System;
using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;
using System.Collections.Generic;

namespace gcgcg
{
    internal class Poligono : ObjetoGeometria
    {
       
        bool estaSendoDesenhado;
        public Poligono(char rotulo, Objeto paiRef) : base(rotulo, paiRef)
        {
            estaSendoDesenhado = true;
        }

        public Ponto4D getPontoFinal()
        {
            
            return base.PontosUltimo();
        }
        public bool getClicouDentro(double xClick, double yClick){
           
            if(base.PrimitivaTipo == PrimitiveType.LineLoop){
                return Matematica.ScanLineTop(base.pontosLista, xClick, yClick,true);
            }

             return Matematica.ScanLineTop(base.pontosLista, xClick, yClick,false);          

        }
        public Ponto4D getVerticeMaisProximo(double xClick, double yClick){
            Ponto4D maisProximo = base.PontosUltimo();
            double distMenor = Double.MaxValue;

            //analisa todos os pontos do poligono
            foreach(Ponto4D pto in pontosLista){
                double distAtual =  Matematica.DistanciaEntrePontos(pto.X,pto.Y,xClick,yClick);
                //se a distancia atual é menor que a salva
                if(distAtual < distMenor){
                    distMenor = distAtual;
                    maisProximo = pto;
                }
               
            }

           return maisProximo;

        }
        private void removePontoFinal()
        {
           base.PontosRemoverUltimo();
        }
         public void removePonto(Ponto4D pto)
        {
            
            //TODO: verificar se preciar criar metodo pra tratar(na objetoGeometria)     
            base.pontosLista.Remove(pto);
        }
        public void addPonto(Ponto4D novo)
        {
           
           

            if ( base.pontosLista.Count.Equals(0)){
                base.PontosAdicionar(novo);
                base.PontosAdicionar(new Ponto4D(novo.X, novo.Y));   
                
            }else {
                 //base.PontosAlterar(novo,pontosLista.Count-1);
                 base.PontosAdicionar(novo);   
            }
            
            

        }
        
        public void finalizaDesenhoPoligono(){
            this.estaSendoDesenhado = false;
            removePontoFinal();
            
        }
        public string imprimePontos()
        {
            //TODO: verifica se esta sendo desenhado
            string retorno;
            retorno = "__ Objeto Poligono: " + base.rotulo + "\n";
            for (var i = 0; i < pontosLista.Count; i++)
            {
                retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
            }
            return (retorno);
        }

        public void setColor(byte r, byte g, byte b)
        {
            base.ObjetoCor.CorR = r;
            base.ObjetoCor.CorG = g;
            base.ObjetoCor.CorB = b;

        }


        protected override void DesenharObjeto()
        {
#if CG_OpenGL && !CG_DirectX
            GL.Color3(base.ObjetoCor.CorR,base.ObjetoCor.CorG, base.ObjetoCor.CorB);
            GL.PointSize(10);
            GL.Begin(PrimitiveType.Points);
            foreach (Ponto4D pto in pontosLista)
            {
                GL.Vertex2(pto.X, pto.Y);
            }
            GL.End();
            GL.Color3(base.ObjetoCor.CorR,base.ObjetoCor.CorG, base.ObjetoCor.CorB);
            GL.Begin(base.PrimitivaTipo);
            foreach (Ponto4D pto in pontosLista)
            {
                GL.Vertex2(pto.X, pto.Y);
            }
            GL.End();



#elif CG_DirectX && !CG_OpenGL
    Console.WriteLine(" .. Coloque aqui o seu código em DirectX");
#elif (CG_DirectX && CG_OpenGL) || (!CG_DirectX && !CG_OpenGL)
    Console.WriteLine(" .. ERRO de Render - escolha OpenGL ou DirectX !!");
#endif
        }

        //TODO: melhorar para exibir não só a lista de pontos (geometria), mas também a topologia ... poderia ser listado estilo OBJ da Wavefrom
#if CG_Debug
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto Poligono: " + base.rotulo + "\n";
            Console.WriteLine("Quantidade de pontos" + pontosLista.Count);
            for (var i = 0; i < pontosLista.Count; i++)
            {
                retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
            }
            return (retorno);
        }
#endif

    }
}