// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls;

internal static class TextBlockMetadata
{
    static TextBlockMetadata()
    {
        System.Windows.Controls.TextBlock.FontSizeProperty.OverrideMetadata(typeof(System.Windows.Controls.TextBlock),
                                                                            new FrameworkPropertyMetadata(14d,
                                                                                                          null,
                                                                                                          static (d, value) =>
                                                                                                          {
                                                                                                              if (d.GetValue(TextBlock.FontTypographyStyleProperty) is Style style)
                                                                                                              {
                                                                                                                  foreach (SetterBase setterBase in style.Setters)
                                                                                                                  {
                                                                                                                      if (setterBase is Setter setter &&
                                                                                                                          setter.Property == System.Windows.Controls.TextBlock.FontSizeProperty)
                                                                                                                      {
                                                                                                                          return setter.Value;
                                                                                                                      }
                                                                                                                  }
                                                                                                              }

                                                                                                              return value;
                                                                                                          }));

        System.Windows.Controls.TextBlock.FontWeightProperty.OverrideMetadata(typeof(System.Windows.Controls.TextBlock),
                                                                              new FrameworkPropertyMetadata(FontWeights.Regular,
                                                                                                            null,
                                                                                                            static (d, value) =>
                                                                                                            {
                                                                                                                if (d.GetValue(TextBlock.FontTypographyStyleProperty) is Style style)
                                                                                                                {
                                                                                                                    foreach (SetterBase setterBase in style.Setters)
                                                                                                                    {
                                                                                                                        if (setterBase is Setter setter &&
                                                                                                                            setter.Property == System.Windows.Controls.TextBlock.FontWeightProperty)
                                                                                                                        {
                                                                                                                            return setter.Value;
                                                                                                                        }
                                                                                                                    }
                                                                                                                }

                                                                                                                return value;
                                                                                                            }));

        System.Windows.Controls.TextBlock.ForegroundProperty.OverrideMetadata(typeof(System.Windows.Controls.TextBlock),
                                                                              new FrameworkPropertyMetadata(Brushes.Black,
                                                                                                            null,
                                                                                                            static (d, value) =>
                                                                                                            {
                                                                                                                if (d.GetValue(TextBlock.AppearanceForegroundProperty) is Brush brush)
                                                                                                                {
                                                                                                                    return brush;
                                                                                                                }

                                                                                                                return value is Brush ? value : Brushes.Black;
                                                                                                            }));
    }

    public static void Initialize() { }
}
