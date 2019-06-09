﻿using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.ServiceLayer.Services;
using MoneyFox.ServiceLayer.ViewModels;
using Moq;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using Should;
using Xunit;

namespace MoneyFox.ServiceLayer.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    [Collection("MvxIocCollection")]
    public class BalanceViewModelTests
    {
        [Fact]
        public async Task GetTotalBalance_Zero()
        {
            var balanceCalculationService = new Mock<IBalanceCalculationService>();
            balanceCalculationService.Setup(x => x.GetEndOfMonthBalanceForAccount(It.IsAny<AccountViewModel>())).ReturnsAsync(() => 0);
            balanceCalculationService.Setup(x => x.GetTotalEndOfMonthBalance()).ReturnsAsync(() => 0);

            var vm = new BalanceViewModel(balanceCalculationService.Object,
                                          new Mock<IMvxLogProvider>().Object,
                                          new Mock<IMvxNavigationService>().Object);

            await vm.UpdateBalanceCommand.ExecuteAsync();

            vm.TotalBalance.ShouldEqual(0);
            vm.EndOfMonthBalance.ShouldEqual(0);
        }

        [Fact]
        public void GetTotalBalance_TwoPayments_SumOfPayments()
        {
            var vm = new BalanceViewModel(new Mock<IBalanceCalculationService>().Object,
                                          new Mock<IMvxLogProvider>().Object,
                                          new Mock<IMvxNavigationService>().Object);
            vm.UpdateBalanceCommand.Execute();

            vm.TotalBalance.ShouldEqual(0);
        }

        [Fact]
        public void GetTotalBalance_TwoAccounts_SumOfAccounts()
        {
            var balanceCalculationService = new Mock<IBalanceCalculationService>();
            balanceCalculationService.Setup(x => x.GetTotalBalance())
                .ReturnsAsync(() => 700);

            var vm = new BalanceViewModel(balanceCalculationService.Object,
                                          new Mock<IMvxLogProvider>().Object,
                                          new Mock<IMvxNavigationService>().Object);
            vm.UpdateBalanceCommand.Execute();

            vm.TotalBalance.ShouldEqual(700);
        }
    }
}