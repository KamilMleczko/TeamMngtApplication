using System.ComponentModel.DataAnnotations;

namespace TeamMngt.Models;

public class Projekt
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Nazwa")]
    public String Nazwa { get; set; }
    
    [DataType(DataType.Date)]
    [Display(Name = "Data Rozpoczęcia")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true, NullDisplayText = "Brak")]
    public DateTime? DataRozpoczecia { get; set; }
    
    [DataType(DataType.Date)]
    [Display(Name = "Deadline")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true, NullDisplayText = "Brak")]
    public DateTime? Deadline { get; set; }
    
    [Display(Name = "Opis")]
    [DisplayFormat(NullDisplayText = "Brak")]
    public String? Opis { get; set; }
    
    
    [DisplayFormat(NullDisplayText = "Brak")]
    public ICollection<ModulProjektu>? ModulyProjektu { get; set; }
}