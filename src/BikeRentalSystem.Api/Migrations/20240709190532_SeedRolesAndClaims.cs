using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BikeRentalSystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedRolesAndClaims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            -- Criando sequências se não existirem
            CREATE SEQUENCE IF NOT EXISTS public.""AspNetRoleClaims_Id_seq"" START 1;
            CREATE SEQUENCE IF NOT EXISTS public.""AspNetRoles_Id_seq"" START 1;
            CREATE SEQUENCE IF NOT EXISTS public.""AspNetUserClaims_Id_seq"" START 1;
            CREATE SEQUENCE IF NOT EXISTS public.""AspNetUsers_Id_seq"" START 1;

            -- Inserindo roles
            INSERT INTO public.""AspNetRoles"" (""Id"", ""Name"", ""NormalizedName"", ""ConcurrencyStamp"")
            VALUES
              (nextval('public.""AspNetRoles_Id_seq""'), 'Courier', 'COURIER', 'stamp-courier'),
              (nextval('public.""AspNetRoles_Id_seq""'), 'Motorcycle', 'MOTORCYCLE', 'stamp-motorcycle'),
              (nextval('public.""AspNetRoles_Id_seq""'), 'Rental', 'RENTAL', 'stamp-rental');

            -- Inserindo claims para roles
            INSERT INTO public.""AspNetRoleClaims"" (""Id"", ""RoleId"", ""ClaimType"", ""ClaimValue"")
            VALUES
              (nextval('public.""AspNetRoleClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetRoles"" WHERE ""Name"" = 'Courier'), 'Courier', 'Get'),
              (nextval('public.""AspNetRoleClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetRoles"" WHERE ""Name"" = 'Courier'), 'Courier', 'Add'),
              (nextval('public.""AspNetRoleClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetRoles"" WHERE ""Name"" = 'Courier'), 'Courier', 'Update'),
              (nextval('public.""AspNetRoleClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetRoles"" WHERE ""Name"" = 'Courier'), 'Courier', 'Delete'),

              (nextval('public.""AspNetRoleClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetRoles"" WHERE ""Name"" = 'Motorcycle'), 'Motorcycle', 'Get'),
              (nextval('public.""AspNetRoleClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetRoles"" WHERE ""Name"" = 'Motorcycle'), 'Motorcycle', 'Add'),
              (nextval('public.""AspNetRoleClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetRoles"" WHERE ""Name"" = 'Motorcycle'), 'Motorcycle', 'Update'),
              (nextval('public.""AspNetRoleClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetRoles"" WHERE ""Name"" = 'Motorcycle'), 'Motorcycle', 'Delete'),

              (nextval('public.""AspNetRoleClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetRoles"" WHERE ""Name"" = 'Rental'), 'Rental', 'Get'),
              (nextval('public.""AspNetRoleClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetRoles"" WHERE ""Name"" = 'Rental'), 'Rental', 'Add'),
              (nextval('public.""AspNetRoleClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetRoles"" WHERE ""Name"" = 'Rental'), 'Rental', 'Update'),
              (nextval('public.""AspNetRoleClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetRoles"" WHERE ""Name"" = 'Rental'), 'Rental', 'Delete');

            -- Inserindo usuário padrão e associando claims e roles
            INSERT INTO public.""AspNetUsers"" (""Id"", ""UserName"", ""NormalizedUserName"", ""Email"", ""NormalizedEmail"", ""EmailConfirmed"", ""PasswordHash"", ""SecurityStamp"", ""ConcurrencyStamp"", ""PhoneNumberConfirmed"", ""TwoFactorEnabled"", ""LockoutEnabled"", ""AccessFailedCount"")
            VALUES
              (nextval('public.""AspNetUsers_Id_seq""'), 'admin@example.com', 'ADMIN@EXAMPLE.COM', 'admin@example.com', 'ADMIN@EXAMPLE.COM', true, 'AQAAAAEAACcQAAAAEAtW5zBtC/5+eQ6xQaLTMCe8VZiYY+a+R4jBIl3G1sZEtbG0C65ufc+g4OJeQiNLdA==', 'stamp', 'stamp', false, false, true, 0);

            -- Associando usuário com roles
            INSERT INTO public.""AspNetUserRoles"" (""UserId"", ""RoleId"")
            VALUES
              ((SELECT ""Id"" FROM public.""AspNetUsers"" WHERE ""UserName"" = 'admin@example.com'), (SELECT ""Id"" FROM public.""AspNetRoles"" WHERE ""Name"" = 'Courier')),
              ((SELECT ""Id"" FROM public.""AspNetUsers"" WHERE ""UserName"" = 'admin@example.com'), (SELECT ""Id"" FROM public.""AspNetRoles"" WHERE ""Name"" = 'Motorcycle')),
              ((SELECT ""Id"" FROM public.""AspNetUsers"" WHERE ""UserName"" = 'admin@example.com'), (SELECT ""Id"" FROM public.""AspNetRoles"" WHERE ""Name"" = 'Rental'));

            -- Associando claims com usuário
            INSERT INTO public.""AspNetUserClaims"" (""Id"", ""UserId"", ""ClaimType"", ""ClaimValue"")
            VALUES
              (nextval('public.""AspNetUserClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetUsers"" WHERE ""UserName"" = 'admin@example.com'), 'Courier', 'Get'),
              (nextval('public.""AspNetUserClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetUsers"" WHERE ""UserName"" = 'admin@example.com'), 'Courier', 'Add'),
              (nextval('public.""AspNetUserClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetUsers"" WHERE ""UserName"" = 'admin@example.com'), 'Courier', 'Update'),
              (nextval('public.""AspNetUserClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetUsers"" WHERE ""UserName"" = 'admin@example.com'), 'Courier', 'Delete'),

              (nextval('public.""AspNetUserClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetUsers"" WHERE ""UserName"" = 'admin@example.com'), 'Motorcycle', 'Get'),
              (nextval('public.""AspNetUserClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetUsers"" WHERE ""UserName"" = 'admin@example.com'), 'Motorcycle', 'Add'),
              (nextval('public.""AspNetUserClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetUsers"" WHERE ""UserName"" = 'admin@example.com'), 'Motorcycle', 'Update'),
              (nextval('public.""AspNetUserClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetUsers"" WHERE ""UserName"" = 'admin@example.com'), 'Motorcycle', 'Delete'),

              (nextval('public.""AspNetUserClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetUsers"" WHERE ""UserName"" = 'admin@example.com'), 'Rental', 'Get'),
              (nextval('public.""AspNetUserClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetUsers"" WHERE ""UserName"" = 'admin@example.com'), 'Rental', 'Add'),
              (nextval('public.""AspNetUserClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetUsers"" WHERE ""UserName"" = 'admin@example.com'), 'Rental', 'Update'),
              (nextval('public.""AspNetUserClaims_Id_seq""'), (SELECT ""Id"" FROM public.""AspNetUsers"" WHERE ""UserName"" = 'admin@example.com'), 'Rental', 'Delete');
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DELETE FROM public.""AspNetRoleClaims"" WHERE ""RoleId"" IN ('role-courier-id', 'role-motorcycle-id', 'role-rental-id');
            DELETE FROM public.""AspNetRoles"" WHERE ""Id"" IN ('role-courier-id', 'role-motorcycle-id', 'role-rental-id');
            DELETE FROM public.""AspNetUserClaims"" WHERE ""UserId"" = 'user-admin-id';
            DELETE FROM public.""AspNetUserRoles"" WHERE ""UserId"" = 'user-admin-id';
            DELETE FROM public.""AspNetUsers"" WHERE ""Id"" = 'user-admin-id';)");
        }
    }
}
