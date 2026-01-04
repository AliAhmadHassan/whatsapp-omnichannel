using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WWeb.UI.WebformMVC.Controllers
{
    public class SituacaoController : Controller
    {
        public ActionResult Index()
        {

            List<Models.Situacao> situacoes = new List<Models.Situacao>();
            using (Models.WWebEntities ctx = new Models.WWebEntities())
                situacoes = ctx.Situacao.ToList();

            return View(situacoes);
        }

        [HttpGet]
        public ActionResult Cadastrar()
        {
            return View(new Models.Situacao());
        }

        [HttpPost]
        public ActionResult Cadastrar(FormCollection forms)
        {
            var chk_bloqueado = forms.AllKeys.Where(c => c.Contains("cb_")).FirstOrDefault();
            using (WWeb.UI.WebformMVC.Models.WWebEntities ctx = new Models.WWebEntities())
            {
                ctx.Situacao.Add(new Models.Situacao() { Descricao = forms["Descricao"], Bloqueado = chk_bloqueado == null ? false : true });
                ctx.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            Models.Situacao situacao = new Models.Situacao();
            using (Models.WWebEntities ctx = new Models.WWebEntities())
                situacao = ctx.Situacao.Where(s => s.Id.Equals(id)).FirstOrDefault();

            return View(situacao);
        }

        [HttpPost]
        public ActionResult Editar(FormCollection situacao)
        {
            var chk_bloqueado = situacao.AllKeys.Where(c => c.Contains("cb_")).FirstOrDefault();
            using (Models.WWebEntities ctx = new Models.WWebEntities())
            {
                int id = Convert.ToInt32(situacao["Id"]);
                var ms = ctx.Situacao.Where(m => m.Id.Equals(id)).FirstOrDefault();
                if (ms != null)
                {
                    ms.Bloqueado = chk_bloqueado == null ? false : true;
                    ms.Descricao = situacao["Descricao"];
                    ctx.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Bloquear(int id)
        {
            using (Models.WWebEntities ctx = new Models.WWebEntities())
            {
                var sit = ctx.Situacao.Where(s => s.Id.Equals(id)).FirstOrDefault();
                if (sit != null)
                {
                    sit.Bloqueado = true;
                    ctx.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }
    }
}