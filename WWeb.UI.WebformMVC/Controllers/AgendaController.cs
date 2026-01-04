using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WWeb.UI.WebformMVC.Controllers
{
    public class AgendaController : Controller
    {
        public ActionResult Index()
        {
            Tb_Usuario usu = (Session["Usuario"] as Tb_Usuario);
            List<WWeb.UI.WebformMVC.Models.DTO.AgendaDTO> agendas = new List<Models.DTO.AgendaDTO>();

            using (Models.WWebEntities ctx = new Models.WWebEntities())
            {
                var data = DateTime.Now;
                DateTime inicio = new DateTime(data.Year, data.Month, data.Day, 0, 0, 0, 0);

                var ag = ctx.Agenda.Where(a => a.UsuarioId == usu.Us_ID && a.Data == inicio);

                foreach (var item in ag)
                    agendas.Add(new Models.DTO.AgendaDTO() { AgendaId = item.AgendaId, UsuarioId = item.UsuarioId.Value, Anotacao = item.Anotacao, Data = item.Data.Value });
            }

            ViewBag.Data = DateTime.Now.ToString("dd/MM/yyyy");
            return PartialView(agendas);
        }

        public ActionResult IndexData(DateTime data)
        {
            Tb_Usuario usu = (Session["Usuario"] as Tb_Usuario);
            List<WWeb.UI.WebformMVC.Models.DTO.AgendaDTO> agendas = new List<Models.DTO.AgendaDTO>();

            using (Models.WWebEntities ctx = new Models.WWebEntities())
            {
                if (data == null)
                    data = DateTime.Now;

                DateTime inicio = new DateTime(data.Year, data.Month, data.Day, 0, 0, 0, 0);
                var ag = ctx.Agenda.Where(a => a.UsuarioId == usu.Us_ID && a.Data == inicio);

                foreach (var item in ag)
                    agendas.Add(new Models.DTO.AgendaDTO() { AgendaId = item.AgendaId, UsuarioId = item.UsuarioId.Value, Anotacao = item.Anotacao, Data = item.Data.Value });
            }

            ViewBag.Data = data.ToString("dd/MM/yyyy");
            return PartialView("Index", agendas);
        }

        [HttpGet]
        public ActionResult Cadastrar()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Cadastrar(Models.DTO.AgendaDTO agenda)
        {
            Tb_Usuario usu = (Session["Usuario"] as Tb_Usuario);

            using (Models.WWebEntities ctx = new Models.WWebEntities())
            {
                ctx.Agenda.Add(new Models.Agenda() { Data = agenda.Data, Anotacao = agenda.Anotacao, UsuarioId = usu.Us_ID });
                ctx.SaveChanges();
            }
            return RedirectToAction("IndexOperador", "Inicio");
        }

        public ActionResult Deletar(int id)
        {
            using (Models.WWebEntities ctx = new Models.WWebEntities())
            {
                ctx.Agenda.Remove(ctx.Agenda.Where(a => a.AgendaId == id).FirstOrDefault());
                ctx.SaveChanges();
            }

            return RedirectToAction("IndexOperador", "Inicio");
        }
    }
}