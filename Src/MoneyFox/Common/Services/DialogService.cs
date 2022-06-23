﻿namespace MoneyFox.Common.Services
{

    using System;
    using System.Threading.Tasks;
    using MoneyFox.Core.Common.Interfaces;
    using MoneyFox.Core.Resources;
    using MoneyFox.Views.Popups;
    using Xamarin.CommunityToolkit.Extensions;
    using Xamarin.Forms;

    public class DialogService : IDialogService
    {
        private LoadingIndicatorPopup? loadingDialog;

        /// <inheritdoc />
        public async Task ShowLoadingDialogAsync(string? message = null)
        {
            if (loadingDialog != null)
            {
                await HideLoadingDialogAsync();
            }

            loadingDialog = new LoadingIndicatorPopup();
            Shell.Current.ShowPopup(loadingDialog);
        }

        /// <inheritdoc />
        public async Task HideLoadingDialogAsync()
        {
            if (loadingDialog == null)
            {
                return;
            }

            try
            {
                loadingDialog.Dismiss(null);
                loadingDialog = null;
                await Task.CompletedTask;
            }
            catch (IndexOutOfRangeException)
            {
                // catch and swallow out of range exceptions when dismissing dialogs.
            }
        }

        public async Task ShowMessageAsync(string title, string message)
        {
            await Shell.Current.DisplayAlert(title: title, message: message, cancel: Strings.OkLabel);
        }

        public async Task<bool> ShowConfirmMessageAsync(string title, string message, string? positiveButtonText = null, string? negativeButtonText = null)
        {
            return await Shell.Current.DisplayAlert(
                title: title,
                message: message,
                accept: positiveButtonText ?? Strings.YesLabel,
                cancel: negativeButtonText ?? Strings.NoLabel);
        }
    }

}
