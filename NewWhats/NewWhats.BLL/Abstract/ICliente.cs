using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWhats.BLL.Abstract
{
    public interface ICliente:IBase<DTO.Cliente>
    {
		List<DTO.Cliente> SelectByStatus(int Status);
    }
}