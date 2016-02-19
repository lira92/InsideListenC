using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FluorineFx;

/// <summary>
/// Summary description for Usuario
/// </summary>
[RemotingService]
public class Usuario
{
    public int usuCodigo;
    public string usuNome;
    public string usuFoto;
    public string usuEmail;
    public Boolean usuAdmin;
    public string usuSenha;

    public Usuario()
    {
    }

    public List<Usuario> listarUsuarios()
    {
        try
        {
            List<Usuario> lista = new List<Usuario>();
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * FROM Usuario";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Usuario u = new Usuario();
                            u.usuCodigo = Convert.ToInt32(reader["usuCodigo"]);
                            u.usuNome = reader["usuNome"].ToString();
                            u.usuFoto = reader["usuFoto"].ToString();
                            u.usuEmail = reader["usuEmail"].ToString();
                            u.usuAdmin = Convert.ToBoolean(reader["usuAdmin"]);
                            u.usuSenha = reader["usuSenha"].ToString();

                            lista.Add(u);
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

    public int gravarUsuario(Usuario objUsuario, int usuCodigo)
    {
        using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;

                if (objUsuario.usuCodigo == 0)
                {
                    cmd.CommandText = "SELECT Usuario.usuEmail FROM Usuario";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (objUsuario.usuEmail.Equals(Convert.ToString(reader["usuEmail"])))
                            {
                                throw new Exception("E-mail já cadastrado");
                            }
                        }
                    }
                }

                if (objUsuario.usuCodigo == 0)
                {
                    cmd.CommandText = "INSERT INTO Usuario (usuNome, usuFoto, usuEmail, usuAdmin, usuSenha) VALUES (@usuNome, @usuFoto, @usuEmail, @usuAdmin, @usuSenha)";
                }
                else
                {
                    cmd.CommandText = "UPDATE Usuario set usuNome = @usuNome, usuFoto = @usuFoto, usuEmail = @usuEmail, usuAdmin = @usuAdmin, usuSenha = @usuSenha WHERE usuCodigo = @usuCodigo";
                    cmd.Parameters.Add("@usuCodigo", SqlDbType.Int).Value = objUsuario.usuCodigo;
                }
                cmd.Parameters.Add("@usuNome", SqlDbType.NVarChar).Value = objUsuario.usuNome;
                cmd.Parameters.Add("@usuFoto", SqlDbType.NVarChar).Value = objUsuario.usuFoto;
                cmd.Parameters.Add("@usuEmail", SqlDbType.NVarChar).Value = objUsuario.usuEmail;
                cmd.Parameters.Add("@usuAdmin", SqlDbType.Bit).Value = objUsuario.usuAdmin;
                cmd.Parameters.Add("@usuSenha", SqlDbType.NVarChar).Value = objUsuario.usuSenha;

                cmd.ExecuteNonQuery();

                if (objUsuario.usuCodigo == 0)
                {
                    cmd.CommandText = "SELECT MAX(usuCodigo) FROM Usuario";
                    objUsuario.usuCodigo = Convert.ToInt32(cmd.ExecuteScalar());
                }

                Funcoes.gravarEvento("U", objUsuario.usuCodigo, usuCodigo);
                return objUsuario.usuCodigo;
            }
        }
    }

    public int excluirUsuario(int usuCodigo)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "DELETE FROM Usuario WHERE usuCodigo = @usuCodigo";
                    cmd.Parameters.Add("@usuCodigo", SqlDbType.Int).Value = usuCodigo;
                    cmd.ExecuteNonQuery();
                }
            }
            return usuCodigo;
        }
        catch (Exception erro)
        {
            throw new Exception(erro.Message);
        }
    }

    public Usuario logarUsuario(Usuario usuario)
    {
        Usuario user = new Usuario();
        try
        {
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario WHERE usuSenha = @senha AND usuEmail = @usuario", con))
                {
                    cmd.Parameters.Add("@senha", SqlDbType.NVarChar).Value = usuario.usuSenha;
                    cmd.Parameters.Add("@usuario", SqlDbType.NVarChar).Value = usuario.usuEmail;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.usuCodigo = Convert.ToInt32(reader["usuCodigo"]);
                            user.usuNome = Convert.ToString(reader["usuNome"]);
                            user.usuFoto = Convert.ToString(reader["usuFoto"]);
                            user.usuEmail = Convert.ToString(reader["usuEmail"]);
                            user.usuAdmin = Convert.ToBoolean(reader["usuAdmin"]);
                            user.usuSenha = Convert.ToString(reader["usuSenha"]);
                        }
                    }
                }
            }
            return user;
        }
        catch (Exception erro)
        {
            throw new Exception(erro.Message);
        }
    }
}

 
    
