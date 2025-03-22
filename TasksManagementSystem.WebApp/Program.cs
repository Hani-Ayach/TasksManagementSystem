using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TasksManagementSystem.BL.Services;
using TasksManagementSystem.DAL.Interfaces;
using TasksManagementSystem.DAL.Repositories;
using TasksManagementSystem.EF.DataContext;
using TasksManagementSystem.EF.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//Add DbContext
string assemblyName = typeof(ApplicationDbContext).Assembly.FullName;
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"),
        x => x.MigrationsAssembly(assemblyName));
});

//Add Identity
builder.Services.AddIdentity<ApplicationUserEntity, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders().AddDefaultUI();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 6;
});

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

builder.Services.AddScoped<ProjectServices>();
builder.Services.AddScoped<TaskServices>();
builder.Services.AddScoped<EmployeeServices>();
builder.Services.AddScoped<CommentServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
