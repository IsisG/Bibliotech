using Bibliotech.Base;
using Bibliotech.Models;
using Bibliotech.Repository;
using Bibliotech.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;


namespace Bibliotech.Business
{
    public class BEmprestimo : BaseBusiness<Emprestimo>
    {
        private static BEmprestimo instance;

        private BEmprestimo()
        {

        }

        public static BEmprestimo Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(BEmprestimo))
                        if (instance == null)
                            instance = new BEmprestimo();

                return instance;
            }
        }

        public override bool ValidarRemover(ref Emprestimo emprestimo)
        {
            return true;
        }

        public override bool ValidarSalvar(ref Emprestimo emprestimo)
        {
            return true;
        }

        public bool ValidarEmprestimo(ref Emprestimo emprestimo, ref Exemplar exemplar)
        {
            List<Emprestimo> emprestimosAtivos = EmprestimoRepository.Instance.GetEmprestimosNaoFinalizadosByUsuario(emprestimo.Usuario);

            //MUDAR DE 0 PARA LIMITE DEFINIDO EM PARÂMETROS
            if (emprestimosAtivos != null && emprestimosAtivos.Count > 0)
            {
                status = Constantes.STATUS_ERRO;
                mensagemSalvar = Mensagens.LIMITE_EMPRESTIMOS_ATINGIDO;
            }

            emprestimo.DataInicio = DateTime.Now.Date;
            //Mudar para tempo definido em parametros
            emprestimo.DataFimPrevisao = DateTime.Now.AddDays(7).Date;
            emprestimo.QuantidadeRenovacoes = 0;

            exemplar.Status = StatusExemplar.Emprestado;

            SetMensagemSalvar(emprestimo);
            SetStatusSucesso();
            return true;
        }

        public bool ValidarDevolucao(ref Emprestimo emprestimo, ref Exemplar exemplar)
        {
            emprestimo.DataFim = DateTime.Now.Date;

            exemplar.Status = StatusExemplar.Disponivel;

            SetMensagemSalvar(emprestimo);
            SetStatusSucesso();
            return true;
        }

        public bool ValidarRenovacao(ref Emprestimo emprestimo, ref Exemplar exemplar)
        {
            if (emprestimo.DataFimPrevisao.Value.Date < DateTime.Now.Date)
            {
                status = Constantes.STATUS_ERRO;
                mensagemSalvar = Mensagens.PRAZO_RENOVACAO_EXPIROU;
            }

            //MUDAR DE 0 PARA LIMITE DEFINIDO EM PARÂMETROS
            if (emprestimo.QuantidadeRenovacoes > 0)
            {
                status = Constantes.STATUS_ERRO;
                mensagemSalvar = Mensagens.LIMITE_RENOVACOES_ATINGIDO;
            }

            //Mudar para tempo definido em parametros
            emprestimo.DataFimPrevisao = emprestimo.DataFimPrevisao.Value.AddDays(7).Date;
            emprestimo.QuantidadeRenovacoes += 1;

            SetMensagemSalvar(emprestimo);
            SetStatusSucesso();
            return true;
        }

        public void EnvioEmailEmprestimo(Emprestimo emprestimo)
        {
            Parametro parametro = ParametroRepository.Instance.GetParametro();
            Usuario usuario = UsuarioRepository.Instance.GetById(emprestimo.Usuario.Id);
            Livro livro = LivroRepository.Instance.GetById(emprestimo.Exemplar.Livro.Id);

            string conteudo = "";

            conteudo += usuario.Nome + ", seu empréstimo foi realizado com sucesso. <br />";
            conteudo += "Seu empréstimo foi realizado com sucesso. <br />";
            conteudo += "Livro: " + livro.Titulo + " < br /> ";
            conteudo += "Limite de entrega: " + emprestimo.DataFimPrevisao + " < br /> ";

            //Envia o e-mail
            MailMessage mail = new MailMessage(parametro.EmailRemetente, usuario.Email);
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;

            mail.Subject = "Comprovante de Empréstimo";
            mail.Body = conteudo;
            mail.IsBodyHtml = true;

            client.Credentials = new System.Net.NetworkCredential(parametro.EmailRemetente, parametro.Senha);
            client.Send(mail);


        }


    }
}