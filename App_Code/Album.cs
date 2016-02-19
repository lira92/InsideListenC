using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using FluorineFx;
using System.Data;

/// <summary>
/// Summary description for Album
/// </summary>
[RemotingService]
public class Album
{
    public int albCodigo;
    public string albNome;
    public int artCodigo;
    public string artNome;

	public Album()
	{
	}

    public int gravarAlbum(Album objAlbum, int usuCodigo)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    if (objAlbum.albCodigo == 0)
                    {
                        cmd.CommandText = "INSERT INTO Album (albNome, artCodigo) VALUES (@albNome, @artCodigo)";
                    }
                    else
                    {
                        cmd.CommandText = "UPDATE Album SET albNome = @albNome, artCodigo = @artCodigo WHERE albCodigo = @albCodigo";
                        cmd.Parameters.Add("@albCodigo", SqlDbType.Int).Value = objAlbum.albCodigo;
                    }
                    cmd.Parameters.Add("@albNome", SqlDbType.NVarChar).Value = objAlbum.albNome;
                    cmd.Parameters.Add("@artCodigo", SqlDbType.Int).Value = objAlbum.artCodigo;
                    cmd.ExecuteNonQuery();

                    if (objAlbum.albCodigo == 0)
                    {
                        cmd.CommandText = "SELECT MAX(artCodigo) FROM Artista";
                        objAlbum.albCodigo = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    Funcoes.gravarEvento("A", objAlbum.albCodigo, usuCodigo);

                    return objAlbum.albCodigo;
                }
            }
        }
        catch (Exception erro)
        {
            throw new Exception(erro.Message);
        }
    }


    public List<Album> listarAlbum()
    {
        try
        {
            List<Album> listaAlbum = new List<Album>();
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT Album.*, Artista.artNome FROM Album "
                                         + "INNER JOIN Artista ON Artista.artCodigo = Album.artCodigo ORDER BY Album.albNome";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Album alb = new Album();
                            alb.albCodigo = Convert.ToInt32(reader["albCodigo"]);
                            alb.albNome = reader["albNome"].ToString();
                            alb.artCodigo = Convert.ToInt32(reader["artCodigo"]);
                            alb.artNome = reader["artNome"].ToString();
                            listaAlbum.Add(alb);
                        }
                    }
                }
            }
            return listaAlbum;
        }
        catch (Exception erro)
        {
            throw new Exception(erro.Message);
        }
    }

    public List<Album> listarAlbumArtista(int artCodigo)
    {
        try
        {
            List<Album> listaAlbum = new List<Album>();
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT Album.*, Artista.artNome FROM Album "
                                         + "INNER JOIN Artista ON Artista.artCodigo = Album.artCodigo "
                                         + "WHERE Album.artCodigo = @artCodigo";
                    cmd.Parameters.Add("@artCodigo", SqlDbType.Int).Value = artCodigo;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Album alb = new Album();
                            alb.albCodigo = Convert.ToInt32(reader["albCodigo"]);
                            alb.albNome = reader["albNome"].ToString();
                            alb.artCodigo = Convert.ToInt32(reader["artCodigo"]);
                            alb.artNome = reader["artNome"].ToString();
                            listaAlbum.Add(alb);
                        }
                    }
                }
            }

           
            return listaAlbum;
        }
        catch (Exception erro)
        {
            throw new Exception(erro.Message);
        }
    }

    public int excluirAlbum(int albCodigo, int usuCodigo)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "DELETE FROM Album WHERE albCodigo = @albCodigo";
                    cmd.Parameters.Add("@albCodigo", SqlDbType.Int).Value = albCodigo;
                    cmd.ExecuteNonQuery();
                }
            }

            Funcoes.gravarEvento("A", albCodigo, usuCodigo);
            return albCodigo;
        }
        catch (Exception erro)
        {
            throw new Exception(erro.Message);
        }
    }
}