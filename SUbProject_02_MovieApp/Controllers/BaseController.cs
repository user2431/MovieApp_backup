using Microsoft.AspNetCore.Mvc;

namespace SUbProject_02_MovieApp.Controllers
{
    [ApiController]
    public class BaseController : Controller
    {
        private readonly LinkGenerator _linkGenerator;

        public BaseController(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
            
        }
        protected string? GetUrl(string linkName, object args)
        {
            return _linkGenerator.GetUriByName(
                HttpContext,
                linkName, args);
        }
        protected string? GetLink(string linkName, int page, int pageSize, object? routeValues = null)
        {
            var values = new Dictionary<string, object>
    {
        { "page", page },
        { "pageSize", pageSize }
    };

            
            if (routeValues != null)
            {
                var properties = routeValues.GetType().GetProperties();
                foreach (var prop in properties)
                {
                    values[prop.Name] = prop.GetValue(routeValues);
                }
            }
            return GetUrl(linkName,values);
        }
        protected object CreatePaging<T>(string linkName, int page, int pageSize, int total, IEnumerable<T?> items,object routevalues)
        {
            const int MaxPageSize = 25;
            pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;

            var numberOfPages =
                (int)Math.Ceiling(total / (double)pageSize);

            var curPage = GetLink(linkName, page, pageSize,routevalues);

            var nextPage =
                page < numberOfPages - 1
                ? GetLink(linkName, page + 1, pageSize,routevalues)
                : null;

            var prevPage =
                page > 0
                ? GetLink(linkName, page - 1, pageSize, routevalues)
                : null;

            var result = new
            {
                CurPage = curPage,
                NextPage = nextPage,
                PrevPage = prevPage,
                NumberOfItems = total,
                NumberPages = numberOfPages,
                Items = items
            };
            return result;
        }
    }
}
