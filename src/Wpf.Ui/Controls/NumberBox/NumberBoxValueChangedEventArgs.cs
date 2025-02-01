using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Ui.Controls;

/// <summary>
/// Provides information for the <see cref="NumberBox.ValueChanged" /> event.
/// </summary>
public class NumberBoxValueChangedEventArgs : RoutedEventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NumberBoxValueChangedEventArgs" /> class. />
    /// </summary>
    /// <param name="oldValue">
    ///     The value of the <see cref="NumberBox.Value"/> property before the change
    ///     reported by the relevant event or state change.
    /// </param>
    /// <param name="newValue">
    ///     The value of the <see cref="NumberBox.Value"/> property after the change
    ///     reported by the relevant event or state change.
    /// </param>
    /// <param name="source">
    ///     An alternate source that will be reported when the event is handled. This pre-populates
    ///     the <see cref="System.Windows.RoutedEventArgs.Source" /> property.
    /// </param>
    internal NumberBoxValueChangedEventArgs(double? oldValue, double? newValue, object source)
        : base(NumberBox.ValueChangedEvent, source)
    {
        this.OldValue = oldValue;
        this.NewValue = newValue;
    }

    /// <summary>Gets the value of the <see cref="NumberBox.Value"/> property before the change.</summary>
    /// <returns>The property value before the change.</returns>
    public double? OldValue { get; private set; }

    /// <summary>Gets the value of the <see cref="NumberBox.Value"/> property after the change.</summary>
    /// <returns>The property value after the change.</returns>
    public double? NewValue { get; private set; }
}

public delegate void NumberBoxValueChangedEvent(object sender, NumberBoxValueChangedEventArgs args);
