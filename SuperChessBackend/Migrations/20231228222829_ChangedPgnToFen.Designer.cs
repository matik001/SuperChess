﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SuperChessBackend.DB;

#nullable disable

namespace SuperChessBackend.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20231228222829_ChangedPgnToFen")]
    partial class ChangedPgnToFen
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GameUser", b =>
                {
                    b.Property<int>("GamesId")
                        .HasColumnType("integer");

                    b.Property<int>("UsersId")
                        .HasColumnType("integer");

                    b.HasKey("GamesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("GameUser");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.Property<int>("RolesId")
                        .HasColumnType("integer");

                    b.Property<int>("UsersId")
                        .HasColumnType("integer");

                    b.HasKey("RolesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("RoleUser");
                });

            modelBuilder.Entity("SuperChessBackend.DB.Repositories.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("GameId");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("GameData")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("GameGuid")
                        .HasColumnType("uuid");

                    b.Property<int>("GameStatus")
                        .HasColumnType("integer");

                    b.Property<int>("GameType")
                        .HasColumnType("integer");

                    b.Property<int?>("RoomId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("game");
                });

            modelBuilder.Entity("SuperChessBackend.DB.Repositories.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("RefreshTokenId");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("refreshtoken");
                });

            modelBuilder.Entity("SuperChessBackend.DB.Repositories.RevokedToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("RevokedTokenId");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("TokenGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("revokedtokens");
                });

            modelBuilder.Entity("SuperChessBackend.DB.Repositories.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("RoleId");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("role");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            RoleName = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            RoleName = "Blocked"
                        });
                });

            modelBuilder.Entity("SuperChessBackend.DB.Repositories.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("RoomId");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RoomName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("room");
                });

            modelBuilder.Entity("SuperChessBackend.DB.Repositories.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("UserId");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("user");
                });

            modelBuilder.Entity("SuperChessBackend.DB.Repositories.UserGame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("UserGameId");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Color")
                        .HasColumnType("integer");

                    b.Property<int>("GameId")
                        .HasColumnType("integer");

                    b.Property<string>("GuestGuid")
                        .HasColumnType("text");

                    b.Property<string>("Nick")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("UserId");

                    b.ToTable("gameuser");
                });

            modelBuilder.Entity("SuperChessBackend.DB.Repositories.UserPassword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("PasswordId");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("PasswordDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PasswordUserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PasswordUserId");

                    b.ToTable("userpassword");
                });

            modelBuilder.Entity("SuperChessBackend.DB.Repositories.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("UserRoleId");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("userrole");
                });

            modelBuilder.Entity("GameUser", b =>
                {
                    b.HasOne("SuperChessBackend.DB.Repositories.Game", null)
                        .WithMany()
                        .HasForeignKey("GamesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SuperChessBackend.DB.Repositories.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.HasOne("SuperChessBackend.DB.Repositories.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SuperChessBackend.DB.Repositories.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SuperChessBackend.DB.Repositories.Game", b =>
                {
                    b.HasOne("SuperChessBackend.DB.Repositories.Room", "Room")
                        .WithMany("Games")
                        .HasForeignKey("RoomId");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("SuperChessBackend.DB.Repositories.RefreshToken", b =>
                {
                    b.HasOne("SuperChessBackend.DB.Repositories.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SuperChessBackend.DB.Repositories.RevokedToken", b =>
                {
                    b.HasOne("SuperChessBackend.DB.Repositories.User", "User")
                        .WithMany("RevokedTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SuperChessBackend.DB.Repositories.UserGame", b =>
                {
                    b.HasOne("SuperChessBackend.DB.Repositories.Game", "Game")
                        .WithMany("UserGames")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SuperChessBackend.DB.Repositories.User", "User")
                        .WithMany("UserGame")
                        .HasForeignKey("UserId");

                    b.Navigation("Game");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SuperChessBackend.DB.Repositories.UserPassword", b =>
                {
                    b.HasOne("SuperChessBackend.DB.Repositories.User", "User")
                        .WithMany("UserPasswords")
                        .HasForeignKey("PasswordUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SuperChessBackend.DB.Repositories.UserRole", b =>
                {
                    b.HasOne("SuperChessBackend.DB.Repositories.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SuperChessBackend.DB.Repositories.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SuperChessBackend.DB.Repositories.Game", b =>
                {
                    b.Navigation("UserGames");
                });

            modelBuilder.Entity("SuperChessBackend.DB.Repositories.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("SuperChessBackend.DB.Repositories.Room", b =>
                {
                    b.Navigation("Games");
                });

            modelBuilder.Entity("SuperChessBackend.DB.Repositories.User", b =>
                {
                    b.Navigation("RefreshTokens");

                    b.Navigation("RevokedTokens");

                    b.Navigation("UserGame");

                    b.Navigation("UserPasswords");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
