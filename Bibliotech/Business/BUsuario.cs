using Bibliotech.Base;
using Bibliotech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bibliotech.Business
{
    public class BUsuario : BaseBusiness<Usuario>
    {
        private static BUsuario instance;

        private BUsuario()
        {

        }

        public static BUsuario Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(BUsuario))
                        if (instance == null)
                            instance = new BUsuario();

                return instance;
            }
        }

        public override bool ValidarRemover(ref Usuario usuario)
        {
            return true;
        }

        public override bool ValidarSalvar(ref Usuario usuario)
        {
            SetMensagemSalvar(usuario);
            SetStatusSucesso();
            return true;
        }
    }
}