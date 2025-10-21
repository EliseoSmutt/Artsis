using AutoMapper;
using Core.AutoMapper;
using Core.Interfaces.IRepositories;
using Infraestructure.Repositories;
using Infraestructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Infraestructura.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.ConfigureOptions<AppSettings>();


builder.Services.AddDbContext<ContextoArtsis>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Artsis"));
});

IMapper mapper = AutoMapperConfiguration.InitializeAutoMapper();
builder.Services.AddSingleton(mapper);

//Repositorios
builder.Services.AddTransient<IEmpleadoRepository, EmpleadoRepository>();
builder.Services.AddTransient<IAbonadoRepository, AbonadoRepository>();
builder.Services.AddTransient<IPersonaRepository, PersonaRepository>();
builder.Services.AddTransient<ITaller_SeminarioRepository, Taller_SeminarioRepository>();
builder.Services.AddTransient<ITalleristaRepository, TalleristaRepository>();

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
//-----------------------------------------------------
builder.Services.AddTransient<ILibroRepository, LibroRepository>();
builder.Services.AddTransient<IReservaRepository, ReservaRepository>();
builder.Services.AddTransient<IFuncionRepository, FuncionRepository>();
builder.Services.AddTransient<IPeliculaRepository, PeliculaRepository>();
builder.Services.AddTransient<ISala_FuncionRepository, Sala_FuncionRepository>();
builder.Services.AddTransient<ITalleristaRepository, TalleristaRepository>();
builder.Services.AddTransient<IEstadosReservaRepository, EstadosReservaRepository>();
builder.Services.AddTransient<IInscripcionesRepository, InscripcionesRepository>();
builder.Services.AddTransient<IGenero_LibrosRepository, Genero_LibrosRepository>();
builder.Services.AddScoped<IGenero_PeliculaRepository, Genero_PeliculaRepository>();

//-----------------------------------------------------




var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    //Para serializar sin $values ni $id
    // options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    options.JsonSerializerOptions.WriteIndented = true;
});

//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(builder =>
//    {
//        builder.WithOrigins("https://localhost:7077", "http://localhost:7077")
//                            .AllowAnyHeader()
//                            .AllowAnyOrigin()
//                            .AllowAnyMethod();
//    });
//});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});





var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors();
    app.UseCors("AllowAll");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
