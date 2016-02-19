using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluorineFx;
using System.Data.SqlClient;
using System.Data;
/// <summary>
/// Summary description for Evento
/// </summary>
[RemotingService]
public class Evento
{
    public int eveCodigo;
    public string eveTipo;
    public string eveDescricao;
    public int eveReferencia;
    public DateTime eveDataHora;
    public int usuCodigo;
    public string usuFoto;

    public string usuNome;
    public string nomeReferencia;

	public Evento()
	{
	}

    public void gravarEvento(Evento evt)
    {

        using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "INSERT INTO Eventos (eveTipo, eveDescricao, eveReferencia, eveDataHora, usuCodigo) VALUES (@eveTipo, @eveDescricao, @eveReferencia, @eveDataHora, @usuCodigo)";

                cmd.Parameters.Add("@eveTipo", SqlDbType.NVarChar).Value = evt.eveTipo;
                cmd.Parameters.Add("@eveDescricao", SqlDbType.NVarChar).Value = evt.eveDescricao;
                cmd.Parameters.Add("@eveReferencia", SqlDbType.Int).Value = evt.eveReferencia;
                cmd.Parameters.Add("@eveDataHora", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@usuCodigo", SqlDbType.Int).Value = evt.usuCodigo;
                
                cmd.ExecuteNonQuery();
            
            }
        }
    }

    public List<Evento> listarEvento()
    {
        List<Evento> listEvento = new List<Evento>();
        using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
        {
            con.Open();
            using(SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "SELECT TOP 1000 Eventos.eveCodigo, "
                                    + "Eventos.eveTipo, "
                                    + "Eventos.eveDescricao, "
                                    + "Eventos.eveReferencia, "
                                    + "Eventos.eveDataHora, "
                                    + "Eventos.usuCodigo, "
                                    + "Usuario.usuNome, "
                                    + "Usuario.usuFoto, "
                                    + "nomeReferencia =   CASE Eventos.eveTipo "
                                                            + "WHEN 'A' THEN (SELECT Album.albNome FROM Album WHERE Album.albCodigo = Eventos.eveReferencia)"
                                                            + "WHEN 'R' THEN (SELECT Artista.artNome FROM Artista  WHERE Artista.artCodigo = Eventos.eveReferencia)"
                                                            + "WHEN 'M' THEN (SELECT Musica.musNome FROM Musica  WHERE Musica.musCodigo = Eventos.eveReferencia)"
                                                            + "WHEN 'G' THEN (SELECT Genero.genNome FROM Genero  WHERE Genero.genCodigo = Eventos.eveReferencia)"
                                                            + "WHEN 'S' THEN (SELECT Musica.musNome FROM Musica  WHERE Musica.musCodigo = Eventos.eveReferencia)"
                                                            + "WHEN 'P' THEN (SELECT Playlist.plaNome FROM Playlist  WHERE Playlist.plaCodigo = Eventos.eveReferencia)"
                                                            + "WHEN 'L' THEN (SELECT Playlist.plaNome FROM Playlist  WHERE Playlist.plaCodigo = Eventos.eveReferencia)"
                                                            + "WHEN 'U' THEN (SELECT Usuario.usuNome FROM Usuario  WHERE Usuario.usuCodigo = Eventos.eveReferencia)  "
                                                            + "WHEN 'T' THEN (SELECT Playlist.plaNome FROM Playlist  WHERE Playlist.plaCodigo = Eventos.eveReferencia)"
                                                        + "END  "
                                    + "FROM Eventos INNER JOIN Usuario ON Usuario.usuCodigo = Eventos.usuCodigo ORDER BY eveDataHora DESC ";
                
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Evento evt = new Evento();
                        evt.eveCodigo = Convert.ToInt32(reader["eveCodigo"]);
                        evt.eveTipo = Convert.ToString(reader["eveTipo"]);
                        evt.eveDescricao = Convert.ToString(reader["eveDescricao"]);
                        evt.eveReferencia = Convert.ToInt32(reader["eveReferencia"]);
                        evt.eveDataHora = Convert.ToDateTime(reader["eveDataHora"]);
                        evt.usuCodigo = Convert.ToInt32(reader["usuCodigo"]);
                        evt.usuNome = Convert.ToString(reader["usuNome"]);
                        evt.usuFoto = Convert.ToString(reader["usuFoto"]);
                        if (reader["nomeReferencia"] != DBNull.Value)
                        {
                            evt.nomeReferencia = Convert.ToString(reader["nomeReferencia"]);
                        }
                        else
                        {
                            evt.nomeReferencia = "";
                        }

                        listEvento.Add(evt);
                    }
                }
            }
        }
        return listEvento;
    }

}