using Microsoft.AspNetCore.Identity;

namespace TodoList.Models;


public enum Gender
{
    Male = 1,
    Female = 2,
    Other = 3
}

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; } = "";
    public string? LastName { get; set; } = "";
    public Gender Gender { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public string? ProfileImage { get; set; } 
    public ICollection<TaskItem>? Tasks { get; set; }

}

