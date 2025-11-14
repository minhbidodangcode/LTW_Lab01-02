using System.ComponentModel.DataAnnotations;

namespace LTW_Lab01.Models
{
    public class Student
    {
        public int Id { get; set; }//Mã sinh viên 
        [Required(ErrorMessage = "Họ tên bắt buộc phải được nhập")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Họ tên phải từ 4 đến 100 ký tự")]
        public string? Name { get; set; } //Họ tên 
        [Required(ErrorMessage = "Email bắt buộc phải được nhập")]
        [RegularExpression(@"^[\w\.-]+@gmail\.com$", ErrorMessage = "Email phải có định dạng hợp lệ và kết thúc bằng @gmail.com")]
        public string? Email { get; set; } //Email 
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Mật khẩu phải từ 8 ký tự trở lên")]
        [Required(ErrorMessage = "Mật khẩu bắt buộc phải được nhập")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,100}$", ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự, bao gồm: chữ hoa, chữ thường, chữ số và ký tự đặc biệt")]
        public string? Password { get; set; }//Mật khẩu 
        [Required]
        public Branch? Branch { get; set; }//Ngành học 
        [Required]
        public Gender? Gender { get; set; }//Giới tính 
        public bool IsRegular { get; set; }//Hệ: true-chính quy, false-phi chính quy 
        [DataType(DataType.MultilineText)]
        [Required]
        public string? Address { get; set; }//Địa chỉ 
        [Range(typeof(DateTime), "1 /1/1963", "12/31/2005")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime DateOfBorth { get; set; }//Ngày sinh 
                                                 
        [Required(ErrorMessage = "Điểm bắt buộc phải được nhập")]
        [Range(0.0, 10.0, ErrorMessage = "Điểm phải là số thực và nằm trong khoảng từ 0.0 đến 10.0")]
        public double Score { get; set; } // Điểm
        public string? ImageUrl { get; set; }
    }
}
