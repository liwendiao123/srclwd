﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Senparc.Core.Models;

namespace Senparc.Web.Migrations
{
    [DbContext(typeof(SenparcEntities))]
    [Migration("20190908085657_UpdateTableRelateV1_2")]
    partial class UpdateTableRelateV1_2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Senparc.Core.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Address");

                    b.Property<decimal>("Balance");

                    b.Property<string>("City")
                        .HasMaxLength(30);

                    b.Property<string>("Country")
                        .HasMaxLength(30);

                    b.Property<string>("District");

                    b.Property<string>("Email");

                    b.Property<bool?>("EmailChecked");

                    b.Property<bool>("Flag")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("HeadImgUrl");

                    b.Property<string>("LastLoginIP");

                    b.Property<DateTime>("LastLoginTime");

                    b.Property<DateTime?>("LastWeixinSignInTime");

                    b.Property<decimal>("LockMoney");

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Note");

                    b.Property<decimal>("Package");

                    b.Property<string>("Password")
                        .HasMaxLength(100);

                    b.Property<string>("PasswordSalt")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("Phone")
                        .HasMaxLength(20);

                    b.Property<bool?>("PhoneChecked");

                    b.Property<string>("PicUrl")
                        .HasMaxLength(300)
                        .IsUnicode(false);

                    b.Property<decimal>("Points");

                    b.Property<string>("Province")
                        .HasMaxLength(20);

                    b.Property<string>("QQ");

                    b.Property<string>("RealName")
                        .HasMaxLength(100);

                    b.Property<byte>("Sex");

                    b.Property<string>("ThisLoginIp")
                        .HasColumnName("ThisLoginIP")
                        .HasMaxLength(30)
                        .IsUnicode(false);

                    b.Property<DateTime>("ThisLoginTime")
                        .HasColumnType("datetime");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("WeixinOpenId");

                    b.Property<int>("WeixinSignTimes");

                    b.Property<string>("WeixinUnionId");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Senparc.Core.Models.AccountPayLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId");

                    b.Property<string>("AddIp")
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("AddTime")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("CompleteTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(250)");

                    b.Property<decimal>("Fee")
                        .HasColumnType("money");

                    b.Property<decimal>("GetPoints")
                        .HasColumnType("money");

                    b.Property<string>("OrderNumber")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<int>("OrderType");

                    b.Property<decimal>("PayMoney")
                        .HasColumnType("money");

                    b.Property<string>("PayParam");

                    b.Property<int>("PayType");

                    b.Property<string>("PrepayId")
                        .HasColumnType("varchar(100)");

                    b.Property<decimal>("Price")
                        .HasColumnType("money");

                    b.Property<byte>("Status");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("money");

                    b.Property<string>("TradeNumber")
                        .HasColumnType("varchar(150)");

                    b.Property<byte?>("Type");

                    b.Property<decimal?>("UsedPoints")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("AccountPayLogs");
                });

            modelBuilder.Entity("Senparc.Core.Models.AdminUserInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddTime")
                        .HasColumnType("datetime");

                    b.Property<string>("LastLoginIp")
                        .HasColumnName("LastLoginIP")
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.Property<DateTime>("LastLoginTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Note");

                    b.Property<string>("Password")
                        .HasMaxLength(50);

                    b.Property<string>("PasswordSalt")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("Phone")
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.Property<string>("RealName")
                        .HasMaxLength(50);

                    b.Property<string>("ThisLoginIp")
                        .HasColumnName("ThisLoginIP")
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.Property<DateTime>("ThisLoginTime")
                        .HasColumnType("datetime");

                    b.Property<string>("UserName")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("AdminUserInfos");
                });

            modelBuilder.Entity("Senparc.Core.Models.DataBaseModel.Activity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<string>("CoverUrl")
                        .IsUnicode(false);

                    b.Property<string>("Description");

                    b.Property<bool>("Flag");

                    b.Property<bool>("IsPublish");

                    b.Property<DateTime>("IssueTime");

                    b.Property<string>("Summary");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("Senparc.Core.Models.DataBaseModel.CompetitionProgram", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BdImgUrl")
                        .IsUnicode(false);

                    b.Property<string>("BdImgUrlPwd");

                    b.Property<int>("Cate");

                    b.Property<string>("Company");

                    b.Property<string>("ControlId");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("CreatorId");

                    b.Property<string>("CreatorName");

                    b.Property<string>("Desc");

                    b.Property<bool>("Flag");

                    b.Property<string>("ImgUrl")
                        .IsUnicode(false);

                    b.Property<string>("Name");

                    b.Property<string>("Remark");

                    b.Property<string>("ScheduleId");

                    b.Property<string>("SignNum");

                    b.Property<int>("Status");

                    b.Property<DateTime>("UpdateTime");

                    b.Property<string>("UpdatorId");

                    b.Property<string>("UpdatorName");

                    b.HasKey("Id");

                    b.HasIndex("ScheduleId");

                    b.ToTable("CompetitionPrograms");
                });

            modelBuilder.Entity("Senparc.Core.Models.DataBaseModel.ProjectMember", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Company");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Duty");

                    b.Property<string>("Email");

                    b.Property<bool>("Flag");

                    b.Property<int>("Gender");

                    b.Property<string>("HeadImgUrl");

                    b.Property<string>("IdCard");

                    b.Property<string>("IdCardImgUrl");

                    b.Property<bool>("IsLeader");

                    b.Property<string>("Name");

                    b.Property<string>("Nation");

                    b.Property<string>("Phone");

                    b.Property<string>("ProjectId");

                    b.Property<int>("Sort");

                    b.Property<DateTime>("UpdateTime");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectMembers");
                });

            modelBuilder.Entity("Senparc.Core.Models.DataBaseModel.Schedule", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActivityId");

                    b.Property<string>("Address");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Creator");

                    b.Property<string>("CreatorId");

                    b.Property<string>("Desc");

                    b.Property<DateTime>("EndTime");

                    b.Property<bool>("Flag");

                    b.Property<string>("Name");

                    b.Property<string>("Remark");

                    b.Property<string>("SignNumber");

                    b.Property<int>("Sort");

                    b.Property<DateTime>("StartTime");

                    b.Property<DateTime>("UpdateTime");

                    b.Property<string>("UpdatorId");

                    b.Property<string>("UpdatorName");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("Senparc.Core.Models.FeedBack", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId");

                    b.Property<DateTime>("AddTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Content");

                    b.Property<bool>("Flag");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("FeedBacks");
                });

            modelBuilder.Entity("Senparc.Core.Models.PointsLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId");

                    b.Property<int?>("AccountPayLogId");

                    b.Property<DateTime>("AddTime")
                        .HasColumnType("datetime");

                    b.Property<decimal>("AfterPoints")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("BeforePoints")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("Description");

                    b.Property<decimal>("Points")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("AccountPayLogId");

                    b.ToTable("PointsLogs");
                });

            modelBuilder.Entity("Senparc.Core.Models.SystemConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("MchId")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("MchKey")
                        .HasColumnType("varchar(300)");

                    b.Property<string>("SystemName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("TenPayAppId")
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("SystemConfigs");
                });

            modelBuilder.Entity("Senparc.Core.Models.AccountPayLog", b =>
                {
                    b.HasOne("Senparc.Core.Models.Account", "Account")
                        .WithMany("AccountPayLogs")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Senparc.Core.Models.DataBaseModel.CompetitionProgram", b =>
                {
                    b.HasOne("Senparc.Core.Models.DataBaseModel.Schedule", "Schedule")
                        .WithMany("CompetitionPrograms")
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Senparc.Core.Models.DataBaseModel.ProjectMember", b =>
                {
                    b.HasOne("Senparc.Core.Models.DataBaseModel.CompetitionProgram", "CompetitionProgram")
                        .WithMany("ProjectMembers")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Senparc.Core.Models.DataBaseModel.Schedule", b =>
                {
                    b.HasOne("Senparc.Core.Models.DataBaseModel.Activity", "Activity")
                        .WithMany("Schedules")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Senparc.Core.Models.FeedBack", b =>
                {
                    b.HasOne("Senparc.Core.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Senparc.Core.Models.PointsLog", b =>
                {
                    b.HasOne("Senparc.Core.Models.Account", "Account")
                        .WithMany("PointsLogs")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Senparc.Core.Models.AccountPayLog", "AccountPayLog")
                        .WithMany("PointsLogs")
                        .HasForeignKey("AccountPayLogId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
