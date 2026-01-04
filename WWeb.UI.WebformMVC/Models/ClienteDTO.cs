using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace WWeb.UI.WebformMVC.Models.DTO
{
    public class ClienteDTO
    {
        private string _Telefone;
        [DataType(DataType.PhoneNumber)]
        public string TelefoneCliente
        {
            get
            {
                return _Telefone;
            }
            set
            {
                if (value.Length.Equals(13)) //Verificar se o telefone contem o código do país
                    _Telefone = value.Substring(2, value.Length - 2);
                else if (value.Length.Equals(12))
                    _Telefone = value.Substring(2, value.Length - 2);
                else
                    _Telefone = value;
            }
        }

        public string Situacao { get; set; }

        public byte[] UrlImagem { get; set; }

        public DateTime DataHoraRegistro { get; set; }

        public DateTime DataVisualizacao { get; set; }

        public string NomeCliente { get; set; }

        public string Cpf { get; set; }

        public int Status { get; set; }

    }
}
