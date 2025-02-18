using SpinningTrainer.Views;
using SERVICES.UserServices;
using REPOSITORY.DBContext;
using Microcharts.Maui;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using CommunityToolkit.Maui;
using MauiIcons.Material;
using UTILITIES.CryptographyDataUtility;
using UTILITIES.MailUtility;
using SERVICES.CompanyDataService;
using SERVICES.DatabaseServices;
using SERVICES.ExerciseServices;
using SERVICES.SessionServices;
using REPOSITORY.CompanyDataRepository;
using REPOSITORY.DatabaseRepository;
using REPOSITORY.ExerciseRepository;
using REPOSITORY.SessionRepository;
using REPOSITORY.UserRepository;
using UTILITIES.ToastMessagesUtility;
using SpinTrainer.ViewModels;
using SERVICES.NavigationServices;
using SpinningTrainer.ViewModels;
using SpinningTrainer.ViewModel;
using SpinningTrainer.View;
using Maui.NullableDateTimePicker;
using SERVICES.SynchronizerServices;
using SpinTrainer.Views;
using REPOSITORY.ExerciseTemplateRepository;
using SERVICES.ExerciseTemplateServices;


namespace SpinningTrainer
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMicrocharts()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureNullableDateTimePicker()
                .UseMauiCommunityToolkit()
                .UseMaterialMauiIcons()
                .RegisterDBContext()
                .RegisterRepositories()
                .RegisterServices()
                .RegisterUtilities()
                .RegisterViewModels()
                .RegisterViews();

            builder.Services.AddSingleton<AppShell>();

#if DEBUG   
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }

        private static MauiAppBuilder RegisterDBContext(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<ApplicationDBContext>();
            mauiAppBuilder.Services.AddTransient<SQLiteDBContext>();

            return mauiAppBuilder;
        }

        private static MauiAppBuilder RegisterUtilities(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddScoped<ICryptographyDataUtility, CryptographyDataUtility>();
            mauiAppBuilder.Services.AddScoped<IToastMessagesUtility, ToastMessagesUtility>();
            mauiAppBuilder.Services.AddScoped<IMailUtility, MailUtility>();

            return mauiAppBuilder;
        }

        private static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddScoped<ICompanyDataService, CompanyDataService>();
            mauiAppBuilder.Services.AddScoped<IDatabaseServices, DatabaseServices>();
            mauiAppBuilder.Services.AddScoped<IExerciseServices, ExerciseServices>();
            mauiAppBuilder.Services.AddScoped<ISessionServices, SessionServices>();
            mauiAppBuilder.Services.AddScoped<IUserServices, UserServices>();
            mauiAppBuilder.Services.AddScoped<INavigationServices, NavigationServices>();
            mauiAppBuilder.Services.AddScoped<ISynchronizerServices, SynchronizerServices>();
            mauiAppBuilder.Services.AddScoped<IExerciseTemplateServices, ExerciseTemplateServices>();

            return mauiAppBuilder;
        }

        private static MauiAppBuilder RegisterRepositories(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddScoped<ICompanyDataRepository, CompanyDataRepository>();
            mauiAppBuilder.Services.AddScoped<IDatabaseRepositoy, DatabaseRepository>();
            mauiAppBuilder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
            mauiAppBuilder.Services.AddScoped<ISessionRepository, SessionRepository>();
            mauiAppBuilder.Services.AddScoped<IUserRepository, UserRepository>();
            mauiAppBuilder.Services.AddScoped<IExerciseTemplateRepository, ExerciseTemplateRepository>();

            return mauiAppBuilder;
        }

        private static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<SplashScreenView>();
            mauiAppBuilder.Services.AddTransient<ConnectionView>();

            mauiAppBuilder.Services.AddTransient<LoginView>();
            mauiAppBuilder.Services.AddTransient<RecoveryLoginDataView>();

            mauiAppBuilder.Services.AddTransient<AdminMenuView>();
            mauiAppBuilder.Services.AddTransient<CompanyDataView>();

            mauiAppBuilder.Services.AddTransient<SuperUserMenuView>();
            mauiAppBuilder.Services.AddTransient<UserListView>();
            mauiAppBuilder.Services.AddTransient<UserDetailsView>();

            mauiAppBuilder.Services.AddTransient<ConfiguratorMenuView>();
            mauiAppBuilder.Services.AddTransient<ExerciseConfiguratorView>();
            mauiAppBuilder.Services.AddTransient<CustomExerciseTemplateView>();

            mauiAppBuilder.Services.AddTransient<ExerciseConfiguratorListView>();
            mauiAppBuilder.Services.AddTransient<CustomExerciseTemplateListView>();

            mauiAppBuilder.Services.AddTransient<MainPageView>();
            mauiAppBuilder.Services.AddTransient<RemoteControlView>();
            mauiAppBuilder.Services.AddTransient<NewSessionView>();
            mauiAppBuilder.Services.AddTransient<NewSessionExercisesListView>();            
            mauiAppBuilder.Services.AddTransient<SessionExerciseFormView>();
            mauiAppBuilder.Services.AddTransient<InsertNewDuplicateSessionDataView>();

            mauiAppBuilder.Services.AddTransient<SessionExerciseSelectionView>();
            mauiAppBuilder.Services.AddTransient<SessionExerciseHandsPositionSelectionView>();
            mauiAppBuilder.Services.AddTransient<SessionExerciseRpmAndEnergyZoneView>();
            mauiAppBuilder.Services.AddTransient<SessionExerciseResistanceAndTimeView>();
            mauiAppBuilder.Services.AddTransient<SessionExerciseResumeView>();
           
            return mauiAppBuilder;
        }

        private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<SplashScreenViewModel>();
            mauiAppBuilder.Services.AddTransient<CompanyDataViewModel>();
            mauiAppBuilder.Services.AddTransient<ConnectionViewModel>();
            mauiAppBuilder.Services.AddTransient<LoginViewModel>();
            mauiAppBuilder.Services.AddTransient<RecoveryLoginDataViewModel>();
            mauiAppBuilder.Services.AddTransient<MainPageViewModel>();
            mauiAppBuilder.Services.AddTransient<UsersViewModel>();
            mauiAppBuilder.Services.AddTransient<SessionViewModel>();
            mauiAppBuilder.Services.AddTransient<NewSessionExerciseViewModel>();
            mauiAppBuilder.Services.AddTransient<RemoteControlViewModel>();

            mauiAppBuilder.Services.AddTransient<ConfiguratorMenuViewModel>();
            mauiAppBuilder.Services.AddTransient<ExerciseConfiguratorViewModel>();
            mauiAppBuilder.Services.AddTransient<CustomExerciseTemplateViewModel>();

            mauiAppBuilder.Services.AddTransient<ExerciseConfiguratorListViewModel>();
            mauiAppBuilder.Services.AddTransient<CustomExerciseTemplateListViewModel>();

            mauiAppBuilder.Services.AddTransient<InsertNewDuplicateSessionDataViewModel>();

            return mauiAppBuilder;
        }
    }
}