using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bibliotech.Util
{
    public class Mensagens
    {
        public static string ADICIONADO_SUCESSO = "Adicionado com sucesso.";
        public static string ALTERADO_SUCESSO = "Alterado com sucesso.";
        public static string REMOVIDO_SUCESSO = "Removido com sucesso.";
        public static string EMPRESTIMO_SUCESSO = "Empréstimo realizado com sucesso.";
        public static string RESERVA_SUCESSO = "Reserva realizada com sucesso.";
        public static string RENOVACAO_SUCESSO = "Renovação realizada com sucesso.";
        public static string DEVOLUCAO_SUCESSO = "Devolução realizada com sucesso.";

        public static string ERRO_GENERICO = "Ocorreu um erro.";
        public static string ERRO_CONCORRENCIA = "Este registro foi modificado recentemente. Atualize a página e tente novamente.";

        public static string USUARIO_SENHA_INVALIDOS = "Usuário ou senha inválidos.";
        public static string CODIGO_ESCANEADO_INVALIDO = "O Código escaneado é inválido.";

        public static string LIMITE_EMPRESTIMOS_ATINGIDO = "Operação não realizada. O limite de Empréstimos ativos já foi atingido.";
        public static string LIMITE_RENOVACOES_ATINGIDO = "Operação não realizada. O limite de Renovações já foi atingido.";
        public static string PRAZO_RENOVACAO_EXPIROU = "Operação não realizada. O prazo para esta Renovação expirou.";
    }
}