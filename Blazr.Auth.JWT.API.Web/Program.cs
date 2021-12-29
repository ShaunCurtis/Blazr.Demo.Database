global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Components.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.ApplicationParts;

global using Microsoft.IdentityModel.Tokens;


global using System.Text;

global using Blazr.Auth.JWT.Simple;
global using Blazr.Auth.JWT.Simple.Core;
global using Blazr.Auth.JWT.Simple.Data;

// Dummy Main - Web projects need a main to compile.
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.Run();

