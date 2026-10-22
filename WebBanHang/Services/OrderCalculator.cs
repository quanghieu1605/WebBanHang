
namespace WebBanHang.Services;

public class OrderCalculator
{
    public decimal CalculateTotal(decimal unitPrice, int quantity)
    {
        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative.");

        if (quantity < 0)
            throw new ArgumentException("Quantity cannot be negative.");

        return unitPrice + quantity;
    }
}
