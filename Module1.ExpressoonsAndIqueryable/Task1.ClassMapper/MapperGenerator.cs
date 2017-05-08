using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Task1.ClassMapper
{
    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>()
        {
            ParameterExpression sourceParam = Expression.Parameter(typeof (TSource));
            NewExpression destinationNewExpression = Expression.New(typeof (TDestination));

            IEnumerable<PropertyInfo> sameProperties = GetSameProperties<TSource, TDestination>();
            List<MemberBinding> memberBindingList = (from propertyInfo in sameProperties
                let properties = destinationNewExpression.Type.GetProperty(propertyInfo.Name)
                let call = Expression.Call(sourceParam, propertyInfo.GetGetMethod())
                select Expression.Bind(properties, call)).Cast<MemberBinding>().ToList();

            var mapFunction = Expression.Lambda<Func<TSource, TDestination>>(
                Expression.MemberInit(destinationNewExpression, memberBindingList), sourceParam);

            return new Mapper<TSource, TDestination>(mapFunction.Compile());
        }

        private static IEnumerable<PropertyInfo> GetSameProperties<TSource, TDestination>()
        {
            PropertyInfo[] sourceProps = typeof (TSource).GetProperties();
            PropertyInfo[] destinationProps = typeof (TDestination).GetProperties();

            IList<PropertyInfo> neededProps = (
                from sourceProp in sourceProps
                from destinationProp in destinationProps
                where destinationProp.Name == sourceProp.Name &&
                      destinationProp.PropertyType.FullName == sourceProp.PropertyType.FullName
                select sourceProp).ToList();

            return neededProps;
        }
    }
}
