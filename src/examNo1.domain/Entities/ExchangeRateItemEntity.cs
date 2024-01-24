using System.Xml.Serialization;

namespace examNo1.domain.Entities;

public class ExchangeRateItemEntity
{
    private const string DateFormat = "yyyy.MM.dd";

    [XmlElement("date")]
    public string DateString
    {
        get => Date.ToString(DateFormat);
        set => Date = DateTime.Parse(value);
    }

    [XmlIgnore]
    public DateTime Date { get; set; }

    [XmlElement("currency")]
    public string Currency { get; set; } = string.Empty;

    [XmlElement("quantity")]
    public int Quantity { get; set; }

    [XmlElement("rate")]
    public decimal Rate { get; set; }

    [XmlElement("unit")]
    public string Unit { get; set; } = string.Empty;
}
