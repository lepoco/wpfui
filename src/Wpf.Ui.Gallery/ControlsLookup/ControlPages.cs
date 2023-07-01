using System.Reflection;

namespace Wpf.Ui.Gallery.ControlsLookup;

static class ControlPages
{
    private const string PageSuffix = "Page";

    public static IEnumerable<GalleryPage> All()
    {
        foreach (
            var type in GalleryAssembly.Asssembly
                .GetTypes()
                .Where(t => t.IsDefined(typeof(GalleryPageAttribute)))
        )
        {
            var galleryPageAttribute = type.GetCustomAttributes<GalleryPageAttribute>()
                .FirstOrDefault();

            if (galleryPageAttribute is not null)
            {
                yield return new GalleryPage(
                    type.Name.Substring(0, type.Name.LastIndexOf(PageSuffix)),
                    galleryPageAttribute.Description,
                    galleryPageAttribute.Icon,
                    type
                );
            }
        }
    }

    public static IEnumerable<GalleryPage> FromNamespace(string namespaceName)
    {
        return All().Where(t => t.PageType?.Namespace?.StartsWith(namespaceName) ?? false);
    }
}
