using Bibliotech.Base;
using Bibliotech.Models;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bibliotech.Repository
{
    public class ExemplarRepository : BaseRepository<Exemplar>
    {
        private static ExemplarRepository instance;

        private ExemplarRepository() { }

        public static ExemplarRepository Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(ExemplarRepository))
                        if (instance == null)
                            instance = new ExemplarRepository();

                return instance;
            }
        }

        public List<Exemplar> GetListExemplarByExample(Exemplar entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Exemplar));

                if (entity.Id != null)
                    criteria.Add(Restrictions.Eq("Id", entity.Id));

                if (entity.Codigo != null)
                    criteria.Add(Restrictions.InsensitiveLike("Titulo", "%" + entity.Codigo + "%"));

                if (entity.Status != null)
                    criteria.Add(Restrictions.Eq("Status", entity.Status));

                return criteria.List<Exemplar>().ToList();
            }
        }

        public override void LazyProperties(Exemplar entity)
        {
            if (entity.Livro != null)
                entity.Livro.ToString();
        }
    }
}