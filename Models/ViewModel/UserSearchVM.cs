using System.Collections.Generic;
using PagedList;
using MyStore.Models.EDM;

namespace MyStore.Models.ViewModel
{
    public class UserSearchVM
    {
        public string SearchTerm { get; set; }
        public string SortOrder { get; set; }
        public IPagedList<User> Users { get; set; }
    }
}