using Bibliotech.Base;
using Bibliotech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bibliotech.Business
{
    public class BAutor : BaseBusiness<Autor>
    {
        private static BAutor instance;

        private BAutor()
        {

        }

        public static BAutor Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(BAutor))
                        if (instance == null)
                            instance = new BAutor();

                return instance;
            }
        }

        public override bool ValidarRemover(ref Autor autor)
        {
            return true;
        }

        public override bool ValidarSalvar(ref Autor autor)
        {
            SetMensagemSalvar(autor);
            SetStatusSucesso();
            return true;
        }
    }
}