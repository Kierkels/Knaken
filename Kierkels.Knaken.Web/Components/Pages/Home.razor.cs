using System.Transactions;
using Kierkels.Knaken.Application.Models;
using Kierkels.Knaken.Domain.Entities;
using Kierkels.Knaken.Infrastructure.Persistence;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

namespace Kierkels.Knaken.Web.Components.Pages;

public partial class Home : ComponentBase
{
    private List<TransactionDto> transactions; // Data from the database

    // Inject the DbContext (Assume a DbContext named ApplicationDbContext is used)
    [Inject]
    private ApplicationDbContext ApplicationDbContext { get; set; }

    // On initialization, fetch data from the database
    protected override async Task OnInitializedAsync()
    {
        var entities = await ApplicationDbContext.Set<TransactionEntity>().AsNoTracking().ToListAsync();
        transactions = [];
        foreach (var entity in entities)
        {
            transactions.Add(new TransactionDto()
            {
                GroupIdentifier = entity.Date.ToString("yyyy-MM"),
                Amount = entity.Amount,
                Description = entity.Description,
                Date = entity.Date,
                DebitCredit = entity.DebitCredit,
                MutationType = entity.MutationType,
                CounterAccount = entity.CounterAccount,
                Remarks = entity.Remarks
            });
        }
        await base.OnInitializedAsync();
    }
    
    void OnRender(DataGridRenderEventArgs<TransactionDto> args)
    {
        if(args.FirstRender)
        {
            args.Grid.Groups.Add(new GroupDescriptor { Property = "GroupIdentifier", SortOrder = SortOrder.Descending, Title = "Maand/Jaar"});
            args.Grid.Groups.Add(new GroupDescriptor { Property = "Date", SortOrder = SortOrder.Descending, Title = "Datum"});
            StateHasChanged();
        }
    }

    void OnGroupRowRender(GroupRowRenderEventArgs args)
    {
        if (args.FirstRender && args.Group.Data.Key.ToString() == DateTime.Now.AddMonths(-1).ToString("yyyy-MM"))
        {
            args.Expanded = true;
        }
        else if (args.FirstRender && args.Group.Level == 1 && args.Group.Data.Key.Month == DateTime.Now.Month)
        {
            args.Expanded = true;
        }
        else if (args.FirstRender)
        {
            args.Expanded = false;
        }
    }
}
