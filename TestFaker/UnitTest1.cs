using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakerLib;
using System;
using System.Collections.Generic;

namespace TestFaker
{
    [TestClass]
    public class UnitTest1
    {
        private Faker faker = new Faker();

        [TestMethod]
        public void TestSimpleType()
        {
            var testValue = faker.create<int>();
            Assert.IsInstanceOfType(testValue, typeof(int));
        }

        [TestMethod]
        public void TestCharType()
        {
            var testValue = faker.create<char>();
            Assert.IsInstanceOfType(testValue, typeof(char));
        }

        [TestMethod]
        public void TestDateType()
        {
            var testValue = faker.create<DateTime>();
            Assert.IsInstanceOfType(testValue, typeof(DateTime));
        }

        [TestMethod]
        public void TestCollectionType()
        {
            var testValue = faker.create<List<string>>();
            Assert.IsInstanceOfType(testValue[0], typeof(string));
        }

        [TestMethod]
        public void TestClass()
        {
            var testValue = faker.create<Class1>();
            Assert.IsInstanceOfType(testValue, typeof(Class1));
        }

        [TestMethod]
        public void TestRekursive()
        {
            var testValue = faker.create<A>();
            Assert.IsNull(testValue.b.c.a);
        }

        public class Class1
        {
            public int number;
            public string str;
            public DateTime dateTime;

            public Class1(int _number, string _str, DateTime _dateTime)
            {
                number = _number;
                str = _str;
                dateTime = new DateTime(_dateTime.Year, _dateTime.Month, _dateTime.Day, _dateTime.Hour, _dateTime.Minute, _dateTime.Second, _dateTime.Millisecond);

            }
        }

        public class A
        {
            public B b;
        }

        public class B
        {
            public C c;
        }
        public class C
        {
            public A a;
        }


    }
}
