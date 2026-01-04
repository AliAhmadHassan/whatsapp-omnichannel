using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WWeb.UI.WebformMVC.Models.DTO;

namespace WWeb.UI.WebformMVC.Models.DAL
{
    public class ClienteDAL : Base<ClienteDTO>
    {
        public void Cadastrar(ClienteDTO cliente)
        {
            using (SqlConnection ctx = new SqlConnection(Conexao.strConn))
            {
                string query = "";
                if (ObterPeloTelefone(cliente.TelefoneCliente).TelefoneCliente == null)
                {

                    query = @"insert cliente(TelefoneCliente, Situacao, UrlImagem, DataHoraRegistro,NomeCliente) 
                                     values(@TelefoneCliente, @Situacao, @UrlImagem, @DataHoraRegistro,@NomeCliente)";
                }
                else
                {
                    query = @"update cliente 
                                 set Situacao           = @Situacao, 
                                     UrlImagem          = @UrlImagem, 
                                     DataHoraRegistro   = @DataHoraRegistro,
                                     NomeCliente        = @NomeCliente
                               where TelefoneCliente    = @TelefoneCliente";
                }


                ctx.Open();
                using (SqlCommand cmd = new SqlCommand(query, ctx))
                {
                    cmd.Parameters.AddWithValue("@TelefoneCliente", cliente.TelefoneCliente);
                    cmd.Parameters.AddWithValue("@Situacao", cliente.Situacao);
                    cmd.Parameters.AddWithValue("@UrlImagem", cliente.UrlImagem);
                    cmd.Parameters.AddWithValue("@DataHoraRegistro", cliente.DataHoraRegistro);
                    cmd.Parameters.AddWithValue("@NomeCliente", cliente.NomeCliente);

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                ctx.Close();
            }
        }

        public ClienteDTO ObterPeloTelefone(string telefone)
        {
            return AuxConsultas<ClienteDTO>.Entidade("SPSClientePeloTelefone", new SqlParameter("@TelefoneCliente", telefone));
        }

        public List<ClienteStatusDTO> ListarComStatus()
        {
            return AuxConsultas<ClienteStatusDTO>.Lista("SPSClienteComStatus", new SqlParameter("@Parametro", 1));
        }

        public List<ClienteDTO> Listar()
        {
            return AuxConsultas<ClienteDTO>.Lista("SPSCliente", new SqlParameter("@Parametro", 1));
        }

        public List<ClienteDTO> ListarPeloTelefoneOrcozol(string telefone_orcozol)
        {
            return AuxConsultas<ClienteDTO>.Lista("SPSClientePeloNumeroOrcozol", new SqlParameter("@TelefoneOrcozol", telefone_orcozol));
        }

        public List<ClienteDTO> ListarClientesOnlines(int qtd_clientes)
        {
            return AuxConsultas<ClienteDTO>.Lista("SPSClienteOnline", new SqlParameter("@Qtd", qtd_clientes));
        }

        public int QtdClienteXTelefoneOrcozol(string telefone_orcozol)
        {
            int qtd = 0;
            using (SqlConnection ctx = new SqlConnection(Conexao.strConn))
            {
                ctx.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Connection = ctx;
                    cmd.CommandText = "SPSQtdClienteTelefone";
                    cmd.Parameters.AddWithValue("@TelefoneOrcozol", telefone_orcozol);

                    qtd = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return qtd;
        }

        public List<ClienteDTO> ListarClientesOnlinesPeloUsuario(int usuario, string telefone_cliente)
        {
            return AuxConsultas<ClienteDTO>.Lista("SPSClienteOnLinePeloUsuario", new SqlParameter[] { new SqlParameter("@Usuario", usuario), new SqlParameter("@TelefoneCliente", telefone_cliente) });
        }

        public List<ClienteHoraHoraDTO> ListarClientesHoraHora(DateTime inicio, DateTime fim)
        {
            return AuxConsultas<ClienteHoraHoraDTO>.Lista("SPSClienteHoraHora", new SqlParameter[] {
                new SqlParameter("@Inicio",inicio),
                new SqlParameter("@Fim",fim)
            });
        }

        public void AtualizarVisualizacao(string telefone_cliente)
        {
            var retorno = AuxConsultas<ClienteDTO>.Entidade("SPUClienteVisualizacao", new SqlParameter("@TelefoneCliente", telefone_cliente));
        }
    }
}
