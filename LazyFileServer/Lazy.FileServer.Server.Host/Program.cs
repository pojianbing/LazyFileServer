var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLazyFileServer(builder.Configuration);

//builder.Services.AddLazyFileServer(builder.Configuration)
//    .AddFilePathCalculator<CustomFilePathCalculator>() // 自定义文件路径计算方式
//    .ReplaceAppFinder<CustomAppFinder>(); // 自定义app信息查找器

builder.Services.AddControllers();

var app = builder.Build();

app.UseSimpleFileServer("/");
app.MapControllers();

app.Run();