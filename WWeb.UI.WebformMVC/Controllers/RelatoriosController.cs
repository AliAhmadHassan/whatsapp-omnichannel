using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WWeb.UI.WebformMVC.Models;
using WWeb.UI.WebformMVC.Models.DTO;


namespace WWeb.UI.WebformMVC.Controllers
{
    public class RelatoriosController : Controller
    {
        public ActionResult RelatorioSituacao()
        {
            List<Models.Situacao> situacoes = new List<Situacao>();
            using (Models.WWebEntities ctx = new WWebEntities())
            {
                situacoes = ctx.Situacao.ToList();
                situacoes.ForEach(c => c.Cliente.ToList());
            }

            return View(situacoes);
        }

        public ActionResult Clientes(int id_situacao)
        {
            List<Models.Cliente> clientes = new List<Cliente>();
            using (Models.WWebEntities ctx = new WWebEntities())
                clientes = ctx.Situacao.Where(c => c.Id.Equals(id_situacao)).FirstOrDefault().Cliente.ToList();

            return View(clientes);
        }

        public ActionResult ClientesPelaData(DateTime data_acionamento)
        {
            List<Models.Cliente> clientes = new List<Cliente>();

            data_acionamento = new DateTime(data_acionamento.Year, data_acionamento.Month, data_acionamento.Day, 0, 0, 0);
            DateTime data_fim = new DateTime(data_acionamento.Year, data_acionamento.Month, data_acionamento.Day, 23, 59, 59);

            using (Models.WWebEntities ctx = new WWebEntities())
                clientes = ctx.Cliente.Where(c => c.DataHoraRegistro > data_acionamento && c.DataHoraRegistro < data_fim).ToList();

            return View("Clientes", clientes);
        }

        public ActionResult ColocarFila(int id_situacao)
        {
            using (WWeb.UI.WebformMVC.Models.WWebEntities ctx = new Models.WWebEntities())
            {
                var clientes = ctx.Situacao.Where(c => c.Id.Equals(id_situacao)).FirstOrDefault().Cliente.ToList();
                foreach (var cli in clientes)
                {
                    var msg = cli.Mensagem.Take(1).FirstOrDefault();
                    msg.Enviada = false;
                }

                ctx.SaveChanges();
            }

            return RedirectToAction("RelatorioSituacao");
        }

        [HttpGet]
        public ActionResult RelatorioAcionamento()
        {
            return View(new List<RelatorioAcionamentoDTO>());
        }

        [HttpPost]
        public ActionResult RelatorioAcionamento(FormCollection form)
        {
            DateTime inicio = DateTime.Now;
            DateTime fim = DateTime.Now;

            if (!string.IsNullOrEmpty(form["data_inicio"]))
                inicio = Convert.ToDateTime(form["data_inicio"]);

            if (!string.IsNullOrEmpty(form["data_fim"]))
                fim = Convert.ToDateTime(form["data_fim"]);

            return View(new RelatoriosBLL().ListarComStatus(inicio, fim));
        }
    }
}