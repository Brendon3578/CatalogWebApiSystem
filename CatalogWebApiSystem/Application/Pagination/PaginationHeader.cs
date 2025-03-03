﻿using System.Text.Json;

namespace CatalogWebApiSystem.Application.Pagination
{
    public class PaginationHeader
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }

        public PaginationHeader(int totalCount, int pageSize, int currentPage, int totalPages, bool hasNext, bool hasPrevious)
        {
            TotalCount = totalCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPages = totalPages;
            HasNext = hasNext;
            HasPrevious = hasPrevious;
        }


        public static void SetPaginationHeaderOnResponse<T>(PagedList<T> pagedList, HttpResponse response) where T : class
        {
            var metadata = new PaginationHeader
            (
                pagedList.TotalCount,
                pagedList.PageSize,
                pagedList.CurrentPage,
                pagedList.TotalPages,
                pagedList.HasNext,
                pagedList.HasPrevious
            );

            var jsonMetadata = JsonSerializer.Serialize(metadata, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            response.Headers.Append("X-Pagination", jsonMetadata);
        }
    }
}
