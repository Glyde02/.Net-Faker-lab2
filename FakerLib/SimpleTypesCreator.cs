using FakerLib.Types;
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


            add(dict, new IntGenerator());
            add(dict, new StringGenerator());

            return dict;
        }

        private static void add(Dictionary<Type, ISimpleTypeGenerator> dict, ISimpleTypeGenerator creator)
        {
            dict.Add(creator.type, creator);
        }

    }
}
