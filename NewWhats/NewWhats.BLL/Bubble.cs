using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NewWhats.BLL
{
    public static class Bubble
    {
        public static DTO.Mensagem getMessage(string content)
        {
            //string a = ASCIIEncoding.ASCII.GetString(Encoding.ASCII.GetBytes(content));
            //string pattern = "(.*)data-id=\"(?<NosEnviamos>.*)_(?<TelCliente>\\d{13})\\@(.*)(?<hh>\\d{1}|\\d{2})\\:(?<mm>\\d{1}|\\d{2})\\, (?<dd>\\d{1}|\\d{2})[\\/](?<MM>\\d{1}|\\d{2})[\\/](?<yyyy>\\d{4})(.*)\\+(?<DDI>\\d{2})\\s(?<DDD>\\d{2})\\s(?<TEL1>\\d{5})\\-(?<TEL2>\\d{4})(.*)(<span(.*?)>)(?<Texto>.*|⁠⁠⁠⁠\n)(\\</span>)";
            //string pattern = "(.*)data-id=\"(?<NosEnviamos>.*)_(?<TelCliente>\\d{13})\\@(.*)(?<hh>\\d{1}|\\d{2})\\:(?<mm>\\d{1}|\\d{2})\\, (?<dd>\\d{1}|\\d{2})[\\/](?<MM>\\d{1}|\\d{2})[\\/](?<yyyy>\\d{4})(.*)\\+(?<DDI>\\d{2})\\s(?<DDD>\\d{2}|\\d{3})\\s(?<TEL1>\\d{5})\\-(?<TEL2>\\d{4})(.*)(<span(.*?)>)(?<Texto>.*)(\\</span>)";

            DTO.Mensagem mensagem = new DTO.Mensagem();
            string pattern = File.ReadAllText("RegexBubble.txt");

            Regex reg = new Regex(pattern);

            var matches = reg.Matches(content);
            if(matches.Count<=0)
            {
                pattern = "(.*)data-id=\"(?<NosEnviamos>.*)_(?<TelCliente>\\d{13})\\@(.*)(?<hh>\\d{1}|\\d{2})\\:(?<mm>\\d{1}|\\d{2})\\, (?<dd>\\d{1}|\\d{2})[\\/](?<MM>\\d{1}|\\d{2})[\\/](?<yyyy>\\d{4})(.*)\\+(?<DDI>\\d{2})\\s(?<DDD>\\d{2}|\\d{3})\\s(?<TEL1>\\d{5})\\-(?<TEL2>\\d{4})(.*)(<span(.*?)>)<div(.*?)>(?<Texto>.*)</div>";
                matches = reg.Matches(content);
            }

            foreach (Match match in matches)
            {
                string NosEnviamos = match.Result("${NosEnviamos}");
                string TelCliente = match.Result("${TelCliente}");
                string hh = match.Result("${hh}");
                string mm = match.Result("${mm}");
                string dd = match.Result("${dd}");
                string MM = match.Result("${MM}");
                string yyyy = match.Result("${yyyy}");
                string DDI = match.Result("${DDI}");
                string DDD = match.Result("${DDD}");
                string TEL1 = match.Result("${TEL1}");
                string TEL2 = match.Result("${TEL2}");
                /*string Texto = match.Result("${Texto}");

                if (Texto.Contains("<img"))
                {
                    Regex reg_msg = new Regex("<!-- react-text:(.*)");
                    Texto = "[Imagem]";
                }
                else
                    Texto = Texto.Substring(Texto.IndexOf(">") + 1, Texto.LastIndexOf("<") - Texto.IndexOf(">") - 1);
                    */
                mensagem.DataInclusao = DateTime.Now;
                mensagem.DataMensagem = DateTime.Parse(string.Format("{0}-{1}-{2} {3}:{4}", yyyy, MM, dd, hh, mm));
                mensagem.Enviada = true;
                mensagem.Id = -1;
               // mensagem.Msg = Texto;
                mensagem.TelefoneCliente = TelCliente;
                if (Convert.ToBoolean(NosEnviamos))
                    mensagem.Tipo = 1;
                else
                    mensagem.Tipo = 2;
            }
            return mensagem;
        }
    }
}
