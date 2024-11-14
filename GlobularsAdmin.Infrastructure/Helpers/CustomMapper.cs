
namespace GlobularsAdmin.Infrastructure
{
    public class CustomMapper
    {
        public static TDestination Map<TSource, TDestination>(TSource sourceObject)
        {
            var destinationObject = Activator.CreateInstance<TDestination>();
            if (sourceObject != null)
            {
                foreach (var sourceProperty in typeof(TSource).GetProperties())
                {
                    var destinationProperty =
                    typeof(TDestination).GetProperty
                    (sourceProperty.Name);
                    if (destinationProperty != null)
                    {
                        destinationProperty.SetValue
                        (destinationObject,
                       sourceProperty.GetValue(sourceObject));
                    }
                }
            }
            return destinationObject;
        }

        public static List<TDestination> MapList<TSource, TDestination>(List<TSource> sourceList)
        {
            var destinationList = new List<TDestination>();
            if (sourceList != null && sourceList.Any())
            {
                foreach (var sourceObject in sourceList)
                {
                    var destinationObject = Map<TSource, TDestination>(sourceObject);
                    destinationList.Add(destinationObject);
                }
            }
            return destinationList;
        }
    }
}