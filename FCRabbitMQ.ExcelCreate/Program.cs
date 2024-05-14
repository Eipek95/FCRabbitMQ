using FCRabbitMQ.ExcelCreate.Hubs;
using FCRabbitMQ.ExcelCreate.Models;
using FCRabbitMQ.ExcelCreate.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddSingleton(sp => new ConnectionFactory()
{
    Uri = new Uri(builder.Configuration.GetConnectionString("RabbitMQ")),
    DispatchConsumersAsync = true
});
builder.Services.AddSingleton<RabbitMQClientService>();
builder.Services.AddSingleton<RabbitMQPublisher>();



builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});
builder.Services.AddDbContext<AdventureWorks2019Context>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer2"));
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>(opt =>
{
    opt.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<AppDbContext>();



var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    appDbContext.Database.Migrate();

    if (!appDbContext.Users.Any())
    {
        userManager.CreateAsync(new IdentityUser() { UserName = "deneme", Email = "deneme@gmail.com" }, password: "Password12*").Wait();//async to sync
        userManager.CreateAsync(new IdentityUser() { UserName = "deneme2", Email = "deneme2@gmail.com" }, password: "Password12*").Wait();
        userManager.CreateAsync(new IdentityUser() { UserName = "deneme3", Email = "deneme3@gmail.com" }, password: "Password12*").Wait();
        userManager.CreateAsync(new IdentityUser() { UserName = "deneme4", Email = "deneme4@gmail.com" }, password: "Password12*").Wait();
        userManager.CreateAsync(new IdentityUser() { UserName = "deneme5", Email = "deneme5@gmail.com" }, password: "Password12*").Wait();
    }

    if (!appDbContext.Departments.Any())
    {
        appDbContext.Departments.AddRange(new Department[]
        {
            new Department(){Name="Department1"},
            new Department(){Name="Department2"}
        });
        appDbContext.SaveChangesAsync().Wait();
    }

    if (!appDbContext.UserDepartments.Any())
    {
        appDbContext.UserDepartments.AddRange(new UserDepartment[]
        {
            new UserDepartment(){DepartmentId=1,UserId="23258478-8dff-42f4-8d66-3f203a11d351"},
            new UserDepartment(){DepartmentId=1,UserId="8dda65c6-069d-4f78-8f38-b4fcb4df352c"},
            new UserDepartment(){DepartmentId=1,UserId="4d866636-1302-4555-8207-bd62852a58fe"},
            new UserDepartment(){DepartmentId=2,UserId="a1bd3b40-c6b4-41a2-99e7-1857cac18bac"},
            new UserDepartment(){DepartmentId=2,UserId="5357c432-0c4c-41c1-b90d-9b4d254a4410"},
        });

        appDbContext.SaveChangesAsync().Wait();
    }
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<MyHub>("/MyHub");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
