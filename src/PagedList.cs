using System.Collections.Generic;
using System.Linq;

namespace System.Data
{
    /// <summary>
    /// 如果你作为一个分页需要的数据集合的话，你必须支持此接口
    /// </summary>
    public interface IPagedList
    {
        int Count { get; }
        int TotalItemCount { get; }
        int CurrentPageIndex { get; set; }
        int PageSize { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
        int PageCount { get; }
        int StartIndex { get; }
        int EndIndex { get; }
    }

    /// <summary>
    /// 通用的分页类泛型实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : List<T>, IPagedList
    {
        public PagedList(IList<T> pagedSource, int page, int pageSize, int total)
        {
            this.TotalItemCount = total;
            this.CurrentPageIndex = page;
            this.PageSize = pageSize;
            if (total > 0 && null != pagedSource)
                this.AddRange(pagedSource);
        }

        public PagedList(IQueryable<T> source, int page, int pageSize)
        {
            this.TotalItemCount = source.Count();
            this.CurrentPageIndex = page;
            this.PageSize = pageSize;

            if (this.TotalItemCount != 0)
            {
                if (this.PageCount < page || page <= 0)
                    throw new ArgumentOutOfRangeException("page");
            }
            else
            {
                this.CurrentPageIndex = 0;
            }

            if (this.TotalItemCount != 0)
                this.AddRange(source.Skip((page - 1) * pageSize).Take(pageSize).ToList());
        }

        public PagedList(List<T> source, int page, int pageSize)
        {
            this.TotalItemCount = source.Count();
            this.CurrentPageIndex = page;
            this.PageSize = pageSize;

            if (this.TotalItemCount != 0)
            {
                if (this.PageCount < page || page <= 0)
                    throw new ArgumentOutOfRangeException("page");
            }
            else
            {
                this.CurrentPageIndex = 0;
            }

            if (this.TotalItemCount != 0)
                this.AddRange(source.Skip((page - 1) * pageSize).Take(pageSize).ToList());
        }

        public int TotalItemCount { get; private set; }
        public int PageSize { get; private set; }

        public bool HasPreviousPage
        {
            get
            {
                return (CurrentPageIndex - 1) > 0;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (CurrentPageIndex * PageSize) < TotalItemCount;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int CurrentPageIndex { get; set; }

        public int StartIndex
        {
            get
            {
                if (TotalItemCount == 0)
                    return 0;

                return (this.CurrentPageIndex - 1) * this.PageSize + 1;
            }
        }

        public int EndIndex
        {
            get
            {
                if (TotalItemCount == 0)
                    return 0;

                return StartIndex + this.Count() - 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int PageCount
        {
            get
            {
                if (PageSize == 0)
                    throw new ArgumentOutOfRangeException("page");

                if (TotalItemCount == 0)
                {
                    return 0;
                }

                int remainder = TotalItemCount % PageSize;

                if (remainder == 0)
                    return TotalItemCount / PageSize;
                else
                    return (TotalItemCount / PageSize) + 1;
            }
        }
    }

}
