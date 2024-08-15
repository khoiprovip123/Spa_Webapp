using AutoMapper;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Spa.Application.Authorize.Authorization;
using Spa.Application.Configuration;
using Spa.Application.Automapper;
using Spa.Application.Commands;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Domain.IRepository;
using Spa.Domain.IService;
using Spa.Domain.Service;
using Spa.Infrastructure;
using Spa.Infrastructures;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Text;
using Spa.Application.MIiddleware;
using Spa.Application.SignalR;






var builder = WebApplication.CreateBuilder(args); // cấu hình service và midleware cần thiết

// Add services to the container.


builder.Services.AddControllers();  //xử lí request http và phản hồi dựa trên controller
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowCredentials()
        .AllowAnyHeader()
        .WithOrigins("http://localhost:4200");
     
    });
});
//builder.Services.AddSwaggerGen(); //add swagger để test api 
//Add authentication to Swagger UI
//builder.Services.AddSwaggerGen();


//MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterCommand).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginCommand).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateUserForEmployeeCommand).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCustomerTypeCommand).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateAccountCommand).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateAdminCommand).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddMessageCommand).Assembly));





//add Mapper
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

//register Configuration
ConfigurationManager configuration = builder.Configuration;


//Add Database Service
builder.Services.AddDbContext<SpaDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("Spa.Infrastructure")));



//Redis Cache

var redisConfiguration = new RedisConfiguration();
configuration.GetSection("RedisConfiguration").Bind(redisConfiguration);
builder.Services.AddSingleton(redisConfiguration);
 if(redisConfiguration.Enabled)
{
    builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConfiguration.ConnectionString));
    builder.Services.AddStackExchangeRedisCache(option => option.Configuration = redisConfiguration.ConnectionString);
    builder.Services.AddSingleton<IResponseCacheService, ResponseCacheService>();
}


//Redis Cache


builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<SpaDbContext>()
    .AddSignInManager()
    .AddRoles<IdentityRole>();

//JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        //Jwt in appsettings.json
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});
//Add authentication to Swagger UI

builder.Services.AddHttpContextAccessor();
//Add authentication to Swagger UI
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

//Register services
//Customer
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
//Apppointment
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
//Service
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

//Payment
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
//Register services
//User
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();

builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IJobRepository, JobRepository>();

builder.Services.AddScoped<ICustomerTypeService, CustomerTypeService>();
builder.Services.AddScoped<ICustomerTypeRepository, CustomerTypeRepository>();

//Bill
builder.Services.AddScoped<IBillService, BillService>();
builder.Services.AddScoped<IBillRepository, BillRepository>();

//Permission
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

//TreatmentCard
builder.Services.AddScoped<ITreatmentRepository, TreatmentRepository>();
builder.Services.AddScoped<ITreatmentService, TreatmentService>();

//Message
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

// Phieuthuchi
builder.Services.AddScoped<IIncomeExpensesRepository, IncomeExpensesRepository>();
builder.Services.AddScoped<IIncomeExpensesService, IncomeExpensesService>();

//SignalR
builder.Services.AddSignalR(o =>
{
    o.EnableDetailedErrors = true;
});
//Redis 


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();  //sử dụng swagger
}
//allow accept api to font-end
app.UseCors("MyPolicy");

//app.UseCors(op => op.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Photos")),
    RequestPath = "/Photos"
});

app.UseHttpsRedirection();
//thêm middleware để chuyển http sang https để thêm bảo mật
//app.UseMiddleware<AuthorizationExceptionMiddleware>();
app.UseRouting();
//app.UseMiddleware<RequestTimingMiddleware>(); //test time response

app.UseHttpsRedirection();  //thêm middleware để chuyển http sang https để thêm bảo mật


app.UseAuthentication();
app.UseAuthorization();  //middleware xử lí ủy uyền 
//định tuyến controller 
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chatHub");

});

app.MapControllers();  //định tuyến controller 

app.Run();  // xử lí yêu cầu http đến server
