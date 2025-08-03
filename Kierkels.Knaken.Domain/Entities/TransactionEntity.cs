namespace Kierkels.Knaken.Domain.Entities;

public class TransactionEntity
{
    public int Id { get; set; } // Primary key for EF Core

    public DateTime Date { get; set; } // Maps to "Datum"
        
    public string Description { get; set; } = null!; // Maps to "Naam / Omschrijving"
        
    public string Account { get; set; } = null!; // Maps to "Rekening"
        
    public string CounterAccount { get; set; } = null!; // Maps to "Tegenrekening"
        
    public string Code { get; set; } = null!; // Maps to "Code"
        
    public string DebitCredit { get; set; } = null!; // Maps to "Af Bij" ("Af" for Debit, "Bij" for Credit)
        
    public decimal Amount { get; set; } // Maps to "Bedrag (EUR)", taking into account the comma decimal separator
        
    public string MutationType { get; set; } = null!; // Maps to "Mutatiesoort"
        
    public string Remarks { get; set; } = null!; // Maps to "Mededelingen"
        
    public decimal BalanceAfterMutation { get; set; } // Maps to "Saldo na mutatie"
        
    public string Tag { get; set; } = null!; // Maps to "Tag"
}
