﻿namespace MoneyFox.Ui.Views.Accounts;

using MoneyFox.Core.Resources;
using ViewModels.Accounts;

public partial class EditAccountPage
{
    private readonly int accountId;

    public EditAccountPage(int accountId)
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<EditAccountViewModel>();
        this.accountId = accountId;
        var cancelItem = new ToolbarItem
        {
            Command = new Command(async () => await Navigation.PopModalAsync()),
            Text = Strings.CancelLabel,
            Priority = -1,
            Order = ToolbarItemOrder.Primary
        };

        var saveItem = new ToolbarItem
        {
            Command = new Command(() => ViewModel.SaveCommand.Execute(null)),
            Text = Strings.SaveLabel,
            Priority = 1,
            Order = ToolbarItemOrder.Primary
        };

        ToolbarItems.Add(cancelItem);
        ToolbarItems.Add(saveItem);
    }

    private EditAccountViewModel ViewModel => (EditAccountViewModel)BindingContext;

    protected override async void OnAppearing()
    {
        await ViewModel.InitializeAsync(accountId);
    }
}
