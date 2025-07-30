namespace Kierkels.Knaken.Application.Models;

public class TransactionDto
{
    public string GroupIdentifier { get; set; }
    
    public DateTime Date { get; set; } // Maps to "Datum"
        
    public string Description { get; set; } // Maps to "Naam / Omschrijving"
    
    public string CounterAccount { get; set; } // Maps to "Tegenrekening"
    
    public string DebitCredit { get; set; } // Maps to "Af Bij" ("Af" for Debit, "Bij" for Credit)
        
    public decimal Amount { get; set; } // Maps to "Bedrag (EUR)", taking into account the comma decimal separator
        
    public string MutationType { get; set; } // Maps to "Mutatiesoort"
        
    public string Remarks { get; set; } // Maps to "Mededelingen"
    
}
