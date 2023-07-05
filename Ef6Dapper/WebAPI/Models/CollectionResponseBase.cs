using System;

namespace OnePage.WebAPI.Models
{
    public abstract class CollectionResponseBase
    {
        private int totalCount;
        private int totalPages;

        public int TotalCount
        {
            get
            {
                return totalCount;
            }
        }

        public int TotalPages
        {
            get
            {
                return totalPages;
            }
        }

        public CollectionResponseBase(int totalCount, int pageSize)
        {
            this.totalCount = totalCount;
            totalPages = pageSize == 0 ? 0 : (int)Math.Ceiling((double)totalCount / pageSize);
        }
    }
}