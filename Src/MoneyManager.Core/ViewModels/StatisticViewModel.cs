using System;
using MoneyManager.Core.Extensions;
using MoneyManager.Foundation.Messages;
using MoneyManager.Localization;
using MvvmCross.Plugins.Messenger;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class StatisticViewModel : BaseViewModel
    {
        //this token ensures that we will be notified when a message is sent.
        private readonly MvxSubscriptionToken token;

        /// <summary>
        ///     Creates a StatisticViewModel Object.
        /// </summary>
        public StatisticViewModel()
        {
            StartDate = DateTime.Today.GetFirstDayOfMonth();
            EndDate = DateTime.Today.GetLastDayOfMonth();

            token = MessageHub.Subscribe<DateSelectedMessage>(message =>
            {
                StartDate = message.StartDate;
                EndDate = message.EndDate;
            });
        }

        /// <summary>
        ///     Startdate for a custom statistic
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        ///     Enddate for a custom statistic
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        ///     Returns the title for the category view
        /// </summary>
        public string Title => Strings.StatisticTitle + " " + StartDate.ToString("d") +
                               " - " +
                               EndDate.ToString("d");
    }
}