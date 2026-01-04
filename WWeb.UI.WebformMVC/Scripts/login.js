$('#btn_confirmar').click(function () {
    validar();
});

function validar() {

    var operador = $('#txtUsuario').val();
    var senha = $('#txtSenha').val();

    var jsonData = '{ "usuario":"' + operador + '","senha":"' + senha + '" }';

    var protocolo = window.location.protocol;
    var root_site = window.location.host;
    var uri = protocolo + '//' + root_site;

    $.ajax({
        url: uri + "/Inicio/Validar",
        data: jsonData,
        dataType: 'json',
        type: 'POST',
        contentType: 'application/json',
        success: function (data) {
            try {

                var autenticado = data.responseAutenticado;
                var erro = data.responseErro;
                var url = data.responseUrl;
                var mensagem = data.responseMsg;

                if (autenticado == '1') {
                    window.location.replace(url);
                }
                else {
                    Mensagem(mensagem);
                }

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
