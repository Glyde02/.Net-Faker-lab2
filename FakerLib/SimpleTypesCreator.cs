using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakerLib
{
    class SimpleTypesCreator
    {
        public static void addDict(Dictionary<Type, ISimpleTypeGenerator> dict, ISimpleTypeGenerator generator)
        {
            dict.Add(generator.type, generator);
        }

        public static Dictionary<Type, ISimpleTypeGenerator> getSimpleTypes()
        {
            var dict = new Dictionary<Type, ISimpleTypeGenerator>();

            //todo
            //add to dictionary simple types

            return dict;
        }

    }
}
