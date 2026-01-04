using System;
using System.Reflection;
namespace NewWhats.DTO
{
    public abstract class BaseRetorno
    {
        public virtual T GetModels<T>()
        {
            T _entidade = (T)Activator.CreateInstance(typeof(T));


            foreach (PropertyInfo pi in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!pi.DeclaringType.Namespace.Contains(".DTO"))
                    continue;

                foreach (PropertyInfo piModel in _entidade.GetType().GetProperties())
                {
                    if (pi.Name != piModel.Name)
                        continue;

                    else if (pi.GetValue(this, null) == DBNull.Value)
                        pi.SetValue(_entidade, null, null);
                    else
                        pi.SetValue(_entidade, pi.GetValue(this, null), null);
                }
            }
            return _entidade;
        }
    }
}
