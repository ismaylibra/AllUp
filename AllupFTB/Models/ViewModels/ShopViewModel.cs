using AllupFTB.DAL.Entities;

namespace AllupFTB.Models.ViewModels
{
    public class ShopViewModel
    {
        public Category SelectedCategory { get; set; }
        public List<Category> Categories { get; set; }
    }
}
