using Bibliotech.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bibliotech.Base
{
    public abstract class BaseBusiness<T> where T : class
    {
        protected int status;
        protected string mensagemSalvar;
        protected string mensagemRemover;

        public abstract bool ValidarSalvar(ref T model);
        public abstract bool ValidarRemover(ref T model);

        public int Status()
        {
            return this.status;
        }

        public string MensagemSalvar()
        {
            return this.mensagemSalvar;
        }

        public string MensagemRemover()
        {
            return this.mensagemRemover;
        }

        protected virtual void SetMensagemSalvar(T entity)
        {
            if (entity.GetType().GetProperty("Id").GetValue(entity) == null)
                mensagemSalvar = Mensagens.ADICIONADO_SUCESSO;
            else
                mensagemSalvar = Mensagens.ALTERADO_SUCESSO;
        }

        protected void SetStatusSucesso()
        {
            status = Constantes.STATUS_SUCESSO;
        }

        protected void SetStatusErro()
        {
            status = Constantes.STATUS_ERRO;
        }
    }
}