// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WPFUI.Controls
{
    /// <summary>
    /// Small card with buttons displayed at the bottom for a short time.
    /// </summary>
    public class Snackbar : System.Windows.Controls.ContentControl
    {
        private readonly string _characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        private string _currentThread = "";

        /// <summary>
        /// Property for <see cref="Show"/>.
        /// </summary>
        public static readonly DependencyProperty ShowProperty = DependencyProperty.Register("Show",
            typeof(bool), typeof(Snackbar), new PropertyMetadata(false));

        /// <summary>
        /// Property for <see cref="Timeout"/>.
        /// </summary>
        public static readonly DependencyProperty TimeoutProperty = DependencyProperty.Register("Timeout",
            typeof(int), typeof(Snackbar), new PropertyMetadata(2000));

        /// <summary>
        /// Property for <see cref="Glyph"/>.
        /// </summary>
        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register("Glyph",
            typeof(Common.Icon), typeof(Snackbar), new PropertyMetadata(Common.Icon.Empty, OnGlyphChanged));

        /// <summary>
        /// <see cref="System.String"/> property for <see cref="Glyph"/>.
        /// </summary>
        public static readonly DependencyProperty RawGlyphProperty = DependencyProperty.Register("RawGlyph",
            typeof(string), typeof(Snackbar), new PropertyMetadata(""));

        /// <summary>
        /// Property for <see cref="IsGlyph"/>.
        /// </summary>
        public static readonly DependencyProperty IsGlyphProperty = DependencyProperty.Register("IsGlyph",
            typeof(bool), typeof(Snackbar), new PropertyMetadata(false));

        /// <summary>
        /// Property for <see cref="Title"/>.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title",
            typeof(string), typeof(Snackbar), new PropertyMetadata(""));

        /// <summary>
        /// Property for <see cref="Message"/>.
        /// </summary>
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message",
            typeof(string), typeof(Snackbar), new PropertyMetadata(""));

        // TODO: Remove
        /// <summary>
        /// Property for <see cref="SlideTransform"/>.
        /// </summary>
        public static readonly DependencyProperty SlideTransformProperty = DependencyProperty.Register("SlideTransform",
            typeof(TranslateTransform), typeof(Snackbar), new PropertyMetadata(new TranslateTransform()));

        /// <summary>
        /// Property for <see cref="ButtonCloseCommand"/>.
        /// </summary>
        public static readonly DependencyProperty ButtonCloseCommandProperty =
            DependencyProperty.Register("ButtonCloseCommand",
                typeof(Common.RelayCommand), typeof(Snackbar), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets information whether the snackbar should be displayed.
        /// </summary>
        public bool Show
        {
            get => (bool)GetValue(ShowProperty);
            set => SetValue(ShowProperty, value);
        }

        /// <summary>
        /// Time for which the snackbar is to be displayed.
        /// </summary>
        public int Timeout
        {
            get => (int)GetValue(TimeoutProperty);
            set => SetValue(TimeoutProperty, value);
        }

        /// <summary>
        /// Gets information whether the <see cref="Glyph"/> is set.
        /// </summary>
        public bool IsGlyph
        {
            get => (bool)GetValue(IsGlyphProperty);
            internal set => SetValue(IsGlyphProperty, value);
        }

        /// <summary>
        /// Gets or sets displayed <see cref="Common.Icon"/>.
        /// </summary>
        public Common.Icon Glyph
        {
            get => (Common.Icon)GetValue(GlyphProperty);
            set => SetValue(GlyphProperty, value);
        }

        /// <summary>
        /// Gets or sets the text displayed on the top of the snackbar.
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        /// <summary>
        /// Gets or sets the text displayed on the bottom of the snackbar.
        /// </summary>
        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        /// <summary>
        /// Gets or sets the text displayed on the bottom of the snackbar.
        /// </summary>
        public TranslateTransform SlideTransform
        {
            get => (TranslateTransform)GetValue(SlideTransformProperty);
            set => SetValue(SlideTransformProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="Common.RelayCommand"/> triggered after clicking close button.
        /// </summary>
        public Common.RelayCommand ButtonCloseCommand => (Common.RelayCommand)GetValue(ButtonCloseCommandProperty);

        public Snackbar()
        {
            UpdateThread();

            SetValue(ButtonCloseCommandProperty, new Common.RelayCommand(o => Hide()));
        }

        /// <summary>
        /// Shows the snackbar for the amount of time specified in <see cref="Timeout"/>.
        /// </summary>
        public void Expand()
        {
            ShowComponent();
        }

        /// <summary>
        /// Sets <see cref="Title"/> and <see cref="Message"/>, then shows the snackbar for the amount of time specified in <see cref="Timeout"/>.
        /// </summary>
        public async void Expand(string title, string message)
        {
            if (Show)
            {
                HideComponent();

                await Task.Run(() =>
                {
                    Thread.Sleep((int)300);

                    if (Application.Current == null)
                        return;

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Title = title;
                        Message = message;

                        ShowComponent();
                    });
                });

                return;
            }

            Title = title;
            Message = message;

            ShowComponent();
        }

        public void Hide()
        {
            HideComponent(0);
        }

        private void ShowComponent()
        {
            if (Show)
            {
                return;
            }

            UpdateThread();

            // TODO: Animation in XAML when Show changes
            //DoubleAnimation db = new DoubleAnimation();

            //db.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut };
            //db.To = 0;
            //db.From = ActualHeight;
            //db.Duration = TimeSpan.FromSeconds(0.5);

            Show = true;

            //SlideTransform.BeginAnimation(TranslateTransform.YProperty, db);

            if (Timeout > 0)
                HideComponent(Timeout);
        }

        private async void HideComponent(int timeout = 0)
        {
            if (!Show)
            {
                return;
            }

            if (timeout < 1)
            {
                Show = false;
            }

            string masterThread = _currentThread;

            await Task.Run(() =>
            {
                Thread.Sleep(timeout);

                if (Application.Current == null)
                    return;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (_currentThread == masterThread)
                    {
                        Show = false;
                    }
                });
            });
        }

        private void UpdateThread()
        {
            Random random = new Random();

            _currentThread =
                new string(Enumerable.Repeat(_characters, 8).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static void OnGlyphChanged(DependencyObject dependency, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependency is not Snackbar control) return;

            control.SetValue(IsGlyphProperty, control.Glyph != Common.Icon.Empty);
            control.SetValue(RawGlyphProperty, Common.Glyph.ToString(control.Glyph));
        }
    }
}