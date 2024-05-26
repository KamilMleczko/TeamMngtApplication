using System.ComponentModel.DataAnnotations;

namespace TeamMngt.Models;

public class Pracownik
{
    [Key]
    public int Id { get; set; }
    
    [Display(Name = "Imię")]
    public String Imie { get; set; }
    
    [Display(Name = "Nazwisko")]
    public String Nazwisko { get; set; }
    
    [Display(Name = "Stanowisko")]
    public String Stanowsiko { get; set; }
    
    [Display(Name = "Email")]
    public String Email { get; set; }

    [DisplayFormat(NullDisplayText = "Brak")]
    public ICollection<Zadanie>? Zadania{ get; set; }
    
    [DisplayFormat(NullDisplayText = "Brak")]
    public Zespol? Zespol { get; set; }
}