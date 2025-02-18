using CommunityToolkit.Maui;
using MauiIcons.Material;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
using REPOSITORY.CompanyDataRepository;
using REPOSITORY.DatabaseRepository;
using REPOSITORY.DBContext;
using REPOSITORY.ExerciseRepository;
using REPOSITORY.ExerciseTemplateRepository;
using REPOSITORY.SessionRepository;
using REPOSITORY.UserRepository;
using SERVICES.CompanyDataService;
using SERVICES.DatabaseServices;
using SERVICES.ExerciseServices;
using SERVICES.ExerciseTemplateServices;
using SERVICES.NavigationServices;
using SERVICES.SessionServices;
using SERVICES.UserServices;
using SkiaSharp.Views.Maui.Controls.Hosting;
using SpinningTrainerTV.Socket;
using SpinningTrainerTV.ViewModelsTV;
using SpinningTrainerTV.ViewTV;
using SpinTrainer.ViewModels;
using UTILITIES.CryptographyDataUtility;
using UTILITIES.MailUtility;
using UTILITIES.ToastMessagesUtility;

namespace SpinningTrainerTV
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .UseMaterialMauiIcons()
                .AddAudio()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseMauiCommunityToolkit()
                .RegisterDBContext()
                .RegisterRepositories()
                .RegisterServices()
                .RegisterUtilities()
                .RegisterViewModels()
                .RegisterViews();

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
            
            mauiAppBuilder.Services.AddScoped<DeviceResponder>();

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
            mauiAppBuilder.Services.AddTransient<ConnectionViewTV>();
            mauiAppBuilder.Services.AddTransient<PlaySessionViewTV>();
            mauiAppBuilder.Services.AddTransient<RequestUserPINViewTV>();
            mauiAppBuilder.Services.AddTransient<SelectUserViewTV>();
            mauiAppBuilder.Services.AddTransient<SessionFinalResultsViewTV>();
            mauiAppBuilder.Services.AddTransient<SessionListViewTV>();
            mauiAppBuilder.Services.AddTransient<SplashScreenViewTV>();          

            return mauiAppBuilder;
        }

        private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<ConnectionViewModelTV>();
            mauiAppBuilder.Services.AddScoped<PlaySessionViewModelTV>();
            mauiAppBuilder.Services.AddTransient<RequestUserPINViewModelTV>();
            mauiAppBuilder.Services.AddTransient<SelectUserViewModelTV>();
            mauiAppBuilder.Services.AddTransient<SessionListViewModelTV>();
            mauiAppBuilder.Services.AddTransient<SplashScreenViewModelTV>();
            mauiAppBuilder.Services.AddScoped<SessionFinalResultsViewModelTV>();

            return mauiAppBuilder;
        }
    }
}
