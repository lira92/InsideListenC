using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluorineFx;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for Artista
/// </summary>
[RemotingService]
public class Artista
{
    public int artCodigo;
    public string artNome;

    public Artista()
    {
    }

    public int gravarArtista(Artista objArtista, int usuCodigo)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    if (objArtista.artCodigo == 0)
                    {
                        cmd.CommandText = "INSERT INTO ARTISTA (artNome) VALUES (@artNome)";
                    }
                    else
                    {
                        cmd.CommandText = "UPDATE ARTISTA SET artNome = @artNome WHERE artCodigo = @artCodigo";
                        cmd.Parameters.Add("@artCodigo", SqlDbType.Int).Value = objArtista.artCodigo;
                    }
                    cmd.Parameters.Add("@artNome", SqlDbType.NVarChar).Value = objArtista.artNome;
                    cmd.ExecuteNonQuery();

                    if (objArtista.artCodigo == 0)
                    {
                        cmd.CommandText = "SELECT MAX(artCodigo) FROM Artista";
                        objArtista.artCodigo = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    Funcoes.gravarEvento("R", objArtista.artCodigo, usuCodigo);
                    return objArtista.artCodigo;
                }
            }
        }
        catch (Exception erro)
        {
            throw new Exception(erro.Message);
        }
    }
    public List<Artista> listarArtistas()
    {
        try
        {
            List<Artista> listaArtistas = new List<Artista>();
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * FROM ARTISTA ORDER BY artNome";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Artista art = new Artista();
                            art.artCodigo = Convert.ToInt32(reader["artCodigo"]);
                            art.artNome = reader["artNome"].ToString();
                            listaArtistas.Add(art);
                        }
                    }
                }
            }
            return listaArtistas;
        }
        catch (Exception erro)
        {
            throw new Exception(erro.Message);
        }
    }

    public int excluirArtista(int artCodigo, int usuCodigo)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using(SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "DELETE FROM ARTISTA WHERE artCodigo = @artCodigo";
                    cmd.Parameters.Add("@artCodigo", SqlDbType.Int).Value = artCodigo;
                    cmd.ExecuteNonQuery();
                }
            }

            Funcoes.gravarEvento("R", artCodigo, usuCodigo);

            return artCodigo;
        }
        catch (Exception erro)
        {
                throw new Exception(erro.Message);
        }
    }
}
