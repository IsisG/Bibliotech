using Bibliotech.Base;
using Bibliotech.Models;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bibliotech.Repository
{
    public class PagamentoMultaRepository : BaseRepository<PagamentoMulta>
    {

        private static PagamentoMultaRepository instance;

        private PagamentoMultaRepository() { }

        public static PagamentoMultaRepository Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(PagamentoMultaRepository))
                        if (instance == null)
                            instance = new PagamentoMultaRepository();

                return instance;
            }
        }

        public List<PagamentoMulta> GetListPagamentoMultaByExample(PagamentoMulta entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(PagamentoMulta));

                if (entity.Id != null)
                    criteria.Add(Restrictions.Eq("Id", entity.Id));

                if (entity.Emprestimo != null && entity.Emprestimo.Id != null)
                    criteria.Add(Restrictions.Eq("Emprestimo.Id", entity.Emprestimo.Id));

                return criteria.List<PagamentoMulta>().ToList();
            }
        }

        public override void LazyProperties(PagamentoMulta entity)
        {

        }
    }
}

