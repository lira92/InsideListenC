﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing.Imaging;

/// <summary>
/// Summary description for Image
/// </summary>
public class Image
{
    public Image()
    {


    }

    /// <summary>
    /// Redimensiona imagem
    /// scrPath = path da imagem original
    /// destPath = path para a nova imagem
    /// caso o destPath seja igual ao scrPath, a nova imagem substitui a anterior
    /// </summary>
    public static void Resize(string srcPath, string destPath, int nWidth, int nHeight)
    {

        string temp;
        // abre arquivo original
        System.Drawing.Image img = System.Drawing.Image.FromFile(srcPath);
        int oWidth = img.Width; // largura original
        int oHeight = img.Height; // altura original

        // redimensiona se necessario
        if (oWidth > nWidth || oHeight > nHeight)
        {

            if (oWidth > oHeight)
            {
                // imagem horizontal
                nHeight = (oHeight * nWidth) / oWidth;
            }
            else
            {
                // imagem vertical
                nWidth = (oWidth * nHeight) / oHeight;
            }
        }

        // cria a copia da imagem
        System.Drawing.Image imgThumb = img.GetThumbnailImage(nWidth, nHeight, null, new System.IntPtr(0));

        if (srcPath == destPath)
        {
            temp = destPath + ".tmp";
            imgThumb.Save(temp, ImageFormat.Jpeg);
            img.Dispose();
            imgThumb.Dispose();
            File.Delete(srcPath); // deleta arquivo original
            File.Copy(temp, srcPath); // copia a nova imagem
            File.Delete(temp); // deleta temporário
        }
        else
        {
            imgThumb.Save(destPath, ImageFormat.Jpeg); // salva nova imagem no destino
            imgThumb.Dispose(); // libera memoria
            img.Dispose(); // libera memória
        }
    }
}