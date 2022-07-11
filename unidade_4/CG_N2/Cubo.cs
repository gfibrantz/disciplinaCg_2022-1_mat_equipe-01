/**
  Autor: Dalton Solano dos Reis
**/

#define CG_Debug
#define CG_OpenGL
// #define CG_DirectX

using System;
using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;



namespace gcgcg
{
  internal class Cubo : ObjetoGeometria
  {

    public Cubo(char rotulo, Objeto paiRef) : base(rotulo, paiRef)
    {
        


    }

    static public int CarregaTextura(string caminho)
{
    // cria um objeto de textura
    int id = GL.GenTexture();

    // seleciona a texture
    GL.BindTexture(TextureTarget.Texture2D, id);

    // carrega uma imagem
    Bitmap bmp = new Bitmap(caminho);

    
    BitmapData bmp_data = bmp.LockBits(
            new Rectangle(0, 0, bmp.Width, bmp.Height),
            System.Drawing.Imaging.ImageLockMode.ReadOnly,
            System.Drawing.Imaging.PixelFormat.Format32bppArgb);

    // importa a imagem para a textura
    GL.TexImage2D(TextureTarget.Texture2D,
                  0,
                  PixelInternalFormat.Rgba,
                  bmp_data.Width,
                  bmp_data.Height,
                  0,
                  OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                  OpenTK.Graphics.OpenGL.PixelType.UnsignedByte,
                  bmp_data.Scan0);

    bmp.UnlockBits(bmp_data);

    GL.TexParameter(TextureTarget.Texture2D,
            TextureParameterName.TextureWrapS,
            (int)TextureWrapMode.Clamp);
    GL.TexParameter(TextureTarget.Texture2D,
            TextureParameterName.TextureWrapT,
            (int)TextureWrapMode.Clamp);


    GL.TexParameter(TextureTarget.Texture2D,
            TextureParameterName.TextureMinFilter,
            (int)TextureMinFilter.Linear);
    GL.TexParameter(TextureTarget.Texture2D,
            TextureParameterName.TextureMagFilter,
            (int)TextureMagFilter.Linear);

    // retorna o id do objeto
    return id;
}

    protected override void DesenharObjeto()
    {


      GL.BindTexture(TextureTarget.Texture2D, CarregaTextura("C:/Users/gfibr/Downloads/image.png"));

      GL.Color3(1.0f, 1.0f, 1.0f);
      GL.Begin(PrimitiveType.Quads);


      // Face da frente
      //GL.Normal3(0, 0, 1);
      GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f, -1.0f, 1.0f);
      GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, -1.0f, 1.0f);
      GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, 1.0f, 1.0f);
      GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f, 1.0f, 1.0f);
      // Face do fundo
      GL.Normal3(0, 0, -1);
      GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f, -1.0f, -1.0f);
      GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-1.0f, 1.0f, -1.0f);
      GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, 1.0f, -1.0f);
      GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, -1.0f);
      // Face de cima
      GL.Normal3(0, 1, 0);
      GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f, 1.0f, 1.0f);
      GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 1.0f);
      GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, 1.0f, -1.0f);
      GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f, 1.0f, -1.0f);
      // Face de baixo
      GL.Normal3(0, -1, 0);
      GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f, -1.0f, 1.0f);
      GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-1.0f, -1.0f, -1.0f);
      GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, -1.0f);
      GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 1.0f);
      // Face da direita
      GL.Normal3(1, 0, 0);
      GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, -1.0f, 1.0f);
      GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, -1.0f, -1.0f);
      GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, 1.0f, -1.0f);
      GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(1.0f, 1.0f, 1.0f);
      // Face da esquerda
      GL.Normal3(-1, 0, 0);
      GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f, -1.0f, 1.0f);
      GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-1.0f, 1.0f, 1.0f);
      GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-1.0f, 1.0f, -1.0f);
      GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, -1.0f);
      
      GL.End();
  

     
  
    }
    
    //TODO: melhorar para exibir não só a lista de pontos (geometria), mas também a topologia ... poderia ser listado estilo OBJ da Wavefrom
#if CG_Debug
    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto Cubo: " + base.rotulo + "\n";
      for (var i = 0; i < pontosLista.Count; i++)
      {
        retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
      }
      return (retorno);
    }
#endif

  }
}