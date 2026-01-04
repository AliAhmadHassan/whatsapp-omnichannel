using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWhats.BLL.Abstract
{
    public interface IMensagem:IBase<DTO.Mensagem>
    {
		List<DTO.Mensagem> SelectByTelefoneCliente(string TelefoneCliente);
    }
}