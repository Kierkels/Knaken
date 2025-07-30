namespace Kierkels.Knaken.Domain.Entities;

public class TransactionEntity
{
    public int Id { get; set; } // Primary key for EF Core

    public DateTime Date { get; set; } // Maps to "Datum"
        
    public string Description { get; set; } // Maps to "Naam / Omschrijving"
        
    public string Account { get; set; } // Maps to "Rekening"
        
    public string CounterAccount { get; set; } // Maps to "Tegenrekening"
        
    public string Code { get; set; } // Maps to "Code"
        
    public string DebitCredit { get; set; } // Maps to "Af Bij" ("Af" for Debit, "Bij" for Credit)
        
    public decimal Amount { get; set; } // Maps to "Bedrag (EUR)", taking into account the comma decimal separator
        
    public string MutationType { get; set; } // Maps to "Mutatiesoort"
        
    public string Remarks { get; set; } // Maps to "Mededelingen"
        
    public decimal BalanceAfterMutation { get; set; } // Maps to "Saldo na mutatie"
        
    public string Tag { get; set; } // Maps to "Tag"
}
