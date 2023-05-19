using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebFrontToBack.Areas.Admin.ViewModels
{
    public class CreateTeamMemberVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Boş ola bilməz"), MaxLength(50, ErrorMessage = "Uzunluq maximum 50 simvol olmalıdır")]
        public string FullName { get; set; }
        [Required]
        public string Profession { get; set; }//ixtisasi
        [Required, NotMapped]
        public IFormFile Photo { get; set; }
    }
}
