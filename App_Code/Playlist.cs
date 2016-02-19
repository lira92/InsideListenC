using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using FluorineFx;
using System.Data;
/// <summary>
/// Summary description for Playlist
/// </summary>
[RemotingService]
public class Playlist
{
    public int plaCodigo;
    public string plaNome;
    public int genCodigo;
    public int usuCodigo;

    public string usuNome;
    public String genNome;

    public Playlist()
    {
    }

    public List<Playlist> listarPlaylist()
    {
        try
        {
            List<Playlist> lista = new List<Playlist>();
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT Playlist.*, Usuario.usuNome, Genero.genNome FROM Playlist "
                                      + "INNER JOIN Usuario ON Playlist.usuCodigo = Usuario.usuCodigo "
                                      + "LEFT JOIN Genero ON Playlist.genCodigo = Genero.genCodigo ORDER BY Playlist.plaNome";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Playlist p = new Playlist();
                            p.plaCodigo = Convert.ToInt32(reader["plaCodigo"]);
                            p.plaNome = reader["plaNome"].ToString();
                            if (reader["genCodigo"] != DBNull.Value)
                            {
                                p.genCodigo = Convert.ToInt32(reader["genCodigo"]);
                                p.genNome = reader["genNome"].ToString();
                            }
                            p.usuCodigo = Convert.ToInt32(reader["usuCodigo"]);
                            p.usuNome = reader["usuNome"].ToString();
                            

                            lista.Add(p);
                        }
                    }
                }
            }
            return lista;
        }
        catch (Exception erro)
        {
            throw new Exception(erro.Message);
        }
    }

    public int gravarPlaylist(Playlist objPlaylist)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    if (objPlaylist.plaCodigo == 0)
                    {
                        cmd.CommandText = "INSERT INTO Playlist (plaNome, genCodigo, usuCodigo) VALUES (@plaNome, @genCodigo, @usuCOdigo)";
                    }
                    else
                    {
                        cmd.CommandText = "UPDATE Playlist SET plaNome = @plaNome, genCodigo = @genCodigo, usuCodigo = @usuCodigo WHERE plaCodigo = @plaCodigo";
                        cmd.Parameters.Add("@plaCodigo", SqlDbType.Int).Value = objPlaylist.plaCodigo;
                    }
                    cmd.Parameters.Add("@plaNome", SqlDbType.NVarChar).Value = objPlaylist.plaNome;
                    if (objPlaylist.genCodigo == 0)
                    {
                        cmd.Parameters.Add("@genCodigo", SqlDbType.Int).Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters.Add("@genCodigo", SqlDbType.Int).Value = objPlaylist.genCodigo;
                    }
                    cmd.Parameters.Add("@usuCodigo", SqlDbType.Int).Value = objPlaylist.usuCodigo;
                    cmd.ExecuteNonQuery();

                    if (objPlaylist.plaCodigo == 0)
                    {
                        cmd.CommandText = "SELECT MAX(plaCodigo) FROM Playlist";
                        objPlaylist.plaCodigo = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    Funcoes.gravarEvento("P", objPlaylist.plaCodigo, objPlaylist.usuCodigo);
                    return objPlaylist.plaCodigo;
                }
            }
        }
        catch (Exception erro)
        {
            throw new Exception(erro.Message);
        }
    }

    public int excluirPlaylist(int plaCodigo, int usuCodigo)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "DELETE FROM Playlist WHERE plaCodigo = @plaCodigo";
                    cmd.Parameters.Add("@plaCodigo", SqlDbType.Int).Value = plaCodigo;
                    cmd.ExecuteNonQuery();
                }
            }
            Funcoes.gravarEvento("P", plaCodigo, usuCodigo);
            return plaCodigo;
        }
        catch (Exception erro)
        {
            throw new Exception(erro.Message);
        }
    }
}