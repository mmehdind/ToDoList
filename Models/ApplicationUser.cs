using Microsoft.AspNetCore.Identity;
using TodoList.Enums;

namespace TodoList.Models;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; } = "";
    public string? LastName { get; set; } = "";
    public Gender Gender { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public string? ProfileImage { get; set; } 
    public ICollection<TaskItem>? Tasks { get; set; }

}

