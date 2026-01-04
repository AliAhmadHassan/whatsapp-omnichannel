using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WWeb.UI.WebformMVC.Models.DTO;

namespace WWeb.UI.WebformMVC.Models.DAL
{
    public class MensagemDAL : Base<DTO.MensagemDTO>
    {
        public void Cadastrar(DTO.MensagemDTO mensagem)
        {
            using (SqlConnection ctx = new SqlConnection(Conexao.strConn))
            {
                string query = @"declare @maxData datetime
                                  Select @maxData = coalesce(MAX(DataMensagem), '2015-01-01') from mensagem where TelefoneCliente = @TelefoneCliente
                                      if(@maxData < @DataMensagem)
	                                        insert into mensagem(TelefoneCliente, DataMensagem, DataInclusao, Tipo, Mensagem, Enviada) 
                                            values(@TelefoneCliente, @DataMensagem, @DataInclusao, @Tipo,@Mensagem, @Enviada)";
                ctx.Open();

                using (SqlCommand cmd = new SqlCommand(query, ctx))
                {
                    cmd.Parameters.AddWithValue("@TelefoneCliente", mensagem.TelefoneCliente);
                    cmd.Parameters.AddWithValue("@DataMensagem", mensagem.DataMensagem);
                    cmd.Parameters.AddWithValue("@DataInclusao", mensagem.DataInclusao);
                    cmd.Parameters.AddWithValue("@Tipo", mensagem.Tipo);
                    cmd.Parameters.AddWithValue("@Mensagem", mensagem.Mensagem);
                    cmd.Parameters.AddWithValue("@Enviada", mensagem.Enviada);

                    var resultado = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                ctx.Close();
            }
        }

        public List<MensagemDTO> ObterMensagensCliente(string telefone_cliente)
        {
            return AuxConsultas<MensagemDTO>.Lista("SPSMensagemPeloCliente", new SqlParameter("@TelefoneCliente", telefone_cliente));
        }

        public List<MensagemModelDTO> ObterMensagensNaoEnviadas(string telefone_orcozol)
        {
            return AuxConsultas<MensagemModelDTO>.Lista("SPSEnviarMensagemNumeroOrcozol", new SqlParameter("@TelefoneOrcozol", telefone_orcozol));
        }

        public void AtualizarStatusEnviada(int id_mensagem)
        {
            using (SqlConnection ctx = new SqlConnection(Conexao.strConn))
            {
                ctx.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = ctx;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "SPUMensagemEnviada";
                    cmd.Parameters.AddWithValue("@Id", id_mensagem);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AtualizarMensagemEnviada(string telefone_cliente)
        {
            var retorno = AuxConsultas<MensagemDTO>.Entidade("SPUMensagensVisualizadas", new SqlParameter("@TelefoneCliente", telefone_cliente));
        }

        public List<MensagemUsuarioDashboardDTO> UsuarioXMensagens(DateTime inicio, DateTime fim)
        {
            return AuxConsultas<MensagemUsuarioDashboardDTO>.Lista("SPSMensagemUsuarioPeriodo", new SqlParameter[] {
                new SqlParameter("@Inicio",inicio),
                new SqlParameter("@Fim",fim)
            });
        }

        public List<MensagemPeriodoDashboardDTO> MensagemXPeriodo(DateTime inicio, DateTime fim)
        {
            return AuxConsultas<MensagemPeriodoDashboardDTO>.Lista("SPSMensagemPeriodo", new SqlParameter[] {
                new SqlParameter("@Inicio",inicio),
                new SqlParameter("@Fim",fim)
            });
        }

        public List<MensagemHoraHoraDTO> MensagemHoraHora(DateTime inicio, DateTime fim)
        {
            return AuxConsultas<MensagemHoraHoraDTO>.Lista("SPSMensagemHoraHora", new SqlParameter[] {
                new SqlParameter("@Inicio",inicio),
                new SqlParameter("@Fim",fim)
            });
        }
    }
}