using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CitnDev.System.Extensions
{
    public static class ObjectExtensions
    {
        #region Map properties

        //
        // Source Type, Destination Type, Map methods
        //
        private static readonly Dictionary<Type, Dictionary<Type, List<Action<object, object>>>> Mappings = new Dictionary<Type, Dictionary<Type, List<Action<object, object>>>>();

        public static TU To<TU>(this object sourceInstance)
        {
            var sourceType = sourceInstance.GetType();
            var returnInstance = Activator.CreateInstance<TU>();
            var generateMapping = false;

            lock (Mappings)
            {

                if (!Mappings.ContainsKey(sourceType))
                {
                    generateMapping = true;
                    Mappings[sourceType] = new Dictionary<Type, List<Action<object, object>>>();
                }

                if (!Mappings[sourceType].ContainsKey(typeof(TU)))
                {
                    generateMapping = true;
                    Mappings[sourceType][typeof(TU)] = new List<Action<object, object>>();
                }
            }

            #region Generation getter->setter

            if (generateMapping)
            {
                var sourceProperties = sourceType.GetProperties(BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty | BindingFlags.Public);
                var destinationProperties = typeof(TU).GetProperties(BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.SetProperty | BindingFlags.Public);
                var mapping = new Dictionary<PropertyInfo, PropertyInfo>();

                foreach (var sourceProperty in sourceProperties)
                {
                    var destinationProperty = destinationProperties.FirstOrDefault(p =>
                            (p.PropertyType.IsEnum && sourceProperty.PropertyType.IsEnum || p.PropertyType == sourceProperty.PropertyType)
                            && p.Name == sourceProperty.Name && p.CanWrite
                        );
                    if (destinationProperty != null)
                    {
                        mapping.Add(sourceProperty, destinationProperty);
                    }
                }


                foreach (var propertyInfo in mapping.Keys)
                {
                    lock (Mappings)
                    {
                        Mappings[sourceType][typeof(TU)].Add(GenerateCastProperty(sourceType, typeof(TU), propertyInfo, mapping[propertyInfo]));
                    }
                }
            }

            #endregion

            foreach (var action in Mappings[sourceType][typeof(TU)])
            {
                action(returnInstance, sourceInstance);
            }

            return returnInstance;
        }

        public static Action<object, object> GenerateCastProperty(Type sourceType, Type destinationType, PropertyInfo propSource, PropertyInfo propDest)
        {
            var destParameter = Expression.Parameter(typeof(object), "instance");
            var sourceParameter = Expression.Parameter(typeof(object), "param");

            var propertyGetExpression = Expression.Property(Expression.Convert(sourceParameter, sourceType), propSource);
            var propertySetExpression = Expression.Property(Expression.Convert(destParameter, destinationType), propDest);

            Expression assignExpression;
            if (propSource.PropertyType.IsEnum)
                assignExpression = Expression.Assign(propertySetExpression, Expression.Convert(propertyGetExpression, propDest.PropertyType));
            else
                assignExpression = Expression.Assign(propertySetExpression, propertyGetExpression);

            var lambda = Expression.Lambda<Action<object, object>>(assignExpression, destParameter, sourceParameter);

            return lambda.Compile();
        }

        #endregion
    }
}
