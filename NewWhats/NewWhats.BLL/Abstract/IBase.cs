using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWhats.BLL.Abstract{
    public interface IBase<T>
    {
        List<T> Select();

        T SelectById(int Id);

        void Remover(T Entidade);

        void Cadastro(T Entidade);
    }
}