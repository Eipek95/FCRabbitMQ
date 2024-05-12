using FCRabbitMQ.ExcelCreate.Hubs;
using FCRabbitMQ.ExcelCreate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace FCRabbitMQ.ExcelCreate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHubContext<MyHub> _hubContext;


        public FilesController(AppDbContext appDbContext, IHubContext<MyHub> hubContext)
        {
            _appDbContext = appDbContext;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, int fileId)
        {
            if (file is not { Length: > 0 }) return BadRequest();

            var userFile = await _appDbContext.UserFiles.FirstAsync(x => x.Id == fileId);

            var filePath = userFile.FileName + Path.GetExtension(file.FileName);

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", filePath);

            using FileStream stream = new(path, FileMode.Create);

            await file.CopyToAsync(stream);

            userFile.CreatedDate = DateTime.Now;
            userFile.FilePath = filePath;
            userFile.FileStatus = FileStatus.Completed;

            await _appDbContext.SaveChangesAsync();
            //signalr notification

            var usersInSameDepartment = _appDbContext.UserDepartments
             .Where(ud => ud.DepartmentId == _appDbContext.UserDepartments.FirstOrDefault(d => d.UserId == userFile.UserId).DepartmentId)
             .Select(ud => ud.UserId)
             .ToList();

            var fileUploadUserName = _appDbContext.Users.Find(userFile.UserId)!.UserName;
            await _hubContext.Clients.Users(usersInSameDepartment).SendAsync("ReceiveNotification", $"Uploaded file by {fileUploadUserName}");
            return Ok();
        }
    }
}
