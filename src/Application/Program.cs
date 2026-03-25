using WorkflowEngineSimulator.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<WorkflowService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
