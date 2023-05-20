using ECommerceAPI.Application;
using ECommerceAPI.Application.Validators.Product;
using ECommerceAPI.Infrastructure;
using ECommerceAPI.Infrastructure.Filters;
using ECommerceAPI.Infrastructure.Services.Storage.Azure;
using ECommerceAPI.Infrastructure.Services.Storage.Local;
using ECommerceAPI.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddHttpClient();

//Add Storage Type
builder.Services.AddStorage<AzureStorage>(); //when we use StorageService method it will get them from Azure storage

//CORS Policy
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
.AllowAnyHeader().AllowAnyMethod()));


// Applying created Validation Filter and Fluent Validation (applied to all Validations by giving one of the class in Application layer) and removing default filter approach of ASP .Net Core
builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>()).AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>
()).ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
    {
        options.TokenValidationParameters = new()
        {
            //Parameters to be used for validation
            ValidateAudience = true,
            ValidateIssuer = true, //Service'in yayınlanacağı uzantı
            ValidateLifetime = true, //Token'ın süresi
            ValidateIssuerSigningKey = true, //Üretilen token'ın uygulamaya ait bir değer olduğunu ifade eden security key

            //Values to be used for validation
            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])), //Symetric key against IssuerSigningKey
            //To unauthenticate when token expire. If expires time exists, check whether it's expired or not
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//****//
app.UseStaticFiles();
app.UseCors();


app.UseHttpsRedirection();


//Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

