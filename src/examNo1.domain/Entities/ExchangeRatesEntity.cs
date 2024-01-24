using System.Xml.Serialization;

namespace examNo1.domain.Entities;

[XmlRoot("ExchangeRates")]
public class ExchangeRatesEntity
{
    [XmlElement("item")]
    public List<ExchangeRateItemEntity> Items { get; set; } = [];
}
