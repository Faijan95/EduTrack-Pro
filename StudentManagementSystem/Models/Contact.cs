using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Message { get; set; }

        public string Status { get; set; } = "Pending";


        public DateTime CreatedDate { get; set; }
         = DateTime.Now;
    }

}