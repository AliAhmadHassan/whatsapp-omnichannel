var mensagens = new Array();
var contador_mensagens = 0;
var existe_mensagem = false;
var contador = $('#hdn_timeout').val();


function handler_beforeunload() {

    var protocolo = window.location.protocol;
    var root_site = window.location.host;
    var uri = protocolo + '//' + root_site;

    var telefone_cliente = '{ "telefone_cliente":"' + $('#hdn_telefonecliente').val() + '" }';
    var retorno_servidor = false;

    $.ajax({
        url: uri + "/BatePapo/RemoverConversa/",
        dataType: 'json',
        data: telefone_cliente,
        type: 'POST',
        contentType: 'application/json',
        cache: false,
        async: false,
        success: function (data) {
            try { }
            catch (e) { }
        },
        error: function (request, status, err) {
            retorno_servidor = true;
            Mensagem(status);
            Mensagem(err);
        }
    });
}

window.onbeforeunload = handler_beforeunload;

$(window).bind('beforeunload', handler_beforeunload);
$(window).on('beforeunload', handler_beforeunload);

var intervalo_consulta = self.setInterval(function () { ConsultarMensagens() }, 1000);

String.prototype.padleft = function (padString, length) {
    var str = this;

    while (str.length < length)
        str = padString + str;

    return str;
}

if (!String.prototype.trim) {
    (function () {
        var rtrim = /^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g;
        String.prototype.trim = function () {
            return this.replace(rtrim, '');
        };
    })();
}

$(window).unload(function () {

    var protocolo = window.location.protocol;
    var root_site = window.location.host;
    var uri = protocolo + '//' + root_site;

    var telefone_cliente = '{ "telefone_cliente":"' + $('#hdn_telefonecliente').val() + '" }';
    var retorno_servidor = false;

    $.ajax({
        url: uri + "/BatePapo/RemoverConversa/",
        dataType: 'json',
        data: telefone_cliente,
        type: 'POST',
        contentType: 'application/json',
        cache: false,
        async: false,
        success: function (data) {
            try { }
            catch (e) { }
        },
        error: function (request, status, err) {
            retorno_servidor = true;
            Mensagem(status);
            Mensagem(err);
        }
    });
});

$('#btn-recarregar').click(function () {
    ConsultarMensagens()
});

$('#txt_mensagem').focus();

$('').click(function () {
    $(this).effect("size", {
        to: { width: 200, height: 60 }
    }, 1000);
})

$("#img-foto").hover(
   function () {
       $(this).animate({ height: "+=400", width: "+=550" }, "fast");
   },
   function () {
       $(this).animate({ height: "-=400", width: "-=550" }, "fast");
   }
);

$('#btn_buscar_cobnet').click(function () {
    var cpf = $('#txt_cpfcliente').val();
    cpf = cpf.replace('.', '').replace('.', '').replace('-', '').replace('/', '').replace('-', '').trim().padleft('0', 15);

    var url = $('#hdn_urlcrm').val() + cpf;
    AbrirPopup(url);
});

$('#btn_finalizar_conversa').click(function () {

    var sit = $('#Sit_ID').val();
    if (sit == '0') {
        alert('Você deve selecionar uma situação para esta conversa.');
        return;
    }

    var telefone_cliente = '{ "telefone_cliente":"' + $('#hdn_telefonecliente').val() + '","telefone_orcozol":"' + $('#hdn_telefoneorcozol').val() + '","id_situacao":"' + sit + '" }';

    $('#txt_mensagem').prop('disabled', true);
    $('#btn_enviar').prop('disabled', true);

    $.ajax({
        url: "/BatePapo/FinalizarConversa/",
        data: telefone_cliente,
        dataType: 'json',
        type: 'POST',
        cache: false,
        async: false,
        contentType: 'application/json',
        success: function (data) {
            try {

            } catch (e) {

            }
        },
        error: function (request, status, err) {
            Mensagem(status);
            Mensagem(err);
        }
    });

    window.close();
});

function AbrirPopup(url) {
    window.open(url, "", "");
}

var primeira_consulta_mensagens = true;
var total_mensagens_exibidas = 0;

function ConsultarMensagens() {
    clearInterval(intervalo_consulta);
    var telefone_cliente = '{ "telefone_cliente":"' + $('#hdn_telefonecliente').val() + '" }';
    $.ajax({
        url: "/BatePapo/Mensagens/",
        data: telefone_cliente,
        dataType: 'json',
        type: 'POST',
        contentType: 'application/json',
        success: function (data) {
            try {

                if (primeira_consulta_mensagens) {
                    data = JSON.parse(data);
                    for (var i = 0; i < data.length; i++) {
                        existe_mensagem = false;
                        for (var x = 0; x < mensagens.length; x++) {
                            if (mensagens[x] == data[i].Id) {
                                existe_mensagem = true;
                                break;
                            }
                        }

                        if (!existe_mensagem)
                            AddMensagems(data[i].Id, data[i].Mensagem, data[i].DataMensagem, data[i].Tipo);
                    }

                    total_mensagens_exibidas = data.length;
                    primeira_consulta_mensagens = false;
                }
                else {

                    data = JSON.parse(data);
                    if (total_mensagens_exibidas != data.length) {
                        for (var i = 0; i < data.length; i++) {
                            existe_mensagem = false;
                            for (var x = 0; x < mensagens.length; x++) {
                                if (mensagens[x] == data[i].Id) {
                                    existe_mensagem = true;
                                    break;
                                }
                            }

                            if (!existe_mensagem)
                                AddMensagems(data[i].Id, data[i].Mensagem, data[i].DataMensagem, data[i].Tipo);
                        }
                    }
                }

                intervalo_consulta = self.setInterval(function () { ConsultarMensagens() }, 5000);

            } catch (e) {
                Mensagem(e);
                intervalo_consulta = self.setInterval(function () { ConsultarMensagens() }, 5000);
            }
        },
        error: function (request, status, err) {
            Mensagem(status);
            Mensagem(err);
            intervalo_consulta = self.setInterval(function () { ConsultarMensagens() }, 5000);
        }
    });
}

function ConsultarMensagensPredefinida() {
    $.ajax({
        url: "/Cadastro/RetornaMensagensPredefinidas/",
        type: 'GET',
        success: function (data) {
            try {
                $('#content_mensagem').html(data);
                $('.content_mensagem_item').click(function () {
                    $('#txt_mensagem').val('');
                    $('#txt_mensagem').val(this.innerText);
                });
            } catch (e) {
                Mensagem(e);
            }
        },
        error: function (request, status, err) {
            Mensagem(status);
            Mensagem(err);
        }
    });
}

function EnviarMensagem() {

    var telefone_cliente = '{ "telefone_cliente":"' + $('#hdn_telefonecliente').val() + '","mensagem":"' + $('#txt_mensagem').val() + '","telefone_orcozol":"' + $('#hdn_telefoneorcozol').val() + '" }';
    if ($('#txt_mensagem').val() == '') {
        alert("Digite uma mensagem para ser enviada!")
        $('#txt_mensagem').focus();
        return;
    }

    $('#txt_mensagem').prop('disabled', true);
    $('#btn_enviar').prop('disabled', true);

    $.ajax({
        url: "/BatePapo/PublicarMensagen/",
        data: telefone_cliente,
        dataType: 'json',
        type: 'POST',
        cache: false,
        async: false,
        contentType: 'application/json',
        success: function (data) {
            try {
                ConsultarMensagens();
                $('#txt_mensagem').val('');
                $('#txt_mensagem').focus();
                contador = $('#hdn_timeout').val();
            } catch (e) {

            }

            $('#txt_mensagem').prop('disabled', false);
            $('#btn_enviar').prop('disabled', false);
        },
        error: function (request, status, err) {
            Mensagem(status);
            Mensagem(err);
        }
    });
}

function AddMensagems(id, msg, data, tipo) {

    var newLI = document.createElement("LI");

    var dateString = data.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    var hora = currentTime.getHours();
    var minuto = currentTime.getMinutes();
    var segundo = currentTime.getSeconds();
    var date = day + "/" + month + "/" + year + " " + hora + ":" + minuto + ":" + segundo;

    var div_container = "";
    var div_mensagem = "<div class='msg-msg' style='font-size:16px;font-family:Trebuchet MS'; text-decoration:dotted;>" + msg + "</div>";
    var div_datahora = "";


    if (tipo == "1") {
        div_datahora = "<div class='data-msg'>» Orcozol:" + date + "</div>";
        div_container = "<div id='msg-orcozol'>" + div_datahora + " <hr style='width: 100%; height: 1px; border: none; background-color: #efefef;' />" + div_mensagem + "</div>";
    }
    else if (tipo == "2") {
        div_datahora = "<div class='data-msg'>» Cliente:" + date + "</div>";
        div_container = "<div id='msg-cliente'>" + div_datahora + " <hr style='width: 100%; height: 1px; border: none; background-color: #efefef;' />" + div_mensagem + "</div>";
    }
    else if (tipo == "3") {
        div_datahora = "<div class='data-msg'>» Orcozol:" + date + "</div>";
        div_container = "<div id='msg-orcozol-finalizada'>" + div_datahora + " <hr style='width: 100%; height: 1px; border: none; background-color: #efefef;' />" + div_mensagem + "</div>";
    }
    else if (tipo == "4") {
        div_datahora = "<div class='data-msg'>» Orcozol:" + date + "</div>";
        div_container = "<div id='msg-orcozol-expirada'>" + div_datahora + " <hr style='width: 100%; height: 1px; border: none; background-color: #efefef;' />" + div_mensagem + "</div>";
    }

    var ul = document.getElementById("lst_mensagens");
    var newLI = document.createElement("LI");
    newLI.innerHTML = div_container;
    ul.insertBefore(newLI, ul.childNodes[0]);

    mensagens[contador_mensagens] = id;
    contador_mensagens++;
}

$('#btn_enviar').click(function () {
    EnviarMensagem();
})

$('#txt_mensagem').keypress(function (e) {
    if (e.which == 13) {
        EnviarMensagem();
    }
});

$('#btn_atualizar_nome').click(function () {
    AtualizarNomeCliente();
});

function AtualizarNomeCliente() {
    var telefone_cliente = '{ "telefone_cliente":"' + $('#hdn_telefonecliente').val() + '","nome_cliente":"' + $('#txt_nomecliente').val() + '","cpf_cliente":"' + $('#txt_cpfcliente').val() + '" }';
    $.ajax({
        url: "/BatePapo/AtualizarNomeCliente/",
        data: telefone_cliente,
        dataType: 'json',
        type: 'POST',
        contentType: 'application/json',
        success: function (data) {
            try {
                $('#span-mensagem').html(data);
            } catch (e) {

            }
        },
        error: function (request, status, err) {
        }
    });
}

ConsultarMensagensPredefinida();

