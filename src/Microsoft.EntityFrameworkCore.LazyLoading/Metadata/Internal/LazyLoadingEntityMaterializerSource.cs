using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Metadata.Internal
{
    public class LazyLoadingEntityMaterializerSource<TDbContext> : EntityMaterializerSource, ILazyLoadingEntityMaterializerSource<TDbContext>
        where TDbContext : DbContext
    {
        private TDbContext _ctx;

        public void SetDbContext(TDbContext ctx)
        {
            _ctx = ctx;
        }

        public override Expression CreateMaterializeExpression(IEntityType entityType, Expression valueBufferExpression, int[] indexMap = null)
        {
            if (_ctx == null)
            {
                throw new InvalidOperationException("DbContext must be manually passed to LazyLoadingEntityMaterializerSource in order to use LazyLoading.");
            }

            var expr = base.CreateMaterializeExpression(entityType, valueBufferExpression, indexMap);

            var blockExpr = expr as BlockExpression;

            Debug.Assert(blockExpr != null, "First Expression was not a BlockExpression.");

            var constructExpr = blockExpr.Expressions[0] as BinaryExpression;

            Debug.Assert(constructExpr != null, "First Expression of BlockExpression was not a BinaryExpression.");

            var instanceExpr = constructExpr.Left;

            var newExpressions = blockExpr.Expressions.Take(blockExpr.Expressions.Count - 1).ToList();

            var navigations = entityType.GetNavigations();

            // Search LazyCollection navigations
            foreach (var navigation in navigations)
            {
                var navigationTypeInfo = navigation.ClrType.GetTypeInfo();
                if (navigationTypeInfo.IsGenericType && navigationTypeInfo.GenericTypeArguments.Length == 1 && navigationTypeInfo.GenericTypeArguments[0].GetTypeInfo().IsClass)
                {
                    if (navigationTypeInfo.IsAssignableFrom(typeof(LazyCollection<>).MakeGenericType(navigationTypeInfo.GenericTypeArguments).GetTypeInfo()))
                    {
                        var createLazyCollectionConstructExpression = CreateLazyCollectionConstructExpression(navigationTypeInfo.GenericTypeArguments[0], _ctx, instanceExpr, navigation.Name);
                        var propertyOrFieldExpr = Expression.PropertyOrField(instanceExpr, navigation.Name);
                        var assignExpr = Expression.Assign(propertyOrFieldExpr, createLazyCollectionConstructExpression);
                        newExpressions.Add(assignExpr);
                    }
                }
            }


            // Search LazyReference fields
            foreach (var field in entityType.ClrType.GetTypeInfo().DeclaredFields)
            {
                var fieldTypeInfo = field.FieldType.GetTypeInfo();
                if (fieldTypeInfo.IsGenericType && fieldTypeInfo.GenericTypeArguments.Length == 1 && fieldTypeInfo.GenericTypeArguments[0].GetTypeInfo().IsClass)
                {
                    if (fieldTypeInfo.IsAssignableFrom(typeof(LazyReference<>).MakeGenericType(fieldTypeInfo.GenericTypeArguments).GetTypeInfo()))
                    {
                        var fieldExpr = Expression.Field(instanceExpr, field.Name);
                        var setDbContextMethodCallExpr = Expression.Call(fieldExpr, nameof(LazyReference<DbContext>.SetContext), new Type[] { }, Expression.Constant(_ctx));
                        newExpressions.Add(setDbContextMethodCallExpr);
                    }
                }
            }

            newExpressions.Add(blockExpr.Expressions.Last());

            return Expression.Block(blockExpr.Variables, newExpressions);
        }

        private Expression CreateLazyCollectionConstructExpression(Type itemsType, DbContext ctx, Expression parentExpr, string collectionName)
        {
            return CreateLazyCollectionConstructExpression(itemsType, Expression.Constant(ctx), parentExpr, Expression.Constant(collectionName));
        }

        private Expression CreateLazyCollectionConstructExpression(Type itemsType, Expression ctxExpr, Expression parentExpr, Expression collectionNameExpr)
        {
            var lazyCollectionType = typeof(LazyCollection<>).MakeGenericType(itemsType);
            var constructors = lazyCollectionType.GetTypeInfo().DeclaredConstructors;
            var constructor = constructors
                .Select(c => new { Constructor = c, Parameters = c.GetParameters() })
                .Where(x => x.Parameters.Length == 3
                    && x.Parameters[0].ParameterType == typeof(DbContext)
                    && x.Parameters[1].ParameterType == typeof(object)
                    && x.Parameters[2].ParameterType == typeof(string))
                .Select(x => x.Constructor)
                .FirstOrDefault();

            Debug.Assert(constructor != null, "Valid constructor for LazyCollection was not found.");

            return Expression.New(constructor, ctxExpr, parentExpr, collectionNameExpr);
        }
    }
}
