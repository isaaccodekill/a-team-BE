using System;

namespace Searchify.Domain.Models
{
    public class SearchHit
    {
        public SearchHit(Document doc, string previewText)
        {
            link = doc.url;
            text = previewText;
        }
        public string link { get; }
        public string text { get; }
    }
}
