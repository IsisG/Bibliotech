using Bibliotech.Base;
using Bibliotech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bibliotech.Business
{
    public class BEditora : BaseBusiness<Editora>
    {
        private static BEditora instance;

        private BEditora()
        {

        }

        public static BEditora Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(BEditora))
                        if (instance == null)
                            instance = new BEditora();

                return instance;
            }
        }

        public override bool ValidarRemover(ref Editora editora)
        {
            return true;
        }

        public override bool ValidarSalvar(ref Editora editora)
        { 
            SetMensagemSalvar(editora);
            SetStatusSucesso();
            return true;
        }
    }
}