using WebBanHang.Services;

namespace WebBanHang.Tests;

public class UnitTest1
{
    [Fact]
    public void CalculateTotal_ShouldReturnCorrectTotal()
    {
        var calculator = new OrderCalculator();

        var result = calculator.CalculateTotal(50000m, 2);

        Assert.Equal(100000m, result);
    }
}