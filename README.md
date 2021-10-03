# WPF UI
[Created with ❤ in Poland by lepo.co](https://dev.lepo.co/)  
A simple way to make your application written in WPF keep up with modern design trends. Library changes the base elements like Window, Page or Button, and also includes additional controls like Navigation, ToggleButton or Snackbar.

[![GitHub license](https://img.shields.io/github/license/lepoco/wpfui)](https://github.com/lepoco/wpfui/blob/master/LICENSE) [![NET](https://img.shields.io/badge/.NET-5.0.0-red)](https://github.com/lepoco/wpfui/blob/main/WPFUI/WPFUI.csproj) [![Nuget](https://img.shields.io/nuget/v/WPF-UI)](https://www.nuget.org/packages/WPF-UI/) [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/WPF-UI?label=nuget-pre)](https://www.nuget.org/packages/WPF-UI/) [![Nuget](https://img.shields.io/nuget/dt/WPF-UI?label=nuget-downloads)](https://www.nuget.org/packages/WPF-UI/)

![Screen-1](https://github.com/lepoco/wpfui/blob/main/.github/assets/screen-1.png?raw=true)

## Microsoft Property
Design of the interface, choice of colors and the appearance of the controls were inspired by projects made by Microsoft for Windows 11.  
The WPFUI.Demo app includes icons from Shell32 for Windows 11. These icons are the legal property of Microsoft and you may not use them in your own app without permission. They are used here as an example of creating tools for Microsoft systems.

## What's included?
| Name| Framework | Build Status |
| --- | --- | --- | 
| **WPFUI**<br />Library that allows you to use all features in your own application | .NET 5.0 Windows,<br/>.NET Core 3.1<br/>.NET Framework 4.8 | [![Build status](https://github.com/rapiddev/MaterialWPF/workflows/CI/badge.svg)](https://github.com/rapiddev/MaterialWPF/actions) | 
| **WPFUI.Demo**<br />An application written in WPF .NET 5 where you can test the features. | .NET 5.0 Windows | [![Build status](https://github.com/rapiddev/MaterialWPF/workflows/CI/badge.svg)](https://github.com/rapiddev/MaterialWPF/actions) |

## Custom controls
| Control | Namespace | Description |
| --- | --- | --- |
| **Button** | [WPFUI.Controls](https://github.com/lepoco/WPFUI/tree/master/WPFUI/Controls) | Class with which you can send a Toast to the Windows notification center. |
| **CardAction** | [WPFUI.Controls](https://github.com/lepoco/WPFUI/tree/master/WPFUI/Controls) | . |
| **CardCollapse** | [WPFUI.Controls](https://github.com/lepoco/WPFUI/tree/master/WPFUI/Controls) | A control that you can display in the middle of the application, e.g. with a "Save as" information or whatever... |
| **CardProfile** | [WPFUI.Controls](https://github.com/lepoco/WPFUI/tree/master/WPFUI/Controls) | A button that navigates to the browser window. |
| **CodeBlock** | [WPFUI.Controls](https://github.com/lepoco/WPFUI/tree/master/WPFUI/Controls) | Prepared TextBlock with "Glyph" attribute with which you can select an icon. |
| **Hyperlink** | [WPFUI.Controls](https://github.com/lepoco/WPFUI/tree/master/WPFUI/Controls) | A control that opens a website. |
| **Icon** | [WPFUI.Controls](https://github.com/lepoco/WPFUI/tree/master/WPFUI/Controls) | A collection of all [Micon](https://github.com/xtoolkit/Micon) font glyphs that you can use in an application. |
| **Navigation** | [WPFUI.Controls](https://github.com/lepoco/WPFUI/tree/master/WPFUI/Controls) | Navigation styled as UWP apps. |
| **NavigationBubble** | [WPFUI.Controls](https://github.com/lepoco/WPFUI/tree/master/WPFUI/Controls) | Navigation styled as Windows 11 Store app |
| **NavigationFluent** | [WPFUI.Controls](https://github.com/lepoco/WPFUI/tree/master/WPFUI/Controls) | Navigation styled as Windows 11 Settings app. |
| **WindowNavigation** | [WPFUI.Controls](https://github.com/lepoco/WPFUI/tree/master/WPFUI/Controls) | A set of buttons that can replace the default window navigation, giving it a new, modern look. |

## Custom tools
| Class | Namespace | Description |
| --- | --- | --- |
| **Theme** | [WPFUI](https://github.com/lepoco/WPFUI/tree/master/WPFUI) | Class with which you can change and control the application theme. |