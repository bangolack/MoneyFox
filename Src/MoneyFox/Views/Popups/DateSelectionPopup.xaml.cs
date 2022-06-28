﻿namespace MoneyFox.Views.Popups
{

    using System;
    using Core.Common.Messages;
    using ViewModels.Dialogs;

    public partial class DateSelectionPopup
    {
        public DateSelectionPopup(DateTime dateFrom, DateTime dateTo)
        {
            InitializeComponent();
            BindingContext = App.GetViewModel<SelectDateRangeDialogViewModel>();
            ViewModel.StartDate = dateFrom;
            ViewModel.EndDate = dateTo;
        }

        public DateSelectionPopup(DateSelectedMessage message)
        {
            InitializeComponent();
            BindingContext = App.GetViewModel<SelectDateRangeDialogViewModel>();
            ViewModel.Initialize(message);
        }

        private SelectDateRangeDialogViewModel ViewModel => (SelectDateRangeDialogViewModel)BindingContext;

        private void Button_OnClicked(object sender, EventArgs e)
        {
            ViewModel.DoneCommand.Execute(null);
            Dismiss(null);
        }
    }

}