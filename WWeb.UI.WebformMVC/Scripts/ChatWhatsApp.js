var handler_consulta = null;
var intervalo_consulta = 1000;
var minimizado = false;

$('#loader-bg').remove();

function CriarDivClientesOnline() {
    try {

        var iDiv = document.createElement('div');
        iDiv.id = 'div-clientes';
        iDiv.setAttribute('style', 'padding:10px;position:absolute;height:450px; width:550px; float:right; bottom:5px; right:5px; border:1px solid #efefef;background-color:#fff;overflow:hidden;');

        var body = document.getElementsByTagName('body');
        body[0].appendChild(iDiv);

    } catch (e) {
        alert(e);
    }
}

var primeira_consulta = true;

function LoadClientes() {
    try {


        clearInterval(handler_consulta);

        var protocolo = window.location.protocol;
        var root_site = window.location.host;
        var uri = protocolo + '//' + root_site;

        //var var_telefone_cliente = '{ "primeira_consulta":"' + primeira_consulta +'" }';localho9st:40224
        $.ajax({
            url: uri + "/Cliente/RetornarClientesOnlines?primeira_consulta=" + primeira_consulta,
            dataType: 'html',
            type: 'GET',
            contentType: 'text/html',
            cache: false,
            async: true,
            context: document.body,
            success: function (data) {
                try {
                    if (primeira_consulta) {
                        $('#div-clientes').empty();
                        $('#div-clientes').html(data);

                        primeira_consulta = false;
                        handler_consulta = setInterval(LoadClientes, intervalo_consulta);
                    }
                    else {

                        if (data.indexOf('Sessão finalizada, redicionando usuário para a página de login') > 0) {
                            $('#div-clientes').empty();
                            $('#div-clientes').html(data);
                            return;
                        }

                        if (data.indexOf('<p>0</p>') < 0) {
                            $('#div-clientes').empty();
                            $('#div-clientes').html(data);
                            handler_consulta = setInterval(LoadClientes, intervalo_consulta);
                        }
                        else {
                            handler_consulta = setInterval(LoadClientes, intervalo_consulta);
                        }
                    }

                } catch (e) {
                    alert('Entra no catch');
                    handler_consulta = setInterval(LoadClientes, intervalo_consulta);
                }
            },
            error: function (request, status, err) {
                alert('Entra no error');
                handler_consulta = setInterval(LoadClientes, intervalo_consulta);
            }
        });

    } catch (e) {
    }
    finally {

    }
}

$(document).ready(function () {

    $.support.cors = true;

    $.ajaxSetup({ cache: false });

    $.ajaxSettings.cache = false;

    CriarDivClientesOnline();

    LoadClientes();
});

