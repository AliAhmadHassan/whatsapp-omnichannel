using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WWeb.UI.WebformMVC
{
    [Serializable()]
    public class RespostaAutenticacao
    {
        public string responseAutenticado { get; set; } //0 - Não, 1 - Sim

        public string responseErro { get; set; } //0 - Não, 1 - Sim

        public string responseUrl { get; set; }

        public string responseMsg { get; set; }
    }


    /// <summary>
    /// Desenvolvido por: Francisco Silva
    ///             Data: 30/12/2015
    /// </summary>
    [Serializable()]
    public class Tb_Usuario
    {
        public int Us_ID { get; set; }

        public string Us_Nome { get; set; }

        public int Dep_ID { get; set; }

        public string Us_Login { get; set; }

        public string Us_Matricula { get; set; }

        public int Cargo_Id { get; set; }
    }

    public class Tb_Departamento
    {
        public int Dep_ID { get; set; }

        public string Dep_Nome { get; set; }
    }

    /// <summary>
    /// Desenvolvido por: Francisco Silva
    ///             Data: 29/12/2015
    /// </summary>
    public class Devedor
    {
        public Devedor()
        {
            Telefones = new List<Telefone>();
        }

        public string CPFCNPJ { get; set; }

        public List<Telefone> Telefones { get; set; }
    }

    /// <summary>
    /// Desenvolvido por: Francisco Silva
    ///             Data: 29/12/2015
    /// </summary>
    public class Telefone
    {
        public string DDDTelefone { get; set; }

        public string NumeroTelefone { get; set; }

        public string TipoTelefone { get; set; }

        public string Qualidade { get; set; }

        public string TratadoPor { get; set; }
    }

    /// <summary>
    /// Desenvolvido por: Francisco Silva
    ///             Data: 29/12/2015
    /// </summary>
    public class CobnetAdapter
    {
        public void AtualizarTelefone(Devedor devedor)
        {
            using (SqlConnection ctx = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["cobNet"].ConnectionString))
            {
                ctx.Open();

                string query = @"IF NOT EXISTS 
                           (
                              SELECT * 
                                FROM Tb_Telefone (nolock)
                               WHERE Tel_DDD = @Tel_DDD 
                                 and Tel_Telefone = @Tel_Telefone
                                 and Tel_CPF = @Tel_CPF
                            )
                        BEGIN
                            INSERT INTO tb_telefone (Tel_Tipo_Telefone, Tel_DDD, Tel_Telefone, Tel_Data_Cadastro, Tel_CPF, Tel_Ativo, Tel_Credor, Tel_Tratado_Por, Tel_Padrao, Tel_Qualidade) 
                            VALUES (@Tel_Tipo_Telefone, @Tel_DDD, @Tel_Telefone, GetDate(), @Tel_CPF, 1, 1,@Tel_Tratado_Por, @Tel_Padrao, @Tel_Qualidade);
                        END
                        ELSE
                        BEGIN
                            UPDATE tb_telefone 
                               set Tel_Tratado_Por      = @Tel_Tratado_Por,  
                                   Tel_Padrao           = @Tel_Padrao,
                                   Tel_Qualidade        = @Tel_Qualidade
                             WHERE Tel_DDD              = @Tel_DDD 
                               AND Tel_Telefone         = @Tel_Telefone
                               AND Tel_CPF              = @Tel_CPF;
                        END";

                foreach (var t in devedor.Telefones)
                {
                    using (SqlCommand cmd = new SqlCommand(query, ctx))
                    {
                        cmd.Parameters.AddWithValue("@Tel_CPF", devedor.CPFCNPJ);
                        cmd.Parameters.AddWithValue("@Tel_Tipo_Telefone", "WhatsApp");
                        cmd.Parameters.AddWithValue("@Tel_DDD", t.DDDTelefone);
                        cmd.Parameters.AddWithValue("@Tel_Telefone", t.NumeroTelefone);
                        cmd.Parameters.AddWithValue("@Tel_Tratado_Por", t.TratadoPor);
                        cmd.Parameters.AddWithValue("@Tel_Padrao", 1);
                        cmd.Parameters.AddWithValue("@Tel_Qualidade", t.Qualidade);

                        var result = cmd.ExecuteScalar();
                    }
                }
            }
        }

        public Tb_Usuario ValidarUsuarioCobnet(string usuario, string senha)
        {
            Tb_Usuario usuario_cobnet = null;

            using (SqlConnection ctx = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["cobNet"].ConnectionString))
            {
                ctx.Open();

                string query = @"SELECT Us_ID, Us_Nome, Dep_ID, Us_Login, Us_Matricula, Cargo_Id
                                   FROM Tb_Usuario (NOLOCK) 
                                  WHERE Us_Login = @Us_Login 
                                    AND PWDCOMPARE(@Us_Senha, Us_Nova_Senha) = 1";

                using (SqlCommand cm = new SqlCommand(query, ctx))
                {
                    cm.Parameters.Clear();
                    cm.CommandText = query;
                    cm.Parameters.AddWithValue("@Us_Login", usuario);
                    cm.Parameters.AddWithValue("@Us_Senha", senha);

                    SqlDataReader r = cm.ExecuteReader();
                    while (r.Read())
                    {
                        usuario_cobnet = new Tb_Usuario();

                        usuario_cobnet.Us_ID = Convert.ToInt32(r["Us_ID"]);
                        usuario_cobnet.Cargo_Id = Convert.ToInt32(r["Cargo_Id"]);
                        usuario_cobnet.Dep_ID = Convert.ToInt32(r["Dep_ID"]);

                        usuario_cobnet.Us_Login = r["Us_Login"].ToString();
                        usuario_cobnet.Us_Matricula = r["Us_Matricula"].ToString();
                        usuario_cobnet.Us_Nome = r["Us_Nome"].ToString();
                    }
                }
            }

            return usuario_cobnet;
        }

        public Tb_Usuario ObterUsuario(int Us_ID)
        {
            Tb_Usuario usuario_cobnet = null;

            using (SqlConnection ctx = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["cobNet"].ConnectionString))
            {
                ctx.Open();

                string query = @"SELECT Us_ID, Us_Nome, Dep_ID, Us_Login, Us_Matricula, Cargo_Id
                                   FROM Tb_Usuario (NOLOCK) 
                                  WHERE Us_ID = @Us_ID";

                using (SqlCommand cm = new SqlCommand(query, ctx))
                {
                    cm.Parameters.Clear();
                    cm.CommandText = query;
                    cm.Parameters.AddWithValue("@Us_ID", Us_ID);

                    SqlDataReader r = cm.ExecuteReader();
                    while (r.Read())
                    {
                        usuario_cobnet = new Tb_Usuario();

                        usuario_cobnet.Us_ID = Convert.ToInt32(r["Us_ID"]);
                        usuario_cobnet.Cargo_Id = Convert.ToInt32(r["Cargo_Id"]);
                        usuario_cobnet.Dep_ID = Convert.ToInt32(r["Dep_ID"]);

                        usuario_cobnet.Us_Login = r["Us_Login"].ToString();
                        usuario_cobnet.Us_Matricula = r["Us_Matricula"].ToString();
                        usuario_cobnet.Us_Nome = r["Us_Nome"].ToString();
                    }
                }
            }

            return usuario_cobnet;
        }

        public List<Tb_Departamento> ListarDepartamentos()
        {
            List<Tb_Departamento> Departamentos = new List<Tb_Departamento>();
            using (SqlConnection ctx = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["cobNet"].ConnectionString))
            {
                ctx.Open();
                string query = @"select Dep_ID, Dep_Nome from tb_departamento";
                using (SqlCommand cmd = new SqlCommand(query, ctx))
                {
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            if (!string.IsNullOrEmpty(r["Dep_Nome"].ToString()))
                                Departamentos.Add(new Tb_Departamento() { Dep_ID = Convert.ToInt32(r["Dep_Id"].ToString()), Dep_Nome = r["Dep_Nome"].ToString() });
                        }
                    }
                }
            }

            return Departamentos;
        }
    }
}