// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using Wpf.Ui.Markup;
using MessageBoxButton = Wpf.Ui.Controls.MessageBoxButton;
using MessageBoxResult = Wpf.Ui.Controls.MessageBoxResult;
using UiMessageBox = Wpf.Ui.Controls.MessageBox;

namespace Wpf.Ui.UnitTests.Controls;

public sealed class MessageBoxTests
{
    [Theory]
    [InlineData(null, MessageBoxButton.Primary)]
    [InlineData(MessageBoxButton.Primary, MessageBoxButton.Primary)]
    [InlineData(MessageBoxButton.Secondary, MessageBoxButton.Secondary)]
    [InlineData(MessageBoxButton.Close, MessageBoxButton.Close)]
    public Task InitialFocus_UsesRequestedButton(MessageBoxButton? requested, MessageBoxButton expected)
    {
        return RunOnDispatcher(async () =>
        {
            TestMessageBox messageBox = CreateMessageBox();
            messageBox.DefaultFocusedButton = requested;
            await WithMessageBox(
                messageBox,
                () =>
                {
                    Assert.Same(GetButton(messageBox, expected), Keyboard.FocusedElement);
                    Assert.True(GetButton(messageBox, MessageBoxButton.Primary).IsDefault);
                    Assert.False(GetButton(messageBox, MessageBoxButton.Secondary).IsDefault);
                    Assert.False(GetButton(messageBox, MessageBoxButton.Close).IsDefault);
                    Assert.True(GetButton(messageBox, MessageBoxButton.Close).IsCancel);
                    return Task.CompletedTask;
                }
            );
        });
    }

    [Theory]
    [InlineData(MessageBoxButton.Primary, MessageBoxButton.Secondary)]
    [InlineData(MessageBoxButton.Secondary, MessageBoxButton.Primary)]
    [InlineData(MessageBoxButton.Close, MessageBoxButton.Primary)]
    public Task InitialFocus_WhenRequestedButtonIsDisabled_UsesFallback(
        MessageBoxButton requested,
        MessageBoxButton expected
    )
    {
        return RunOnDispatcher(async () =>
        {
            TestMessageBox messageBox = CreateMessageBox();
            messageBox.DefaultFocusedButton = requested;
            messageBox.IsPrimaryButtonEnabled = requested != MessageBoxButton.Primary;
            messageBox.IsSecondaryButtonEnabled = requested != MessageBoxButton.Secondary;
            messageBox.IsCloseButtonEnabled = requested != MessageBoxButton.Close;
            await WithMessageBox(
                messageBox,
                () =>
                {
                    Assert.Same(GetButton(messageBox, expected), Keyboard.FocusedElement);
                    return Task.CompletedTask;
                }
            );
        });
    }

    [Theory]
    [InlineData(Visibility.Hidden)]
    [InlineData(Visibility.Collapsed)]
    public Task InitialFocus_WhenRequestedButtonIsInvisible_UsesFallback(Visibility visibility)
    {
        return RunOnDispatcher(async () =>
        {
            TestMessageBox messageBox = CreateMessageBox();
            messageBox.DefaultFocusedButton = MessageBoxButton.Secondary;
            messageBox.Loaded += (_, _) =>
                GetButton(messageBox, MessageBoxButton.Secondary).Visibility = visibility;
            await WithMessageBox(
                messageBox,
                () =>
                {
                    Assert.Same(GetButton(messageBox, MessageBoxButton.Primary), Keyboard.FocusedElement);
                    return Task.CompletedTask;
                }
            );
        });
    }

    [Fact]
    public Task InitialFocus_WhenPrimaryAndSecondaryAreDisabled_UsesClose()
    {
        return RunOnDispatcher(async () =>
        {
            TestMessageBox messageBox = CreateMessageBox();
            messageBox.IsPrimaryButtonEnabled = false;
            messageBox.IsSecondaryButtonEnabled = false;
            await WithMessageBox(
                messageBox,
                () =>
                {
                    Assert.Same(GetButton(messageBox, MessageBoxButton.Close), Keyboard.FocusedElement);
                    return Task.CompletedTask;
                }
            );
        });
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public Task Initialization_WithExplicitFocus_PreservesContentFocus(bool useContentRendered)
    {
        return RunOnDispatcher(async () =>
        {
            TextBox textBox = new();
            TestMessageBox messageBox = CreateMessageBox();
            messageBox.Content = textBox;
            messageBox.DefaultFocusedButton = MessageBoxButton.Close;
            if (useContentRendered)
            {
                messageBox.ContentRendered += (_, _) => textBox.Focus();
            }
            else
            {
                messageBox.Loaded += (_, _) => textBox.Focus();
            }

            await WithMessageBox(
                messageBox,
                () =>
                {
                    Assert.Same(textBox, Keyboard.FocusedElement);
                    return Task.CompletedTask;
                }
            );
        });
    }

    [Fact]
    public Task InitialFocus_WithLogicalFocus_PreservesContentFocus()
    {
        return RunOnDispatcher(async () =>
        {
            TextBox textBox = new();
            TestMessageBox messageBox = CreateMessageBox();
            messageBox.Content = textBox;
            FocusManager.SetFocusedElement(messageBox, textBox);
            await WithMessageBox(
                messageBox,
                () =>
                {
                    Assert.Same(textBox, Keyboard.FocusedElement);
                    return Task.CompletedTask;
                }
            );
        });
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public Task ContentRendered_WithInvalidLogicalFocus_UsesFallback(bool hidden)
    {
        return RunOnDispatcher(async () =>
        {
            TextBox textBox = new();
            TestMessageBox messageBox = CreateMessageBox();
            messageBox.Content = textBox;
            await WithMessageBox(
                messageBox,
                async () =>
                {
                    Keyboard.ClearFocus();
                    if (hidden)
                    {
                        textBox.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        textBox.IsEnabled = false;
                    }

                    FocusManager.SetFocusedElement(messageBox, textBox);
                    messageBox.RenderContent();
                    await DrainDispatcher();
                    Assert.Same(GetButton(messageBox, MessageBoxButton.Primary), Keyboard.FocusedElement);
                }
            );
        });
    }

    [Fact]
    public Task ContentRendered_WithExternalLogicalFocus_UsesFallback()
    {
        return RunOnDispatcher(async () =>
        {
            TextBox externalTextBox = new();
            Window externalWindow = new()
            {
                Content = externalTextBox,
                Width = 300,
                Height = 200,
            };
            externalWindow.Show();
            try
            {
                TestMessageBox messageBox = CreateMessageBox();
                await WithMessageBox(
                    messageBox,
                    async () =>
                    {
                        Keyboard.ClearFocus();
                        FocusManager.SetFocusedElement(messageBox, externalTextBox);
                        messageBox.Activate();
                        Assert.True(messageBox.IsActive);
                        messageBox.RenderContent();
                        await DrainDispatcher();
                        Assert.Same(GetButton(messageBox, MessageBoxButton.Primary), Keyboard.FocusedElement);
                    }
                );
            }
            finally
            {
                externalWindow.Close();
            }
        });
    }

    [Fact]
    public Task TemplateReapplication_PreservesExplicitContentFocus()
    {
        return RunOnDispatcher(async () =>
        {
            TextBox textBox = new();
            TestMessageBox messageBox = CreateMessageBox();
            messageBox.Content = textBox;
            await WithMessageBox(
                messageBox,
                async () =>
                {
                    Assert.Same(textBox, Keyboard.Focus(textBox));
                    ControlTemplate template = messageBox.Template;
                    messageBox.Template = null;
                    messageBox.ApplyTemplate();
                    messageBox.Template = template;
                    messageBox.ApplyTemplate();
                    await DrainDispatcher();
                    Assert.Same(textBox, Keyboard.FocusedElement);
                }
            );
        });
    }

    [Fact]
    public Task TemplateReapplication_WhenButtonIsReplaced_RestoresRequestedFocus()
    {
        return RunOnDispatcher(async () =>
        {
            TestMessageBox messageBox = CreateMessageBox();
            messageBox.DefaultFocusedButton = MessageBoxButton.Secondary;
            await WithMessageBox(
                messageBox,
                async () =>
                {
                    Wpf.Ui.Controls.Button oldButton = GetButton(messageBox, MessageBoxButton.Secondary);
                    ControlTemplate template = messageBox.Template;
                    messageBox.Template = null;
                    messageBox.ApplyTemplate();
                    messageBox.Template = template;
                    messageBox.ApplyTemplate();
                    await DrainDispatcher();
                    Assert.NotSame(oldButton, GetButton(messageBox, MessageBoxButton.Secondary));
                    Assert.Same(GetButton(messageBox, MessageBoxButton.Secondary), Keyboard.FocusedElement);
                }
            );
        });
    }

    [Fact]
    public Task InitialFocus_DoesNotReplaceDefaultFocusedButtonBinding()
    {
        return RunOnDispatcher(async () =>
        {
            TestMessageBox messageBox = CreateMessageBox();
            BindingOperations.SetBinding(
                messageBox,
                UiMessageBox.DefaultFocusedButtonProperty,
                new Binding { Source = MessageBoxButton.Secondary }
            );
            await WithMessageBox(
                messageBox,
                () =>
                {
                    Assert.Same(GetButton(messageBox, MessageBoxButton.Secondary), Keyboard.FocusedElement);
                    Assert.True(
                        BindingOperations.IsDataBound(messageBox, UiMessageBox.DefaultFocusedButtonProperty)
                    );
                    return Task.CompletedTask;
                }
            );
        });
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public Task DefaultPositioning_DoesNotOptIntoOwnerCentering(bool specifyOwner)
    {
        return RunOnDispatcher(async () =>
        {
            Window owner = new()
            {
                Width = 700,
                Height = 500,
                Left = 80,
                Top = 60,
                ShowInTaskbar = false,
            };
            owner.Show();
            try
            {
                await DrainDispatcher();
                TestMessageBox messageBox = CreateMessageBox();
                if (specifyOwner)
                {
                    messageBox.Owner = owner;
                }

                await WithMessageBox(
                    messageBox,
                    () =>
                    {
                        Assert.Same(specifyOwner ? owner : null, messageBox.Owner);
                        Assert.Equal(WindowStartupLocation.Manual, messageBox.WindowStartupLocation);
                        Assert.InRange(
                            Math.Abs(
                                ((SystemParameters.PrimaryScreenWidth - messageBox.Width) / 2)
                                    - messageBox.Left
                            ),
                            0,
                            1
                        );
                        Assert.InRange(
                            Math.Abs(
                                ((SystemParameters.PrimaryScreenHeight - messageBox.Height) / 2)
                                    - messageBox.Top
                            ),
                            0,
                            1
                        );
                        return Task.CompletedTask;
                    }
                );
            }
            finally
            {
                owner.Close();
            }
        });
    }

    [Theory]
    [InlineData(WindowState.Normal)]
    [InlineData(WindowState.Maximized)]
    public Task CenterOwner_UsesDisplayedOwnerBounds(WindowState windowState)
    {
        return RunOnDispatcher(async () =>
        {
            Window owner = new()
            {
                Width = 700,
                Height = 500,
                Left = 80,
                Top = 60,
                WindowStyle = WindowStyle.None,
                ShowInTaskbar = false,
            };
            owner.Show();
            owner.WindowState = windowState;
            try
            {
                await DrainDispatcher();
                TestMessageBox messageBox = CreateMessageBox();
                messageBox.Owner = owner;
                messageBox.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                await WithMessageBox(
                    messageBox,
                    () =>
                    {
                        Point ownerCenter = GetWindowCenter(owner);
                        Point messageBoxCenter = GetWindowCenter(messageBox);
                        Assert.InRange(Math.Abs(ownerCenter.X - messageBoxCenter.X), 0, 3);
                        Assert.InRange(Math.Abs(ownerCenter.Y - messageBoxCenter.Y), 0, 3);
                        return Task.CompletedTask;
                    }
                );
            }
            finally
            {
                owner.Close();
            }
        });
    }

    [Fact]
    public Task ManualPositioning_PreservesExplicitCoordinates()
    {
        return RunOnDispatcher(async () =>
        {
            TestMessageBox messageBox = CreateMessageBox();
            messageBox.WindowStartupLocation = WindowStartupLocation.Manual;
            messageBox.Left = 40;
            messageBox.Top = 60;
            await WithMessageBox(
                messageBox,
                () =>
                {
                    Assert.Equal(WindowStartupLocation.Manual, messageBox.WindowStartupLocation);
                    Assert.Equal(40, messageBox.Left);
                    Assert.Equal(60, messageBox.Top);
                    return Task.CompletedTask;
                }
            );
        });
    }

    [Fact]
    public Task ContentRendered_WhenKeyboardFocusIsNull_FocusesPrimaryButton()
    {
        return RunOnDispatcher(async () =>
        {
            TestMessageBox messageBox = CreateMessageBox();
            Task<MessageBoxResult> result = messageBox.ShowDialogAsync(false);
            try
            {
                await DrainDispatcher();
                Keyboard.ClearFocus();
                FocusManager.SetFocusedElement(messageBox, null);
                Assert.Null(Keyboard.FocusedElement);

                messageBox.RenderContent();
                await DrainDispatcher();

                Assert.Same(GetButton(messageBox, MessageBoxButton.Primary), Keyboard.FocusedElement);
            }
            finally
            {
                ((Window)messageBox).Close();
            }

            Assert.Equal(MessageBoxResult.None, await result);
        });
    }

    [Fact]
    public Task Loaded_WithExplicitContentFocus_PreservesTextBoxFocus()
    {
        return RunOnDispatcher(async () =>
        {
            TextBox textBox = new();
            TestMessageBox messageBox = CreateMessageBox();
            messageBox.Content = textBox;
            Task<MessageBoxResult> result = messageBox.ShowDialogAsync(false);
            try
            {
                await DrainDispatcher();
                Assert.Same(textBox, Keyboard.Focus(textBox));

                messageBox.LoadContent();
                await DrainDispatcher();

                Assert.Same(textBox, Keyboard.FocusedElement);
            }
            finally
            {
                ((Window)messageBox).Close();
            }

            Assert.Equal(MessageBoxResult.None, await result);
        });
    }

    [Fact]
    public Task NonModal_WhenOwnerCloses_CompletesWithNone()
    {
        return RunOnDispatcher(async () =>
        {
            Window owner = new()
            {
                Width = 300,
                Height = 200,
                ShowInTaskbar = false,
            };
            TestMessageBox messageBox = CreateMessageBox();
            owner.Show();
            messageBox.Owner = owner;
            Task<MessageBoxResult> result = messageBox.ShowDialogAsync(false);
            await DrainDispatcher();
            bool closed = false;
            bool closing = false;
            messageBox.Closed += (_, _) => closed = true;
            messageBox.Closing += (_, _) => closing = true;

            owner.Close();
            await DrainDispatcher();

            Assert.True(closed);
            Assert.False(closing);
            Assert.True(result.IsCompleted, "Closing the owner must complete ShowDialogAsync.");
            Assert.Equal(MessageBoxResult.None, await result);
        });
    }

    [Fact]
    public Task PrimaryButton_RemainsDefault_WhenSecondaryIsInitiallyFocused()
    {
        return RunOnDispatcher(async () =>
        {
            TextBox textBox = new();
            TestMessageBox messageBox = CreateMessageBox();
            messageBox.Content = textBox;
            messageBox.DefaultFocusedButton = MessageBoxButton.Secondary;
            Task<MessageBoxResult> result = messageBox.ShowDialogAsync(false);
            try
            {
                await DrainDispatcher();
                Assert.Same(GetButton(messageBox, MessageBoxButton.Secondary), Keyboard.FocusedElement);
                Assert.Same(textBox, Keyboard.Focus(textBox));
                Assert.True(GetButton(messageBox, MessageBoxButton.Primary).IsDefault);

                AccessKeyManager.ProcessKey(null, "\r", false);
                await DrainDispatcher();

                Assert.True(result.IsCompleted);
                Assert.Equal(MessageBoxResult.Primary, await result);
            }
            finally
            {
                if (messageBox.IsVisible)
                {
                    ((Window)messageBox).Close();
                }
            }
        });
    }

    [Theory]
    [InlineData(MessageBoxButton.Primary, MessageBoxResult.Primary)]
    [InlineData(MessageBoxButton.Secondary, MessageBoxResult.Secondary)]
    [InlineData(MessageBoxButton.Close, MessageBoxResult.None)]
    public Task Enter_OnFocusedButton_ReturnsButtonResult(MessageBoxButton button, MessageBoxResult expected)
    {
        return RunOnDispatcher(async () =>
        {
            TestMessageBox messageBox = CreateMessageBox();
            messageBox.DefaultFocusedButton = button;
            Task<MessageBoxResult> result = messageBox.ShowDialogAsync(false);
            try
            {
                await DrainDispatcher();
                Wpf.Ui.Controls.Button target = GetButton(messageBox, button);
                Assert.Same(target, Keyboard.FocusedElement);
                RaiseKey(target, Key.Enter, Keyboard.KeyDownEvent);
                Assert.Equal(
                    expected,
                    await result.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken)
                );
            }
            finally
            {
                if (messageBox.IsVisible)
                {
                    ((Window)messageBox).Close();
                }
            }
        });
    }

    [Fact]
    public Task Space_OnFocusedSecondaryButton_ReturnsSecondary()
    {
        return RunOnDispatcher(async () =>
        {
            TestMessageBox messageBox = CreateMessageBox();
            messageBox.DefaultFocusedButton = MessageBoxButton.Secondary;
            Task<MessageBoxResult> result = messageBox.ShowDialogAsync(false);
            try
            {
                await DrainDispatcher();
                Wpf.Ui.Controls.Button button = GetButton(messageBox, MessageBoxButton.Secondary);
                RaiseKey(button, Key.Space, Keyboard.KeyDownEvent);
                RaiseKey(button, Key.Space, Keyboard.KeyUpEvent);
                Assert.Equal(
                    MessageBoxResult.Secondary,
                    await result.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken)
                );
            }
            finally
            {
                if (messageBox.IsVisible)
                {
                    ((Window)messageBox).Close();
                }
            }
        });
    }

    [Fact]
    public Task Escape_FromTextBox_ReturnsNone()
    {
        return RunOnDispatcher(async () =>
        {
            TextBox textBox = new();
            TestMessageBox messageBox = CreateMessageBox();
            messageBox.Content = textBox;
            Task<MessageBoxResult> result = messageBox.ShowDialogAsync(false);
            try
            {
                await DrainDispatcher();
                Assert.Same(textBox, Keyboard.Focus(textBox));
                AccessKeyManager.ProcessKey(null, "\x1b", false);
                Assert.Equal(
                    MessageBoxResult.None,
                    await result.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken)
                );
            }
            finally
            {
                if (messageBox.IsVisible)
                {
                    ((Window)messageBox).Close();
                }
            }
        });
    }

    [Fact]
    public Task TabNavigation_VisitsButtonsInBothDirections()
    {
        return RunOnDispatcher(async () =>
        {
            TestMessageBox messageBox = CreateMessageBox();
            await WithMessageBox(
                messageBox,
                () =>
                {
                    Wpf.Ui.Controls.Button primary = GetButton(messageBox, MessageBoxButton.Primary);
                    Wpf.Ui.Controls.Button secondary = GetButton(messageBox, MessageBoxButton.Secondary);
                    Wpf.Ui.Controls.Button close = GetButton(messageBox, MessageBoxButton.Close);
                    Assert.True(primary.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next)));
                    Assert.Same(secondary, Keyboard.FocusedElement);
                    Assert.True(secondary.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next)));
                    Assert.Same(close, Keyboard.FocusedElement);
                    Assert.True(close.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous)));
                    Assert.Same(secondary, Keyboard.FocusedElement);
                    Assert.True(secondary.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous)));
                    Assert.Same(primary, Keyboard.FocusedElement);
                    return Task.CompletedTask;
                }
            );
        });
    }

    [Fact]
    public Task InitialFocus_DoesNotActivateInactiveMessageBox()
    {
        return RunOnDispatcher(async () =>
        {
            TextBox textBox = new();
            Window otherWindow = new()
            {
                Content = textBox,
                Width = 300,
                Height = 200,
            };
            TestMessageBox messageBox = CreateMessageBox();
            await WithMessageBox(
                messageBox,
                async () =>
                {
                    otherWindow.Show();
                    try
                    {
                        Assert.Same(textBox, Keyboard.Focus(textBox));
                        Assert.False(messageBox.IsActive);
                        messageBox.LoadContent();
                        messageBox.RenderContent();
                        await DrainDispatcher();
                        Assert.Same(textBox, Keyboard.FocusedElement);
                        Assert.False(messageBox.IsActive);
                    }
                    finally
                    {
                        otherWindow.Close();
                    }
                }
            );
        });
    }

    [Theory]
    [InlineData(MessageBoxButton.Primary, MessageBoxResult.Primary)]
    [InlineData(MessageBoxButton.Secondary, MessageBoxResult.Secondary)]
    [InlineData(MessageBoxButton.Close, MessageBoxResult.None)]
    public Task Modal_ButtonResultIsPreserved(MessageBoxButton button, MessageBoxResult expected)
    {
        return RunOnDispatcher(async () =>
        {
            TestMessageBox messageBox = CreateMessageBox();
            messageBox.DefaultFocusedButton = button;
            IInputElement? focusedElement = null;
            Wpf.Ui.Controls.Button? expectedButton = null;
            messageBox.ContentRendered += (_, _) =>
                messageBox.Dispatcher.BeginInvoke(
                    DispatcherPriority.ApplicationIdle,
                    new Action(() =>
                    {
                        expectedButton = GetButton(messageBox, button);
                        focusedElement = Keyboard.FocusedElement;
                        messageBox.TemplateButtonCommand.Execute(button);
                    })
                );
            Assert.Equal(expected, await messageBox.ShowDialogAsync());
            Assert.Same(expectedButton, focusedElement);
        });
    }

    [Fact]
    public Task Closing_WhenCancelled_DoesNotCompleteTask()
    {
        return RunOnDispatcher(async () =>
        {
            TestMessageBox messageBox = CreateMessageBox();
            Task<MessageBoxResult> result = messageBox.ShowDialogAsync(false);
            bool cancel = true;
            messageBox.Closing += (_, args) => args.Cancel = cancel;
            try
            {
                await DrainDispatcher();
                ((Window)messageBox).Close();
                await DrainDispatcher();
                Assert.False(result.IsCompleted);
                Assert.True(messageBox.IsVisible);
            }
            finally
            {
                cancel = false;
                ((Window)messageBox).Close();
            }

            Assert.Equal(MessageBoxResult.None, await result);
        });
    }

    [Fact]
    public Task InitialFocus_WhenShownInactive_IsSetAfterActivation()
    {
        return RunOnDispatcher(async () =>
        {
            TestMessageBox messageBox = CreateMessageBox();
            messageBox.ShowActivated = false;
            messageBox.DefaultFocusedButton = MessageBoxButton.Secondary;
            await WithMessageBox(
                messageBox,
                async () =>
                {
                    messageBox.Activate();
                    await DrainDispatcher();
                    Assert.Same(GetButton(messageBox, MessageBoxButton.Secondary), Keyboard.FocusedElement);
                }
            );
        });
    }

    [Fact]
    public Task ContentRendered_WhenFocusedCloseIsDisabled_UsesFallback()
    {
        return RunOnDispatcher(async () =>
        {
            TestMessageBox messageBox = CreateMessageBox();
            messageBox.DefaultFocusedButton = MessageBoxButton.Close;
            await WithMessageBox(
                messageBox,
                async () =>
                {
                    Assert.Same(GetButton(messageBox, MessageBoxButton.Close), Keyboard.FocusedElement);
                    messageBox.IsCloseButtonEnabled = false;
                    messageBox.RenderContent();
                    await DrainDispatcher();
                    Assert.Same(GetButton(messageBox, MessageBoxButton.Primary), Keyboard.FocusedElement);
                }
            );
        });
    }

    private static void RaiseKey(UIElement target, Key key, RoutedEvent routedEvent)
    {
        target.RaiseEvent(
            new KeyEventArgs(
                Keyboard.PrimaryDevice,
                PresentationSource.FromVisual(target),
                Environment.TickCount,
                key
            )
            {
                RoutedEvent = routedEvent,
            }
        );
    }

    private static TestMessageBox CreateMessageBox()
    {
        TestMessageBox messageBox = new()
        {
            Title = "MessageBox regression test",
            Content = "Content",
            PrimaryButtonText = "Primary",
            SecondaryButtonText = "Secondary",
            CloseButtonText = "Close",
            ShowInTaskbar = false,
        };
        messageBox.Resources.MergedDictionaries.Add(new ThemesDictionary());
        messageBox.Resources.MergedDictionaries.Add(new ControlsDictionary());
        messageBox.Style = (Style)messageBox.Resources[typeof(UiMessageBox)];
        return messageBox;
    }

    private static Wpf.Ui.Controls.Button GetButton(UiMessageBox messageBox, MessageBoxButton button)
    {
        return Assert.IsType<Wpf.Ui.Controls.Button>(
            messageBox.Template.FindName($"PART_MessageBox{button}Button", messageBox)
        );
    }

    private static async Task DrainDispatcher()
    {
        await Dispatcher.Yield(DispatcherPriority.ApplicationIdle);
    }

    private static async Task WithMessageBox(TestMessageBox messageBox, Func<Task> test)
    {
        Task<MessageBoxResult> result = messageBox.ShowDialogAsync(false);
        try
        {
            await DrainDispatcher();
            await test();
        }
        finally
        {
            if (messageBox.IsVisible)
            {
                ((Window)messageBox).Close();
            }
        }

        await result.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
    }

    private static Task RunOnDispatcher(Func<Task> test)
    {
        TaskCompletionSource completion = new(TaskCreationOptions.RunContinuationsAsynchronously);
        Thread thread = new(() =>
        {
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
            SynchronizationContext.SetSynchronizationContext(
                new DispatcherSynchronizationContext(dispatcher)
            );
            _ = dispatcher.InvokeAsync(async () =>
            {
                try
                {
                    await test();
                    completion.SetResult();
                }
                catch (Exception exception)
                {
                    completion.SetException(exception);
                }
                finally
                {
                    dispatcher.BeginInvokeShutdown(DispatcherPriority.Send);
                }
            });
            Dispatcher.Run();
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.IsBackground = true;
        thread.Start();
        return completion.Task.WaitAsync(TimeSpan.FromSeconds(30), TestContext.Current.CancellationToken);
    }

    private static Point GetWindowCenter(Window window)
    {
        Assert.True(NativeMethods.GetWindowRect(new WindowInteropHelper(window).Handle, out WindowRect rect));
        return new Point((rect.Left + rect.Right) / 2.0, (rect.Top + rect.Bottom) / 2.0);
    }

    private static class NativeMethods
    {
        [DllImport("user32.dll", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr window, out WindowRect rect);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct WindowRect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    private sealed class TestMessageBox : UiMessageBox
    {
        public void RenderContent() => OnContentRendered();

        public void LoadContent() => OnLoaded();
    }
}
