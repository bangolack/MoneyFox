namespace MoneyFox.Ui.Views.Popups;

using MoneyFox.Core.Resources;
using ViewModels.Categories;

public partial class CategorySelectionPopup
{
    public CategorySelectionPopup()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SelectCategoryViewModel>();

        var cancelItem = new ToolbarItem
        {
            Command = new Command(async () => await Navigation.PopModalAsync()),
            Text = Strings.CancelLabel,
            Priority = -1,
            Order = ToolbarItemOrder.Primary
        };

        ToolbarItems.Add(cancelItem);
    }

    private SelectCategoryViewModel ViewModel => (SelectCategoryViewModel)BindingContext;

    protected override async void OnAppearing()
    {
        await ViewModel.InitializeAsync();
    }
}
