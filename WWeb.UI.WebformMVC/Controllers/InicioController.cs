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
    public class InicioController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult IndexOperador()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public JsonResult Validar(string usuario, string senha)
        {
            RespostaAutenticacao resposta = new RespostaAutenticacao();

            Tb_Usuario usu = new CobnetAdapter().ValidarUsuarioCobnet(usuario, senha);
            if (usu != null)
            {
                resposta.responseAutenticado = "1";
                resposta.responseErro = "0";
                resposta.responseMsg = "Usuário OK";

                if (Session["Usuario"] == null)
                    Session.Add("Usuario", usu);

                var us = WWebSessaoGlobal.UsuariosOnLines.Where(u => u.Usuario.Us_ID.Equals(usu.Us_ID)).FirstOrDefault();
                if (us == null)
                    WWebSessaoGlobal.UsuariosOnLines.Add(new UsuarioOnLine() { Usuario = usu, SessionID = Session.SessionID });

                List<WWeb.UI.WebformMVC.Models.Acesso> acessos = new List<Models.Acesso>();
                using (WWeb.UI.WebformMVC.Models.WWebEntities ctx = new Models.WWebEntities())
                    acessos = ctx.Acesso.Where(a => a.Departamento.Equals(usu.Dep_ID)).ToList();

                Session.Add("Acesso", acessos);

                if (acessos.Count > 0)
                    resposta.responseUrl = "/Dashboard/Index/";
                else
                    resposta.responseUrl = "/Inicio/IndexOperador/";


            }
            else
            {
                resposta.responseAutenticado = "0";
                resposta.responseErro = "0";
                resposta.responseMsg = "Falha na autenticação! Verifique o usuário e a senha informados.";
                resposta.responseUrl = "";
            }

            return Json(resposta);
        }

        public JsonResult Logout(string usuario, string senha)
        {
            RespostaAutenticacao resposta = new RespostaAutenticacao();

            try
            {
                Tb_Usuario usu = (Session["Usuario"] as Tb_Usuario);
                if (usu == null)
                {
                    resposta.responseAutenticado = "1";
                    resposta.responseErro = "0";
                    resposta.responseMsg = "A sessão do usuário não foi encontrada.";
                    resposta.responseUrl = "/Inicio/Login/";

                    return Json(resposta);
                }

                WWebSessaoGlobal.UsuariosOnLines.Remove(WWebSessaoGlobal.UsuariosOnLines.Where(u => u.SessionID == Session.SessionID).FirstOrDefault());
                List<ClientesOnLines> clientes_on_lines = WWebSessaoGlobal.ClientesEmNegociacao.Where(c => c.UsuarioConversando.Us_ID.Equals(usu.Us_ID)).ToList();
                foreach (var cli in clientes_on_lines)
                    WWebSessaoGlobal.RemoverCliente(cli);

                Session.Abandon();

                resposta.responseAutenticado = "1";
                resposta.responseErro = "0";
                resposta.responseMsg = "Usuário OK";
                resposta.responseUrl = "/Inicio/Login/";
            }
            catch (Exception ex)
            {
                resposta.responseAutenticado = "1";
                resposta.responseErro = "1";
                resposta.responseMsg = ex.Message;
                resposta.responseUrl = "/Inicio/Login/";
            }

            return Json(resposta);
        }

        public ActionResult LogoutEnd()
        {
            Tb_Usuario usu = (Session["Usuario"] as Tb_Usuario);

            List<ClientesOnLines> clientes_on_lines = WWebSessaoGlobal.ClientesEmNegociacao.Where(c => c.UsuarioConversando.Us_ID.Equals(usu.Us_ID)).ToList();
            foreach (var cli in clientes_on_lines)
                WWebSessaoGlobal.RemoverCliente(cli);

            Session.Abandon();
           return RedirectToAction("Login");
        }
    }
}