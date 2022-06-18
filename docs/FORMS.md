# Forms
The text input elements are the same as any other [controls](https://Wpf.Ui.lepo.co/documentation/controls), but collecting them here can help you create your forms.

### Text fields
You can use the default `TextBox` or `wpfui:TextBox` to create a text input field. The second one has additional options like Placeholder or Icon.

```xml
<TextBox/>
```

```xml
<wpfui:TextBox
  Icon="FoodCake24"
  Placeholder="The cake is a lie!"/>
```

![image](https://user-images.githubusercontent.com/13592821/158706248-50d4cd24-beab-4b00-be00-2dad92af10dc.png)

### Text field for numbers
A custom WPF UI control for inputting numbers only allows you to manage entered data faster.
```xml
<wpfui:NumberBox
  Margin="0,0,0,8"
  DecimalPlaces="4"
  Icon="AccessTime24"
  IconFilled="True"
  Max="10"
  Min="-10"
  Step="0.2"
  Value="8.2211" />
```

![image](https://user-images.githubusercontent.com/13592821/158706301-31a77f51-b893-45c2-bb56-6b907d9fd74d.png)

### Password
A masked text field that displays asterisks instead of text can be added using `PasswordBox`.
```xml
<PasswordBox/>
```

![image](https://user-images.githubusercontent.com/13592821/158706376-726d8527-539c-47b4-97a3-c567d1f05ab7.png)

### Search box
The `SearchBox` can help you create text fields with search results.
```xml
<wpfui:SearchBox
  Placeholder="Is the cake a lie?" />
```

![image](https://user-images.githubusercontent.com/13592821/158706416-c3f23eb4-0f10-4afd-b161-ee85086adadc.png)