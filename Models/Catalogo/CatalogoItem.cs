namespace SiservieCatering.API.Models.Catalogo;

public class CatalogoItem
{
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;

    public CatalogoItem() { }

    public CatalogoItem(string label, string value)
    {
        Label = label;
        Value = value;
    }
}
