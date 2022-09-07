using System.Reflection;

using Library.Mapping;

namespace AutoMapperConsoleApp.Mapping;

public static class MapperExtensions
{
    public static TEntity ForMember<TEntity>(this TEntity entity, in Action<TEntity> action)
    //x where TEntity : IEntity
    {
        action(entity);
        return entity;
    }

    internal static void Copy<TSource, TDestination>(TSource source, TDestination destination, PropertyInfo dstProp)
        where TDestination : class
    {
        var mapping = dstProp.GetCustomAttribute<MappingAttribute>()?.MapFrom;
        var name = mapping is { } info && (info.SourceClassName is null || info.SourceClassName == typeof(TDestination).Name)
                ? info.SourcePropertyName ?? dstProp.Name
                : dstProp.Name;
        if (source!.GetType().GetProperty(name) is { } srcProp)
        {
            try
            {
                var found = srcProp.GetValue(source) == dstProp.GetValue(destination);
                if (found)
                {
                    dstProp.SetValue(destination, srcProp.GetValue(source));
                }
            }
            catch
            {
            }
        }
    }
}