using System.ComponentModel.DataAnnotations;

namespace DeveloperRoadmapApi.Models
{
    public class Language
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        
    }
}