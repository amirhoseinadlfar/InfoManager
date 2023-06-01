using InfoManager.DbContexts;
using InfoManager.Services.Repositorys;
using InfoManager.Services.Repositorys.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();



if(builder.Environment.IsDevelopment())
{
    bool useSqlServer = true;
    if (useSqlServer)
    {
        builder.Services.AddSqlServer<MainDbContext>(@"Data Source=.;DataBase=InfoManager;Initial Catalog=InfoManager;Integrated Security=True;TrustServerCertificate=True",
            x =>
            {

            },
            x =>
            {

            });
    }
    else
    {
        builder.Services.AddSqlite<MainDbContext>("Data Source=mydb.db;");
    }
}
builder.Services.AddScoped<IUserRepository,UserRepository>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    
}
else
{
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Services.CreateScope().ServiceProvider.GetService<MainDbContext>()!.Database.EnsureCreated();
app.Run();
