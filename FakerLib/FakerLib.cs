using System;
using System.Collections.Generic;
using System.Reflection;

namespace FakerLib
{
    public class Faker
    {

        private Dictionary<Type, ISimpleTypeGenerator> simpleTypeGenerator;
        private Dictionary<Type, IGenericGenerator> genericTypeGenerator;
        private List<Type> generatedTypesInClass;
        private Dictionary<PropertyInfo, ISimpleTypeGenerator> customTypeGenerator = new Dictionary<PropertyInfo, ISimpleTypeGenerator>();


        public Faker()
        {
            this.simpleTypeGenerator = SimpleTypesCreator.getSimpleTypes();
            this.genericTypeGenerator = new Dictionary<Type, IGenericGenerator>();

            this.generatedTypesInClass = new List<Type>();
        }

        public T create<T>()
        {
            return (T)createObject(typeof(T));
        }

        private object createObject(Type type)
        {
            object createdObject = null;


            if (simpleTypeGenerator.TryGetValue(type, out ISimpleTypeGenerator creator))
            {
                createdObject = creator.Create();
            }
            else if (type.IsValueType)
            {
                createdObject = Activator.CreateInstance(type);
            }
            else if (type.IsGenericType && genericTypeGenerator.TryGetValue(type.GetGenericTypeDefinition(), out IGenericGenerator genCreator))
            {
                createdObject = genCreator.create(type.GenericTypeArguments[0]); //type of object in collection
            }
            else if (type.IsClass && 
                    !type.IsArray && 
                    !type.IsPointer && 
                    !type.IsAbstract && 
                    !type.IsGenericType)
            {
                if (!generatedTypesInClass.Contains(type))
                {
                    createdObject = createClass(type);
                }
                else
                {
                    createdObject = null;
                }
            }

            return createdObject;
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

            generatedTypesInClass.Add(type);


            if (constructor != null)
            {
                createdClass = CreateFromConstructor(constructor, type);
            }

            createdClass = CreateProperties(type, createdClass);

            generatedTypesInClass.Remove(type);

            return createdClass;
        }

        private object CreateProperties(Type type, object createdObject)
        {
            object created = null;
            if (createdObject == null)
            {
                created = Activator.CreateInstance(type);
            }
            else
            {
                created = createdObject;
            }

            foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic))
            {
                if (fieldInfo.GetValue(created) == null)
                {
                    object value = null;
                    if (!FieldCreator(fieldInfo, out value))
                    {
                        value = createObject(fieldInfo.FieldType);
                    }
                    fieldInfo.SetValue(created, value);
                }
            }

            foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic))
            {
                if (propertyInfo.CanWrite)
                {
                    if (propertyInfo.GetValue(created) == null)
                    {
                        object value = null;
                        if (!PropertyCreator(propertyInfo, out value))
                        {
                            value = createObject(propertyInfo.PropertyType);
                        }
                        propertyInfo.SetValue(created, value);
                    }
                }
            }

            return created;
        }



        private object CreateFromConstructor(ConstructorInfo constructor, Type type)
        {
            var parameters = new List<object>();

            foreach (ParameterInfo parameterInfo in constructor.GetParameters())
            {
                object value = null;
                if (!createByCustomCreator(parameterInfo,type,out value)) {
                    value = createObject(parameterInfo.ParameterType);
                }
                parameters.Add(value);
            }

            return constructor.Invoke(parameters.ToArray());

        }

        private bool createByCustomCreator(ParameterInfo parameterInfo, Type type, out object created)
        {
            foreach (KeyValuePair<PropertyInfo, ISimpleTypeGenerator> keyValue in customTypeGenerator)
            {
                if (keyValue.Key.Name == parameterInfo.Name && keyValue.Value.type.Equals(parameterInfo.ParameterType) && keyValue.Key.ReflectedType.Equals(type))
                {
                    created = keyValue.Value.Create();
                    return true;
                }
            }
            created = null;
            return false;
        }

        private bool FieldCreator(FieldInfo fieldInfo, out object created)
        {
            foreach (KeyValuePair<PropertyInfo, ISimpleTypeGenerator> keyValue in customTypeGenerator)
            {
                if (keyValue.Key.Name == fieldInfo.Name && keyValue.Value.type.Equals(fieldInfo.FieldType) && keyValue.Key.ReflectedType.Equals(fieldInfo.ReflectedType))
                {
                    created = keyValue.Value.Create();
                    return true;
                }
            }
            created = null;
            return false;
        }

        private bool PropertyCreator(PropertyInfo propertyInfo, out object created)
        {
            if (customTypeGenerator.TryGetValue(propertyInfo, out ISimpleTypeGenerator creator))
            {
                created = creator.Create();
                return true;
            }
            else
            {
                created = null;
                return false;
            }
        }


    }
}
