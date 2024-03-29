using InfoManager.Server.DbContexts;
using InfoManager.Server.Middlewares;
using InfoManager.Server.Models;
using InfoManager.Server.Services;
using InfoManager.Server.Services.Auth;
using InfoManager.Server.Services.Repositorys;
using InfoManager.Server.Services.Repositorys.Interfaces;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using System.Net;
using System.Reflection;
// TODO s:
//  Use cancell token for Get requests


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen(options=>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddAuthentication()
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,x=>
    {
        x.EventsType = typeof(CustomCookieAuthenticationEvents);
    });
builder.Services.AddAuthorization(configure=>
{
    configure.AddPolicy("User", configurePolicy =>
    {
        configurePolicy.RequireClaim("loginKey");
    });
});


// TODO : Change it before publish
if(builder.Environment.IsDevelopment() || true)
{
    bool useSqlServer = true;
    if (useSqlServer)
    {
        builder.Services.AddSqlServer<MainDbContext>(builder.Configuration["ConnectionStrings:LocalSqlServer"]);
    }
    else
    {
        builder.Services.AddSqlite<MainDbContext>(builder.Configuration["ConnectionStrings:LocalSqlite"]);
    }
}
else
{
    // TODO : Implement my sql
}
builder.Services.AddSingleton<CustomCookieAuthenticationEvents>();
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<ISessionRepository,SessionRepository>();
builder.Services.AddScoped<ISpaceRepository,SpaceRepository>();
builder.Services.AddScoped<ISpaceMemberRepository,SpaceMemberRepository>();
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<MainDbUnitOfWork>();
builder.Services.AddSingleton<SessionAuthMiddleware>();
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
app.UseAuthentication();
app.UseAuthorization();


app.UseMiddleware<SessionAuthMiddleware>();
app.MapControllers();

app.MapRazorPages();
app.MapFallbackToFile("index.html");

var db = app.Services.CreateScope().ServiceProvider.GetService<MainDbContext>()!.Database;
db.EnsureCreated();
#if DEBUG
db.Migrate();
#endif
app.Run();
