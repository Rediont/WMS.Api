var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxODAwNDg5NjAwIiwiaWF0IjoiMTc2ODk4NTgzNSIsImFjY291bnRfaWQiOiIwMTliZGZjNTRhMjU3YWJhYmM3MjNmMDQxNWM1Yzk0NyIsImN1c3RvbWVyX2lkIjoiY3RtXzAxa2Zmd2JlYnY4NGtoazluNnI1OThmYXc2Iiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.09FMeRc4FMA9MomKT1o_pDEpo1D6BNI8177EoWfLy-WPD-tbUs6jhEgXfVw928tjKoReuzwHy8DF3gN6qklaBDag0cotiAkOzuYlvM1OcQ7Eim7fJXipKTMAYIQN65M2Bvp31mS__hgkqA5KHOXlvGzdAIJP5bmnWIBzfGX4rm6-zF_McWiVMmyDD5XOCULVIzjFm99l5R7-DI-WfdrKCazui6o8sJFlolke5uEVlufQgcGZqkcT9NvERJ-Ufpq9pK0LkCYDa7MkEy-MNM80BKV5ObhMoXQxI972lUl4zgBFdPfpLoWlowOvasJw0LC_-LvFdlXO4kHPujvyxZ48cw";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
