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
    public class AutorRepository : BaseRepository<Autor>
    {
        private static AutorRepository instance;

        private AutorRepository() { }

        public static AutorRepository Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(AutorRepository))
                        if (instance == null)
                            instance = new AutorRepository();

                return instance;
            }
        }


        public List<Autor> GetListAutorByExample(Autor entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Autor));

                if (entity.Id != null)
                    criteria.Add(Restrictions.Eq("Id", entity.Id));

                if (entity.DataNascimento != null)
                    criteria.Add(Restrictions.Eq("DataNascimento", entity.DataNascimento));

                if (entity.Nome != null)
                    criteria.Add(Restrictions.InsensitiveLike("Nome", "%" + entity.Nome + "%"));

                return criteria.List<Autor>().ToList();
            }
        }

        public override void LazyProperties(Autor entity)
        {

        }
    }
}