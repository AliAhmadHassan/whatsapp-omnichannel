using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WWeb.UI.WebformMVC.Models;

namespace WWeb.UI.WebformMVC.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Inicio = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.Fim = DateTime.Now.ToString("dd/MM/yyyy");

            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            DateTime inicio = DateTime.Now;
            DateTime fim = DateTime.Now;

            if (!string.IsNullOrEmpty(form["data_inicio"]))
                inicio = Convert.ToDateTime(form["data_inicio"]);

            if (!string.IsNullOrEmpty(form["data_fim"]))
                fim = Convert.ToDateTime(form["data_fim"]);

            ViewBag.Inicio = inicio.ToString("dd/MM/yyyy");
            ViewBag.Fim = fim.ToString("dd/MM/yyyy");

            return View();
        }

        public ActionResult UsuarioXMensagens(DateTime inicio, DateTime fim)
        {
            var relatorio = new Models.BLL.MensagemBLL().UsuarioXMensagens(inicio, new DateTime(fim.Year, fim.Month, fim.Day, 23, 59, 59));
            List<Object> dados = new List<object>();
            foreach (var item in relatorio)
                dados.Add(new object[] { item.Usuario == 0 ? "0" : new CobnetAdapter().ObterUsuario(item.Usuario).Us_Nome, item.Total });
            /*.SetTitle(new Title { Text = string.Format("Mensagens enviadas no perído de {0} a {1}",inicio.ToString("dd/MM/yyyy"),fim.ToString("dd/MM/yyyy")) })*/
            Highcharts chart = new Highcharts("chart")
               .InitChart(new Chart { PlotShadow = false, PlotBackgroundColor = null, PlotBorderWidth = null, Height = 300 })
                .SetTitle(new Title { Text = "Usuários x Mensagens" })
                .SetSubtitle(new Subtitle { Text = string.Format("De {0} a {1}", inicio.ToString("dd/MM/yyyy"), fim.ToString("dd/MM/yyyy")) })
                .SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.percentage +' %'; }" })
                .SetPlotOptions(new PlotOptions
                {
                    Pie = new PlotOptionsPie
                    {
                        AllowPointSelect = true,
                        Cursor = Cursors.Pointer,
                        DataLabels = new PlotOptionsPieDataLabels { Enabled = false },
                        ShowInLegend = true
                    }
                })
                .SetSeries(new Series
                {
                    Type = ChartTypes.Pie,
                    Name = "Mensagens x Usuários",
                    Data = new Data(dados.ToArray())
                });

            return View(chart);
        }

        public ActionResult MensagensXData(DateTime inicio, DateTime fim)
        {
            var relatorio = new Models.BLL.MensagemBLL().MensagemXPeriodo(inicio, new DateTime(fim.Year, fim.Month, fim.Day, 23, 59, 59));

            List<Series> series = new List<Series>();
            List<string> categorias = new List<string>();
            List<object> valores = new List<object>();

            foreach (var item in relatorio)
            {
                categorias.Add(item.Data.ToString());
                valores.Add(item.Total);

            }

            series.Add(new Series() { Name = "Quantidade", Data = new Data(valores.ToArray()) });

            Highcharts chart = new Highcharts("chart_mensagem_x_data")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Bar, Height = 300 })
                .SetTitle(new Title { Text = "Mensagem x Data" })
                .SetSubtitle(new Subtitle { Text = string.Format("De {0} a {1}", inicio.ToString("dd/MM/yyyy"), fim.ToString("dd/MM/yyyy")) })
               .SetXAxis(new XAxis
               {
                   Categories = categorias.ToArray(),
                   Labels = new XAxisLabels
                   {
                       Rotation = -45,
                       Align = HorizontalAligns.Right,
                       Style = "fontSize: '13px',fontFamily: 'Verdana, sans-serif'"
                   }
               })
               .SetYAxis(new YAxis
               {
                   Min = 0,
                   Title = new YAxisTitle { Text = "Quantidade" }
               }).SetPlotOptions(new PlotOptions
               {
                   Column = new PlotOptionsColumn
                   {
                       DataLabels = new PlotOptionsColumnDataLabels
                       {
                           Enabled = true,
                           Rotation = -90,
                           Color = ColorTranslator.FromHtml("#FFFFFF"),
                           Align = HorizontalAligns.Right,
                           X = 4,
                           Y = 10,
                           Formatter = "function() { return this.y; }",
                           Style = "fontSize: '13px',fontFamily: 'Verdana, sans-serif',textShadow: '0 0 3px black'"
                       }
                   }
               })
                .SetSeries(series.ToArray());

            //Highcharts chart = new Highcharts("chart")
            //   .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column, Margin = new[] { 50, 50, 100, 80 } })
            //   .SetTitle(new Title { Text = "World\\'s largest cities per 2008" })
            //   .SetXAxis(new XAxis
            //   {
            //       Categories = categorias.ToArray(),
            //       Labels = new XAxisLabels
            //       {
            //           Rotation = -45,
            //           Align = HorizontalAligns.Right,
            //           Style = "fontSize: '13px',fontFamily: 'Verdana, sans-serif'"
            //       }
            //   })
            //   .SetYAxis(new YAxis
            //   {
            //       Min = 0,
            //       Title = new YAxisTitle { Text = "Population (millions)" }
            //   })
            //   .SetLegend(new Legend { Enabled = false })
            //   .SetTooltip(new Tooltip { Formatter = "TooltipFormatter" })
            //   .SetPlotOptions(new PlotOptions
            //   {
            //       Column = new PlotOptionsColumn
            //       {
            //           DataLabels = new PlotOptionsColumnDataLabels
            //           {
            //               Enabled = true,
            //               Rotation = -90,
            //               Color = ColorTranslator.FromHtml("#FFFFFF"),
            //               Align = HorizontalAligns.Right,
            //               X = 4,
            //               Y = 10,
            //               Formatter = "function() { return this.y; }",
            //               Style = "fontSize: '13px',fontFamily: 'Verdana, sans-serif',textShadow: '0 0 3px black'"
            //           }
            //       }
            //   })
            //   .SetSeries(series.ToArray());

            return View(chart);
        }

        public ActionResult MensagensHoraHora(DateTime inicio, DateTime fim)
        {
            var relatorio = new Models.BLL.MensagemBLL().MensagemHoraHora(inicio, new DateTime(fim.Year, fim.Month, fim.Day, 23, 59, 59));

            var query = from r in relatorio
                        group r by r.Hora into g
                        select new
                        {
                            Hora = g.First().Hora,
                            Total = g.Sum(x => x.Total)
                        };

            List<Series> series = new List<Series>();
            List<string> categorias = new List<string>();
            List<object> valores = new List<object>();

            foreach (var item in query)
            {
                if (!string.IsNullOrEmpty(item.Hora))
                {
                    categorias.Add(item.Hora.ToString());
                    valores.Add(item.Total);
                }
            }

            series.Add(new Series() { Name = "Total", Data = new Data(valores.ToArray()) });

            Highcharts chart = new Highcharts("chart_mensagem_hora_x_hora")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Line, Height = 300 })
                .SetTitle(new Title { Text = "Mensagens Hora a Hora" })
                .SetSubtitle(new Subtitle { Text = string.Format("De {0} a {1}", inicio.ToString("dd/MM/yyyy"), fim.ToString("dd/MM/yyyy")) })
               .SetXAxis(new XAxis
               {
                   Categories = categorias.ToArray(),
                   Labels = new XAxisLabels
                   {
                       Rotation = -45,
                       Align = HorizontalAligns.Right,
                       Style = "fontSize: '13px',fontFamily: 'Verdana, sans-serif'"
                   }
               })
               .SetYAxis(new YAxis
               {
                   Min = 0,
                   Title = new YAxisTitle { Text = "Quantidade" }
               }).SetPlotOptions(new PlotOptions
               {
                   Column = new PlotOptionsColumn
                   {
                       DataLabels = new PlotOptionsColumnDataLabels
                       {
                           Enabled = true,
                           Rotation = -90,
                           Color = ColorTranslator.FromHtml("#FFFFFF"),
                           Align = HorizontalAligns.Right,
                           X = 4,
                           Y = 10,
                           Formatter = "function() { return this.y; }",
                           Style = "fontSize: '13px',fontFamily: 'Verdana, sans-serif',textShadow: '0 0 3px black'"
                       }
                   }
               })
                .SetSeries(series.ToArray());

            return View(chart);
        }

        public ActionResult ClienteHoraHora(DateTime inicio, DateTime fim)
        {
            var relatorio = new Models.BLL.ClienteBLL().ListarClientesHoraHora(inicio, new DateTime(fim.Year, fim.Month, fim.Day, 23, 59, 59));

            var query = from r in relatorio
                        group r by r.Hora into g
                        select new
                        {
                            Hora = g.First().Hora,
                            Total = g.Sum(x => x.Total)
                        };

            List<Series> series = new List<Series>();
            List<string> categorias = new List<string>();
            List<object> valores = new List<object>();

            foreach (var item in query)
            {
                if (!string.IsNullOrEmpty(item.Hora))
                {
                    categorias.Add(item.Hora.ToString());
                    valores.Add(item.Total);
                }
            }

            series.Add(new Series() { Name = "Total", Data = new Data(valores.ToArray()) });

            Highcharts chart = new Highcharts("chart_cliente_hora_x_hora")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Spline, Height = 300 })
                .SetTitle(new Title { Text = "Clientes Hora a Hora" })
                .SetSubtitle(new Subtitle { Text = string.Format("De {0} a {1}", inicio.ToString("dd/MM/yyyy"), fim.ToString("dd/MM/yyyy")) })
               .SetXAxis(new XAxis
               {
                   Categories = categorias.ToArray(),
                   Labels = new XAxisLabels
                   {
                       Rotation = -45,
                       Align = HorizontalAligns.Right,
                       Style = "fontSize: '13px',fontFamily: 'Verdana, sans-serif'"
                   }
               })
               .SetYAxis(new YAxis
               {
                   Min = 0,
                   Title = new YAxisTitle { Text = "Quantidade" }
               }).SetPlotOptions(new PlotOptions
               {
                   Column = new PlotOptionsColumn
                   {
                       DataLabels = new PlotOptionsColumnDataLabels
                       {
                           Enabled = true,
                           Rotation = -90,
                           Color = ColorTranslator.FromHtml("#FFFFFF"),
                           Align = HorizontalAligns.Right,
                           X = 4,
                           Y = 10,
                           Formatter = "function() { return this.y; }",
                           Style = "fontSize: '13px',fontFamily: 'Verdana, sans-serif',textShadow: '0 0 3px black'"
                       }
                   }
               })
               .SetSeries(series.ToArray());

            return View(chart);
        }

        public ActionResult ClienteAcionamento(DateTime inicio, DateTime fim)
        {
            var relatorio = new RelatoriosBLL().ListarComStatus(inicio, new DateTime(fim.Year, fim.Month, fim.Day, 23, 59, 59));
            List<Series> series = new List<Series>();
            List<string> categorias = new List<string>();
            List<object> valores = new List<object>();

            foreach (var item in relatorio)
            {
                categorias.Add(item.Data.ToString());
                valores.Add(item.Quantidade);

            }

            if (relatorio.Count > 1)
            {
                categorias.Add("MÉDIA");
                valores.Add(relatorio.Sum(s => s.Quantidade) / relatorio.Count);
            }

            series.Add(new Series() { Name = "Quantidade", Data = new Data(valores.ToArray()) });

            Highcharts chart = new Highcharts("chart_acionamento")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column, Height = 300 })
                .SetTitle(new Title { Text = "Clientes x Acionamento" })
                .SetSubtitle(new Subtitle { Text = string.Format("De {0} a {1}", inicio.ToString("dd/MM/yyyy"), fim.ToString("dd/MM/yyyy")) })
                .SetXAxis(new XAxis
                {
                    Categories = categorias.ToArray(),
                    Labels = new XAxisLabels
                    {
                        Rotation = -45,
                        Align = HorizontalAligns.Right,
                        Style = "fontSize: '13px',fontFamily: 'Verdana, sans-serif'"
                    }
                })
                .SetYAxis(new YAxis
                {
                    Min = 0,
                    Title = new YAxisTitle
                    {
                        Text = "Quantidade",
                        Align = AxisTitleAligns.High
                    }
                })
                .SetSeries(series.ToArray());

            return View(chart);
        }

        public ActionResult UsuarioXMensagensQuantidade(DateTime inicio, DateTime fim)
        {
            var relatorio = new Models.BLL.MensagemBLL().UsuarioXMensagens(inicio, new DateTime(fim.Year, fim.Month, fim.Day, 23, 59, 59));

            List<Series> series = new List<Series>();
            List<string> categorias = new List<string>();


            foreach (var item in relatorio)
            {
                List<object> valores = new List<object>();
                var nome_usuario = item.Usuario == 0 ? "0" : new CobnetAdapter().ObterUsuario(item.Usuario).Us_Nome;
                categorias.Add("Usuário(s)");
                valores.Add(item.Total);
                series.Add(new Series() { Name = nome_usuario, Data = new Data(valores.ToArray()) });
            }



            Highcharts chart = new Highcharts("chart_acionamento_qyuantidade")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Bar, Height = 300 })
                .SetTitle(new Title { Text = "Usuário x Mensagens" })
                .SetSubtitle(new Subtitle { Text = string.Format("De {0} a {1}", inicio.ToString("dd/MM/yyyy"), fim.ToString("dd/MM/yyyy")) })
                .SetXAxis(new XAxis
                {
                    Categories = categorias.ToArray(),
                    Labels = new XAxisLabels
                    {
                        Rotation = -45,
                        Align = HorizontalAligns.Right,
                        Style = "fontSize: '13px',fontFamily: 'Verdana, sans-serif'"
                    }
                })
                .SetYAxis(new YAxis
                {
                    Min = 0,
                    Title = new YAxisTitle
                    {
                        Text = "Quantidade",
                        Align = AxisTitleAligns.High
                    }
                })
                .SetSeries(series.ToArray());

            return View(chart);
        }

        public ActionResult StatusRobos()
        {

            List<Models.Chavewhatsapp> status = new List<Chavewhatsapp>();
            using (WWebEntities ctx = new WWebEntities())
                status = ctx.Chavewhatsapp.ToList();

            return View(status);
        }

        public ActionResult FinalizarSessao(int us_id)
        {
            var usu = WWebSessaoGlobal.UsuariosOnLines.Where(u => u.Usuario.Us_ID.Equals(us_id)).FirstOrDefault();
            if (usu != null)
                WWebSessaoGlobal.UsuariosOnLines.Remove(usu);

            return RedirectToAction("Index");
        }

        public ActionResult ClientesXTelefone(string telefone_orcozol)
        {
            List<WWeb.UI.WebformMVC.Models.DTO.ClienteDTO> clientes = new List<Models.DTO.ClienteDTO>();
            return View(new Models.BLL.ClienteBLL().ListarPeloTelefoneOrcozol(telefone_orcozol));
        }
    }
}