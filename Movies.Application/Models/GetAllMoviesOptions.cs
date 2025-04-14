using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Movies.Application.Enums;

namespace Movies.Application.Models
{
    public class GetAllMoviesOptions
    {
        public string? Title { get; set; }
        public int? YearOfRelease { get; set; }
        public Guid? UserId { get; set; }
        public string? SortField { get; set; }
        public SortOrder? SortOrder { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 2;
    }
}
