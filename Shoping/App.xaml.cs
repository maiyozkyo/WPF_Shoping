using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shoping.ApiBusiness;
using Shoping.Business;
using Shoping.Business.UserServices;
using Shoping.Data_Access.DTOs;
using Shoping.Presentation;
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
        public static IUserServices iUserServices { get; set; }
        public static IApiService iApiService { get; set; }
        public static Auth Auth { get; private set; } 
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
            containerBuilder.RegisterType<UserBusiness>().WithParameter("_dbName", dbName).As<IUserServices>();
            containerBuilder.RegisterType<ApiService>().As<IApiService>();
            #endregion

            #region Resolve
            var container = containerBuilder.Build();
            iUserServices = container.Resolve<IUserServices>();
            iApiService = container.Resolve<IApiService>();
            #endregion
        }

        public static void SetAuth(UserDTO loginUser)
        {
            if (Auth == null)
            {
                Auth = new Auth();
            }

            if (loginUser != null)
            {
                Auth.Email = loginUser.Email;
                Auth.CreatedBy = loginUser.CreatedBy;
                Auth.CreatedOn = loginUser.CreatedOn;
                Auth.UserName = loginUser.UserName;
                Auth.LoginOn = DateTime.Now;

            }
        }
    }

}
