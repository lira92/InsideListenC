using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Configuration;
using FluorineFx;

/// <summary>
/// Summary description for Funcoes
/// </summary>
[RemotingService]
public class Funcoes
{
    public Funcoes()
    {
    }

    /// <summary>
    /// Metodo que altera, encripta e armazena no arquivo AppSettings.config o caminho da conexão com a base de dados do sistema (forma utilizada no Sistema Oracúlo)
    /// </summary>
    /// <param name="servidor">Caminho do Servidor - deve ser passado como String - entre "" - Ex: "192.168.0.19\\SQLEXPRESS"</param>
    /// <param name="banco">Base de Dados - deve ser passado como String - entre "" - Ex: "ServiceInternacional" </param>
    /// <param name="usuario">o usuario do - deve ser passado como String - entre "" - Ex: "sa"</param>
    /// <param name="senha">Senha da base de dados - deve ser passado como String - entre "" - Ex: "123"</param>
    /// <returns>True caso operação realizada com sucesso</returns>
    public Boolean alterarConexaoBanco(string servidor, string banco, string usuario, string senha)
    {
        try
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            if (servidor.Trim() != "" && banco.Trim() != "" && usuario.Trim() != "")
            {
                config.AppSettings.Settings["conexaoBanco"].Value = encryptConexao("Data Source=" + servidor.Trim() + ";Initial Catalog=" + banco.Trim() + ";User Id=" + usuario.Trim() + ";Password=" + senha.Trim() + ";MultipleActiveResultSets=true");
                config.Save();
                ConfigurationManager.RefreshSection("conexaoBanco");

                using (SqlConnection conn = new SqlConnection(conexao()))
                {
                    conn.Open();

                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Dispose();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }
        catch (Exception erro)
        {
            throw (new Exception(erro.Message + " - " + erro.Source));
        }
    }

    /// <summary>
    /// RETORNA A ConectionString ENCRIPTADA no AppSettings.config
    /// </summary>
    /// <returns>RETORNA A ConectionString ENCRIPTADA no AppSettings.config</returns>
    public string retornaConnectionString()
    {
        string conexao = decryptConexao(ConfigurationManager.AppSettings.Get("conexaoBanco").ToString());
        return conexao;
    }

    /// <summary>
    /// ENCRIPTA A CONEXAO DO AppSettings.config
    /// </summary>
    /// <param name="conexao">CONEXAO Encriptada no AppSettings.config</param>
    /// <returns>Retorna a ConectionString decriptada</returns>
    public static string encryptConexao(string conexao)
    {
        Byte[] by = System.Text.ASCIIEncoding.ASCII.GetBytes(conexao);
        string encrytada = Convert.ToBase64String(by);

        return encrytada;
    }

    /// <summary>
    /// DECRIPTA A CONEXAO DO AppSettings.config
    /// </summary>
    /// <param name="conexao">CONEXAO Decriptada no AppSettings.config</param>
    /// <returns>Retorna a ConectionString decriptada</returns>
    public static string decryptConexao(string conexao)
    {
        Byte[] by = Convert.FromBase64String(ConfigurationManager.AppSettings["conexaoBanco"]);
        string decrypt = System.Text.ASCIIEncoding.ASCII.GetString(by);

        return decrypt;
    }

    public static string conexao()
    {
        string conexao = decryptConexao(ConfigurationManager.AppSettings.Get("conexaoBanco").ToString());
        return conexao;
    }

    public static void gravarEvento(String tipo, int referencia, int usuCodigo)
    {
        Evento evento = new Evento();
        evento.eveTipo = tipo;
        evento.eveReferencia = referencia;
        evento.usuCodigo = usuCodigo;
        evento.eveDataHora = DateTime.Now;
        switch (evento.eveTipo)
        {
            case "A":
                evento.eveDescricao = "CADASTRO/ATUALIZACAO ALBUM";
                break;
            case "R":
                evento.eveDescricao = "CADASTRO/ATUALIZACAO ARTISTA";
                break;
            case "G":
                evento.eveDescricao = "CADASTRO/ATUALIZACAO GENERO";
                break;
            case "M":
                evento.eveDescricao = "CADASTRO/ATUALIZACAO MUSICA";
                break;
            case "S":
                evento.eveDescricao = "OUVIU A MUSICA";
                break;
            case "P":
                evento.eveDescricao = "CADASTRO/ATUALIZACAO PLAYLIST";
                break;
            case "L":
                evento.eveDescricao = "OUVIU A PLAYLIST";
                break;
            case "U":
                evento.eveDescricao = "CADASTRO/ATUALIZACAO USUARIO";
                break;
            case "T":
                evento.eveDescricao = "ATUALIZAÇÃO DE PLAYLIST (insere/exclui músicas)";
                break;
        }
        evento.gravarEvento(evento);
    }
}