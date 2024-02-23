using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace EsGiris.Admin.WebGrid
{
    /// <summary>
    /// Helper to evaluate different method on dynamic objects
    /// </summary>
    internal static class DynamicHelper
    {
        public static bool TryGetMemberValue(object obj, string memberName, out object result)
        {
            try
            {
                result = GetMemberValue(obj, memberName);
                return true;
            }
            catch (RuntimeBinderException)
            {
            }
            catch (RuntimeBinderInternalCompilerException)
            {
            }

            // We catch the C# specific runtime binder exceptions since we're using the C# binder in this case
            result = null;
            return false;
        }

        public static bool TryGetMemberValue(object obj, GetMemberBinder binder, out object result)
        {
            try
            {
                // VB us an instance of GetBinderAdapter that does not implement FallbackGetMemeber. This causes lookup of property expressions on dynamic objects to fail.
                // Since all types are private to the assembly, we assume that as long as they belong to CSharp runtime, it is the right one. 
                if (typeof(Binder).Assembly.Equals(binder.GetType().Assembly))
                {
                    // Only use the binder if its a C# binder.
                    result = GetMemberValue(obj, binder);
                }
                else
                {
                    result = GetMemberValue(obj, binder.Name);
                }
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static object GetMemberValue(object obj, string memberName)
        {
            var callSite = GetMemberAccessCallSite(memberName);
            return callSite.Target(callSite, obj);
        }

        public static object GetMemberValue(object obj, GetMemberBinder binder)
        {
            var callSite = GetMemberAccessCallSite(binder);
            return callSite.Target(callSite, obj);
        }

        public static CallSite<Func<CallSite, object, object>> GetMemberAccessCallSite(string memberName)
        {
            var binder = Binder.GetMember(CSharpBinderFlags.None, memberName, typeof(DynamicHelper), new[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) });
            return GetMemberAccessCallSite(binder);
        }

        public static CallSite<Func<CallSite, object, object>> GetMemberAccessCallSite(CallSiteBinder binder) => CallSite<Func<CallSite, object, object>>.Create(binder);

        public static IEnumerable<string> GetMemberNames(object obj)
        {
            var provider = obj as IDynamicMetaObjectProvider;
            Debug.Assert(provider != null, "obj doesn't implement IDynamicMetaObjectProvider");

            var parameter = Expression.Parameter(typeof(object));
            return provider.GetMetaObject(parameter).GetDynamicMemberNames();
        }
    }
}
