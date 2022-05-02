// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;

namespace WPFUI.Win32
{
    /// <summary>
    /// Modes for expressions
    /// </summary>
    internal enum ExpressionMode
    {
        /// <summary>
        ///     No options
        /// </summary>
        None = 0,

        /// <summary>
        ///     Expression may not be set in multiple places
        /// </summary>
        /// <remarks>
        ///     Even if a non-shareable Expression has been attached and
        ///     then detached, it still may not be reused
        /// </remarks>
        NonSharable,

        /// <summary>
        ///     Expression forwards invalidations to the property on which it is set.
        /// </summary>
        /// <remarks>
        ///     This option implies <see cref="NonSharable"/> as well.
        ///     Whenever the expression is notified of an invalidation of one
        ///     of its sources via OnPropertyInvalidation, it
        ///     promises to invalidate the property on which it is set, so the
        ///     property engine doesn't have to.
        ///
        ///     The property engine does not
        ///     need to keep a reference to the target <see cref="DependencyObject"/>
        ///     in this case, which can allow the target to be garbage-collected
        ///     when it is no longer in use.
        /// </remarks>
        ForwardsInvalidations,

        /// <summary>
        ///     Expression supports DependencySources on a different Dispatcher.
        /// </summary>
        /// <remarks>
        ///     This option implies <see cref="ForwardsInvalidations"/> as well.
        ///     When set, it suppresses the property engine's check that the
        ///     DependencyObject to which the expression is attached belongs to
        ///     the same Thread as all its DependencySources, and allows
        ///     OnPropertyInvalidation notifications to arrive on the "wrong"
        ///     Thread.  It is the expression's responsibility to handle these
        ///     correctly, typically by marshalling them to the right Thread.
        ///     Note:  The check is only suppressed when the source isn't owned
        ///     by any Thread (i.e. source.Dispatcher == null).
        /// </remarks>
        SupportsUnboundSources
    }
}
