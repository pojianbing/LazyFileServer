# Lazy FileServer
非常简单的文件服务，仅实现上传然后生成url。

### 服务端
服务端提供统一接口，以便各子应用统一上传文件。

#### 1. 安装
```
Install-Package TianHua.FileServer.Server -Version 1.0.1
```

#### 2. 配置
``` c#
"FileServer": {
  "LocalBase": "C:\\upload",
  "HttpBase": "http://localhost",
  /*
  * 文件路径计算方式，分为Date和Hash
  * Date:
  *   格式: appName/year/month/day/上传文件名
  *   重复文件处理:  重复文件追加序号, 例如上传a.txt, 存在重复，则编号为a.1.txt。 再次上传则为a.2.txt
  *   示例: http://localhost/spider/2022/03/03/c220210105154619.348.jpg
  *
  * Hash:
  *   格式: appName/hash前两位/hash3到4位/hash.扩展名
  *   重复文件处理: 重复hash会覆盖
  *   示例: http://localhost/spider/zq/I9/zqI98OULV8j60XbNSTTxQg==.jpg
  */
  "FilePathCalculatorType": "hash",
  // 应用
  "Apps": [
    {
      "AppId": "1",
      "AppName": "spider",
      "AppKey": "123456"
    }
  ]
}
```

``` c#
builder.Services.AddSimpleFileServer(builder.Configuration);
app.UseSimpleFileServer("/");
```

#### 3.自定义路径计算方式

- 定义**IFilePathCalculator**实现类
``` c#
public class CustomFilePathCalculator : IFilePathCalculator
{
    public string Name
    {
        get
        {
            return "custom";
        }
    }

    /// <summary>
    /// 直接返回文件名
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public string Calculate(FilePathCalculatorInput input)
    {
        return input.FileName;
    }
}
```

- 注入服务
``` c#
builder.Services.AddScoped<IFilePathCalculator, CustomFilePathCalculator>();
```

- 修改配置
``` c#
"FilePathCalculatorType": "custom"
```

#### 4.自定义应用查找器
默认从AppSetting查找.

- 定义**CustomAppFinder**实现类
``` c#
public class CustomAppFinder : IAppFinder
{
    private static List<AppInfo> Apps = new List<AppInfo>()
    {
        new AppInfo{ AppId = "1", AppKey = "654321", AppName = "spider" }
    };

    public Task<AppInfo> FindAsync(string appid)
    {
        return Task.FromResult(Apps.FirstOrDefault(e => e.AppId == appid));
    }
}
```

- 替换默认服务
``` c#
builder.Services.AddSimpleFileServer(builder.Configuration).ReplaceAppFinder<CustomAppFinder>();
```

#### 5.可能遇到的问题
访问图片服务时报错 **请求筛选模块被配置为拒绝包含双重转义序列的请求**
![Image text](http://git.kjjl100.com/pojianbing/tianhua_fileserver/-/raw/main/imgs/1.png)
解决办法： 设置requestFiltering allowDoubleEscaping="True"/
![Image text](http://git.kjjl100.com/pojianbing/tianhua_fileserver/-/raw/main/imgs/2.png)

### 客户端
前端应用理论上可以直接调用服务的上传接口，但这样会将appid,AppKey裸露在外界。因此需要各应用包裹下，提供一个上传端点。

#### 1. 安装
```
Install-Package TianHua.FileServer.Client -Version 1.0.1
```

#### 2. 示例

``` c#
using Microsoft.AspNetCore.Mvc;

namespace TianHua.FileServer.Client.WebApi.Host.Controllers
{
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;

        private IHttpContextAccessor _httpContextAccessor;

        public FileController(ILogger<FileController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost()]
        [Route("Upload")]
        public async Task<IEnumerable<string>> UploadAsync()
        {
            var client = new FileServerClient("http://localhost:5001", "1", "123456");
            return await client.UploadAsync(_httpContextAccessor.HttpContext);
        }
    }
}
```
