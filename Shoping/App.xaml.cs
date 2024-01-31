using Autofac;
using Autofac.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shoping.Business;
using Shoping.Presentation;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace Shoping
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; set; }
        public static IConfiguration iConfiguration { get; set; }
        public static IUserBusiness iUserBusiness { get; set; }
        public static IUserBusiness iOrderBusiness { get; set; }
        public static IApiService iApiService { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSetting.json", optional: false, reloadOnChange: true);
            iConfiguration = builder.Build();
            var containerBuilder = new ContainerBuilder();
            BuildupContainer(containerBuilder);
            

            var serviceCollection = new ServiceCollection();
            ConfigurationService(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            var mainWindow = ServiceProvider.GetRequiredService<Login>();
            mainWindow.Show();
        }

        private void ConfigurationService(IServiceCollection services)
        {
            services.AddTransient(typeof(Login));
        }

        private void BuildupContainer(ContainerBuilder containerBuilder)
        {
            var dbName = iConfiguration.GetSection("Database").GetSection("DBName").Value;

            #region Register
            containerBuilder.RegisterType<UserBusiness>().WithParameter("_dbName", dbName).As<IUserBusiness>();
            containerBuilder.RegisterType<OrderBusiness>().WithParameter("_dbName", dbName).As<IOrderBusiness>();
            containerBuilder.RegisterType<ApiService>().As<IApiService>();
            #endregion

            #region Resolve
            var container = containerBuilder.Build();
            iUserBusiness = container.Resolve<IUserBusiness>();
            iOrderBusiness = container.Resolve<IOrderBusiness>();
            iApiService = container.Resolve<IApiService>();
            #endregion
        }
    }

}
