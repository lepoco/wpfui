using Wpf.Ui.Extensions;

namespace Wpf.Ui.UnitTests;

public class SymbolExtensionsTests
{
    [Fact]
    public void GivenAllRegularSymbols_Swap_ReturnsValidFilledSymbol()
    {
        foreach (
            Wpf.Ui.Common.SymbolRegular regularSymbol in Enum.GetValues(
                typeof(Wpf.Ui.Common.SymbolRegular)
            )
        )
        {
            _ = regularSymbol.Swap();
        }
    }

    [Fact]
    public void GivenAllFilledSymbols_Swap_ReturnsValidRegularSymbol()
    {
        foreach (
            Wpf.Ui.Common.SymbolFilled filledSymbol in Enum.GetValues(
                typeof(Wpf.Ui.Common.SymbolFilled)
            )
        )
        {
            _ = filledSymbol.Swap();
        }
    }

    [Fact]
    public void GivenAllRegularSymbols_GetString_ReturnsValidString()
    {
        foreach (
            Wpf.Ui.Common.SymbolRegular regularSymbol in Enum.GetValues(
                typeof(Wpf.Ui.Common.SymbolRegular)
            )
        )
        {
            var receivedString = regularSymbol.GetString();

            Assert.NotEqual(String.Empty, receivedString);
        }
    }

    [Fact]
    public void GivenAllFilledSymbols_GetString_ReturnsValidString()
    {
        foreach (
            Wpf.Ui.Common.SymbolFilled filledSymbol in Enum.GetValues(
                typeof(Wpf.Ui.Common.SymbolFilled)
            )
        )
        {
            var receivedString = filledSymbol.GetString();

            Assert.NotEqual(String.Empty, receivedString);
        }
    }
}
