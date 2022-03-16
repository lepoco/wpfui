# Colors
WPF UI uses three kinds of colors. Theme colors, system accents and palette.

### Palette
The palette colors are available in the [Palette](https://github.com/lepoco/wpfui/blob/main/WPFUI/Styles/Assets/Palette.xaml) dictionary.
```xml
<Color x:Key="PalettePrimaryColor">#333333</Color>
<Color x:Key="PaletteRedColor">#F44336</Color>
<Color x:Key="PalettePinkColor">#E91E63</Color>
<Color x:Key="PalettePurpleColor">#9C27B0</Color>
<Color x:Key="PaletteDeepPurpleColor">#673AB7</Color>
<Color x:Key="PaletteIndigoColor">#3F51B5</Color>
<Color x:Key="PaletteBlueColor">#2196F3</Color>
<Color x:Key="PaletteLightBlueColor">#03A9F4</Color>
<Color x:Key="PaletteCyanColor">#00BCD4</Color>
<Color x:Key="PaletteTealColor">#009688</Color>
<Color x:Key="PaletteGreenColor">#4CAF50</Color>
<Color x:Key="PaletteLightGreenColor">#8BC34A</Color>
<Color x:Key="PaletteLimeColor">#CDDC39</Color>
<Color x:Key="PaletteYellowColor">#FFEB3B</Color>
<Color x:Key="PaletteAmberColor">#FFC107</Color>
<Color x:Key="PaletteOrangeColor">#FF9800</Color>
<Color x:Key="PaletteDeepOrangeColor">#FF5722</Color>
<Color x:Key="PaletteBrownColor">#795548</Color>
<Color x:Key="PaletteGreyColor">#9E9E9E</Color>
<Color x:Key="PaletteBlueGreyColor">#607D8B</Color>
```

### System Accents
System accents are saved in the [Accent](https://github.com/lepoco/wpfui/blob/main/WPFUI/Styles/Assets/Accent.xaml) dictionary by default.
```xml
<Color x:Key="SystemAccentColor">#3379d9</Color>
<Color x:Key="SystemAccentColorLight1">#559ce4</Color>
<Color x:Key="SystemAccentColorLight2">#80b9ee</Color>
<Color x:Key="SystemAccentColorLight3">#add8ff</Color>
```

You can change your accents on the fly with the Accent class.
```c#
WPFUI.Appearance.Accent.Change(SystemTheme.GlassColor, WPFUI.Appearance.ThemeType.Light, true)
```