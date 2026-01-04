using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;

namespace WWeb.UI.WebformMVC.Controllers
{
    public class EnriquecimentoController : Controller
    {
        public ActionResult Index()
        {
            List<WWeb.UI.WebformMVC.Models.Carga> cargas = new List<Models.Carga>();
            using (WWeb.UI.WebformMVC.Models.WWebEntities ctx = new Models.WWebEntities())
                cargas = ctx.Carga.ToList();

            return View(cargas);
        }

        [HttpGet]
        public ActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Cadastro(FormCollection form, HttpPostedFileBase file)
        {
            Models.Carga carga = new Models.Carga();
            carga.DataEnvio = DateTime.Now;
            carga.Descricao = form["txt_descricao"];
            carga.ProcessamentoStatus = 0;
            carga.Usuario = (Session["Usuario"] as Tb_Usuario).Us_ID;

            string nome_arquivo = Path.Combine(Server.MapPath("~/Upload"), DateTime.Now.ToString(carga.Usuario + "_ddMMyyyyffff.upload") + ".txt");
            file.SaveAs(nome_arquivo);
            string[] linhas_arquivo = System.IO.File.ReadAllLines(nome_arquivo);

            carga.TotalLinhas = linhas_arquivo.Length;
            using (Models.WWebEntities ctx = new Models.WWebEntities())
            {
                ctx.Carga.Add(carga);
                ctx.SaveChanges();

                foreach (string l in linhas_arquivo)
                    if (l.Length > 11)
                        ctx.CargaHistorico.Add(new Models.CargaHistorico() { Carga = carga, IdCarga = carga.Id, TelefoneCliente = l.Substring(1, 11).Trim() });

                ctx.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult DownloadClientes(int id_carga)
        {
            List<Models.CargaHistorico> historicos = new List<Models.CargaHistorico>();
            StringBuilder sb = new StringBuilder();

            using (Models.WWebEntities ctx = new Models.WWebEntities())
                historicos = ctx.Carga.Where(h => h.Id == id_carga).FirstOrDefault().CargaHistorico.ToList();

            foreach (Models.CargaHistorico hc in historicos)
                sb.AppendLine(hc.TelefoneCliente + ";" + hc.Encontrado + ";" + hc.Status);

            string nome_arquivo = DateTime.Now.ToString((Session["Usuario"] as Tb_Usuario).Us_ID + "_ddMMyyyyffff.download") + ".txt";
            string path_nome_arquivo = Path.Combine(Server.MapPath("~/Upload"), nome_arquivo);

            using (StreamWriter streamWriter = System.IO.File.AppendText(path_nome_arquivo))
                streamWriter.WriteLine(sb.ToString());

            return File(new FileStream(path_nome_arquivo, FileMode.Open, FileAccess.Read), "text/plain", string.Format("{0}_" + nome_arquivo, id_carga));
        }
    }
}