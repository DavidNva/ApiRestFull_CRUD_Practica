var builder = WebApplication.CreateBuilder(args);

var misReglasCors = "ReglasCors";//Para permitir cualquier origen
builder.Services.AddCors(option =>
    option.AddPolicy(name: misReglasCors,//Con esto se permite cualquier web o app usar nuestra Api
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    )
);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseCors(misReglasCors);//uso de la regla

app.MapControllers();

app.Run();
