using System.Collections.Generic;
using System.Data.SqlClient;
using FluorineFx;
using System;
using System.Data;

[RemotingService]
public class EstatisticaTipo
{

    public int tipoA;
    public int tipoR;
    public int tipoG;
    public int tipoM;
    public int tipoS;
    public int tipoP;
    public int tipoL;
    public int tipoT;
    public int tipoU;
    public int total;

    public EstatisticaTipo()
    {
    }

    public List<EstatisticaTipo> listarEventosEstatistica(int codigo)
    {
        try
        {
            List<EstatisticaTipo> listaEventos = new List<EstatisticaTipo>();
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "select count(e.eveTipo) as eveTipo, " +
                        "(select count(eA.eveTipo) from eventos ea where ea.eveTipo = 'A' and ea.usuCodigo = @usuCodigo) as tipoA, " +
                        "(select count(eA.eveTipo) from eventos ea where ea.eveTipo = 'R' and ea.usuCodigo = @usuCodigo) as tipoR, " +
                        "(select count(eA.eveTipo) from eventos ea where ea.eveTipo = 'G' and ea.usuCodigo = @usuCodigo) as tipoG, " +
                        "(select count(eA.eveTipo) from eventos ea where ea.eveTipo = 'M' and ea.usuCodigo = @usuCodigo) as tipoM, " +
                        "(select count(eA.eveTipo) from eventos ea where ea.eveTipo = 'S' and ea.usuCodigo = @usuCodigo) as tipoS, " +
                        "(select count(eA.eveTipo) from eventos ea where ea.eveTipo = 'P' and ea.usuCodigo = @usuCodigo) as tipoP, " +
                        "(select count(eA.eveTipo) from eventos ea where ea.eveTipo = 'L' and ea.usuCodigo = @usuCodigo) as tipoL, " +
                        "(select count(eA.eveTipo) from eventos ea where ea.eveTipo = 'T' and ea.usuCodigo = @usuCodigo) as tipoT, " +
                        "(select count(eA.eveTipo) from eventos ea where ea.eveTipo = 'U' and ea.usuCodigo = @usuCodigo) as tipoU " +
                        "from Eventos e where e.usuCodigo = @usuCodigo";

                    cmd.Parameters.Add("@usuCodigo", SqlDbType.Int).Value = codigo;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            EstatisticaTipo ev = new EstatisticaTipo();

                            ev.tipoA = Convert.ToInt32(reader["tipoA"]);
                            ev.tipoR = Convert.ToInt32(reader["tipoR"]);
                            ev.tipoG = Convert.ToInt32(reader["tipoG"]);
                            ev.tipoM = Convert.ToInt32(reader["tipoM"]);
                            ev.tipoS = Convert.ToInt32(reader["tipoS"]);
                            ev.tipoP = Convert.ToInt32(reader["tipoP"]);
                            ev.tipoL = Convert.ToInt32(reader["tipoL"]);
                            ev.tipoT = Convert.ToInt32(reader["tipoT"]);
                            ev.tipoU = Convert.ToInt32(reader["tipoU"]);
                            ev.total = Convert.ToInt32(reader["eveTipo"]);

                            listaEventos.Add(ev);
                        }
                    }
                }
            }
            return listaEventos;
        }
        catch (Exception erro)
        {
            throw new Exception(erro.Message);
        }
    }
}