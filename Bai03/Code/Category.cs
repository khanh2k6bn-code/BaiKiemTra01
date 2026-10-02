namespace TechMartManager
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public Category(int id, string name)
        {
            CategoryId = id;
            CategoryName = name;
        }
    }
}
