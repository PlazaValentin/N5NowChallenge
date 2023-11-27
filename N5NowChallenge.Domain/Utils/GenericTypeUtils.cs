namespace N5NowChallenge.Domain.Utils;

public static class GenericTypeUtils
{
    public static string GetGenericTypeName(this Type type)
    {
        string genericTypeName;
        if (type.IsGenericType)
        {
            string str = string.Join(",", (type.GetGenericArguments()).Select(t => t.Name).ToArray());
            genericTypeName = type.Name.Remove(type.Name.IndexOf('`')) + "<" + str + ">";
        }
        else
            genericTypeName = type.Name;
        return genericTypeName;
    }

    public static string GetGenericTypeName(this object @object) => GetGenericTypeName(@object.GetType());
}