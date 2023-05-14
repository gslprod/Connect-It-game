using ConnectIt.Utilities;
using System;

namespace ConnectIt.Save.SaveNames
{
    public struct ItemName
    {
        public Type Type { get; }
        public string Name { get; }
        public Type ParentType { get; }

        public ItemName(Type type, string name, Type parentType = null)
        {
            Assert.ArgIsNotNull(type);
            Assert.ThatArgIs(!string.IsNullOrEmpty(name));

            Type = type;
            Name = name;
            ParentType = parentType;
        }
    }
}
