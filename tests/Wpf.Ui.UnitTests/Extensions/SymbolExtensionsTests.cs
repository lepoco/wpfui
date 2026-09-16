// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;

namespace Wpf.Ui.UnitTests.Extensions;

public class SymbolExtensionsTests
{
    [Fact]
    public void GivenAllRegularSymbols_Swap_ReturnsValidFilledSymbol()
    {
        foreach (SymbolRegular regularSymbol in Enum.GetValues(typeof(SymbolRegular)))
        {
            _ = regularSymbol.Swap();
        }
    }

    [Fact]
    public void GivenAllFilledSymbols_Swap_ReturnsValidRegularSymbol()
    {
        foreach (SymbolFilled filledSymbol in Enum.GetValues(typeof(SymbolFilled)))
        {
            _ = filledSymbol.Swap();
        }
    }

    [Fact]
    public void GivenAllRegularSymbols_GetString_ReturnsValidString()
    {
        foreach (SymbolRegular regularSymbol in Enum.GetValues(typeof(SymbolRegular)))
        {
            if (regularSymbol == SymbolRegular.Empty)
            {
                continue;
            }

            var receivedString = regularSymbol.GetString();

            Assert.NotEqual(string.Empty, receivedString);
        }
    }

    [Fact]
    public void GivenAllFilledSymbols_GetString_ReturnsValidString()
    {
        foreach (SymbolFilled filledSymbol in Enum.GetValues(typeof(SymbolFilled)))
        {
            if (filledSymbol == SymbolFilled.Empty)
            {
                continue;
            }

            var receivedString = filledSymbol.GetString();

            Assert.NotEqual(string.Empty, receivedString);
        }
    }

    [Fact]
    public void GivenEmptyRegularSymbol_GetString_ReturnsEmptyString()
    {
        var receivedString = SymbolRegular.Empty.GetString();

        Assert.Equal(string.Empty, receivedString);
    }

    [Fact]
    public void GivenEmptyFilledSymbol_GetString_ReturnsEmptyString()
    {
        var receivedString = SymbolFilled.Empty.GetString();

        Assert.Equal(string.Empty, receivedString);
    }

    [Fact]
    public void GivenBasicPlaneRegularSymbol_GetString_ReturnsSingleUtf16CodeUnit()
    {
        // Savings24 = U+F686 lives in the Basic Multilingual Plane and needs no surrogate pair.
        var receivedString = SymbolRegular.Savings24.GetString();

        Assert.Equal("\uF686", receivedString);
    }

    [Fact]
    public void GivenSupplementaryPlaneRegularSymbol_GetString_ReturnsSurrogatePair()
    {
        // CoinStack24 = U+F0665 lives in Supplementary Private Use Area-A and must be encoded as a surrogate pair.
        var receivedString = SymbolRegular.CoinStack24.GetString();

        Assert.Equal("\U000F0665", receivedString);
    }

    [Fact]
    public void GivenSupplementaryPlaneFilledSymbol_GetString_ReturnsSurrogatePair()
    {
        // CoinStack24 = U+F06E1 lives in Supplementary Private Use Area-A and must be encoded as a surrogate pair.
        var receivedString = SymbolFilled.CoinStack24.GetString();

        Assert.Equal("\U000F06E1", receivedString);
    }

    [Fact]
    public void GivenAllRegularSymbols_GetString_RoundTripsToCodePoint()
    {
        foreach (SymbolRegular regularSymbol in Enum.GetValues(typeof(SymbolRegular)))
        {
            if (regularSymbol == SymbolRegular.Empty)
            {
                continue;
            }

            var receivedString = regularSymbol.GetString();

            Assert.Equal((int)regularSymbol, char.ConvertToUtf32(receivedString, 0));
            Assert.Equal((int)regularSymbol > 0xFFFF ? 2 : 1, receivedString.Length);
        }
    }

    [Fact]
    public void GivenAllFilledSymbols_GetString_RoundTripsToCodePoint()
    {
        foreach (SymbolFilled filledSymbol in Enum.GetValues(typeof(SymbolFilled)))
        {
            if (filledSymbol == SymbolFilled.Empty)
            {
                continue;
            }

            var receivedString = filledSymbol.GetString();

            Assert.Equal((int)filledSymbol, char.ConvertToUtf32(receivedString, 0));
            Assert.Equal((int)filledSymbol > 0xFFFF ? 2 : 1, receivedString.Length);
        }
    }
}
