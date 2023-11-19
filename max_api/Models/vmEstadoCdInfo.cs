using System.ComponentModel.DataAnnotations;

namespace max_api.Models
{
    public class vmEstadoCdInfo
    {
        [Key]
        public int id { get; set; }
        public string estado_cd { get; set;}
    }
}
