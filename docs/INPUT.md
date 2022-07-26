# Forms

**WPF UI** modifies the styles of the default text-editing controls and provides some new ones.

# TextBox

`TextBox` enables you to display or edit unformatted text.

### Implementation

```cpp
class System.Windows.Controls.TextBox
```

## Exposes

```cpp
// Gets or sets the displayed / edited text.
TextBox.Text = "Hello World";
```

### How to use

```xml
<TextBox
  Text="Hello World"/>
```

# ui:TextBox

`ui:TextBox` enables you to display or edit unformatted text. Additionally, it has parameters such as icon or placeholder.

### Implementation

```cpp
class Wpf.Ui.Controls.TextBox
```

## Exposes

```cpp
// Gets or sets the displayed / edited text.
TextBox.Text = "Hello World";
```

```cpp
// Gets or sets text displayed if the parameter Text is empty.
TextBox.PlaceholderText = "Hello World";
```

```cpp
// Gets or sets a value determining whether to display the placeholder.
TextBox.PlaceholderEnabled = true;
```

```cpp
// Gets or sets a value determining whether to enable the clear button.
TextBox.ClearButtonEnabled = true;
```

```cpp
// Gets or sets displayed icon.
TextBox.Icon = SymbolRegular.Fluent24;
```

```cpp
// Defines whether or not the SymbolFilled should be used.
TextBox.IconFilled = false;
```

```cpp
// Foreground of the icon.
TextBox.IconForeground = Brushes.White;
```

```cpp
// Defines which side the icon should be placed on.
TextBox.IconPlacement = ElementPlacement.Left;
```

### How to use

```xml
<ui:TextBox
  Text="Hello World"
  PlaceholderText="Fill me"
  PlaceholderEnabled="True"
  ClearButtonEnabled="True"
  Icon="Fluent24"
  IconPlacement="Left"
  IconFilled="True"
  IconForeground="White"/>
```

# PasswordBox

`PasswordBox` represents a control designed for entering and handling passwords.

### Implementation

```cpp
class System.Windows.Controls.PasswordBox
```

## Exposes

```cpp
// Gets or sets the password.
PasswordBox.Password = "Secret";
```

```cpp
// Gets or sets the maximum length for passwords to be handled by this PasswordBox.
PasswordBox.MaxLength = 64;
```

```cpp
// Gets or sets the masking character for the PasswordBox.
PasswordBox.PasswordChar = '#';
```

### How to use

```xml
<PasswordBox
  MaxLength="64"
  PasswordChar="#"/>
```

# ui:PasswordBox

`ui:PasswordBox` represents a control designed for entering and handling passwords. Additionally, it has parameters such as icon or placeholder.

### Implementation

```cpp
class Wpf.Ui.Controls.PasswordBox
```

## Exposes

```cpp
// Gets or sets the password.
PasswordBox.Password = "Secret";
```

```cpp
// Gets or sets the maximum length for passwords to be handled by this PasswordBox.
PasswordBox.MaxLength = 64;
```

```cpp
// Gets or sets character used to mask the password.
PasswordBox.PasswordChar = '*';
```

```cpp
// Gets or sets a value deciding whether to display the reveal password button.
PasswordBox.RevealButtonEnabled = true;
```

```cpp
// Gets or sets text displayed if the parameter Text is empty.
PasswordBox.PlaceholderText = "Hello World";
```

```cpp
// Gets or sets a value determining whether to display the placeholder.
PasswordBox.PlaceholderEnabled = true;
```

```cpp
// Gets or sets a value determining whether to enable the clear button.
PasswordBox.ClearButtonEnabled = true;
```

```cpp
// Gets or sets displayed icon.
PasswordBox.Icon = SymbolRegular.Fluent24;
```

```cpp
// Defines whether or not the SymbolFilled should be used.
PasswordBox.IconFilled = false;
```

```cpp
// Foreground of the icon.
PasswordBox.IconForeground = Brushes.White;
```

```cpp
// Defines which side the icon should be placed on.
PasswordBox.IconPlacement = ElementPlacement.Left;
```

### How to use

```xml
<ui:PasswordBox
  Password="Secret"
  MaxLength="64"
  PasswordChar="*"
  RevealButtonEnabled="True"
  PlaceholderText="Fill me"
  PlaceholderEnabled="True"
  ClearButtonEnabled="True"
  Icon="Fluent24"
  IconPlacement="Left"
  IconFilled="True"
  IconForeground="White"/>
```

# ui:NumberBox

`ui:NumberBox` is a control adapted to entering numerical values. Additionally, it has parameters such as icon or placeholder.

### Implementation

```cpp
class Wpf.Ui.Controls.NumberBox
```

## Exposes

```cpp
// Gets or sets current numeric value.
NumberBox.Value = 10.0;
```

```cpp
// Gets or sets value by which the given number will be increased or decreased after pressing the button.
NumberBox.Step = 10.0;
```

```cpp
// Maximum allowable value.
NumberBox.Max = 100.0;
```

```cpp
// Minimum allowable value.
NumberBox.Max = 0.0;
```

```cpp
// Number of decimal places.
NumberBox.DecimalPlaces = 2;
```

```cpp
// Gets or sets numbers pattern.
NumberBox.Mask = "";
```

```cpp
// Gets or sets value determining whether to display the button controls.
NumberBox.SpinButtonsEnabled = true;
```

```cpp
// Gets or sets value which determines whether only integers can be entered.
NumberBox.IntegersOnly = false;
```

```cpp
// Gets or sets text displayed if the parameter Text is empty.
NumberBox.PlaceholderText = "Hello World";
```

```cpp
// Gets or sets a value determining whether to display the placeholder.
NumberBox.PlaceholderEnabled = true;
```

```cpp
// Gets or sets a value determining whether to enable the clear button.
NumberBox.ClearButtonEnabled = true;
```

```cpp
// Gets or sets displayed icon.
NumberBox.Icon = SymbolRegular.Fluent24;
```

```cpp
// Defines whether or not the SymbolFilled should be used.
NumberBox.IconFilled = false;
```

```cpp
// Foreground of the icon.
NumberBox.IconForeground = Brushes.White;
```

```cpp
// Defines which side the icon should be placed on.
NumberBox.IconPlacement = ElementPlacement.Left;
```

```cpp
// Event occurs when a value is incremented by button or arrow key.
NumberBox.Incremented += OnNumberBoxIncremented;
```

```cpp
// Event occurs when a value is decremented by button or arrow key.
NumberBox.Decremented += OnNumberBoxDecremented;
```

### How to use

```xml
<ui:NumberBox
  Value="10.0"
  Step="1.0"
  Max="100.0"
  Min="0.0"
  Mask=""
  SpinButtonsEnabled="True"
  DecimalPlaces="2"
  IntegersOnly="False"
  PlaceholderText="Fill me"
  PlaceholderEnabled="True"
  ClearButtonEnabled="True"
  Icon="Fluent24"
  IconPlacement="Left"
  IconFilled="True"
  IconForeground="White"/>
```

# ui:AutoSuggestBox

`ui:AutoSuggestBox` represents a text control that makes suggestions to users as they enter text. Additionally, it has parameters such as icon or placeholder.

### Implementation

```cpp
class Wpf.Ui.Controls.AutoSuggestBox
```

## Exposes

```cpp
// ItemsSource specifies a collection used to generate the list of suggestions.
AutoSuggestBox.ItemsSource = new List<string>{"One", "Two"};
```

```cpp
// Gets or sets the displayed / edited text.
AutoSuggestBox.Text = "Hello World";
```

```cpp
// Gets or sets text displayed if the parameter Text is empty.
AutoSuggestBox.PlaceholderText = "Hello World";
```

```cpp
// Gets or sets a value determining whether to display the placeholder.
AutoSuggestBox.PlaceholderEnabled = true;
```

```cpp
// Gets or sets a value determining whether to enable the clear button.
AutoSuggestBox.ClearButtonEnabled = true;
```

```cpp
// Gets or sets displayed icon.
AutoSuggestBox.Icon = SymbolRegular.Fluent24;
```

```cpp
// Defines whether or not the SymbolFilled should be used.
AutoSuggestBox.IconFilled = false;
```

```cpp
// Foreground of the icon.
AutoSuggestBox.IconForeground = Brushes.White;
```

```cpp
// Defines which side the icon should be placed on.
AutoSuggestBox.IconPlacement = ElementPlacement.Left;
```

### How to use

```xml
<ui:AutoSuggestBox
  ItemsSource="{Binding MySuggestions, Mode=OneWay}"
  Text=""
  PlaceholderText="Fill me"
  PlaceholderEnabled="True"
  ClearButtonEnabled="True"
  Icon="Fluent24"
  IconPlacement="Left"
  IconFilled="True"
  IconForeground="White"/>
```
