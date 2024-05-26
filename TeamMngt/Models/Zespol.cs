using System.ComponentModel.DataAnnotations;

namespace TeamMngt.Models;

public class Zespol
{
    [Key]
    [Display(Name = "Id")]
    public int Id { get; set; }
    
    [Display(Name = "Nazwa zespołu")]
    public String Nazwa { get; set; }
    
    [Display(Name = "Opis")]
    [DisplayFormat(NullDisplayText = "Brak")]
    public String? Opis { get; set; }

    [DisplayFormat(NullDisplayText = "Brak")]
    public Modul? Modul { get; set; }
    
    [DisplayFormat(NullDisplayText = "Brak")]
    public ICollection<Pracownik>? Pracownicy { get; set; }
    
    [DisplayFormat(NullDisplayText = "Brak")]
    public Pracownik? Lider { get; set; }
    
    
    
    
}