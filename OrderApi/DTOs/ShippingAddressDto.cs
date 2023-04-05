namespace OrderApi.DTOs;

public class ShippingAddressDto
{
    public string Street { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string ToString()
    {
        return $"{Street} | {City} | {ZipCode}";
    }
    public ShippingAddressDto(string address)
    {
        //Need more validations and formatting etc
        var list = address.Split('|').ToList();
        Street = list[0];
        City = list[1];
        ZipCode = list[2];
    }
    public ShippingAddressDto() { }
}
