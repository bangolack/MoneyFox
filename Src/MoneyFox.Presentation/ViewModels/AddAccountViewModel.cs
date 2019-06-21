﻿using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.QueryObject;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Services;
using IDialogService = MoneyFox.Presentation.Interfaces.IDialogService;

namespace MoneyFox.Presentation.ViewModels
{
    public class AddAccountViewModel : ModifyAccountViewModel
    {
        private readonly ICrudServicesAsync crudService;
        private readonly IDialogService dialogService;

        public AddAccountViewModel(ICrudServicesAsync crudService,
                                   ISettingsFacade settingsFacade,
                                   IBackupService backupService,
                                   IDialogService dialogService,
                                   INavigationService navigationService)
            : base(settingsFacade, backupService, navigationService)
        {
            this.crudService = crudService;
            this.dialogService = dialogService;

            SelectedAccount = new AccountViewModel();
        }
        
        protected override async Task SaveAccount()
        {
            if (await crudService.ReadManyNoTracked<AccountViewModel>()
                                 .AnyWithNameAsync(SelectedAccount.Name))
            {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            await crudService.CreateAndSaveAsync(SelectedAccount, "ctor(4)");
            if (!crudService.IsValid) await dialogService.ShowMessage(Strings.GeneralErrorTitle, crudService.GetAllErrors());

            NavigationService.GoBack();
        }
    }
}