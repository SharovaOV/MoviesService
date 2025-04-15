using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Models
{
    public class Link
    {
        public required string Href { get; init; }
        public required string Rel { get; init; }
        public required string Type { get; init; }
    }
}
