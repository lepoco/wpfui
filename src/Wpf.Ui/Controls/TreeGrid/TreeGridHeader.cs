// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Work in progress.
/// </summary>
public class TreeGridHeader : System.Windows.FrameworkElement
{
    /// <summary>Identifies the <see cref="Title"/> dependency property.</summary>
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title),
        typeof(string),
        typeof(TreeGridHeader),
        new PropertyMetadata(string.Empty, OnTitleChanged)
    );

    /// <summary>Identifies the <see cref="Group"/> dependency property.</summary>
    public static readonly DependencyProperty GroupProperty = DependencyProperty.Register(
        nameof(Group),
        typeof(string),
        typeof(TreeGridHeader),
        new PropertyMetadata(string.Empty)
    );

    /// <summary>
    /// Gets or sets the title that will be displayed.
    /// </summary>
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the column group name.
    /// </summary>
    [Localizability(LocalizationCategory.NeverLocalize)]
    [MergableProperty(false)]
    public string Group
    {
        get => (string)GetValue(GroupProperty);
        set => SetValue(GroupProperty, value);
    }

    /// <summary>
    /// This virtual method is called when <see cref="Title"/> is changed.
    /// </summary>
    protected virtual void OnTitleChanged()
    {
        var title = Title;

        if (!string.IsNullOrEmpty(Group) || string.IsNullOrEmpty(title))
        {
            return;
        }

        SetCurrentValue(GroupProperty, title.ToLower().Trim());
    }

    private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TreeGridHeader header)
        {
            return;
        }

        header.OnTitleChanged();
    }
}
