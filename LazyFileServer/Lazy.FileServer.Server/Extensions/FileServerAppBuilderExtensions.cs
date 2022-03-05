using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Lazy.FileServer.Server;
using Lazy.FileServer.Server.Exceptions;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.AspNetCore.Builder
{
    public static class FileServerAppBuilderExtensions
    {
        public static IApplicationBuilder UseLazyFileServer(this IApplicationBuilder app, string route)
        {
            app.UseWhen(context => context.Request.Path.Equals(route) && context.Request.Method == HttpMethod.Post.ToString(), builder =>
            {
                builder.Use(async (context, next) =>
                {
                    try
                    {
                        using (var scope = builder.ApplicationServices.CreateScope())
                        {
                            var fileServer = scope.ServiceProvider.GetService(typeof(IFileServer)) as IFileServer;

                            var result = new List<string>();
                            foreach (IFormFile file in context.Request.Form.Files)
                            {
                                var url = await fileServer.UploadFileAsync(file.Name, file.GetAllBytes());
                                result.Add(url);
                            }

                            await context.Response.WriteAsync(ToResponse(result));
                        }
                    }
                    catch (FobiddenException)
                    {
                        await context.Response.WriteAsync(ToError(401, "无权权限"));
                    }
                    catch (Exception ex)
                    {
                        await context.Response.WriteAsync(ToError(500, ex.Message));
                    }
                });
            });

            return app;
        }

        private static string ToResponse(List<string> urls)
        {
            var sb = new StringBuilder();

            urls.ForEach(url =>
            {
                sb.Append(string.Concat("\"", url, "\"", ","));
            });

            var data = "[" + sb.ToString().TrimEnd(',') + "]";
            return "{ \"Code\": 200 ,\"Result\": " + data + "}";
        }

        private static string ToError(int code, string error)
        {
            return "{ \"Code\": " + code + ", \"Error\" : \"" + error + "\" }";
        }
    }
}
