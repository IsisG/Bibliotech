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
    public class EmprestimoRepository : BaseRepository<Emprestimo>
    {
        private static EmprestimoRepository instance;

        private EmprestimoRepository() { }

        public static EmprestimoRepository Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(EmprestimoRepository))
                        if (instance == null)
                            instance = new EmprestimoRepository();

                return instance;
            }
        }
        public List<Emprestimo> GetEmprestimosNaoFinalizadosByUsuario(Usuario usuario)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Emprestimo))
                    .Add(Restrictions.IsNull("DataFim"));

                if (usuario.Id != null)
                    criteria.Add(Restrictions.Eq("Usuario.Id", usuario.Id));

                return criteria.List<Emprestimo>().ToList();
            }
        }

        public override void LazyProperties(Emprestimo entity)
        {

        }

        //Salva novo empréstimo e atualiza exemplar na mesma transação
        public virtual void SaveOrUpdateEmprestimoUpdateExemplar(Emprestimo emprestimo, Exemplar exemplar)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Clear();

                BeforeCommitSaveOrUpdate(session, ref emprestimo);
                session.SaveOrUpdate(emprestimo);

                ExemplarRepository.Instance.BeforeCommitSaveOrUpdate(session, ref exemplar);
                session.SaveOrUpdate(exemplar);

                transaction.Commit();
            }
        }
    }
}