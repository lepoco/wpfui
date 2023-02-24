using System;
using System.ComponentModel;
using System.Globalization;
using Wpf.Ui.Common;
using Wpf.Ui.Extensions;

namespace Wpf.Ui.Controls.IconElements;

/// <summary>
/// Tries to convert <see cref="SymbolRegular"/> and <seealso cref="SymbolFilled"/>  to <see cref="SymbolRegular"/>.
/// </summary>
public class IconElementConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        if (sourceType == typeof(SymbolRegular))
            return true;

        if (sourceType == typeof(SymbolFilled))
            return true;

        return false;
    }

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) => false;

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value is SymbolRegular symbolRegular)
            return new SymbolIcon(symbolRegular);

        if (value is SymbolFilled symbolFilled)
        {
            return new SymbolIcon(symbolFilled.Swap(), true);
        }

        return new SymbolIcon(SymbolRegular.Empty);
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        throw GetConvertFromException(value);
    }
}
