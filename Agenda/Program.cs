using Agenda.Repository;
using Agenda.Repository.Context;
using Agenda.Repository.Interfaces;
using Agenda.Services;
using Agenda.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<ITempContext, TempContext>(options => options.UseInMemoryDatabase("TempDb"));

        builder.Services.AddScoped<IMedicoRepository, MedicoRepository>();
        builder.Services.AddScoped<IMedicoService, MedicoService>();
        builder.Services.AddScoped<IPacienteService, PacienteService>();
        builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionHandlerMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}