using System;
using System.Collections.Generic;
using HotChocolate.Types;

namespace HotChocolate.Configuration
{
    internal partial class TypeRegistry
        : ITypeRegistry
    {
        public bool TryGetTypeBinding<T>(string typeName, out T typeBinding)
            where T : ITypeBinding
        {
            if (_typeBindings.TryGetValue(typeName, out ITypeBinding binding)
                && binding is T tb)
            {
                typeBinding = tb;
                return true;
            }
            typeBinding = default;
            return false;
        }

        public bool TryGetTypeBinding<T>(
            INamedType namedType,
            out T typeBinding)
            where T : ITypeBinding
        {
            return TryGetTypeBinding(namedType.Name, out typeBinding);
        }

        public bool TryGetTypeBinding<T>(
            Type nativeNamedType,
            out T typeBinding)
            where T : ITypeBinding
        {
            if (_typeInspector.TryCreate(nativeNamedType,
                    out Utilities.TypeInfo typeInfo)
                && _dotnetTypeToSchemaType.TryGetValue(
                    typeInfo.NativeNamedType, out string namedTypeName)
                && _namedTypes.TryGetValue(namedTypeName,
                    out INamedType namedType)
                && TryGetTypeBinding(namedType, out typeBinding))
            {
                return true;
            }
            typeBinding = default;
            return false;
        }

        public IEnumerable<ITypeBinding> GetTypeBindings()
        {
            return _typeBindings.Values;
        }
    }
}
