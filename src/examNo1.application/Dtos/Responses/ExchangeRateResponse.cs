using System.Xml.Serialization;

namespace examNo1.application.Dtos.Responses;

public class ExchangeRateResponse
{
    public DateTime Date { get; set; }
    public string Currency { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Rate { get; set; }
    public string Unit { get; set; } = string.Empty;
}
