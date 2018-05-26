using Bibliotech.Base;
using Bibliotech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bibliotech.Business
{
    public class BLivro : BaseBusiness<Livro>
    {
        private static BLivro instance;

        private BLivro()
        {

        }

        public static BLivro Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(BLivro))
                        if (instance == null)
                            instance = new BLivro();

                return instance;
            }
        }

        public override bool ValidarRemover(ref Livro livro)
        {
            return true;
        }

        public override bool ValidarSalvar(ref Livro livro)
        {
            SetMensagemSalvar(livro);
            SetStatusSucesso();
            return true;
        }
    }
}