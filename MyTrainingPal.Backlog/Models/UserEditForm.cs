using System.ComponentModel.DataAnnotations;

namespace MyTrainingPal.Backlog.Models;

public class UserEditForm
{
    [Required]
    public string FormFirstName { get; set; }

    [Required]
    public string FormLastName { get; set; }

    [Required]
    [EmailAddress]
    public string FormEmail { get; set; }       
}
