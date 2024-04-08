using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shoping.ApiBusiness;
using Shoping.Business;
using Shoping.Business.CategoryServices;
using Shoping.Business.CustomerServices;
using Shoping.Business.OrderDetailServices;
using Shoping.Business.OrderServices;
using Shoping.Business.ProductServices;
using Shoping.Business.SettingServices;
using Shoping.Business.UserServices;
using Shoping.Business.VoucherServices;
using Shoping.Data_Access.DTOs;
using Shoping.Presentation;
using System.IO;
using System.Reflection.Metadata;
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
        public static IApiService iApiService { get; set; }
        public static IProductBusiness iProductBusiness { get; set; }
        public static ICategoryBusiness iCategoryBusiness { get; set; }
        public static IOrderBusiness iOrderBusiness { get; set; }
        public static ICustomerBusiness iCustomerBusiness { get; set; }
        public static IOrderDetailBusiness iOrderDetailBusiness { get; set; }
        public static IVoucherBusiness iVoucherBusiness { get; set; }
        public static ISettingBusiness iSettingBusiness { get; set; }
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
            var conntectionString = iConfiguration.GetSection("Database").GetSection("MongoDatabase").Value;
            #region Register
            containerBuilder.RegisterType<UserBusiness>().WithParameter("_dbName", dbName).As<IUserBusiness>();
            containerBuilder.RegisterType<OrderBusiness>().WithParameter("_dbName", dbName).As<IOrderBusiness>();
            containerBuilder.RegisterType<CustomerBusiness>().WithParameter("_dbName", dbName).As<ICustomerBusiness>();
            containerBuilder.RegisterType<ProductBusiness>().WithParameter("_dbName", dbName).As<IProductBusiness>();
            containerBuilder.RegisterType<CategoryBusiness>().WithParameter("_dbName", dbName).As<ICategoryBusiness>();
            containerBuilder.RegisterType<ApiService>().As<IApiService>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            containerBuilder.RegisterType<OrderDetailBusiness>().WithParameter("_dbName", dbName).As<IOrderDetailBusiness>();
            containerBuilder.RegisterType<VoucherBusiness>().WithParameter("_dbName", dbName).As<IVoucherBusiness>();
            containerBuilder.RegisterType<SettingBusiness>().WithParameters(new List<Autofac.NamedParameter>
            {
                new Autofac.NamedParameter("_conntectionString", conntectionString),
                new Autofac.NamedParameter("_dbName", dbName),
            }).As<ISettingBusiness>();
            
            #endregion

            #region Resolve
            var container = containerBuilder.Build();
            iUserBusiness = container.Resolve<IUserBusiness>();
            iApiService = container.Resolve<IApiService>();
            iProductBusiness = container.Resolve<IProductBusiness>();
            iCategoryBusiness = container.Resolve<ICategoryBusiness>();
            iOrderBusiness = container.Resolve<IOrderBusiness>();
            iCustomerBusiness = container.Resolve<ICustomerBusiness>();
            iOrderDetailBusiness = container.Resolve<IOrderDetailBusiness>();
            iVoucherBusiness = container.Resolve<IVoucherBusiness>();
            iSettingBusiness = container.Resolve<ISettingBusiness>();
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
