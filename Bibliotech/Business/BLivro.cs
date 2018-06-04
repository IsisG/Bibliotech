using Bibliotech.Base;
using Bibliotech.Models;
using Bibliotech.Repository;
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

        public StatusExemplar TratarStatusExemplar(Guid? exemplarId)
        {
            Exemplar exemplar = ExemplarRepository.Instance.GetById(exemplarId);

            if (exemplar.ExclusivoBiblioteca == true)
                return StatusExemplar.ExclusivoBiblioteca;

            else if (ReservaRepository.Instance.GetListReservasAtivasByExemplo(new Reserva { Exemplar = new Exemplar { Id = exemplarId } }).Count > 0)
                return StatusExemplar.Reservado;

            else
                return (StatusExemplar)exemplar.Status;
        }
    }
}