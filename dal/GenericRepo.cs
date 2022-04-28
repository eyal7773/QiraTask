using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class GenericRepository<T> : IRepo<T> where T : class
                                                                 
    {
        private const int MAX_RECORDS_IN_ONE_QUERY = 500;

        protected readonly DbContext _context;
        private DbSet<T> _set;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _set = _context.Set<T>();
        }
        #region private functions
        private IQueryable<T> SkipTakeAndOrder(int pageLength,
                                               int startRecord,
                                               IQueryable<T> set = null,
                                               string sort = "")
        {
            IQueryable<T> query = (set is null) ? _set : set;
            pageLength = (pageLength > MAX_RECORDS_IN_ONE_QUERY) ? MAX_RECORDS_IN_ONE_QUERY : pageLength;


            if (sort == "")
            {
                query = query.Skip(startRecord).Take(pageLength);
            }
            else
            {
                query = query.Skip(startRecord).Take(pageLength).AsQueryable();
            }

            return query;
        }
        private DataWrapper<T> GetAllRecords(int pageLength = 10,
                                             int startRecord = 0,
                                             string sort = "")
        {

            var query = SkipTakeAndOrder(pageLength, startRecord, null, sort);

            var recordsTotal = _set.Count();
            var recordsFiltered = recordsTotal;


            var data = query.ToList();
            var result = DataWrapper<T>.Create(recordsTotal, recordsFiltered, data);
            return result;
        }
        #endregion

        #region public functions

        public void Add(T entity)
        {
            _set.Add(entity);
            _context.SaveChanges();
        }
        public void Update(T entity, int id)
        {
            _set.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public  DataWrapper<T> GetAll(int pageLength = 10,
                                      int startRecord = 0,
                                      string sort = "",
                                      string filterColumn = "", 
                                      string filterTerm = "")
        {
            if (string.IsNullOrEmpty(filterColumn) || string.IsNullOrEmpty(filterTerm))
            {
                return GetAllRecords(pageLength, startRecord, sort);
            }

            var filteredSet = _set.Where($"{filterColumn}.Contains(@0)", filterTerm);
            var query = SkipTakeAndOrder(pageLength, startRecord, filteredSet, sort);

            var recordsTotal = _set.Count();
            var recordsFiltered = filteredSet.Count();

            var data = query.ToList();
            var result = DataWrapper<T>.Create(recordsTotal, recordsFiltered, data);
            return result;
        }

        

        public T GetById(int id)
        {
            return  _set.Find(id);
        }
        #endregion


        


    }
}
