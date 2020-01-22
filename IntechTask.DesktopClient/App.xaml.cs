using IntechTask.DataAccess.DataMappers;
using IntechTask.DesktopClient.ViewModels;
using IntechTask.DesktopClient.Views;
using IntechTask.Services;
using Microsoft.Extensions.Logging.Abstractions;
using System.Data.SqlClient;
using System.Reactive.Concurrency;
using System.Windows;

namespace IntechTask.DesktopClient
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //TODO di-container
            SqlConnection ConnectionFactory() =>
                new SqlConnection(new SqlConnectionStringBuilder
                {
                    DataSource = "np:\\\\.\\pipe\\LOCALDB#70225A81\\tsql\\query",
                    InitialCatalog = "IntechTask",
                    IntegratedSecurity = true
                }.ConnectionString);

            var employeesService = new EmployeeService(
                new GenderMsSqlDataMapper(ConnectionFactory),
                new EmployeeMsSqlDataMapper(ConnectionFactory),
                new NullLogger<EmployeeService>());

            var mainViewModel = new EmployeesViewModel(employeesService, DispatcherScheduler.Current);

            MainWindow = new MainWindow {DataContext = mainViewModel};
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
