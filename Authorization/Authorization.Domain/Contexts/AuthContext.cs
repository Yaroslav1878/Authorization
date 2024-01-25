using Authorization.Domain.Configurations.Abstractions;
using Authorization.Domain.Enums;
using Authorization.Domain.Models.Dtos;
using Authorization.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Subject = Authorization.Domain.Enums.Subject;

namespace Authorization.Domain.Contexts;

public class AuthContext(
        DbContextOptions<AuthContext> contextOptions,
        IPersistenceConfiguration persistenceConfiguration) : DbContext(contextOptions)
{
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<RoleScope> RoleScopes { get; set; }
    public DbSet<Scope> Scopes { get; set; }
    public DbSet<ConfirmationRequest> ConfirmationRequests { get; set; }
    public DbSet<UserAuth> UserAuths { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(persistenceConfiguration.Schema);

        modelBuilder.Entity<Role>().ToTable("role");
        modelBuilder.Entity<Role>().HasKey(role => role.Id);
        modelBuilder.Entity<Role>().Property(role => role.Id).HasColumnName("id");

        modelBuilder.Entity<User>().ToTable("user");
        modelBuilder.Entity<User>().HasKey(user => user.Id);
        modelBuilder.Entity<User>().Property(user => user.Id).HasColumnName("id");
        modelBuilder.Entity<User>().HasIndex(user => user.Email).IsUnique();
        modelBuilder.Entity<User>().Property(user => user.Email).HasColumnName("email");
        modelBuilder.Entity<User>().Property(user => user.FirstName).HasColumnName("first_name");
        modelBuilder.Entity<User>().Property(user => user.LastName).HasColumnName("last_name");
        modelBuilder.Entity<User>().Property(user => user.RoleId).HasColumnName("role_name");
        modelBuilder.Entity<User>().Property(user => user.Status).HasColumnName("status")
            .HasConversion(new EnumToStringConverter<UserStatus>());
        modelBuilder.Entity<User>().Property(user => user.AvatarId).HasColumnName("avatar");
        modelBuilder.Entity<User>().Property(user => user.CreatedAt).HasColumnName("created_at");
        modelBuilder.Entity<User>().Property(user => user.RemovedAt).HasColumnName("removed_at");
        modelBuilder.Entity<User>().HasOne(user => user.Role).WithMany(role => role.Users).HasForeignKey(user => user.RoleId);

        modelBuilder.Entity<UserAuth>().ToTable("user_auth");
        modelBuilder.Entity<UserAuth>().HasKey(userAuth => userAuth.Id);
        modelBuilder.Entity<UserAuth>().Property(userAuth => userAuth.Id).HasColumnName("id");
        modelBuilder.Entity<UserAuth>().Property(userAuth => userAuth.UserId).HasColumnName("user_id");
        modelBuilder.Entity<UserAuth>().Property(userAuth => userAuth.AuthType).HasColumnName("auth_type")
            .HasConversion(new EnumToStringConverter<AuthType>());
        modelBuilder.Entity<UserAuth>().Property(userAuth => userAuth.Subject).HasColumnName("subject")
            .HasConversion(new EnumToStringConverter<Subject>());
        modelBuilder.Entity<UserAuth>().Property(userAuth => userAuth.PasswordHash).HasColumnName("password_hash");
        modelBuilder.Entity<UserAuth>().Property(userAuth => userAuth.Salt).HasColumnName("salt");
        modelBuilder.Entity<UserAuth>().Property(userAuth => userAuth.CreatedAt).HasColumnName("created_at");
        modelBuilder.Entity<UserAuth>().Property(userAuth => userAuth.UpdatedAt).HasColumnName("updated_at");
        modelBuilder.Entity<UserAuth>().HasOne(userAuth => userAuth.User).WithMany(user => user.UserAuths).HasForeignKey(x => x.UserId);

        modelBuilder.Entity<ClientDto>().ToTable("client");
        modelBuilder.Entity<ClientDto>().HasKey(client => client.Id);
        modelBuilder.Entity<ClientDto>().Property(client => client.Id).HasColumnName("id");
        modelBuilder.Entity<ClientDto>().HasIndex(client => client.Name).IsUnique();
        modelBuilder.Entity<ClientDto>().Property(client => client.Name).HasColumnName("name");
        modelBuilder.Entity<ClientDto>().Property(client => client.CreatedAt).HasColumnName("created_at");

        modelBuilder.Entity<RefreshToken>().ToTable("refresh_token");
        modelBuilder.Entity<RefreshToken>().HasKey(refreshToken => refreshToken.Id);
        modelBuilder.Entity<RefreshToken>().Property(refreshToken => refreshToken.Id).HasColumnName("id");
        modelBuilder.Entity<RefreshToken>().Property(refreshToken => refreshToken.ClientId).HasColumnName("client_id");
        modelBuilder.Entity<RefreshToken>().Property(refreshToken => refreshToken.UserId).HasColumnName("user_id");
        modelBuilder.Entity<RefreshToken>().Property(refreshToken => refreshToken.Token).HasColumnName("refresh_token");
        modelBuilder.Entity<RefreshToken>().Property(refreshToken => refreshToken.ExpireAt).HasColumnName("expire_at");
        modelBuilder.Entity<RefreshToken>().Property(refreshToken => refreshToken.CreatedAt).HasColumnName("created_at");
        modelBuilder.Entity<RefreshToken>().Property(refreshToken => refreshToken.RevokedAt).HasColumnName("revoked_at");
        modelBuilder.Entity<RefreshToken>().Property(refreshToken => refreshToken.RevokeReason).HasColumnName("revoke_reason")
            .HasConversion(new EnumToStringConverter<RefreshTokenRevokeReason>());
        modelBuilder.Entity<RefreshToken>().HasOne(refreshToken => refreshToken.Client).WithMany().HasForeignKey(x => x.ClientId);
        modelBuilder.Entity<RefreshToken>().HasOne(refreshToken => refreshToken.User).WithMany().HasForeignKey(x => x.UserId);

        modelBuilder.Entity<RoleScope>().ToTable("role_scope");
        modelBuilder.Entity<RoleScope>().HasKey(roleScope => new { roleScope.RoleId, roleScope.ScopeId });
        modelBuilder.Entity<RoleScope>().Property(roleScope => roleScope.RoleId).HasColumnName("role_id");
        modelBuilder.Entity<RoleScope>().Property(roleScope => roleScope.ScopeId).HasColumnName("scope_id");
        modelBuilder.Entity<RoleScope>().HasOne(roleScope => roleScope.Role).WithMany(role => role.RoleScopes).HasForeignKey(x => x.RoleId);
        modelBuilder.Entity<RoleScope>().HasOne(roleScope => roleScope.Scope).WithMany(scope => scope.RoleScopes).HasForeignKey(x => x.ScopeId);

        modelBuilder.Entity<Scope>().ToTable("scope");
        modelBuilder.Entity<Scope>().HasKey(scope => scope.Id);
        modelBuilder.Entity<Scope>().Property(scope => scope.Id).HasColumnName("id");

        modelBuilder.Entity<ConfirmationRequest>().ToTable("confirmation_request");
        modelBuilder.Entity<ConfirmationRequest>().HasKey(confirmationRequest => confirmationRequest.Id);
        modelBuilder.Entity<ConfirmationRequest>().Property(confirmationRequest => confirmationRequest.Id).HasColumnName("id");
        modelBuilder.Entity<ConfirmationRequest>().Property(confirmationRequest => confirmationRequest.UserId).HasColumnName("user_id");
        modelBuilder.Entity<ConfirmationRequest>().Property(confirmationRequest => confirmationRequest.Subject).HasColumnName("subject")
            .HasConversion(new EnumToStringConverter<ConfirmationRequestSubject>());
        modelBuilder.Entity<ConfirmationRequest>().Property(confirmationRequest => confirmationRequest.ConfirmationType).HasColumnName("confirmation_type")
            .HasConversion(new EnumToStringConverter<ConfirmationRequestType>());
        modelBuilder.Entity<ConfirmationRequest>().Property(confirmationRequest => confirmationRequest.ConfirmedAt).HasColumnName("confirmed_at");
        modelBuilder.Entity<ConfirmationRequest>().Property(confirmationRequest => confirmationRequest.Receiver).HasColumnName("receiver");
        modelBuilder.Entity<ConfirmationRequest>().Property(confirmationRequest => confirmationRequest.RevokedAt).HasColumnName("revoked_at");
        modelBuilder.Entity<ConfirmationRequest>().Property(confirmationRequest => confirmationRequest.CreatedAt).HasColumnName("created_at");
        modelBuilder.Entity<ConfirmationRequest>().Property(confirmationRequest => confirmationRequest.ExpiredAt).HasColumnName("expired_at");
        modelBuilder.Entity<ConfirmationRequest>().HasOne(confirmationRequest => confirmationRequest.User).WithMany(user => user.ConfirmationRequests).HasForeignKey(x => x.UserId);
    }
}
