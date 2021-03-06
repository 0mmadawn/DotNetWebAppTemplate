using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using web.Common.DataAccess;
using web.Common.Enums;
using web.DAO;
using web.Repositories;
using web.Services;
namespace web
{
    public class Startup
    {
        private IWebHostEnvironment _webHostEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this._webHostEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var mvcBuilder = services.AddRazorPages()
                .AddViewOptions(options =>
                {
                    // .NETCORE MVCによるクライアントサイド検証の無効化
                    options.HtmlHelperOptions.ClientValidationEnabled = false;
                });

            if (this._webHostEnvironment.IsDevelopment())
            {
                // Razorの実行時コンパイル
                mvcBuilder.AddRazorRuntimeCompilation();
            }

            // DB接続文字列の辞書
            var connectionDict = new ReadOnlyDictionary<ConnectionName, string>
            (
                new Dictionary<ConnectionName, string>
                {
                    { ConnectionName.ReadOnlyConnection, this.Configuration[nameof(ConnectionName.ReadOnlyConnection)] },
                    { ConnectionName.WriteConnection, this.Configuration[nameof(ConnectionName.WriteConnection)] }
                }
            );

            // DB接続DI
            services.AddSingleton<IReadOnlyDictionary<ConnectionName, string>>(connectionDict);
            services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();

            // Session
            // Formの偽装防止用
            // 例: Edit->Update
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //CSRF
            services.AddControllersWithViews(options =>
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
            //DI
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserDAO, UserDAO>();
            services.AddTransient<IUserRepository, UserRepository>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/error");
                //app.UseStatusCodePagesWithReExecute("/error/{0}");
            }
            else
            {
                // 500, 404用のページを用意
                // SystemController(エラーハンドラ)のテストをする場合はこの内容をtrueの節に移す
                app.UseExceptionHandler("/error");
                app.UseStatusCodePagesWithReExecute("/error/{0}");
                // レスポンスヘッダにHTTP Strict Transport Security追加
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            //nginx用
            //app.UseForwardedHeaders(new ForwardedHeadersOptions
            //{
            //    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            //});

            //認証
            //app.UseAuthorization();

            //セッション
            app.UseSession();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "error",
                    pattern: "error/{id?}",
                    defaults: new { controller = "System", action = "Error", }
                );

                endpoints.MapControllerRoute(
                    name: "users",
                    pattern: "users/",
                    defaults: new { controller = "User", action = "Index", }
                );
                endpoints.MapControllerRoute(
                    name: "user",
                    pattern: "user/{id}/",
                    defaults: new { controller = "User", action = "Show", }
                );
                endpoints.MapControllerRoute(
                    name: "user edit",
                    pattern: "user/{id}/edit/",
                    defaults: new { controller = "User", action = "Edit", }
                );
                endpoints.MapControllerRoute(
                    name: "user update",
                    pattern: "user/{id}/update",
                    defaults: new { controller = "User", action = "Update", }
                );

                // TODO: delete
                // エラーを明示的に発生させるためのテスト用のルーティング
                endpoints.MapControllerRoute(
                    name: "error_sample",
                    pattern: "users/error/",
                    defaults: new { controller = "User", action = "ErrorSample", }
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "/",
                    defaults: new { controller = "User", action = "Index", }
                );
            });
        }
    }
}