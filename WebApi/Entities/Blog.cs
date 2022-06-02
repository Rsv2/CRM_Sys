using Microsoft.EntityFrameworkCore;

namespace WebApi
{
    /// <summary>
    /// Блог
    /// </summary>
    public class Blog : IProject
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public Blog() { }
        public Blog(int id, string header, string image, string description)
        {
            Id = id;
            Header = header;
            Image = image;
            Description = description;
        }
    }
}
