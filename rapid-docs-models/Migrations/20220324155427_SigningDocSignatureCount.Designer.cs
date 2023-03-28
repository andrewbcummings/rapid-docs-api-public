﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using rapid_docs_models.DataAccess;

#nullable disable

namespace rapid_docs_models.Migrations
{
    [DbContext(typeof(VidaDocsDbContext))]
    [Migration("20220324155427_SigningDocSignatureCount")]
    partial class SigningDocSignatureCount
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("rapid_docs_models.DbModels.CustomerAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.HasKey("Id");

                    b.ToTable("CustomerAccounts");
                });

            modelBuilder.Entity("rapid_docs_models.DbModels.InputField", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DefaultValue")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<bool>("Required")
                        .HasColumnType("bit");

                    b.Property<int?>("SigningFormPageId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("SigningFormPageId");

                    b.ToTable("InputFields");
                });

            modelBuilder.Entity("rapid_docs_models.DbModels.InputOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("InputFieldId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("InputFieldId");

                    b.ToTable("InputOptions");
                });

            modelBuilder.Entity("rapid_docs_models.DbModels.Signing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ApiVersion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CustomerAccountId")
                        .HasColumnType("int");

                    b.Property<int>("DateCreated")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateLastOpened")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateSent")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmailToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsTemplate")
                        .HasColumnType("bit");

                    b.Property<int>("NumberOfTimesOpened")
                        .HasColumnType("int");

                    b.Property<int?>("SigningFormId")
                        .HasColumnType("int");

                    b.Property<string>("TemplateName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("UserHasStarted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CustomerAccountId");

                    b.HasIndex("SigningFormId");

                    b.ToTable("Signings");
                });

            modelBuilder.Entity("rapid_docs_models.DbModels.SigningDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileContentType")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FileExtension")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("FileGuid")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FileSize")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("FileUrl")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<bool>("IsTemplate")
                        .HasColumnType("bit");

                    b.Property<int?>("NumberOfSignatures")
                        .HasColumnType("int");

                    b.Property<string>("SignerGuid")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SignerIpAddress")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("SigningId")
                        .HasColumnType("int");

                    b.Property<int>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("SigningId");

                    b.ToTable("SigningDocuments");
                });

            modelBuilder.Entity("rapid_docs_models.DbModels.SigningForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("SigningForms");
                });

            modelBuilder.Entity("rapid_docs_models.DbModels.SigningFormPage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int>("PageOrder")
                        .HasColumnType("int");

                    b.Property<int?>("SigningFormId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("SigningFormId");

                    b.ToTable("SigningFormPages");
                });

            modelBuilder.Entity("rapid_docs_models.DbModels.SigningRecipientMapping", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPreFilled")
                        .HasColumnType("bit");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("SigningId")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<long?>("UserId")
                        .IsRequired()
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("SigningId");

                    b.HasIndex("UserId");

                    b.ToTable("SigningRecipientMappings");
                });

            modelBuilder.Entity("rapid_docs_models.DbModels.Thumbnail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FileExtension")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<Guid>("FileGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FileUrl")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int>("SigningId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SigningId")
                        .IsUnique();

                    b.ToTable("Thumbnail");
                });

            modelBuilder.Entity("rapid_docs_models.DbModels.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CustomerAccountId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NickName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("ProfilePicture")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SocialId")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerAccountId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("rapid_docs_models.DbModels.InputField", b =>
                {
                    b.HasOne("rapid_docs_models.DbModels.SigningFormPage", null)
                        .WithMany("InputFields")
                        .HasForeignKey("SigningFormPageId");
                });

            modelBuilder.Entity("rapid_docs_models.DbModels.InputOption", b =>
                {
                    b.HasOne("rapid_docs_models.DbModels.InputField", null)
                        .WithMany("Options")
                        .HasForeignKey("InputFieldId");
                });

            modelBuilder.Entity("rapid_docs_models.DbModels.Signing", b =>
                {
                    b.HasOne("rapid_docs_models.DbModels.CustomerAccount", null)
                        .WithMany("Signings")
                        .HasForeignKey("CustomerAccountId");

                    b.HasOne("rapid_docs_models.DbModels.SigningForm", "SigningForm")
                        .WithMany()
                        .HasForeignKey("SigningFormId");

                    b.Navigation("SigningForm");
                });

            modelBuilder.Entity("rapid_docs_models.DbModels.SigningDocument", b =>
                {
                    b.HasOne("rapid_docs_models.DbModels.Signing", "Signing")
                        .WithMany("Documents")
                        .HasForeignKey("SigningId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Signing");
                });

            modelBuilder.Entity("rapid_docs_models.DbModels.SigningFormPage", b =>
                {
                    b.HasOne("rapid_docs_models.DbModels.SigningForm", null)
                        .WithMany("SigningFormPages")
                        .HasForeignKey("SigningFormId");
                });

            modelBuilder.Entity("rapid_docs_models.DbModels.SigningRecipientMapping", b =>
                {
                    b.HasOne("rapid_docs_models.DbModels.Signing", "Signing")
                        .WithMany()
                        .HasForeignKey("SigningId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("rapid_docs_models.DbModels.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Signing");

                    b.Navigation("User");
                });

            modelBuilder.Entity("rapid_docs_models.DbModels.Thumbnail", b =>
                {
                    b.HasOne("rapid_docs_models.DbModels.Signing", "Signing")
                        .WithOne("Thumbnail")
                        .HasForeignKey("rapid_docs_models.DbModels.Thumbnail", "SigningId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Signing");
                });

            modelBuilder.Entity("rapid_docs_models.DbModels.User", b =>
                {
                    b.HasOne("rapid_docs_models.DbModels.CustomerAccount", null)
                        .WithMany("Users")
                        .HasForeignKey("CustomerAccountId");
                });

            modelBuilder.Entity("rapid_docs_models.DbModels.CustomerAccount", b =>
                {
                    b.Navigation("Signings");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("rapid_docs_models.DbModels.InputField", b =>
                {
                    b.Navigation("Options");
                });

            modelBuilder.Entity("rapid_docs_models.DbModels.Signing", b =>
                {
                    b.Navigation("Documents");

                    b.Navigation("Thumbnail")
                        .IsRequired();
                });

            modelBuilder.Entity("rapid_docs_models.DbModels.SigningForm", b =>
                {
                    b.Navigation("SigningFormPages");
                });

            modelBuilder.Entity("rapid_docs_models.DbModels.SigningFormPage", b =>
                {
                    b.Navigation("InputFields");
                });
#pragma warning restore 612, 618
        }
    }
}
