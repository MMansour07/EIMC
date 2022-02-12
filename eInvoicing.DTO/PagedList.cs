using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace eInvoicing.DTO
{
    public class PagedList<T> : List<T>
    {
        [JsonConstructor]
        public PagedList() { }
        public int CurrentPage { get; set; } 
        public int TotalPages { get; set; }   
        public int PageSize { get; set; }    
        public int TotalCount { get; set; }   
        public int TotalFiltered { get; set; }

        public PagedList(List<T> items, int Count,int pagenNumber, int pageSize, int totalcount)
        {
            this.CurrentPage = pagenNumber;
            this.PageSize =  pageSize;
            //this.TotalFiltered= Count;
            this.TotalCount = totalcount;
            this.TotalPages = (int)Math.Ceiling(Count /  (double)pageSize);
            this.AddRange(items);
        } 
        public static PagedList<T> Create(IEnumerable<T> Source , int pageNumber , int pageSize, int totalcount) {
            var Count =  Source.Count();
            var items =  Source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, Count, pageNumber, pageSize, totalcount);
        }

    }
}