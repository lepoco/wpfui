# NavigationView empty-space pane toggle

## Design and compatibility

`NavigationView.IsPaneToggleOnEmptySpaceEnabled` is a Boolean dependency property,
defaulting to `false`. It can be changed or bound at runtime. Enable it on the
concrete `NavigationView`; `INavigationView` has no new members, so existing
implementations remain compatible.

The existing hamburger calls `OnToggleButtonClick`, which uses
`SetCurrentValue(IsPaneOpenProperty, !IsPaneOpen)`. Empty-space clicks use this
same method, preserving bindings, `PaneOpened` / `PaneClosed`, menu state and
visual-state animations. This version of WPF UI has no `PaneOpening`,
`PaneClosing`, or cancellable pane transition API. Navigation cancellation is
separate and is unchanged.

The shared left template has five pane rows: fixed header, top separator,
scrollable menu, footer separator, and fixed footer. `Header` is in the main
content column; `PaneHeader`, search and `PaneFooter` belong to the pane.
Settings is a regular `FooterMenuItems` item.

Two optional template backgrounds identify unused space:

- `PART_PaneToggleSpace`: behind the hamburger row. With `PaneTitle`, the
  existing hamburger can occupy the full row; that button keeps its behavior.
- `PART_PaneMenuSpace`: around the top-aligned menu inside `PART_ScrollViewer`.
  It fills the remaining viewport below short menus in both open and compact
  states. Item insertion/removal and resizing use normal WPF layout.

These backgrounds become transparent when the option is enabled. Children and
scroll bars retain their hit targets. No foreground overlay, coordinate lookup,
or visual-tree traversal is used. Only an exact `OriginalSource` match qualifies;
item icons/text, buttons, search, custom header/footer content, flyouts and main
content do not qualify. Custom templates can omit the optional parts, in which
case the feature does nothing.

Press and release must occur on the same background, within WPF drag thresholds.
Only the first left click qualifies. Pointer movement beyond the thresholds,
wheel input, mouse leave, option/mode changes, unload, or template reapplication
cancel a pending click. Only an accepted release is marked handled. Mouse
capture and externally registered input handlers are unnecessary.

Supported modes are `Left` and `LeftMinimal`. `LeftFluent` has no compact/open
visual states; it, `Top` and `Bottom` ignore empty-space toggles. `LeftCompact`
does not exist on this main branch and was not added.

## Automated checks

Run on Windows with .NET 10 and an unlocked interactive desktop for UI tests.
Do not operate the mouse/keyboard while the FlaUI suite runs.

```powershell
dotnet build src/Wpf.Ui -m:1 -p:GeneratePackageOnBuild=false
dotnet build tests/Wpf.Ui.UnitTests -m:1
dotnet run --project tests/Wpf.Ui.UnitTests --no-build -- --output Detailed
dotnet build tests/Wpf.Ui.Gallery.IntegrationTests -m:1
dotnet run --project tests/Wpf.Ui.Gallery.IntegrationTests --no-build -- --output Detailed
```

Unit tests use STA threads, real templates, routed mouse events and real window
hit tests. They cover opt-in/default/off, both toggle directions, themes, menu
and footer exclusion, controls, wheel input, unmatched releases, mode/template
changes, event counts and two-way bindings. Hit tests wait for the existing pane
animation to finish. The Gallery UI test uses actual mouse input and item bounds
to cover A/B/C, opt-in/off, controls, drag, scrolling and mode changes.

## Gallery verification

Open **Navigation > NavigationView**. The optional feature example follows the
existing Bottom example at the end of the page. Scroll until its checkbox, pane,
`IsPaneOpen` and `PaneDisplayMode` readouts are visible. The checkbox starts unchecked.

1. Click beside the hamburger and below the menu with the checkbox off: no change.
2. Enable the checkbox. Click A (beside hamburger): open to compact, once.
3. Click C (below the last compact icon, above Settings): compact to open, once.
4. Click B (below the expanded menu, above Settings): open to compact, once.
5. Use the hamburger to open/close. Click Dashboard, Items, Settings, search,
   Footer action and main content: their normal behavior remains available.
6. Drag from blank space and use the mouse wheel: no pane transition.
7. Select LeftMinimal and repeat. Select Top, Bottom and LeftFluent: blank space
   does not toggle. Return to Left and verify normal toggling, then disable it.

Additional manual coverage for release review:

- Repeat in Light and Dark themes and with different system DPI/scaling values.
- Resize, maximize, minimize/restore, and move between monitors with different DPI.
- Add/remove menu items live; use enough items to overflow, and operate the scroll
  bar thumb and track. Check that footer items remain fixed and usable.
- Remove/re-add footer items (including Settings), `PaneHeader` and `PaneFooter`.
  Put interactive custom controls in the header/footer and exercise each control.
- Open a child-menu flyout while compact and verify its items never toggle the pane.
- If using a custom control template, retain the optional named background parts
  behind the intended unused space. Never overlay interactive controls.

Fast Explorer can opt in with
`<ui:NavigationView IsPaneToggleOnEmptySpaceEnabled="True" />` after updating its
WPF UI reference. Test its custom templates and pane bindings. This change does
not include the separate LeftCompact feature or any Fast Explorer changes.

## Verification record (2026-10-24, Windows)

- Library build: succeeded for net10.0-windows, net9.0-windows, net8.0-windows,
  net481, net472 and net462. Existing analyzer and NU1902 warnings remain.
- Gallery and integration test projects: build succeeded.
- Complete unit suite: 31 passed, 0 failed, 0 skipped (11s); 25 cases cover this
  feature and 6 are existing regression cases.
- Complete Gallery UI suite: 9 passed, 0 failed, 0 skipped (2m 19s).
- A/B/C were also clicked directly in the running Gallery. The added UI test
  The example is placed after the existing Bottom example. UI tests scroll to
  this section before exercising the feature.
- During test development, hit tests initially ran before the pane animation
  completed. The UI mode test also needed to close the ComboBox popup after UIA
  selection, so its next click reached the pane. These were test setup issues.
  The AutoSuggestBox test attaches its control after creating the window, because
  AutoSuggestBox requires an HWND when its template installs a window hook.
- The additional release-review matrix above, including multiple physical DPI
  configurations and monitors, remains manual coverage rather than a claim of
  exhaustive verification on this machine.

## Follow-up: display-mode animation teardown

A real-window test reproduced the reported `DoubleAnimation` exception on Top
and Bottom selection: removing the left template reset `PaneGrid.Width` to Auto
(`NaN`) while its old width animation was still attached. The earlier disconnected
mode test did not exercise template teardown with running animation clocks.

Before template removal or entry into a mode without pane animation,
NavigationView now selects the empty `PaneAuto` state and immediately detaches
the old width animation. Storyboard removal alone is deferred until an animation
tick and does not prevent a synchronous template invalidation exception. Left
pane state is restored after style triggers have restored `OpenPaneLength`, and
after a new template is applied.

The regression cases cover open/compact panes, Top, Bottom and LeftFluent,
completed/in-progress animation, and return to Left with the expected pane width.
The Gallery UI tests select actual popup items with the mouse and assert the
bound `PaneDisplayMode` readout, in addition to the pane's open state. Separate
cases cover Top and Bottom from both open and compact LeftMinimal panes.

Follow-up verification: the complete unit suite passes 43 cases with no failures
or skips. The library builds for all six target frameworks; the compatibility
build used `-p:RunAnalyzers=false -p:GeneratePackageOnBuild=false`. STA test
dispatchers are explicitly shut down after each case so window/animation resources
do not leave foreground threads running at test-process exit.
The complete Gallery UI suite passes 13 cases with no failures or skips (3m 58s),
including all four Top/Bottom mouse-selection cases and the existing pane input,
navigation, search, dialog and window tests. Gallery and integration test output
assemblies were checked to contain the same updated Wpf.Ui library as the build.

After placing the optional example after Bottom at the end of the page, Gallery
and the integration test project were rebuilt successfully. The targeted
`EmptySpaceClicks_RespectControlsAndDisplayMode` UI test passed (1 passed, 0 failed,
0 skipped), including scrolling to the example in its new location. All five
example contents were compared with the previous revision and remain identical;
only their order changed.
