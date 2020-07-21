using Microsoft.Diagnostics.Runtime;
using Raven.Client.Documents.Conventions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DumpExplorer.Core
{
    public class DumpExplorerContractSerializer : DefaultRavenContractResolver
    {
        protected override List<MemberInfo> GetSerializableMembers(System.Type objectType)
        {
            var serializableMembers = base.GetSerializableMembers(objectType);
            foreach (var toRemove in serializableMembers
                .Where(MembersToFilterOut)
                .ToArray())
            {
                serializableMembers.Remove(toRemove);
            }
            return serializableMembers;
        }


        private static bool MembersToFilterOut(MemberInfo info)
        {
            if (info is EventInfo)
                return true;
            var fieldInfo = info as FieldInfo;
            if (fieldInfo != null &&
                (fieldInfo.IsPublic == false ||
                    fieldInfo.FieldType.IsByRef ||
                    fieldInfo.FieldType.IsByRefLike ||
                    fieldInfo.FieldType.IsPointer ||
                    fieldInfo.Name == "Helpers" ||
                    fieldInfo.Name == "ClrObjectHelpers" ||
                    fieldInfo.Name == "Heap" ||
                    fieldInfo.Name == "DataReader" ||
                    fieldInfo.Name == "ClrInfo" ||
                    fieldInfo.Name == "DacLibrary" ||
                    fieldInfo.Name == "ClrVersions" ||
                    fieldInfo.Name == "Threads" ||
                    fieldInfo.Name == "Runtime" ||
                    fieldInfo.Name == "Module" ||
                    fieldInfo.FieldType == typeof(IntPtr)))
                return true;

            var propertyInfo = info as PropertyInfo;
            if (propertyInfo != null &&
                (propertyInfo.PropertyType.IsByRef ||
                propertyInfo.PropertyType.IsByRefLike ||
                propertyInfo.PropertyType.IsPointer ||
                propertyInfo.PropertyType == typeof(IntPtr) ||
                propertyInfo.Name == "Helpers" ||
                propertyInfo.Name == "ClrObjectHelpers" ||
                propertyInfo.Name == "Heap" ||
                propertyInfo.Name == "DataReader" ||
                propertyInfo.Name == "ClrInfo" ||
                propertyInfo.Name == "DacLibrary" ||
                propertyInfo.Name == "ClrVersions" ||
                propertyInfo.Name == "Threads" ||
                propertyInfo.Name == "Runtime" ||
                propertyInfo.Name == "Module" ||
                propertyInfo.PropertyType == typeof(ImmutableArray<ILToNativeMap>)))
                return true;

            return info.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length > 0;
        }
    }
}
