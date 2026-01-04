using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWhats.BLL.Abstract
{
    public interface ITelefones:IBase<DTO.Telefones>
    {
		List<DTO.Telefones> SelectByTelefoneCliente(string TelefoneCliente);
    }
}