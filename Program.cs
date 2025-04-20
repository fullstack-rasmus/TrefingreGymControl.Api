using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using TrefingreGymControl.Api.Domain.Users;
using TrefingreGymControl.Api.Persistence;
using Serilog;
using TrefingreGymControl.Api.Infrastructure.Auth.Permissions;
using Microsoft.AspNetCore.Authorization;
using TrefingreGymControl.Api.Infrastructure.Auth.Requirements;
using TrefingreGymControl.Api.Domain.Notifications;
using TrefingreGymControl.Api.Domain.Subscriptions;
using TrefingreGymControl.Api.BackgroundServices.Subscriptions;
using TrefingreGymControl.Api.Utils;
using TrefingreGymControl.Api.Domain.Receipts;
using TrefingreGymControl.Api.Application.Common;
using TrefingreGymControl.Api.Application.Receipts;
using TrefingreGymControl.Api.Domain.Resources;
using TrefingreGymControl.Api.Domain.Payments;

namespace TrefingreGymControl.Api
{
    public class Program
    {
        private static readonly bool isTestEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test";
        public static void Main(string[] args)
        {
            if (isTestEnv == false)
            {
                Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            }

            try
            {
                Log.Information("Starting application...");
                var builder = WebApplication.CreateBuilder(args);
                ConfigureServices(builder);
                var app = builder.Build();
                ConfigureMiddleware(app);
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
            builder.Logging.ClearProviders().AddSerilog(Log.Logger);
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddFastEndpoints();
            builder.Services.AddHostedService<SubscriptionCancellationBackgroundService>();

            builder.Services.AddSingleton<IAuthorizationHandler, SelfOnlyHandler>();
            builder.Services.AddSingleton<IAuthorizationHandler, SelfOrAdminOnlyHandler>();
            builder.Services.SwaggerDocument();
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<INotificationService, NotificationService>();
            builder.Services.AddTransient<ISubscriptionService, SubscriptionService>();
            builder.Services.AddTransient<IReceiptService, ReceiptService>();
            builder.Services.AddTransient<IResourceService, ResourceService>();
            builder.Services.AddTransient<ISubscriptionTypeService, SubscriptionTypeService>();
            builder.Services.AddTransient<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            builder.Services.Scan(scan => scan
                .FromAssemblyOf<ReceiptRequestedHandler>() // eller typeof(IDomainEvent)
                .AddClasses(classes => classes.AssignableTo(typeof(IEventHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            if (isTestEnv == false)
            {
                builder.Services.AddAuthenticationJwtBearer(opt =>
            {
                opt.SigningKey = "your-super-secret-signing-key-121213131414";
            });
                builder.Services.AddDbContext<TFGymControlDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            }
            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("SelfOnly", p => p.Requirements.Add(new SelfOnlyRequirement()));
                opt.AddPolicy("SelfOrAdminOnly", p => p.Requirements.Add(new SelfOrAdminOnlyRequirement()));
                opt.AddPolicy("UserOrAbove", p=> p.RequireAssertion(ctx => {
                    var role = ctx.User.FindFirst("role")?.Value;
                    if(role != null)
                        return RoleHierarchy.HasAtLeastRole(role, "User");
                    return false;
                }));
                opt.AddPolicy("CoachOrAbove", p=> p.RequireAssertion(ctx => {
                    var role = ctx.User.FindFirst("role")?.Value;
                    if(role != null)
                        return RoleHierarchy.HasAtLeastRole(role, "Coach");
                    return false;
                }));
                opt.AddPolicy("AdminOrAbove", p=> p.RequireAssertion(ctx => {
                    var role = ctx.User.FindFirst("role")?.Value;
                    if(role != null)
                        return RoleHierarchy.HasAtLeastRole(role, "Admin");
                    return false;
                }));
            });
        }

        private static void ConfigureMiddleware(WebApplication app)
        {
            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseFastEndpoints().UseSwaggerGen();
        }
    }
}
