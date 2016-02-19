using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluorineFx;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for PlaylistMusica
/// </summary>
[RemotingService]
public class PlaylistMusica
{
    public int plaCodigo;
    public int musCodigo;

    public string musNome;
    public string genNome;
    public string artNome;
    public PlaylistMusica()
    {
    }

    public int gravarPlaylistMusica(List<PlaylistMusica> playlistMusica, int usuCodigo)
    {
        try
        {
            int qtde = 0;
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                foreach (PlaylistMusica pm in playlistMusica)
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "DELETE FROM PlaylistMusica WHERE plaCodigo = @plaCodigo";
                        cmd.Parameters.Add("@plaCodigo", SqlDbType.Int).Value = pm.plaCodigo;
                        cmd.ExecuteNonQuery();
                        break;
                    }
                }

                foreach (PlaylistMusica pm in playlistMusica)
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "INSERT INTO PlaylistMusica (plaCodigo, musCodigo) VALUES (@plaCodigo, @musCodigo)";
                        cmd.Parameters.Add("@plaCodigo", SqlDbType.Int).Value = pm.plaCodigo;
                        cmd.Parameters.Add("@musCodigo", SqlDbType.Int).Value = pm.musCodigo;
                        cmd.ExecuteNonQuery();

                        if (qtde == 0)
                        {
                            Funcoes.gravarEvento("T", pm.plaCodigo, usuCodigo);
                        }

                        qtde++;
                    }
                }
            }
            
            return qtde;
        }
        catch (Exception erro)
        {
            throw new Exception(erro.Message);
        }
    }

    public List<PlaylistMusica> listarMusicaPlaylist(int plaCodigo)
    {
        try
        {
            List<PlaylistMusica> listaMusica = new List<PlaylistMusica>();
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT PlaylistMusica.*, Musica.musNome FROM PlaylistMusica "
                                        + " INNER JOIN Musica ON Musica.musCodigo = PlaylistMusica.musCodigo "
                                        + " WHERE PlaylistMusica.plaCodigo = @plaCodigo";
                    cmd.Parameters.Add("@plaCodigo", SqlDbType.Int).Value = plaCodigo;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlaylistMusica m = new PlaylistMusica();
                            m.plaCodigo = Convert.ToInt32(reader["plaCodigo"]);
                            m.musCodigo = Convert.ToInt32(reader["musCodigo"]);
                            m.musNome = reader["musNome"].ToString();

                            listaMusica.Add(m);
                        }
                    }
                }
            }
            return listaMusica;
        }
        catch (Exception erro)
        {
            throw new Exception(erro.Message);
        }
    }

    public List<PlaylistMusica> listarMusicaAlbum(int albCodigo)
    {
        try
        {
            List<PlaylistMusica> listaMusica = new List<PlaylistMusica>();
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * From Musica "
                                        + " WHERE albCodigo = @albCodigo";
                    cmd.Parameters.Add("@albCodigo", SqlDbType.Int).Value = albCodigo;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlaylistMusica m = new PlaylistMusica();
                            m.musCodigo = Convert.ToInt32(reader["musCodigo"]);
                            m.musNome = reader["musNome"].ToString();

                            listaMusica.Add(m);
                        }
                    }
                }
            }
            return listaMusica;
        }
        catch (Exception erro)
        {
            throw new Exception(erro.Message);
        }
    }

    public List<PlaylistMusica> listarMusicaArtista(int artCodigo)
    {
        try
        {
            List<PlaylistMusica> listaMusica = new List<PlaylistMusica>();
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * From Musica "
                                        + " WHERE albCodigo in (SELECT albCodigo FROM Album WHERE artCodigo = @artCodigo)";
                    cmd.Parameters.Add("@artCodigo", SqlDbType.Int).Value = artCodigo;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlaylistMusica m = new PlaylistMusica();
                            m.musCodigo = Convert.ToInt32(reader["musCodigo"]);
                            m.musNome = reader["musNome"].ToString();

                            listaMusica.Add(m);
                        }
                    }
                }
            }
            return listaMusica;
        }
        catch (Exception erro)
        {
            throw new Exception(erro.Message);
        }
    }

    public List<PlaylistMusica> listarTop()
    {
        try
        {
            List<PlaylistMusica> listaMusica = new List<PlaylistMusica>();
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT TOP 10 eveReferencia, musNome, musCodigo, COUNT(eveReferencia) quantidade FROM Eventos "
                                        + "INNER JOIN Musica ON Musica.musCodigo = Eventos.eveReferencia "
                                        + "WHERE eveTipo = 'S' GROUP BY eveReferencia, musNome, musCodigo ORDER BY quantidade DESC ";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlaylistMusica m = new PlaylistMusica();
                            m.plaCodigo = 0;
                            m.musCodigo = Convert.ToInt32(reader["musCodigo"]);
                            m.musNome = reader["musNome"].ToString();

                            listaMusica.Add(m);
                        }
                    }
                }
            }
            return listaMusica;
        }
        catch (Exception erro)
        {
            throw new Exception(erro.Message);
        }
    }

    public List<PlaylistMusica> listarMusica(int plaCodigo)
    {
        try
        {
            List<PlaylistMusica> listaMusica = new List<PlaylistMusica>();
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT Musica.*, Genero.genNome, "+
                        "(SELECT artNome FROM Album INNER JOIN Artista on (Artista.artCodigo = Album.artCodigo) WHERE albCodigo = Musica.albCodigo) as artNome " +
                                      "FROM Musica LEFT JOIN Genero on(Musica.genCodigo = Genero.genCodigo) "
                                        + "WHERE Musica.musCodigo NOT IN (SELECT musCodigo FROM PlaylistMusica WHERE plaCodigo = @plaCodigo)";
                    cmd.Parameters.Add("@plaCodigo", SqlDbType.Int).Value = plaCodigo;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlaylistMusica m = new PlaylistMusica();
                            m.plaCodigo = plaCodigo;
                            m.musCodigo = Convert.ToInt32(reader["musCodigo"]);
                            m.musNome = reader["musNome"].ToString();
                            m.genNome = reader["genNome"].ToString();
                            m.artNome = reader["artNome"].ToString();
                            listaMusica.Add(m);
                        }
                    }
                }
            }
            return listaMusica;
        }
        catch (Exception erro)
        {
            throw new Exception(erro.Message);
        }
    }


}
