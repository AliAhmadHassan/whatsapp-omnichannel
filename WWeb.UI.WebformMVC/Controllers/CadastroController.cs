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
    public class CadastroController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            List<Models.MensagemPredefinida> msgs = new List<Models.MensagemPredefinida>();
            using (Models.WWebEntities ctx = new Models.WWebEntities())
                msgs = ctx.MensagemPredefinida.ToList();


            return View(msgs);
        }

        [HttpGet]
        public ActionResult CadastrarMensagem()
        {
            return PartialView(new WWeb.UI.WebformMVC.Models.MensagemPredefinida());
        }

        [HttpGet]
        public ActionResult EditarMensagem(int id_mensagem)
        {
            using (Models.WWebEntities ctx = new Models.WWebEntities())
                return View(ctx.MensagemPredefinida.Where(m => m.Id.Equals(id_mensagem)).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult EditarMensagem(FormCollection msg)
        {
            var chk_bloqueado = msg.AllKeys.Where(c => c.Contains("cb_")).FirstOrDefault();
            using (Models.WWebEntities ctx = new Models.WWebEntities())
            {
                int id = Convert.ToInt32(msg["Id"]);
                var ms = ctx.MensagemPredefinida.Where(m => m.Id.Equals(id)).FirstOrDefault();
                if (ms != null)
                {
                    ms.Bloqueada = chk_bloqueado == null ? false : true;
                    ms.Mensagem = msg["Mensagem"];
                    ctx.SaveChanges();
                }
            }

            return RedirectToAction("Index", "Cadastro");
        }

        [HttpPost]
        public ActionResult CadastrarMensagem(FormCollection forms)
        {
            var chk_bloqueado = forms.AllKeys.Where(c => c.Contains("cb_")).FirstOrDefault();
            using (WWeb.UI.WebformMVC.Models.WWebEntities ctx = new Models.WWebEntities())
            {
                ctx.MensagemPredefinida.Add(new Models.MensagemPredefinida() { Mensagem = forms["Mensagem"], Bloqueada = chk_bloqueado == null ? false : true });
                ctx.SaveChanges();
            }

            return RedirectToAction("Index", "Cadastro");
        }

        public ActionResult DeletarMensagem(int id)
        {
            try
            {
                using (WWeb.UI.WebformMVC.Models.WWebEntities ctx = new Models.WWebEntities())
                {
                    var msg = ctx.MensagemPredefinida.Where(m => m.Id.Equals(id)).FirstOrDefault();
                    if (msg != null)
                    {
                        ctx.MensagemPredefinida.Remove(msg);
                        ctx.SaveChanges();
                    }
                }
            }
            finally
            {

            }
            return RedirectToAction("Index");
        }

        public ActionResult RetornaMensagensPredefinidas()
        {
            List<Models.MensagemPredefinida> msgs = new List<Models.MensagemPredefinida>();
            using (Models.WWebEntities ctx = new Models.WWebEntities())
                msgs = ctx.MensagemPredefinida.Where(m => m.Bloqueada == false).ToList();

            return View(msgs);
        }
    }
}