var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLazyFileServer(builder.Configuration);

//builder.Services.AddLazyFileServer(builder.Configuration)
//    .AddFilePathCalculator<CustomFilePathCalculator>() // �Զ����ļ�·�����㷽ʽ
//    .ReplaceAppFinder<CustomAppFinder>(); // �Զ���app��Ϣ������

builder.Services.AddControllers();

var app = builder.Build();

app.UseSimpleFileServer("/");
app.MapControllers();

app.Run();