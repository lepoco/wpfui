# Data
There is often a need to display datasets. We can use default controls like `ListView` or `ItemsControl`, but **WPF UI** also provides controls like `VirtualizingItemsControl` or `VirtualizingWrapPanel` which allow you to display thousands of controls without slowing down your application.

## ListView
```xml
<ListView
  Margin="0,8,0,0"
  ItemsSource="{Binding ListBoxItemCollection}"
  SelectedIndex="0" />
```

## ListBox
```xml
<ListBox
  Margin="0,8,0,0"
  ItemsSource="{Binding ListBoxItemCollection}"
  SelectedIndex="0" />
```

## TreeView
```xml
<TreeView ScrollViewer.CanContentScroll="False">
    <TreeViewItem Header="Employee1" IsExpanded="True">
        <TreeViewItem Header="Jesper Aaberg" />
        <TreeViewItem Header="Employee Number" IsExpanded="True">
            <TreeViewItem Header="12345" IsSelected="True" />
        </TreeViewItem>
        <TreeViewItem Header="Work Days">
            <TreeViewItem Header="Monday" />
            <TreeViewItem Header="Tuesday" />
            <TreeViewItem Header="Thursday" />
        </TreeViewItem>
    </TreeViewItem>
    <TreeViewItem Header="Employee2">
        <TreeViewItem Header="Dominik Paiha" />
        <TreeViewItem Header="Employee Number">
            <TreeViewItem Header="98765" />
        </TreeViewItem>
        <TreeViewItem Header="Work Days">
            <TreeViewItem Header="Tuesday" />
            <TreeViewItem Header="Wednesday" />
            <TreeViewItem Header="Friday" />
        </TreeViewItem>
    </TreeViewItem>
</TreeView>
```

## DataGrid
```xml
<DataGrid
  Margin="0,8,0,0"
  AutoGenerateColumns="False"
  ItemsSource="{Binding DataGridItemCollection}">
  <DataGrid.Columns>
      <DataGridTextColumn Binding="{Binding FirstName}" Header="First Name" />
      <DataGridTextColumn Binding="{Binding LastName}" Header="Last Name" />
      <!--  The Email property contains a URI.  For example "mailto:lucy0@adventure-works.com"  -->
      <DataGridHyperlinkColumn
          Binding="{Binding Email}"
          ContentBinding="{Binding Email}"
          Header="Email" />
      <DataGridCheckBoxColumn Binding="{Binding IsMember}" Header="Member?" />
      <DataGridComboBoxColumn Header="Order Status" SelectedItemBinding="{Binding Status}" />
  </DataGrid.Columns>
</DataGrid>
```

## VirtualizingWrapPanel
```xml
<ListView
  Height="300"
  ItemsSource="{Binding BrushCollection, Mode=OneWay}"
  SelectedIndex="0">
  <ListView.ItemTemplate>
      <DataTemplate DataType="Brush">
          <Border
              Width="80"
              Height="80"
              Background="{Binding}"
              CornerRadius="4">
              <wpfui:SymbolIcon FontSize="25" Symbol="Fluent24" />
          </Border>
      </DataTemplate>
  </ListView.ItemTemplate>
  <ListView.ItemsPanel>
      <ItemsPanelTemplate>
          <wpfui:VirtualizingWrapPanel
              Orientation="Horizontal"
              SpacingMode="Uniform"
              StretchItems="False" />
      </ItemsPanelTemplate>
  </ListView.ItemsPanel>
</ListView>
```

## VirtualizingItemsControl
```xml
<wpfui:VirtualizingItemsControl
  Height="300"
  Foreground="{DynamicResource TextFillColorSecondaryBrush}"
  ItemsSource="{Binding BrushCollection, Mode=OneWay}"
  VirtualizingPanel.CacheLengthUnit="Pixel">
  <ItemsControl.ItemTemplate>
    <DataTemplate DataType="Brush">
      <wpfui:Button
        Width="80"
        Height="80"
        Margin="2"
        Padding="0"
        Appearance="Secondary"
        Background="{Binding}"
        FontSize="25"
        Icon="Fluent24" />
    </DataTemplate>
  </ItemsControl.ItemTemplate>
</wpfui:VirtualizingItemsControl>
```