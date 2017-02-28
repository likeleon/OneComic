using OneComic.Data.DTO;
using System.Collections.Generic;

namespace OneComic.Web.Models
{
    public class ComicsViewModel
    {
        public IEnumerable<Comic> Comics { get; set; }
    }
}