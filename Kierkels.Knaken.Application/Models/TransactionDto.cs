namespace Kierkels.Knaken.Application.Models;

public class TransactionDto
{
    public string GroupIdentifier { get; set; } = null!;
    
    public DateTime Date { get; set; } // Maps to "Datum"
        
    public string Description { get; set; } = null!; // Maps to "Naam / Omschrijving"
    
    public string CounterAccount { get; set; } = null!; // Maps to "Tegenrekening"
    
    public string DebitCredit { get; set; } = null!; // Maps to "Af Bij" ("Af" for Debit, "Bij" for Credit)
        
    public decimal Amount { get; set; } // Maps to "Bedrag (EUR)", taking into account the comma decimal separator
        
    public string MutationType { get; set; } = null!; // Maps to "Mutatiesoort"
        
    public string Remarks { get; set; } = null!; // Maps to "Mededelingen"
    
}
