using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluorineFx;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for Musica
/// </summary>
[RemotingService]
public class Musica
{
    public int musCodigo;
    public string musNome;
    public int genCodigo;
    public int albCodigo;

    public string genNome;
    public string albNome;
    public string artNome;

    public Musica()
    {
    }

    public List<Musica> listarMusicas()
    {
        try
        {
            List<Musica> listaMusica = new List<Musica>();
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT Musica.*, Genero.genNome, Album.albNome, Artista.artNome FROM Musica "
                                         + "LEFT JOIN Genero ON Genero.genCodigo = Musica.genCodigo "
                                         + "LEFT JOIN Album ON ALbum.albCodigo = Musica.albCodigo "
                                         + "LEFT JOIN Artista ON Artista.artCodigo = Album.artCodigo";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Musica m = new Musica();
                            m.musCodigo = Convert.ToInt32(reader["musCodigo"]);
                            m.musNome = reader["musNome"].ToString();
                            if (reader["albCodigo"] != DBNull.Value)
                            {
                                m.albCodigo = Convert.ToInt32(reader["albCodigo"]);
                            }
                            if (reader["genCodigo"] != DBNull.Value)
                            {
                                m.genCodigo = Convert.ToInt32(reader["genCodigo"]);
                                m.genNome = reader["genNome"].ToString();
                            }
                            if (reader["albCodigo"] != DBNull.Value)
                            {
                                m.albNome = reader["albNome"].ToString();
                                m.artNome = reader["artNome"].ToString();
                            }


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

    public int salvarComoPlaylist(string nomePlaylist, List<Musica> listMusica, int usuCodigo)
    {
        try
        {
            Playlist play = new Playlist();
            play.plaNome = nomePlaylist;
            play.usuCodigo = usuCodigo;
            play.plaCodigo = play.gravarPlaylist(play);
            int qtde = 0;
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                foreach (Musica musica in listMusica)
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        if (musica.musCodigo == 0)
                        {
                            cmd.CommandText = "INSERT INTO Musica (musNome, genCodigo, albCodigo) VALUES (@musNome, @genCodigo, @albCodigo)";
                        }
                        else
                        {
                            cmd.CommandText = "UPDATE Musica SET musNome = @musNome, genCodigo = @genCodigo, albCodigo = @albCodigo "
                                                + "WHERE musCodigo = @musCodigo";
                            cmd.Parameters.Add("@musCodigo", SqlDbType.Int).Value = musica.musCodigo;
                        }
                        cmd.Parameters.Add("@musNome", SqlDbType.NVarChar).Value = musica.musNome;

                        if (musica.genCodigo == 0)
                        {
                            cmd.Parameters.Add("@genCodigo", SqlDbType.Int).Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add("@genCodigo", SqlDbType.Int).Value = musica.genCodigo;
                        }

                        if (musica.albCodigo == 0)
                        {
                            cmd.Parameters.Add("@albCodigo", SqlDbType.Int).Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add("@albCodigo", SqlDbType.Int).Value = musica.albCodigo;
                        }
                        cmd.ExecuteNonQuery();
                        if (musica.musCodigo == 0)
                        {
                            cmd.CommandText = "SELECT MAX(musCodigo) FROM Musica";
                            musica.musCodigo = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                        qtde++;
                        
                        cmd.CommandText = "INSERT INTO PlaylistMusica (plaCodigo, musCodigo) VALUES (@plaCodigo, @musCodigo)";
                        cmd.Parameters.Add("@plaCodigo", SqlDbType.Int).Value = play.plaCodigo;
                        cmd.Parameters.Add("@musCodigo", SqlDbType.Int).Value = musica.musCodigo;
                        cmd.ExecuteNonQuery();

                        Funcoes.gravarEvento("M", musica.musCodigo, usuCodigo);
                    }
                }
            }

            return qtde;
        }
        catch(Exception erro)
        {
            throw new Exception(erro.Message);
        }
    }

    public void salvarComoPlaylistOuvidas(string nomePlaylist, List<Musica> listMusica, int usuCodigo)
    {
        try
        {
            Playlist play = new Playlist();
            play.plaNome = nomePlaylist;
            play.usuCodigo = usuCodigo;
            play.plaCodigo = play.gravarPlaylist(play);
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                foreach (Musica musica in listMusica)
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "INSERT INTO PlaylistMusica (plaCodigo, musCodigo) VALUES (@plaCodigo, @musCodigo)";
                        cmd.Parameters.Add("@plaCodigo", SqlDbType.Int).Value = play.plaCodigo;
                        cmd.Parameters.Add("@musCodigo", SqlDbType.Int).Value = musica.musCodigo;
                        cmd.ExecuteNonQuery();

                        Funcoes.gravarEvento("M", musica.musCodigo, usuCodigo);
                    }
                }
            }
        }
        catch (Exception erro)
        {
            throw new Exception(erro.Message);
        }
    }

    public int gravarMusica(List<Musica> listMusica, int usuCodigo)
    {
        using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
        {
            int qtde = 0;
            con.Open();
            foreach (Musica musica in listMusica)
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    if (musica.musCodigo == 0)
                    {
                        cmd.CommandText = "INSERT INTO Musica (musNome, genCodigo, albCodigo) VALUES (@musNome, @genCodigo, @albCodigo)";
                    }
                    else
                    {
                        cmd.CommandText = "UPDATE Musica SET musNome = @musNome, genCodigo = @genCodigo, albCodigo = @albCodigo "
                                            + "WHERE musCodigo = @musCodigo";
                        cmd.Parameters.Add("@musCodigo", SqlDbType.Int).Value = musica.musCodigo;
                    }
                    cmd.Parameters.Add("@musNome", SqlDbType.NVarChar).Value = musica.musNome;

                    if (musica.genCodigo == 0)
                    {
                        cmd.Parameters.Add("@genCodigo", SqlDbType.Int).Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters.Add("@genCodigo", SqlDbType.Int).Value = musica.genCodigo;
                    }

                    if (musica.albCodigo == 0)
                    {
                        cmd.Parameters.Add("@albCodigo", SqlDbType.Int).Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters.Add("@albCodigo", SqlDbType.Int).Value = musica.albCodigo;
                    }
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "Select @@identity";
                    musica.musCodigo = Convert.ToInt32(cmd.ExecuteScalar());
                    qtde++;
                    Funcoes.gravarEvento("M", musica.musCodigo, usuCodigo);
                }
            }

            
            return qtde;
        }

    }
    public int excluirMusica(int musCodigo, int usuCodigo)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "DELETE FROM Musica WHERE musCodigo = @musCodigo";
                    cmd.Parameters.Add("@musCodigo", SqlDbType.Int).Value = musCodigo;
                    cmd.ExecuteNonQuery();
                }
            }
            Funcoes.gravarEvento("M", musCodigo, usuCodigo);
            return musCodigo;
        }
        catch (Exception erro)
        {
            throw new Exception(erro.Message);
        }
    }
}