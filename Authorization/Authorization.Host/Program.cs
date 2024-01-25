using System;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using Authorization.Application.Controllers;
using Authorization.Application.Extensions;
using Authorization.Application.Handlers.Token;
using Authorization.Application.Mapping;
using Authorization.Application.Models.Responses;
using Authorization.Application.Services;
using Authorization.Application.Services.Abstraction;
using Authorization.Domain.Configurations;
using Authorization.Domain.Configurations.Abstractions;
using Authorization.Domain.Contexts;
using Authorization.Domain.Enums;
using Authorization.Domain.Mapping;
using Authorization.Domain.Repositories;
using Authorization.Domain.Repositories.Abstractions;
using Authorization.Domain.Services;
using Authorization.Domain.Services.Abstraction;
using Authorization.Host.Middleware;
using Authorization.Host.Migrations;
using FluentMigrator.Runner;
using FluentMigrator.Runner.VersionTableInfo;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Serilog;
using HttpClient = Authorization.Domain.Services.HttpClient;

const string PersistenceSectionName = "Persistence";
const string TokensSectionName = "Tokens";
const string JwtSectionName = "Jwt";
const string AccountPolicySectionName = "AccountPolicy";
const string EmailSectionName = "Email";
const string SendGridSectionName = "SendGrid";
const string FileServiceSectionName = "FileService";

const string ReferenceSectionName = "Reference";

const string ApplicationXmlFile = "Authorization.Application.xml";

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");
if (builder.Environment.EnvironmentName.Equals("Local"))
{
    builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", false, true);
}

IServiceCollection serviceCollection = builder.Services;
ConfigureServices(serviceCollection, builder);
serviceCollection.AddEndpointsApiExplorer();
serviceCollection.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Reference API" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer",
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
            },
            Array.Empty<string>()
        },
    });

    var xmlPath = Path.Combine(AppContext.BaseDirectory, ApplicationXmlFile);
    option.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
UpdateDatabase(scope.ServiceProvider);

// if (!builder.Environment.IsProduction()) //todo: uncomment it
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<BasicAuthMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Welcome to Reference project!");
app.Run();

void ConfigureServices(IServiceCollection services, WebApplicationBuilder webApplicationBuilder)
{
    services.AddControllers()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.Converters.Add(new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ",
            });
        })
        .ConfigureApiBehaviorOptions(ConfigureFluentValidationResponse)
        .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); })
        .AddApplicationPart(typeof(UsersController).Assembly);

    // Adding all validators from assembly containing following validator
    services.AddFluentValidationAutoValidation();
    services.AddFluentValidationClientsideAdapters();

    services.AddHttpContextAccessor();
    RegisterConfiguration(webApplicationBuilder.Configuration, services, out var persistenceConfiguration);

    services.AddFluentMigratorCore()
        .ConfigureRunner(rb => rb
            .AddSqlServer()
            .WithGlobalConnectionString(persistenceConfiguration.ConnectionString)
            .ScanIn(typeof(Migration001_AddRoleTable).Assembly).For.Migrations())
        .AddTransient<IVersionTableMetaData, CustomVersionTableMetadata>();

    RegisterRepositories(services);
    RegisterServices(services);
    RegisterHandlers(services);

    services.AddDbContext<AuthContext>((sp, options) =>
    {
        var configuration = sp.GetRequiredService<IPersistenceConfiguration>();
        options.UseSqlServer(configuration.ConnectionString);
    });

    services.AddAutoMapper(configAction => configAction.AddProfile(new ApplicationMappingsProfile()), typeof(Program));
    services.AddAutoMapper(configAction => configAction.AddProfile(new DomainMappingsProfile()), typeof(Program));
}

static void RegisterRepositories(IServiceCollection services)
{
    services
        .AddScoped<IClientRepository, ClientRepository>()
        .AddScoped<IRoleRepository, RoleRepository>()
        .AddScoped<IUserRepository, UserRepository>()
        .AddScoped<IUserAuthRepository, UserAuthRepository>()
        .AddScoped<IRoleScopeRepository, RoleScopeRepository>()
        .AddScoped<IRefreshTokenRepository, RefreshTokenRepository>()
        .AddScoped<IConfirmationRequestRepository, ConfirmationRequestRepository>()
        .AddScoped<IUnitOfWork, UnitOfWork>();
}

static void RegisterServices(IServiceCollection services)
{
    services
        .AddScoped<IConfirmationRequestService, ConfirmationRequestService>()
        .AddScoped<IUserService, UserService>()
        .AddScoped<ITokenService, TokenService>()
        .AddScoped<IJwtService, JwtService>()
        .AddScoped<IRegistrationService, RegistrationService>()
        .AddScoped<IRoleService, RoleService>()
        .AddScoped<IHashService, HashService>()
        .AddScoped<IClaimsPrincipalService, ClaimsPrincipalService>()
        .AddScoped<ITimeProviderService, TimeProviderServiceService>()
        .AddScoped<ICookieService, CookieService>()
        .AddScoped<IHttpClient, HttpClient>()
        .AddScoped<IEmailService, EmailService>();
}

static void RegisterHandlers(IServiceCollection services)
{
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IssueTokenHandler>());
}

static void RegisterConfiguration(
    IConfiguration configuration,
    IServiceCollection services,
    out PersistenceConfiguration persistenceConfiguration)
{
    persistenceConfiguration = new PersistenceConfiguration();
    configuration.GetSection(PersistenceSectionName).Bind(persistenceConfiguration);

    var tokensConfiguration = new TokensConfiguration();
    configuration.GetSection(TokensSectionName).Bind(tokensConfiguration);
    configuration.GetSection(TokensSectionName).GetSection(JwtSectionName).Bind(tokensConfiguration.JwtConfiguration);

    var redirectionPathConfiguration = new RedirectionPathConfiguration();
    configuration.GetSection(ReferenceSectionName).Bind(redirectionPathConfiguration);

    var accountPolicyConfiguration = new AccountPolicyConfiguration();
    configuration.GetSection(AccountPolicySectionName).Bind(accountPolicyConfiguration);

    var emailConfiguration = new EmailConfiguration();
    configuration.GetSection(EmailSectionName).Bind(emailConfiguration);

    var sendGridConfiguration = new SendGridConfiguration();
    configuration.GetSection(SendGridSectionName).Bind(sendGridConfiguration);

    var fileServiceConfiguration = new FileServiceConfiguration();
    configuration.GetSection(FileServiceSectionName).Bind(fileServiceConfiguration);

    services
        .AddSingleton<IPersistenceConfiguration>(persistenceConfiguration)
        .AddSingleton<ITokensConfiguration>(tokensConfiguration)
        .AddSingleton<IEmailConfiguration>(emailConfiguration)
        .AddSingleton<ISendGridConfiguration>(sendGridConfiguration)
        .AddSingleton<IRedirectionPathConfiguration>(redirectionPathConfiguration)
        .AddSingleton<IAccountPolicyConfiguration>(accountPolicyConfiguration)
        .AddSingleton(tokensConfiguration.JwtConfiguration)
        .AddSingleton<IFileServiceConfiguration>(fileServiceConfiguration);
}

static void ConfigureFluentValidationResponse(ApiBehaviorOptions options)
{
    options.InvalidModelStateResponseFactory = c =>
    {
        var errors = c.ModelState.Values.Where(v => v.Errors.Count > 0)
            .SelectMany(v => v.Errors)
            .Select(v => v.ErrorMessage);

        var response = new ErrorResponse
        {
            Code = ErrorCode.ValidationFailed.GetDisplayName(),
            Message = string.Join(" ", errors),
        };

        return new BadRequestObjectResult(response);
    };
}

static void UpdateDatabase(IServiceProvider serviceProvider)
{
    var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

    Log.Information("Starting migration...");

    runner.MigrateUp();
    runner.ListMigrations();

    Log.Information("Migration finished!");
}
