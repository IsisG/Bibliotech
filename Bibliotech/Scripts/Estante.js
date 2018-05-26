function inserirPrateleira() {
    var qtd = $("#qtdPrateleiras").val();

    var divHtml =
        '<input id="Prateleiras_' + qtd + '__Id" name="Prateleiras[' + qtd + '].Id" type="hidden" value>' +
        '<div class="form-row" id="divPrateleiras_' + qtd + '">' +
        '<div class="col-3">' +
        '<input class="form-control" id="Prateleiras_' + qtd + '__Descricao" name="Prateleiras[' + qtd + '].Descricao" type="text" value="">' +
        '</div>' +
        '<div class="col-9">' +
        '<button type="button" class="btn btn-danger btn-sm" onclick="removerPrateleira(' + qtd + ');">' +
        '<i class="fa fa-trash"></i>' +
        '</button>' +
        '</div>' +
        '</div>';

    $("#divPrateleiras_n").append(divHtml);
    $("#qtdPrateleiras").val(+qtd + +1);
    load();
}

function removerPrateleira(i) {
    document.getElementById("divPrateleiras_" + i).remove();
}

function validarSalvarPrateleiras() {
    var qtd = $("#qtdPrateleiras").val();

    for (var i = 0; i < qtdControleFinanceiroSocioAcoes; i++) {
        var descricao = document.getElementById('Prateleiras_' + i + '__Descricao');

        if (descricao != null && descricao.value == "") {
            bootbox.alert("O campo Descrição é obrigatório.");
            return false;
        }
    }
}