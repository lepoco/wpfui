// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace Wpf.Ui.Controls;

/// <summary>Collection operations shared by the TabView item and model paths.</summary>
internal static class TabViewCollection
{
    internal static bool CanAdd(IList collection, object item)
    {
        // Non-generic IList.Add otherwise throws when given a container instead of a model.
        foreach (Type contract in collection.GetType().GetInterfaces())
        {
            if (
                contract.IsGenericType
                && contract.GetGenericTypeDefinition() == typeof(IList<>)
                && !contract.GetGenericArguments()[0].IsInstanceOfType(item)
            )
            {
                return false;
            }
        }

        return true;
    }

    internal static void Move(IList collection, int oldIndex, int newIndex)
    {
        for (Type? type = collection.GetType(); type != null; type = type.BaseType)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ObservableCollection<>))
            {
                // Invoke the public Move API on the closed generic base type, including subclasses.
                try
                {
                    type.GetMethod(nameof(ObservableCollection<object>.Move))!
                        .Invoke(collection, new object[] { oldIndex, newIndex });
                }
                catch (TargetInvocationException exception) when (exception.InnerException != null)
                {
                    ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
                }

                return;
            }
        }

        object? item = collection[oldIndex];
        collection.RemoveAt(oldIndex);
        try
        {
            collection.Insert(newIndex, item);
        }
        catch
        {
            // Restore a removed entry if a custom IList rejects insertion at the target.
            collection.Insert(oldIndex, item);
            throw;
        }
    }
}
