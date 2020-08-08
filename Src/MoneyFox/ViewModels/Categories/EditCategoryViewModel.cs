﻿using AutoMapper;
using MediatR;
using MoneyFox.Application.Categories.Command.CreateCategory;
using MoneyFox.Application.Categories.Queries.GetCategoryById;
using MoneyFox.Domain.Entities;
using MoneyFox.Services;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Categories
{
    public class EditCategoryViewModel : ModifyCategoryViewModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public EditCategoryViewModel(IMediator mediator,
                                    IMapper mapper,
                                    IDialogService dialogService) : base(mediator, dialogService)

        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task Init(int categoryId)
        {
            SelectedCategory = mapper.Map<CategoryViewModel>(await mediator.Send(new GetCategoryByIdQuery(categoryId)));
        }

        protected override async Task SaveCategoryAsync()
        {
            await mediator.Send(new CreateCategoryCommand(mapper.Map<Category>(SelectedCategory)));
        }
    }
}
