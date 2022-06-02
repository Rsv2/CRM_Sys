using WebApi;
using WebApi.ContextFolder;
using WebApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

builder.Services.AddTransient<IUserValidator<User>, CustomUserValidator>();

builder.Services.AddTransient<IPasswordValidator<User>,
            CustomPasswordValidator>(serv => new CustomPasswordValidator(5));

builder.Services.AddLogging();

builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, Microsoft.AspNetCore.Identity.IdentityRole>()
            .AddEntityFrameworkStores<ApplicationContext>();

const string signingSecurityKey = "KIU389uigsaUIvh237JjvdIUGk32o3r0bfdKbkakjgucavdxzGy28493kljbncajk";
var signingKey = new SigningSymmetricKey(signingSecurityKey);
builder.Services.AddSingleton<IJwtSigningEncodingKey>(signingKey);

const string encodingSecurityKey = "vLKBN23jbhldvscaKIYUVFjkvsc3467UJ2KUIUG3hlksvalkhnYIVJafbnklxUjac";
var encryptionEncodingKey = new EncryptingSymmetricKey(encodingSecurityKey);
builder.Services.AddSingleton<IJwtEncryptingEncodingKey>(encryptionEncodingKey);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "CRM система", Version = "v1.0.0" });
    var filePath = Path.Combine(System.AppContext.BaseDirectory, "WebApi.xml");
    options.IncludeXmlComments(filePath);
    options.IncludeXmlComments(filePath, includeControllerXmlComments: true);

    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityDefinition("Bearer", securitySchema);

    var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };

    options.AddSecurityRequirement(securityRequirement);

});

var signingDecodingKey = (IJwtSigningDecodingKey)signingKey;
var encryptingDecodingKey = (IJwtEncryptingDecodingKey)encryptionEncodingKey;
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "JwtBearer";
        options.DefaultChallengeScheme = "JwtBearer";
    })
    .AddJwtBearer("JwtBearer", jwtBearerOptions =>
    {
        jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingDecodingKey.GetKey(),
            TokenDecryptionKey = encryptingDecodingKey.GetKey(),

            ValidateIssuer = true,
            ValidIssuer = "DiplomApi",

            ValidateAudience = true,
            ValidAudience = "DiplomClient",

            ValidateLifetime = false,

            ClockSkew = TimeSpan.FromSeconds(5)
        };
    });

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await RoleInitializer.InitializeAsync(userManager, rolesManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
