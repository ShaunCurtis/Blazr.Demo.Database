global using Blazr.Core;
global using Blazr.Demo.Database.Core;
global using Blazr.Demo.Database.Data;
global using Blazr.NavigationLocker;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.ApplicationParts;
global using System.Threading.Tasks;

// Dummy Main - Web projects need a main to compile.
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.Run();
