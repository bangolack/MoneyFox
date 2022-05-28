﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MoneyFox.Infrastructure.Persistence;

namespace MoneyFox.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.17");

            modelBuilder.Entity("MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("CurrentBalance")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeactivated")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsExcluded")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ChargedAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsCleared")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsRecurring")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.Property<int?>("RecurringPaymentId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("TargetAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ChargedAccountId");

                    b.HasIndex("RecurringPaymentId");

                    b.HasIndex("TargetAccountId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("MoneyFox.Core.ApplicationCore.Domain.Aggregates.BudgetAggregate.Budget", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("IncludedCategories")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("SpendingLimit")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Budgets");
                });

            modelBuilder.Entity("MoneyFox.Core.ApplicationCore.Domain.Aggregates.CategoryAggregate.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.Property<bool>("RequireNote")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("MoneyFox.Core.ApplicationCore.Domain.Aggregates.RecurringPayment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ChargedAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsEndless")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsLastDayOfMonth")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastRecurrenceCreated")
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.Property<int>("Recurrence")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("TargetAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ChargedAccountId");

                    b.HasIndex("TargetAccountId");

                    b.ToTable("RecurringPayments");
                });

            modelBuilder.Entity("MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate.Payment", b =>
                {
                    b.HasOne("MoneyFox.Core.ApplicationCore.Domain.Aggregates.CategoryAggregate.Category", "Category")
                        .WithMany("Payments")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate.Account", "ChargedAccount")
                        .WithMany()
                        .HasForeignKey("ChargedAccountId");

                    b.HasOne("MoneyFox.Core.ApplicationCore.Domain.Aggregates.RecurringPayment", "RecurringPayment")
                        .WithMany("RelatedPayments")
                        .HasForeignKey("RecurringPaymentId");

                    b.HasOne("MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate.Account", "TargetAccount")
                        .WithMany()
                        .HasForeignKey("TargetAccountId");

                    b.Navigation("Category");

                    b.Navigation("ChargedAccount");

                    b.Navigation("RecurringPayment");

                    b.Navigation("TargetAccount");
                });

            modelBuilder.Entity("MoneyFox.Core.ApplicationCore.Domain.Aggregates.RecurringPayment", b =>
                {
                    b.HasOne("MoneyFox.Core.ApplicationCore.Domain.Aggregates.CategoryAggregate.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate.Account", "ChargedAccount")
                        .WithMany()
                        .HasForeignKey("ChargedAccountId");

                    b.HasOne("MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate.Account", "TargetAccount")
                        .WithMany()
                        .HasForeignKey("TargetAccountId");

                    b.Navigation("Category");

                    b.Navigation("ChargedAccount");

                    b.Navigation("TargetAccount");
                });

            modelBuilder.Entity("MoneyFox.Core.ApplicationCore.Domain.Aggregates.CategoryAggregate.Category", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("MoneyFox.Core.ApplicationCore.Domain.Aggregates.RecurringPayment", b =>
                {
                    b.Navigation("RelatedPayments");
                });
#pragma warning restore 612, 618
        }
    }
}
