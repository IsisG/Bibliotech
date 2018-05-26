using Bibliotech.Base;
using Bibliotech.Models;
using Bibliotech.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Bibliotech.Business
{
    public class BParametro : BaseBusiness<Parametro>
    {

        private static BParametro instance;

        private BParametro()
        {

        }

        public static BParametro Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(BParametro))
                        if (instance == null)
                            instance = new BParametro();

                return instance;
            }
        }

        public override bool ValidarRemover(ref Parametro parametro)
        {
            return true;
        }

        public override bool ValidarSalvar(ref Parametro parametro)
        {
            SetMensagemSalvar(parametro);
            SetStatusSucesso();
            return true;
        }
    }
}