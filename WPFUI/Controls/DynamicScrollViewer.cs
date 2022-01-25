// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFUI.Common;

namespace WPFUI.Controls
{
    /// <summary>
    /// Custom <see cref="System.Windows.Controls.ScrollViewer"/> with events depending on actions taken by the user.
    /// </summary>
    [DefaultEvent("ScrollChangedEvent")]
    public class DynamicScrollViewer : System.Windows.Controls.ScrollViewer
    {
        private readonly EventIdentifier _identifier = new();

        /// <summary>
        /// Property for <see cref="IsScrolling"/>.
        /// </summary>
        public static readonly DependencyProperty IsScrollingProperty = DependencyProperty.Register(nameof(IsScrolling),
            typeof(bool), typeof(DynamicScrollViewer), new PropertyMetadata(false));

        /// <summary>
        /// Property for <see cref="Timeout"/>.
        /// </summary>
        public static readonly DependencyProperty TimeoutProperty = DependencyProperty.Register(nameof(Timeout),
            typeof(uint), typeof(DynamicScrollViewer), new PropertyMetadata(1000u));

        /// <summary>
        /// Gets or sets information whether the user was scrolling for the last few seconds.
        /// </summary>
        public bool IsScrolling
        {
            get => (bool)GetValue(IsScrollingProperty);
            set => SetValue(IsScrollingProperty, value);
        }

        /// <summary>
        /// Gets or sets time after which the scroll is to be hidden.
        /// </summary>
        public uint Timeout
        {
            get => (uint)GetValue(TimeoutProperty);
            set => SetValue(TimeoutProperty, value);
        }

        /// <summary>
        /// OnScrollChanged is an override called whenever scrolling state changes on this <see cref="DynamicScrollViewer"/>.
        /// </summary>
        /// <remarks>
        /// OnScrollChanged fires the ScrollChangedEvent. Overriders of this method should call
        /// base.OnScrollChanged(args) if they want the event to be fired.
        /// </remarks>
        /// <param name="e">ScrollChangedEventArgs containing information about the change in scrolling state.</param>
        protected override void OnScrollChanged(ScrollChangedEventArgs e)
        {
            RaiseEvent(e);
            UpdateScrollingState();
        }

        private async void UpdateScrollingState()
        {
            // TODO: Optimize
            // My main assumption here is that each scroll causes a new "event / thread" to be assigned.
            // If more than Timeout has passed since the last event, there is no interaction.
            // We pass this value to the ScrollBar and link it to IsMouseOver.
            // This way we have a dynamic scrollbar that responds to scroll / mouse over.

            uint currentEvent = _identifier.GetNext();

            IsScrolling = true;

            uint timeout = Timeout < 10000 ? Timeout : 1000;

            await Task.Delay((int)timeout);

            if (_identifier.IsEqual(currentEvent))
                IsScrolling = false;
        }
    }
}