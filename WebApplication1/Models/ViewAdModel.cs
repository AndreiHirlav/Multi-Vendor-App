namespace WebApplication1.Models
{
    public class ViewAdModel
    {
        public int ProId { get; set; }
        public string ProName { get; set; }
        public string ProImage { get; set; }
        public Nullable<int> ProPrice { get; set; }
        public string ProDes { get; set; }
        public Nullable<int> ProFkCat { get; set; }
        public Nullable<int> ProFkUser { get; set; }

        public string CatName { get; set; }
        public Nullable<int> CatFkAd { get; set; }

        public string UName { get; set; }
        public string UEmail { get; set; }
        public string UImage { get; set; }
        public string UContact { get; set; }
    }
}
