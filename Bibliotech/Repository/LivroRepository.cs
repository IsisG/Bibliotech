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
    public class LivroRepository : BaseRepository<Livro>
    {
        private static LivroRepository instance;

        private LivroRepository() { }

        public static LivroRepository Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(LivroRepository))
                        if (instance == null)
                            instance = new LivroRepository();

                return instance;
            }
        }

        public List<Livro> GetListLivroByExample(Livro entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Livro));

                if (entity.Id != null)
                    criteria.Add(Restrictions.Eq("Id", entity.Id));

                if (entity.Titulo != null)
                    criteria.Add(Restrictions.InsensitiveLike("Titulo", "%" + entity.Titulo + "%"));

                if (entity.Isbn != null)
                    criteria.Add(Restrictions.InsensitiveLike("Isbn", "%" + entity.Isbn + "%"));

                //Mudar consulta tratando períodos
                if (entity.AnoPublicao != null)
                    criteria.Add(Restrictions.Eq("AnoPublicacao", entity.AnoPublicao));

                if (entity.Edicao != null)
                    criteria.Add(Restrictions.Eq("Edicao", entity.Edicao));

                if (entity.Assunto != null)
                    criteria.Add(Restrictions.InsensitiveLike("Assunto", "%" + entity.Assunto + "%"));

                return criteria.List<Livro>().ToList();
            }
        }

        public override void LazyProperties(Livro entity)
        {
            if (entity.Exemplares != null)
            {
                foreach (var item in entity.Exemplares)
                    item.ToString();
            }
        }

        public override void BeforeCommitSaveOrUpdate(ISession session, ref Livro entity)
        {
            if (entity.Exemplares != null)
            {
                for (int i = 0; i < entity.Exemplares.Count; i++)
                {
                    if (Functions.IsNullExcludingProperties(entity.Exemplares[i], "Id", "ExclusivoBiblioteca"))
                    {
                        if (entity.Exemplares[i].Id != null)
                        {
                            entity.Exemplares[i] = (Exemplar)session.Get("Exemplar", entity.Exemplares[i].Id);
                            session.Delete(entity.Exemplares[i]);
                        }
                        entity.Exemplares.Remove(entity.Exemplares[i]);
                        i--;
                        continue;
                    }
                    else
                    {
                        entity.Exemplares[i].Livro = entity;
                    }

                    if (entity.Exemplares[i].Id == null)
                        entity.Exemplares[i].Status = StatusExemplar.Disponivel;
                    else
                    {
                        Exemplar exemplarBanco = (Exemplar)session.Get("Exemplar", entity.Exemplares[i].Id);
                        entity.Exemplares[i].Status = exemplarBanco.Status;
                    }
                }
            }
        }
    }
}