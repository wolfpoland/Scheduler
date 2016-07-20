using Microsoft.AspNetCore.Http;
using Scheduler.API.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduler.API.Core
{
    public static class Extensions
    {
        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var pagintationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);

            response.Headers.Add("Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(pagintationHeader));
            response.Headers.Add("access-control-expose-headers", "Pagination");
        }
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("access-control-expose-headers", "Application-Errror");
        }
    }
}
