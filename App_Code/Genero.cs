using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using FluorineFx;

/// <summary>
/// Summary description for Genero
/// </summary>
    [RemotingService]
public class Genero
{
    public int genCodigo;
    public string genNome;
    public Genero()
    {
    }

    public List<Genero> listarGenero()
    {
        try
        {
            List<Genero> lista = new List<Genero>();
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * FROM Genero ORDER BY genNome";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Genero g = new Genero();
                            g.genCodigo = Convert.ToInt32(reader["genCodigo"]);
                            g.genNome = Convert.ToString(reader["genNome"]);

                            lista.Add(g);
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

    public int gravarGenero(Genero genero, int usuCodigo)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    if (genero.genCodigo == 0)
                    {
                        cmd.CommandText = "INSERT INTO Genero (genNome) VALUES (@genNome)";
                    }
                    else
                    {
                        cmd.CommandText = "UPDATE Genero set genNome = @genNome WHERE genCodigo = @genCodigo";
                        cmd.Parameters.Add("@genCodigo", SqlDbType.Int).Value = genero.genCodigo;
                    }
                    cmd.Parameters.Add("@genNome", SqlDbType.NVarChar).Value = genero.genNome;
                    cmd.ExecuteNonQuery();

                    if (genero.genCodigo == 0)
                    {
                        cmd.CommandText = "SELECT MAX(genCodigo) FROM Genero";
                        genero.genCodigo = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    Funcoes.gravarEvento("G", genero.genCodigo, usuCodigo);
                    return genero.genCodigo;
                }
            }
        }
        catch (Exception erro)
        {
            throw new Exception(erro.Message);
        }
    }

    public int excluirGenero(int genCodigo, int usuCodigo)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "DELETE FROM Genero WHERE genCodigo = @genCodigo";
                    cmd.Parameters.Add("@genCodigo", SqlDbType.Int).Value = genCodigo;
                    cmd.ExecuteNonQuery();
                }
            }

            Funcoes.gravarEvento("G", genCodigo, usuCodigo);

            return genCodigo;
        }
        catch (Exception erro)
        {
            throw new Exception(erro.Message);
        }
    }
}