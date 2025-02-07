﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Selah.Infrastructure;

#nullable disable

namespace Selah.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250207050158_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Selah.Core.Models.Sql.AccountConnector.AccountConnectorSql", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasColumnOrder(0);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<Guid>("AppLastChangedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("app_last_changed_by");

                    b.Property<DateTimeOffset>("DateConnected")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_connected");

                    b.Property<string>("EncryptedAccessToken")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("encrypted_access_token");

                    b.Property<string>("ExternalEventId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("external_event_id");

                    b.Property<string>("InstitutionId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("institution_id");

                    b.Property<string>("InstitutionName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("institution_name");

                    b.Property<DateTimeOffset>("LastUpdate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_update");

                    b.Property<DateTimeOffset>("OriginalInsert")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("original_insert");

                    b.Property<string>("TransactionSyncCursor")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("transaction_sync_cursor");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_account_connectors");

                    b.ToTable("account_connectors");
                });

            modelBuilder.Entity("Selah.Core.Models.Sql.ApplicationUser.ApplicationUserSql", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid")
                        .HasColumnName("account_id");

                    b.Property<Guid>("AppLastChangedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("app_last_changed_by");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<string>("EmailHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email_hash");

                    b.Property<bool>("EmailVerified")
                        .HasColumnType("boolean")
                        .HasColumnName("email_verified");

                    b.Property<string>("EncryptedEmail")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("encrypted_email");

                    b.Property<string>("EncryptedName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("encrypted_name");

                    b.Property<string>("EncryptedPhone")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("encrypted_phone");

                    b.Property<DateTimeOffset>("LastLogin")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_login");

                    b.Property<string>("LastLoginIp")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_login_ip");

                    b.Property<DateTimeOffset>("LastUpdate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_update");

                    b.Property<DateTimeOffset>("OriginalInsert")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("original_insert");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<bool>("PhoneVerified")
                        .HasColumnType("boolean")
                        .HasColumnName("phone_verified");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pk_application_users");

                    b.ToTable("application_users");
                });

            modelBuilder.Entity("Selah.Core.Models.Sql.FinancialAccount.FinancialAccountSql", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasColumnOrder(0);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("AccountMask")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("account_mask");

                    b.Property<Guid>("AppLastChangedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("app_last_changed_by");

                    b.Property<long>("ConnectorId")
                        .HasColumnType("bigint")
                        .HasColumnName("connector_id");

                    b.Property<decimal>("CurrentBalance")
                        .HasColumnType("numeric")
                        .HasColumnName("current_balance");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("display_name");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("external_id");

                    b.Property<bool>("IsExternalApiImport")
                        .HasColumnType("boolean")
                        .HasColumnName("is_external_api_import");

                    b.Property<DateTimeOffset>("LastApiImportTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_api_import_time");

                    b.Property<DateTimeOffset>("LastUpdate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_update");

                    b.Property<string>("OfficialName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("official_name");

                    b.Property<DateTimeOffset>("OriginalInsert")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("original_insert");

                    b.Property<string>("Subtype")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("subtype");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_financial_accounts");

                    b.ToTable("financial_accounts");
                });

            modelBuilder.Entity("Selah.Core.Models.Sql.UserAccount.UserAccountSql", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("account_name");

                    b.Property<Guid>("AppLastChangedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("app_last_changed_by");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on");

                    b.Property<DateTimeOffset>("LastUpdate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_update");

                    b.Property<DateTimeOffset>("OriginalInsert")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("original_insert");

                    b.HasKey("Id")
                        .HasName("pk_user_accounts");

                    b.ToTable("user_accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
