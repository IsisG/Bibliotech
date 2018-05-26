using Bibliotech.Base;
using Bibliotech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bibliotech.Business
{
    public class BEstante : BaseBusiness<Estante>

    {
        private static BEstante instance;

        private BEstante()
        {

        }
        public static BEstante Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(BEstante))
                        if (instance == null)
                            instance = new BEstante();

                return instance;
            }
        }

        public override bool ValidarRemover(ref Estante estante)
        {
            return true;
        }

        public override bool ValidarSalvar(ref Estante estante)
        {
            SetMensagemSalvar(estante);
            SetStatusSucesso();
            return true;
        }
    }
}