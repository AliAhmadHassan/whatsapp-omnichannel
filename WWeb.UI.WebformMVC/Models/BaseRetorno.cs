using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WWeb.UI.WebformMVC.Models.DAL
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 16/10/2014
    /// </summary>
    public abstract class BaseRetorno
    {
        public virtual T GetModels<T, Y>(Y DadosDTO)
        {
            T _entidade = (T)Activator.CreateInstance(typeof(T));


            foreach (PropertyInfo pi in DadosDTO.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!pi.DeclaringType.Namespace.Contains(".DTO"))
                    continue;

                foreach (PropertyInfo piModel in _entidade.GetType().GetProperties())
                {
                    if (pi.Name != piModel.Name)
                        continue;

                    else if (pi.GetValue(DadosDTO, null) == DBNull.Value)
                        pi.SetValue(_entidade, null, null);
                    else
                        pi.SetValue(_entidade, pi.GetValue(DadosDTO, null), null);
                }
            }
            return _entidade;
        }

        public virtual List<T> GetModels<T, Y>(List<Y> DadosDTO)
        {
            List<T> _entidade = (List<T>)Activator.CreateInstance(typeof(List<T>));

            foreach (Y item in DadosDTO)
                _entidade.Add(GetModels<T, Y>(item));

            return _entidade;
        }
    }
}
