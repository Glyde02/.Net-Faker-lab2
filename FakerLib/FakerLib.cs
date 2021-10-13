using System;
using System.Collections.Generic;
using System.Reflection;

namespace FakerLib
{
    public class Faker
    {

        private Dictionary<Type, ISimpleTypeGenerator> simpleTypeGenerator;
        private Dictionary<Type, IGenericGenerator> genericTypeGenerator;
        private List<Type> GeneratedTypesInClass;
        
        
        public Faker()
        {
            this.simpleTypeGenerator = SimpleTypesCreator.getSimpleTypes();
            this.genericTypeGenerator = new Dictionary<Type, IGenericGenerator>();

            this.GeneratedTypesInClass = new List<Type>();
        }

        object Create(Type type)
        {
            object result = null;

            return result;
        }


        public T Create<T>()
        {
            return (T)Create(typeof(T));
        }

        private object createClass(Type type)
        {
            object createdClass = null;
            int maxLenConstructor = 0;
            ConstructorInfo constructor = null;

            var classConstructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (ConstructorInfo construct in classConstructors)
            {
                var num = construct.GetParameters().Length;

                if (num > maxLenConstructor)
                {
                    maxLenConstructor = num;
                    constructor = construct;
                }
            }

            GeneratedTypesInClass.Add(type);


            if (constructor != null)
            {
                createdClass = CreateFromConstructor(constructor, type);
            }


            GeneratedTypesInClass.Remove(type);

            return createdClass;
        }

        private object CreateFromConstructor(ConstructorInfo constructor, Type type)
        {
            var parameters = new List<object>();

            foreach (ParameterInfo parameterInfo in constructor.GetParameters())
            {
                object value = Create(parameterInfo.ParameterType);
                parameters.Add(value);
            }

            return constructor.Invoke(parameters.ToArray());

        }




    }
}
