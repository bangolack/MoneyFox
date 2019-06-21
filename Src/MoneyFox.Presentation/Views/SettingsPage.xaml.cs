﻿using MoneyFox.Presentation.ViewModels;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage 
	{
		public SettingsPage ()
		{
			InitializeComponent ();

		    SettingsList.ItemTapped += (sender, args) =>
		    {
		        SettingsList.SelectedItem = null;
                (BindingContext as SettingsViewModel)?.GoToSettingCommand.Execute(args.Item);
		    };
        }
	}
}