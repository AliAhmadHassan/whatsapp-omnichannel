using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WWeb.UI.WebformMVC.Models.DAL
{
    public static class Auxiliar
    {
        public static List<SqlParameter> GeraParametros<T>(T DadosDTO)
        {
            List<SqlParameter> LParams = new List<SqlParameter>();

            foreach (PropertyInfo pi in DadosDTO.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!pi.DeclaringType.Namespace.Contains(".DTO"))
                    continue;

                SqlParameter Param;

                if (pi.PropertyType.FullName == "System.DateTime")
                {
                    if (((DateTime)pi.GetValue(DadosDTO, null)).Year == 1)
                        Param = new SqlParameter("@" + pi.Name, DBNull.Value);
                    else
                        Param = new SqlParameter("@" + pi.Name, pi.GetValue(DadosDTO, null));
                }
                else if (pi.PropertyType.FullName == "System.Byte[]")
                {

                    if (pi.GetValue(DadosDTO, null) == null)
                        Param = new SqlParameter("@" + pi.Name, DBNull.Value);
                    else
                        Param = new SqlParameter("@" + pi.Name, pi.GetValue(DadosDTO, null));

                    Param.DbType = System.Data.DbType.Binary;
                }
                else
                {
                    if (pi.GetValue(DadosDTO, null) == null)
                        Param = new SqlParameter("@" + pi.Name, DBNull.Value);
                    else
                        Param = new SqlParameter("@" + pi.Name, pi.GetValue(DadosDTO, null));
                }

                LParams.Add(Param);
            }
            return LParams;
        }

        public static T RetornaDadosEntidade<T>(SqlDataReader Dr)
        {
            T _entidade = (T)Activator.CreateInstance(typeof(T));

            List<string> LColunas = new List<string>();

            for (int i = 0; i < Dr.FieldCount; i++)
                LColunas.Add(Dr.GetName(i));


            foreach (PropertyInfo pi in _entidade.GetType().GetProperties())
            {
                if (!LColunas.Contains(pi.Name))
                    continue;
                else if (Dr[pi.Name] == DBNull.Value)
                    pi.SetValue(_entidade, null, null);
                else
                    pi.SetValue(_entidade, Dr[pi.Name], null);
            }
            return _entidade;
        }

        public static Atributos RetornoAtributos<T>(T Entidade)
        {
            Atributos atributo = null;

            Type tipo = typeof(T);
            var props = typeof(T).GetProperties(); ;

            foreach (var p in props)
            {
                PropertyInfo info = tipo.GetProperty(p.Name);

                object[] Obj = tipo.GetProperty(p.Name).GetCustomAttributes(true);

                if (Obj.Length > 0)
                {
                    foreach (object objAux in Obj)
                    {
                        if (objAux.GetType().Name == "AtributoBind")
                            if (((BaseDTO.AtributoBind)objAux).ChavePrimaria)
                            {
                                atributo = new Atributos();
                                atributo.NomeChavePrimeria = p.Name;
                                atributo.NomeProcedureAlterar = ((BaseDTO.AtributoBind)objAux).ProcedureAlterar;
                                atributo.NomeProcedureInserir = ((BaseDTO.AtributoBind)objAux).ProcedureInserir;
                                atributo.NomeProcedureRemover = ((BaseDTO.AtributoBind)objAux).ProcedureRemover;
                                atributo.NomeProcedureListarTodos = ((BaseDTO.AtributoBind)objAux).ProcedureListarTodos;
                                atributo.NomeProcedurePelaPK = ((BaseDTO.AtributoBind)objAux).ProcedureSelecionar;
                                break;
                            }
                    }
                }
                if (atributo != null)
                    break;
            }

            if (atributo == null)
                throw new Exception("Campo Chave Não Encontrado");

            return atributo;
        }

        public class Atributos
        {
            public string NomeChavePrimeria { get; set; }

            public string NomeProcedureInserir { get; set; }

            public string NomeProcedureAlterar { get; set; }

            public string NomeProcedureRemover { get; set; }

            public string NomeProcedureListarTodos { get; set; }

            public string NomeProcedurePelaPK { get; set; }
        }
    }
}
