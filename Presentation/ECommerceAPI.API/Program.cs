using ECommerceAPI.API.Configurations.ColumnWriters;
using ECommerceAPI.API.Extensions;
using ECommerceAPI.Application;
using ECommerceAPI.Application.Validators.Product;
using ECommerceAPI.Infrastructure;
using ECommerceAPI.Infrastructure.Filters;
using ECommerceAPI.Infrastructure.Services.Storage.Azure;
using ECommerceAPI.Infrastructure.Services.Storage.Local;
using ECommerceAPI.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;
using SignalR;
using SignalR.Hubs;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//To reach HttpContext object which created after request made by user
//We use it to get user's username for basket processes
builder.Services.AddHttpContextAccessor();


// Add services to the container.
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddHttpClient();
builder.Services.AddSignalRServices();

//Add Storage Type
builder.Services.AddStorage<AzureStorage>(); //when we use StorageService method it will get them from Azure storage

//CORS Policy
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
.AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

//---------------Serilog Configuration-------------------//

Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("log/log.txt")
    .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSQL"), "logs",
    needAutoCreateTable: true,
    columnOptions: new Dictionary<string, ColumnWriterBase>
    {

        {"message", new RenderedMessageColumnWriter() }, //creating message column and write data while logging
        {"message_template", new MessageTemplateColumnWriter() },
        {"level", new LevelColumnWriter() },
        {"time_stamp", new TimestampColumnWriter() },
        {"exceptiom", new ExceptionColumnWriter() },
        {"log_event", new LogEventSerializedColumnWriter()},
        //extra olarak user_name bildirilecek. Bu ColumnWriterBase'den türemiş library'de class olmadığı için bu abstract classdan türeyen bir sınıf oluşturucağız. Bu işlem API'da olacak çünkü bir Business işlemi yok
        {"user_name", new UsernameColumnWriter() },
    })
    .WriteTo.Seq(builder.Configuration["Seq:ServerURL"])
    .Enrich.FromLogContext()
    .MinimumLevel.Information() //it will log for information. It could be error etc. 
    .CreateLogger();


builder.Host.UseSerilog(log); //we are using Serilog with log configs


//Http Logging
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;

});
//-----------------------------------------------------//


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
            NameClaimType = ClaimTypes.Name //JWT üzerine Name claimine karşılık gelen değeri User.Identity.Name propertysine karşılık gelen değeri elde edebiliyoruz. Hangi kullanıcının istek yaptığını. //JWT'yi ürettiğimiz yapılanmada bu bilgi girilir.
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

/*Excepiton Handler*/
app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());


app.UseStaticFiles();

app.UseHttpLogging();
app.UseCors();
app.UseHttpsRedirection();


//Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

//if user is authenticated, below middleware will catch its username value to log it
//it is expecting return value as Task so it's async
app.Use(async (context, next) =>
{
    //not null and returns true
    var username = context.User?.Identity?.IsAuthenticated != null || true ? context.User?.Identity?.Name : null;
    //after getting this value, creating property and save it tot he log context

    LogContext.PushProperty("user_name", username);

    await next();
});


app.MapControllers();


app.MapHubs(); //signalR
app.Run();

