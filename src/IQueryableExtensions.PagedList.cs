﻿using System.Data;

namespace System.Linq
{
    public static class IQueryablePagedListExtensions
    {
        #region FirstOrDefault
        public static object FirstOrDefault(this IQueryable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            foreach (object obj in source)
                return obj;

            return null;
        }
        #endregion

        #region PagedList

        public static IQueryable<T> GetPagedQuery<T>(this IQueryable<T> query, int? page, int? pagesize, out int total)
        // where T : new()
        {
            total = query.Count();

            if (page.HasValue == false)
            {
                page = 1;
            }

            if (pagesize.HasValue == false)
            {
                pagesize = 10;
            }

            int skip = (page.Value - 1) * pagesize.Value;

            return query.Skip(skip).Take(pagesize.Value);
        }

        public static PagedList<T> GetPagedList<T>(this IQueryable<T> query, int? page, int? pagesize)
        // where T : new()
        {
            int total = 0;

            var data = GetPagedQuery(query, page, pagesize, out total);

            return new PagedList<T>(data.ToList(), page.Value, pagesize.Value, total);
        }

        public static IQueryable<T> GetPagedQuery<T>(this IQueryable<T> query, int start, int limit, string sort, string dir, out int page, out int total)
        // where T : new()
        {
            Check.Assert(limit != 0);

            page = 1;

            if (start != 0)
            {
                page = start / limit;

                if (start % limit != 0)
                {
                    page++;
                }
            }

            total = query.Count();

            IQueryable<T> orderedQuery = null;

            if (String.IsNullOrEmpty(sort) == false)
            {
                if (dir.ToLower() == "desc")
                {
                    orderedQuery = query.ApplyOrderByDescendingClause<T>(sort);
                }
                else
                {
                    orderedQuery = query.ApplyOrderByClause<T>(sort);
                }
            }
            else
            {
                orderedQuery = query.AsQueryable();
            }

            int skip = start; // -1;

            if (skip < 0)
                skip = 0;

            return orderedQuery.Skip(skip).Take(limit);

        }

        public static PagedList<T> GetPagedList<T>(this IQueryable<T> query, int start, int limit, string sort, string dir)
        // where T : new()
        {
            int page = 1;
            int total = 0;

            var data = GetPagedQuery(query, start, limit, sort, dir, out page, out total);

            return new PagedList<T>(data.ToList(), page, limit, total);
        }
        #endregion
    }
}
