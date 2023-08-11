

using ReadProcedure.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
IServiceCollection services = builder.Services;

services.AddScoped<IProcedureReaderService, ProcedureReaderService>();

services.AddControllers();


services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

WebApplication app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

