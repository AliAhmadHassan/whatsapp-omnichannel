function handler_beforeunload(event) {

    var protocolo = window.location.protocol;
    var root_site = window.location.host;
    var uri = protocolo + '//' + root_site;

    $.ajax({
        url: uri + "/Inicio/Logout",
        dataType: 'json',
        type: 'POST',
        contentType: 'application/json',
        cache: false,
        async: false,
        success: function (data) {
            try { }
            catch (e) { }
        },
        error: function (request, status, err) {
            Mensagem(status);
            Mensagem(err);
        }
    });
}

$(document).ready(function () {

    $("#tabs").tabs();

    $(document).tooltip({ position: { my: "left center", at: "right+10 center", collision: "flipfit" }, tooltipClass: "tooltip_classe" });

    $.ajaxSetup({ cache: false });

    jQuery.fn.center = function () {

        this.css("position", "absolute");

        this.css("top", Math.max(0, (($(window).height() - $(this).outerHeight()) / 2) + $(window).scrollTop()) + "px");
        this.css("left", Math.max(0, (($(window).width() - $(this).outerWidth()) / 2) + $(window).scrollLeft()) + "px");

        return this;
    }

    jQuery.fn.centerhorizontal = function () {

        this.css("position", "absolute");

        this.css("left", Math.max(0, (($(window).width() - $(this).outerWidth()) / 2) + $(window).scrollLeft()) + "px");

        return this;
    }

    $(function () {
        $('table').footable();

        $('#change-page-size').change(function (e) {
            e.preventDefault();
            var pageSize = $(this).val();
            $('.footable').data('page-size', pageSize);
            $('.footable').trigger('footable_initialized');
        });

        $('#change-nav-size').change(function (e) {
            e.preventDefault();
            var navSize = $(this).val();
            $('.footable').data('limit-navigation', navSize);
            $('.footable').trigger('footable_initialized');
        });
    });

    $('table').filterTable({
        callback: function (term, table) {
            table.find('tr').removeClass('striped').filter(':visible:even').addClass('striped');
        }
    });

    Loader('loader-spin');

    $('#btn_logout').click(function () {
        Logout();
    });

    $.support.cors = true;

    $.ajaxSetup({ cache: false });

    $.ajaxSettings.cache = false;

    $("#img-foto").hover(
       function () { $(this).animate({ height: "+=400", width: "+=550" }, "fast"); }
       ,
       function () { $(this).animate({ height: "-=400", width: "-=550" }, "fast"); }
    );

    $(document).ajaxStart(function () {
        $("#loader-bg").css("display", "block");
    });

    $(document).ajaxComplete(function () {
        $("#loader-bg").css("display", "none");
    });

    $('#btn_atualizar_nome').click(function () {
        AtualizarNomeCliente();
    });

});

function EnviarParaFila(telefone_cliente) {

    var jsonData = '{ "telefone_cliente":"' + telefone_cliente + '"}';

    var protocolo = window.location.protocol;
    var root_site = window.location.host;
    var uri = protocolo + '//' + root_site;

    if (confirm('Deseja enviar o cliente: ' + telefone_cliente + ', para a fila de atendimento?')) {
        $.ajax({
            url: uri + "/Cliente/RetornarFila",
            data: jsonData,
            dataType: 'json',
            type: 'POST',
            contentType: 'application/json',
            success: function (data) {
                try {
                    Mensagem(data.responseMsg);
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
}

function Loader(element) {
    var opts = {
        lines: 8, // The number of lines to draw
        length: 0, // The length of each line
        width: 35, // The line thickness
        radius: 14, // The radius of the inner circle
        corners: 10, // Corner roundness (0..1)
        rotate: 3, // The rotation offset
        direction: 1, // 1: clockwise, -1: counterclockwise
        color: '#fff', // #rgb or #rrggbb or array of colors
        speed: 1.0, // Rounds per second
        trail: 13, // Afterglow percentage
        shadow: true, // Whether to render a shadow
        hwaccel: true, // Whether to use hardware acceleration
        className: 'spinner', // The CSS class to assign to the spinner
        zIndex: 2e9, // The z-index (defaults to 2000000000)
        top: '50%', // Top position relative to parent
        left: '50%' // Left position relative to parent
    };
    var target = document.getElementById(element);
    var spinner = new Spinner(opts).spin(target);
}

function Mensagem(mensagem) {

    $('#div_mensagem_dialog').html(mensagem);
    $('#div_mensagem_dialog').dialog({
        title: "Mensagem da Aplicação",
        show: { effect: "shake", duration: 300 },
        hide: { effect: "clip", duration: 300 },
        minHeight: 100,
        minWidth: 100,
        modal: true,
        buttons: {
            Ok: function () {
                $(this).dialog("close");
            }
        }
    });
}

function ConverterData(data) {
    // Obtém a data/hora atual

    var dia = data.getDate();
    var dia_sem = data.getDay();
    var mes = data.getMonth();
    var ano2 = data.getYear();
    var ano4 = data.getFullYear();
    var hora = data.getHours();
    var min = data.getMinutes();
    var seg = data.getSeconds();
    var mseg = data.getMilliseconds();
    var tz = data.getTimezoneOffset();

    var str_data = dia + '/' + (mes + 1) + '/' + ano4;
    var str_hora = hora + ':' + min + ':' + seg + "." + mseg;

    return str_hora; //str_data + ' ' +
}

function Logout() {

    var protocolo = window.location.protocol;
    var root_site = window.location.host;
    var uri = protocolo + '//' + root_site;

    $.ajax({
        url: uri + "/Inicio/Logout",
        dataType: 'json',
        type: 'POST',
        contentType: 'application/json',
        cache: false,
        async: false,
        success: function (data) {
            try {
                var url = data.responseUrl;
                window.location.replace(url);
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

function RemoverClienteAtendimento(tel_cliente) {
    var protocolo = window.location.protocol;
    var root_site = window.location.host;
    var uri = protocolo + '//' + root_site;

    var telefone_cliente = '{ "telefone_cliente":"' + tel_cliente + '" }';

    $.ajax({
        url: uri + "/BatePapo/RemoverConversa/",
        dataType: 'json',
        data: telefone_cliente,
        type: 'POST',
        contentType: 'application/json',
        cache: false,
        async: false,
        success: function (data) {
            try {
                window.location.replace(uri + "/Monitoria/Index/");
                Mensagem(data.responseMsg);
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

function AtualizarNomeCliente() {

    var sit = $('#Sit_ID').val();
    if (sit == '0') {
        alert('Você deve selecionar uma situação para esta conversa.');
        return;
    }

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
            alert("ERRO!" + status);
            alert("ERRO!" + err);
        }
    });

   
    var telefone_cliente_status = '{ "telefone_cliente":"' + $('#hdn_telefonecliente').val() + '","id_situacao":"' + sit + '" }';

    $.ajax({
        url: "/BatePapo/AtualizarStatus/",
        data: telefone_cliente_status,
        dataType: 'json',
        type: 'POST',
        cache: false,
        async: false,
        contentType: 'application/json',
        success: function (data) {
            try {

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