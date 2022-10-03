namespace MoneyFox.Ui.ViewModels.Budget;

using System.Collections.ObjectModel;
using Common.Extensions;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Core.ApplicationCore.Queries.BudgetListLoading;
using MoneyFox.Core.Common.Extensions;
using MoneyFox.Core.Common.Messages;
using Views.Budget;

public sealed class BudgetListViewModel : BaseViewModel, IRecipient<ReloadMessage>
{
    private readonly ISender sender;

    public BudgetListViewModel(ISender sender)
    {
        this.sender = sender;
        WeakReferenceMessenger.Default.Register(this);
    }

    public bool HasBudgets => Budgets.Any();

    public ObservableCollection<BudgetListItemViewModel> Budgets { get; } = new();

    public decimal BudgetedAmount => Budgets.Sum(b => b.SpendingLimit);

    public AsyncRelayCommand InitializeCommand => new(Initialize);

    public AsyncRelayCommand GoToAddBudgetCommand => new(GoToAddBudget);

    public AsyncRelayCommand<BudgetListItemViewModel> EditBudgetCommand => new(EditBudgetAsync);

    public async void Receive(ReloadMessage message)
    {
        await Initialize();
    }

    private async Task Initialize()
    {
        var budgetsListData = await sender.Send(new LoadBudgetListData.Query());
        Budgets.Clear();
        Budgets.AddRange(
            budgetsListData.OrderBy(bld => bld.Name)
                .Select(
                    bld => new BudgetListItemViewModel
                    {
                        Id = bld.Id,
                        Name = bld.Name,
                        SpendingLimit = bld.SpendingLimit,
                        CurrentSpending = bld.CurrentSpending
                    }));

        OnPropertyChanged(nameof(BudgetedAmount));
        OnPropertyChanged(nameof(HasBudgets));
    }

    private static async Task GoToAddBudget()
    {
        await Shell.Current.GoToModalAsync(Routes.AddBudgetRoute);
    }

    private async Task EditBudgetAsync(BudgetListItemViewModel? selectedBudget)
    {
        if (selectedBudget == null)
        {
            return;
        }

        await Shell.Current.Navigation.PushModalAsync(new NavigationPage(new EditBudgetPage(selectedBudget.Id)) { BarBackgroundColor = Colors.Transparent });
    }
}