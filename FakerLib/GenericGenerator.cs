using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakerLib
{
    class GenericGenerator : IGenericGenerator
    {
        public Type[] CollectionType => new Type[] { typeof(List<>), typeof(IEnumerable<>), typeof(IList<>), typeof(ICollection<>) };

        private Dictionary<Type, ISimpleTypeGenerator> primitiveTypeList = new Dictionary<Type, ISimpleTypeGenerator>();

        public object Create(Type type)
        {
            IList result = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
            var length = new Random().Next(1, 10);

            if (primitiveTypeList.TryGetValue(type, out ISimpleTypeGenerator creator))
            {
                for (int i = 0; i < length; i++)
                {
                    result.Add(creator.Create());
                }
            }
            else
            {
                var defaultValue = "DEFAULT";
                for (int i = 0; i < length; i++)
                {
                    result.Add(defaultValue);
                }
            }
            return result;
        }
    }
}
