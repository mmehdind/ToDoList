using System.ComponentModel.DataAnnotations;

namespace TodoList.ViewModels
{
    public class CreateTaskViewModel
    {
        [Required(ErrorMessage = "عنوان تسک الزامی است")]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime DueDate { get; set; }

        public int CategoryId { get; set; }

        
    }
}
