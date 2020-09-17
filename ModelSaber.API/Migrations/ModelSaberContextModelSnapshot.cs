﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ModelSaber.API;
using ModelSaber.Common;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ModelSaber.API.Migrations
{
    [DbContext(typeof(ModelSaberContext))]
    partial class ModelSaberContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.0-rc.1.20451.13");

            modelBuilder.Entity("ModelPlaylist", b =>
                {
                    b.Property<Guid>("ModelsId")
                        .HasColumnType("uuid")
                        .HasColumnName("models_id");

                    b.Property<Guid>("PlaylistsId")
                        .HasColumnType("uuid")
                        .HasColumnName("playlists_id");

                    b.HasKey("ModelsId", "PlaylistsId")
                        .HasName("pk_model_playlist");

                    b.HasIndex("PlaylistsId")
                        .HasDatabaseName("ix_model_playlist_playlists_id");

                    b.ToTable("model_playlist");
                });

            modelBuilder.Entity("ModelSaber.API.Models.Audit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Action")
                        .HasColumnType("text")
                        .HasColumnName("action");

                    b.Property<string>("Source")
                        .HasColumnType("text")
                        .HasColumnName("source");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("time");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_audits");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_audits_user_id");

                    b.ToTable("audits");
                });

            modelBuilder.Entity("ModelSaber.Common.Collection", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("DefaultApprovalStatus")
                        .HasColumnType("integer")
                        .HasColumnName("default_approval_status");

                    b.Property<string>("DefaultInstallPath")
                        .HasColumnType("text")
                        .HasColumnName("default_install_path");

                    b.Property<int>("DefaultVisibility")
                        .HasColumnType("integer")
                        .HasColumnName("default_visibility");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("IconURL")
                        .HasColumnType("text")
                        .HasColumnName("icon_url");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_collections");

                    b.ToTable("collections");
                });

            modelBuilder.Entity("ModelSaber.Common.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("CommenterId")
                        .HasColumnType("uuid")
                        .HasColumnName("commenter_id");

                    b.Property<string>("Message")
                        .HasColumnType("text")
                        .HasColumnName("message");

                    b.Property<Guid>("Source")
                        .HasColumnType("uuid")
                        .HasColumnName("source");

                    b.HasKey("Id")
                        .HasName("pk_comments");

                    b.HasIndex("CommenterId")
                        .HasDatabaseName("ix_comments_commenter_id");

                    b.ToTable("comments");
                });

            modelBuilder.Entity("ModelSaber.Common.Model", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("CollectionId")
                        .HasColumnType("uuid")
                        .HasColumnName("collection_id");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int>("DownloadCount")
                        .HasColumnType("integer")
                        .HasColumnName("download_count");

                    b.Property<string>("DownloadURL")
                        .HasColumnType("text")
                        .HasColumnName("download_url");

                    b.Property<int>("FileType")
                        .HasColumnType("integer")
                        .HasColumnName("file_type");

                    b.Property<string>("Hash")
                        .HasColumnType("text")
                        .HasColumnName("hash");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<string[]>("Tags")
                        .HasColumnType("text[]")
                        .HasColumnName("tags");

                    b.Property<string>("ThumbnailURL")
                        .HasColumnType("text")
                        .HasColumnName("thumbnail_url");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("upload_date");

                    b.Property<Guid?>("UploaderId")
                        .HasColumnType("uuid")
                        .HasColumnName("uploader_id");

                    b.Property<int>("Visibility")
                        .HasColumnType("integer")
                        .HasColumnName("visibility");

                    b.HasKey("Id")
                        .HasName("pk_models");

                    b.HasIndex("CollectionId")
                        .HasDatabaseName("ix_models_collection_id");

                    b.HasIndex("UploaderId")
                        .HasDatabaseName("ix_models_uploader_id");

                    b.ToTable("models");
                });

            modelBuilder.Entity("ModelSaber.Common.Playlist", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int>("DownloadCount")
                        .HasColumnType("integer")
                        .HasColumnName("download_count");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("ThumbnailURL")
                        .HasColumnType("text")
                        .HasColumnName("thumbnail_url");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_playlists");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_playlists_user_id");

                    b.ToTable("playlists");
                });

            modelBuilder.Entity("ModelSaber.Common.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Biography")
                        .HasColumnType("text")
                        .HasColumnName("biography");

                    b.Property<DiscordUser>("Profile")
                        .HasColumnType("jsonb")
                        .HasColumnName("profile");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users");
                });

            modelBuilder.Entity("ModelSaber.Common.Vote", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("IsUpvote")
                        .HasColumnType("boolean")
                        .HasColumnName("is_upvote");

                    b.Property<Guid>("Source")
                        .HasColumnType("uuid")
                        .HasColumnName("source");

                    b.Property<Guid?>("VoterId")
                        .HasColumnType("uuid")
                        .HasColumnName("voter_id");

                    b.HasKey("Id")
                        .HasName("pk_votes");

                    b.HasIndex("VoterId")
                        .HasDatabaseName("ix_votes_voter_id");

                    b.ToTable("votes");
                });

            modelBuilder.Entity("ModelPlaylist", b =>
                {
                    b.HasOne("ModelSaber.Common.Model", null)
                        .WithMany()
                        .HasForeignKey("ModelsId")
                        .HasConstraintName("fk_model_playlist_models_models_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ModelSaber.Common.Playlist", null)
                        .WithMany()
                        .HasForeignKey("PlaylistsId")
                        .HasConstraintName("fk_model_playlist_playlists_playlists_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ModelSaber.API.Models.Audit", b =>
                {
                    b.HasOne("ModelSaber.Common.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_audits_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ModelSaber.Common.Comment", b =>
                {
                    b.HasOne("ModelSaber.Common.User", "Commenter")
                        .WithMany()
                        .HasForeignKey("CommenterId")
                        .HasConstraintName("fk_comments_users_commenter_id");

                    b.Navigation("Commenter");
                });

            modelBuilder.Entity("ModelSaber.Common.Model", b =>
                {
                    b.HasOne("ModelSaber.Common.Collection", "Collection")
                        .WithMany()
                        .HasForeignKey("CollectionId")
                        .HasConstraintName("fk_models_collections_collection_id");

                    b.HasOne("ModelSaber.Common.User", "Uploader")
                        .WithMany()
                        .HasForeignKey("UploaderId")
                        .HasConstraintName("fk_models_users_uploader_id");

                    b.Navigation("Collection");

                    b.Navigation("Uploader");
                });

            modelBuilder.Entity("ModelSaber.Common.Playlist", b =>
                {
                    b.HasOne("ModelSaber.Common.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_playlists_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ModelSaber.Common.Vote", b =>
                {
                    b.HasOne("ModelSaber.Common.User", "Voter")
                        .WithMany()
                        .HasForeignKey("VoterId")
                        .HasConstraintName("fk_votes_users_voter_id");

                    b.Navigation("Voter");
                });
#pragma warning restore 612, 618
        }
    }
}
