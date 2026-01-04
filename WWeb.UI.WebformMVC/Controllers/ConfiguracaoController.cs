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
    public class ConfiguracaoController : Controller
    {
        public ActionResult Index()
        {
            List<WWeb.UI.WebformMVC.Models.Configuracao> confs = new List<Models.Configuracao>();
            using (WWeb.UI.WebformMVC.Models.WWebEntities ctx = new Models.WWebEntities())
                confs = ctx.Configuracao.ToList();

            return View(confs);
        }

        [HttpGet]
        public ActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Cadastro(FormCollection config)
        {
            var chk_bloqueado = config.AllKeys.Where(c => c.Contains("cb_")).FirstOrDefault();
            using (WWeb.UI.WebformMVC.Models.WWebEntities ctx = new Models.WWebEntities())
            {
                ctx.Configuracao.Add(new Models.Configuracao() { Descricao = config["Descricao"], Valor = config["Valor"], Bloqueada = chk_bloqueado == null ? false : true });
                ctx.SaveChanges();
            }

            return RedirectToAction("Index", "Configuracao");
        }

        public ActionResult Deletar(int id)
        {
            using (WWeb.UI.WebformMVC.Models.WWebEntities ctx = new Models.WWebEntities())
            {
                var config = ctx.Configuracao.Where(c => c.Id == id).FirstOrDefault();
                if (config != null)
                {
                    ctx.Configuracao.Remove(config);
                    ctx.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            WWeb.UI.WebformMVC.Models.Configuracao config = null;
            using (WWeb.UI.WebformMVC.Models.WWebEntities ctx = new Models.WWebEntities())
                config = ctx.Configuracao.Where(c => c.Id == id).FirstOrDefault();

            return View(config);
        }

        [HttpPost]
        public ActionResult Editar(FormCollection config)
        {
            var chk_bloqueado = config.AllKeys.Where(c => c.Contains("cb_")).FirstOrDefault();
            using (WWeb.UI.WebformMVC.Models.WWebEntities ctx = new Models.WWebEntities())
            {
                int id = Convert.ToInt32(config["id"]);

                var conf = ctx.Configuracao.Where(c => c.Id == id).FirstOrDefault();
                if (conf != null)
                {
                    conf.Bloqueada = chk_bloqueado == null ? false : true;
                    conf.Descricao = config["Descricao"];
                    conf.Valor = config["Valor"];
                    ctx.SaveChanges();
                }
            }

            return RedirectToAction("Index", "Configuracao");
        }

        [HttpGet]
        public ActionResult IndexAcesso()
        {
            return View(new CobnetAdapter().ListarDepartamentos().OrderBy(d => d.Dep_Nome).ToList<Tb_Departamento>());
        }

        [HttpGet]
        public ActionResult EditarAcesso()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditarAcesso(FormCollection form)
        {
            using (WWeb.UI.WebformMVC.Models.WWebEntities ctx = new Models.WWebEntities())
            {
                int dep = Convert.ToInt32(form["Dep_ID"]);

                ctx.Acesso.RemoveRange(ctx.Acesso.Where(a => a.Departamento.Equals(dep)));
                ctx.SaveChanges();

                if (form["cb_manu"] == "on")
                    ctx.Acesso.Add(new Models.Acesso() { Departamento = dep, Menu = 1 });

                if (form["cb_conf"] == "on")
                    ctx.Acesso.Add(new Models.Acesso() { Departamento = dep, Menu = 2 });

                if (form["cb_moni"] == "on")
                    ctx.Acesso.Add(new Models.Acesso() { Departamento = dep, Menu = 3 });

                if (form["cb_rela"] == "on")
                    ctx.Acesso.Add(new Models.Acesso() { Departamento = dep, Menu = 4 });

                ctx.SaveChanges();

            }
            return RedirectToAction("IndexAcesso");
        }

        public ActionResult VisualizarAcesso(int dep_id, string dep_nome)
        {
            List<WWeb.UI.WebformMVC.Models.Acesso> acessos = new List<Models.Acesso>();
            using (WWeb.UI.WebformMVC.Models.WWebEntities ctx = new Models.WWebEntities())
            {
                ViewBag.Dep_ID = dep_id;
                ViewBag.Dep_Nome = dep_nome;
                acessos = ctx.Acesso.Where(a => a.Departamento.Equals(dep_id)).ToList<WWeb.UI.WebformMVC.Models.Acesso>();
            }

            return View(acessos);
        }
    }
}