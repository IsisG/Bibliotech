function voltar() {
    window.history.back();
}

function salvar(controllerName) {
    $.ajax({
        type: "POST",
        url: "/" + controllerName + "/Salvar",
        data: $("#myForm").serialize(),
        success: function (data) {
            sucesso(data);
        },
        error: function (msg) {
            var title = (/<title>(.*?)<\/title>/m).exec(msg.responseText)[1];
            bootbox.alert(title);
        }
    });
}

function submitAction(controllerName, actionName) {
    $.ajax({
        type: "POST",
        url: "/" + controllerName + "/" + actionName,
        data: $("#myForm").serialize(),
        success: function (data) {
            sucesso(data);
        },
        error: function (msg) {
            var title = (/<title>(.*?)<\/title>/m).exec(msg.responseText)[1];
            bootbox.alert(title);
        }
    });
}

$(document).ready(function () {
    posLoad();
});

function load() {
    posLoad();
}

function adicionar(controllerName) {
    window.location.href = "/" + controllerName + "/Adicionar";
}

function alterar(controllerName, field, id) {
    window.location.href = "/" + controllerName + "/Alterar?" + field + "=" + id;
}

function remover(controllerName, id) {
    bootbox.confirm("Deseja remover o registro?", function (confirmed) {
        if (confirmed) {
            $.ajax({
                url: "/" + controllerName + "/Remover",
                data: { id: id },
                type: 'POST',
                success: function (data) {
                    bootbox.alert(data.Message, function () {
                        if (data.Status == 0) {
                            window.location.href = "/" + controllerName + "/Listar";
                        }
                    });
                },
                error: function (msg) {
                    var title = (/<title>(.*?)<\/title>/m).exec(msg.responseText)[1];
                    bootbox.alert(title);
                }
            });
        }
    });
}

function validarData(dataField) {
    var dia = dataField.value.split("/")[0];
    var mes = dataField.value.split("/")[1];
    var ano = dataField.value.split("/")[2];

    if (ano < 1753)
        return false;

    var MyData = new Date(ano, mes - 1, dia);

    if ((MyData.getMonth() + 1 != mes) ||
       (MyData.getDate() != dia) ||
       (MyData.getFullYear() != ano))
        return false;

    return true;
}

function validarDataTime(dataField) {
    var dia = dataField.value.split("/")[0];
    var mes = dataField.value.split("/")[1];
    var ano = dataField.value.split("/")[2].split(" ")[0];
    var hora = dataField.value.split("/")[2].split(" ")[1].split(":")[0];
    var minuto = dataField.value.split("/")[2].split(" ")[1].split(":")[1];

    if (ano < 1753)
        return false;

    var MyData = new Date(ano, mes - 1, dia, hora, minuto, 0, 0);

    if ((MyData.getMonth() + 1 != mes) ||
       (MyData.getDate() != dia) ||
       (MyData.getFullYear() != ano) ||
        (MyData.getHours() != hora) ||
        (MyData.getMinutes() != minuto))
        return false;

    return true;
}

function validarTime(timeField) {
    var hora = timeField.value.split(":")[0];
    var minuto = timeField.value.split(":")[1];

    var MyData = new Date(1900, 12 - 1, 1, hora, minuto, 0, 0);

    if ((MyData.getHours() != hora) ||
        (MyData.getMinutes() != minuto))
        return false;

    return true;
}

function inserirMascaraTelefone(arrayCamposTelefone) {
    for (var i = 0; i < arrayCamposTelefone.length; i++) {
        var input = "#" + arrayCamposTelefone[i];

        var inputLength = $(input).val().replace(/\D/g, '').length;

        if (inputLength > 10) {
            $(input).mask("(99) 9999?9-9999")
        }
        else {
            $(input).mask("(99) 9999-9999?9")
        }

        $(input).focusout(function (event) {
            var target, phone, element;
            target = (event.currentTarget) ? event.currentTarget : event.srcElement;
            phone = target.value.replace(/\D/g, '');
            element = $(target);
            element.unmask();
            if (phone.length > 10) {
                element.mask("(99) 99999-999?9");
            } else {
                element.mask("(99) 9999-9999?9");
            }
        });

    }
}

function inserirMascaraData(arrayCamposData) {
    for (var i = 0; i < arrayCamposData.length; i++) {
        var input = "#" + arrayCamposData[i];
        $(input).mask("99/99/9999");

        $(input).blur(function () {
            if (!validarData(this)) {
                $("#" + this.id).unmask("99/99/9999");
                this.value = null;
            }
        });

        $(input).focus(function () {
            $("#" + this.id).mask("99/99/9999");
        });
    }
}

function inserirMascaraDataHora(arrayCamposDataHora) {
    for (var i = 0; i < arrayCamposDataHora.length; i++) {
        var input = "#" + arrayCamposDataHora[i];
        $(input).mask("99/99/9999 99:99");

        $(input).blur(function () {
            if (!validarDataTime(this)) {
                $("#" + this.id).unmask("99/99/9999 99:99");
                this.value = null;
            }
        });

        $(input).focus(function () {
            $("#" + this.id).mask("99/99/9999 99:99");
        });
    }
}

function inserirMascaraDinheiroReal(arrayCamposDinheiro) {
    for (var i = 0; i < arrayCamposDinheiro.length; i++) {
        var input = "#" + arrayCamposDinheiro[i];
        $(input).priceFormat({
            prefix: '',
            clearPrefix: true,
            centsSeparator: ',',
            thousandsSeparator: '.'
        });
    }
}

function insertMaskOnlyNumber(input) {
    $("#" + input).keypress(function (e) {
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });
}

function inserirMascaraNumerica(arrayCampos) {
    for (var i = 0; i < arrayCampos.length; i++) {
        document.getElementById(arrayCampos[i]).setAttribute('onpaste', 'return false');
        document.getElementById(arrayCampos[i]).setAttribute('ondrop', 'return false');
        insertMaskOnlyNumber(arrayCampos[i]);
    }
}