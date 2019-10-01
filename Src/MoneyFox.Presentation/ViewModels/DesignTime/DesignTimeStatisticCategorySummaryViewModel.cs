﻿using System.Collections.ObjectModel;
using System.Globalization;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.ViewModels.Statistic;
using MoneyFox.ServiceLayer.Utilities;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeStatisticCategorySummaryViewModel : IStatisticCategorySummaryViewModel
    {
        public LocalizedResources Resources { get; } = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);

        /// <inheritdoc />
        public IncomeExpenseBalanceViewModel IncomeExpenseBalance { get; } = new IncomeExpenseBalanceViewModel {TotalIncome = 400, TotalSpent = 600};

        public ObservableCollection<CategoryOverviewViewModel> CategorySummary => new ObservableCollection<CategoryOverviewViewModel>
        {
            new CategoryOverviewViewModel
            {
                Label = "Einkaufen",
                Value = 745,
                Percentage = 30
            },
            new CategoryOverviewViewModel
            {
                Label = "Beeeeer",
                Value = 666,
                Percentage = 70
            }
        };

        public bool HasData { get; } = true;
        public AsyncCommand LoadedCommand { get; } = null;
    }
}
