namespace MoneyFox.Ui.ViewModels.Statistics;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using MediatR;
using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using MoneyFox.Core.ApplicationCore.Queries.Statistics;
using MoneyFox.Core.Common.Extensions;

internal sealed class StatisticCategorySpreadingViewModel : StatisticViewModel
{
    private PaymentType selectedPaymentType;

    public StatisticCategorySpreadingViewModel(IMediator mediator) : base(mediator) { }

    public List<PaymentType> PaymentTypes => new() { PaymentType.Expense, PaymentType.Income };

    public ObservableCollection<ISeries> Series { get; } = new();

    public PaymentType SelectedPaymentType
    {
        get => selectedPaymentType;

        set
        {
            if (selectedPaymentType == value)
            {
                return;
            }

            selectedPaymentType = value;
            OnPropertyChanged();
            LoadDataCommand.Execute(null);
        }
    }

    public AsyncRelayCommand LoadDataCommand => new(LoadAsync);

    protected override async Task LoadAsync()
    {
        var statisticEntries = await Mediator.Send(new GetCategorySpreadingQuery(startDate: StartDate, endDate: EndDate, paymentType: SelectedPaymentType));
        var pieSeries = statisticEntries.Select(
            x => new PieSeries<decimal>
            {
                Name = x.Label,
                TooltipLabelFormatter = point => $"{point.Context.Series.Name}: {point.PrimaryValue:C}",
                DataLabelsFormatter = point => $"{point.Context.Series.Name}: {point.PrimaryValue:C}",
                Values = new List<decimal> { x.Value },
                InnerRadius = 150
            });

        Series.Clear();
        Series.AddRange(pieSeries);
    }
}