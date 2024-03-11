using The_Gram.Data.Models;

namespace The_Gram.Models
{
    public class SearchViewModel<T>
    {
        public SearchViewModel()
        {
            this.SearchResults = new List<T>();
        }
        public string Query { get; set; }
        public List<T> SearchResults { get; set; }
    }
}