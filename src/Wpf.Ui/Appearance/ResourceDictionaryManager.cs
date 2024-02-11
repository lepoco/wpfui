// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;

namespace Wpf.Ui.Appearance;

/// <summary>
/// Allows managing application dictionaries.
/// </summary>
internal class ResourceDictionaryManager
{
    /// <summary>
    /// Gets the namespace, e.g. the library the resource is being searched for.
    /// </summary>
    public string SearchNamespace { get; }

    public ResourceDictionaryManager(string searchNamespace)
    {
        SearchNamespace = searchNamespace;
    }

    /// <summary>
    /// Shows whether the application contains the <see cref="ResourceDictionary"/>.
    /// </summary>
    /// <param name="resourceLookup">Any part of the resource name.</param>
    /// <returns><see langword="false"/> if it doesn't exist.</returns>
    public bool HasDictionary(string resourceLookup)
    {
        return GetDictionary(resourceLookup) != null;
    }

    /// <summary>
    /// Gets the <see cref="ResourceDictionary"/> if exists.
    /// </summary>
    /// <param name="resourceLookup">Any part of the resource name.</param>
    /// <returns><see cref="ResourceDictionary"/>, <see langword="null"/> if it doesn't exist.</returns>
    public ResourceDictionary? GetDictionary(string resourceLookup)
    {
        Collection<ResourceDictionary> applicationDictionaries = GetApplicationMergedDictionaries();

        if (applicationDictionaries.Count == 0)
        {
            return null;
        }

        resourceLookup = resourceLookup.ToLower().Trim();

        foreach (ResourceDictionary t in applicationDictionaries)
        {
            string resourceDictionaryUri;

            if (t?.Source != null)
            {
                resourceDictionaryUri = t.Source.ToString().ToLower().Trim();

                if (
                    resourceDictionaryUri.Contains(SearchNamespace)
                    && resourceDictionaryUri.Contains(resourceLookup)
                )
                {
                    return t;
                }
            }

            foreach (ResourceDictionary? t1 in t!.MergedDictionaries)
            {
                if (t1?.Source == null)
                {
                    continue;
                }

                resourceDictionaryUri = t1.Source.ToString().ToLower().Trim();

                if (
                    !resourceDictionaryUri.Contains(SearchNamespace)
                    || !resourceDictionaryUri.Contains(resourceLookup)
                )
                {
                    continue;
                }

                return t1;
            }
        }

        return null;
    }

    /// <summary>
    /// Shows whether the application contains the <see cref="ResourceDictionary"/>.
    /// </summary>
    /// <param name="resourceLookup">Any part of the resource name.</param>
    /// <param name="newResourceUri">A valid <see cref="Uri"/> for the replaced resource.</param>
    /// <returns><see langword="true"/> if the dictionary <see cref="Uri"/> was updated. <see langword="false"/> otherwise.</returns>
    public bool UpdateDictionary(string resourceLookup, Uri? newResourceUri)
    {
        Collection<ResourceDictionary> applicationDictionaries = UiApplication
            .Current
            .Resources
            .MergedDictionaries;

        if (applicationDictionaries.Count == 0 || newResourceUri is null)
        {
            return false;
        }

        resourceLookup = resourceLookup.ToLower().Trim();

        for (var i = 0; i < applicationDictionaries.Count; i++)
        {
            string sourceUri;

            if (applicationDictionaries[i]?.Source != null)
            {
                sourceUri = applicationDictionaries[i].Source.ToString().ToLower().Trim();

                if (sourceUri.Contains(SearchNamespace) && sourceUri.Contains(resourceLookup))
                {
                    applicationDictionaries[i] = new() { Source = newResourceUri };

                    return true;
                }
            }

            for (var j = 0; j < applicationDictionaries[i].MergedDictionaries.Count; j++)
            {
                if (applicationDictionaries[i].MergedDictionaries[j]?.Source == null)
                {
                    continue;
                }

                sourceUri = applicationDictionaries[i]
                    .MergedDictionaries[j]
                    .Source.ToString()
                    .ToLower()
                    .Trim();

                if (!sourceUri.Contains(SearchNamespace) || !sourceUri.Contains(resourceLookup))
                {
                    continue;
                }

                applicationDictionaries[i].MergedDictionaries[j] = new() { Source = newResourceUri };

                return true;
            }
        }

        return false;
    }

    private Collection<ResourceDictionary> GetApplicationMergedDictionaries()
    {
        return UiApplication.Current.Resources.MergedDictionaries;
    }
}
