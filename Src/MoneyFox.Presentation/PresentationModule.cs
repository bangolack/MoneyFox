﻿using System;
using Autofac;
using GenericServices.PublicButHidden;
using GenericServices.Setup;
using Microsoft.EntityFrameworkCore;
using MoneyFox.BusinessLogic;
using MoneyFox.DataLayer;
using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Presentation
{
    public class PresentationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<BusinessLogicModule>();

            SetupContextAndCrudServices(builder);

            builder.RegisterType<CrudServicesAsync>().AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("Service", StringComparison.CurrentCultureIgnoreCase))
                   .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("Facade", StringComparison.CurrentCultureIgnoreCase))
                   .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => !t.Name.StartsWith("DesignTime", StringComparison.CurrentCultureIgnoreCase))
                   .Where(t => t.Name.EndsWith("ViewModel", StringComparison.CurrentCultureIgnoreCase))
                   .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => !t.Name.StartsWith("DesignTime", StringComparison.CurrentCultureIgnoreCase))
                   .Where(t => t.Name.EndsWith("ViewModel", StringComparison.CurrentCultureIgnoreCase))
                   .AsSelf();
        }

        private void SetupContextAndCrudServices(ContainerBuilder builder)
        {
            builder.Register(c => SetupEfContext())
                   .As<DbContext>()
                   .AsSelf();

            SetUpCrudServices(builder);
        }

        private static EfCoreContext SetupEfContext()
        {
            var context = new EfCoreContext();
            context.Database.Migrate();

            return context;
        }

        private static void SetUpCrudServices(ContainerBuilder builder)
        {
            var context = SetupEfContext();

            var utData = context.SetupSingleDtoAndEntities<AccountViewModel>();
            utData.AddSingleDto<CategoryViewModel>();
            utData.AddSingleDto<PaymentViewModel>();
            utData.AddSingleDto<RecurringPaymentViewModel>();

            var crudService = new CrudServicesAsync(context, utData.ConfigAndMapper);

            builder.Register(c => crudService);
            builder.Register(c => utData.ConfigAndMapper);
        }
    }
}
