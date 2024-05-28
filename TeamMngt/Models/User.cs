
using System.ComponentModel.DataAnnotations;

namespace TeamMngt.Models;

public class User
{ 
        [Key]
        public string Nazwa { get; set; }
        public byte[] Haslo { get; set; }
}