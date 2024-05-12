namespace FCRabbitMQ.ExcelCreate.Models
{
    public class UserDepartment
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
