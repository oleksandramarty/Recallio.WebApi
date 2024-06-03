using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Recallio.Auth;
using Recallio.Auth.Schemes;
using Recallio.Auth.Tokens;
using Recallio.Domain;
using Recallio.Domain.Models.User;
using Recallio.Interfaces;
using Recallio.Kernel.Extensions;
using Recallio.Mediatr;
using Recallio.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Recallio.Models.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IPasswordHasher<User>, RecallioPasswordHasher>();
builder.Services.AddSignalR();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("RecallioConnection")));

builder.Services.AddTokenGenerator(options =>
{
    options.JwtKey = builder.Configuration["JwtIssuer"];
    options.JwtKey = builder.Configuration["JwtKey"];
    options.JwtExpireDays = builder.Configuration["JwtExpireDays"];
});

builder.Services.AddIdentity<User, Role>(config =>
    {
        config.User.RequireUniqueEmail = true;
        config.Password.RequireDigit = true;
        config.Password.RequiredLength = 6;
        config.Password.RequireLowercase = false;
        config.Password.RequireNonAlphanumeric = false;
        config.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtRecallioDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtRecallioDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtRecallioDefaults.AuthenticationScheme;
    })
    .AddScheme<JwtRecallioOptions, JwtRecallioHandler>(JwtRecallioDefaults.AuthenticationScheme,
        options => { options.Realm = "Protect JwtRecallio"; });

builder.Services.AddAutoMapper(config => config.AddProfile(new MappingProfile()));

//builder.Services.AddScoped<IHotelRoomRepository, HotelRoomRepository>();
builder.Services.AddRouting(option => option.LowercaseUrls = true);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Recallio.WebApi", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'JwtRecallio' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'JwtRecallio 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "JwtRecallio"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });
    // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    // c.IncludeXmlComments(xmlPath);
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped(typeof(IReadGenericService<>), typeof(GenericService<>));
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()); 

builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new MediatrModule());
});

builder.Services.AddMvc();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Host.ConfigureContainer<ContainerBuilder>(
    builder => builder.RegisterModule(new MediatrModule()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options => options.SerializeAsV2 = true);
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Recallio.WebApi v1"));
}

var cts = new CancellationTokenSource();
var cancellationToken = cts.Token;
app.UpdateDatabaseAsync().Wait(cancellationToken);
app.InitDatabase(app.Environment.IsProduction());

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();