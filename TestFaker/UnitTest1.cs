using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakerLib;
using System;

namespace TestFaker
{
    [TestClass]
    public class UnitTest1
    {
        private Faker faker = new Faker();

        [TestMethod]
        public void TestBasicValueType()
        {
            var testValue = faker.create<int>();
            Assert.IsInstanceOfType(testValue, typeof(int));
        }

        [TestMethod]
        public void TestCharValueType()
        {
            var testValue = faker.create<char>();
            Assert.IsInstanceOfType(testValue, typeof(int));
        }

        public class Class1
        {
            public int numb;
            public string str;
            public DateTime dateTime;

            public Class1(int _numb, string _str, DateTime _dateTime)
            {
                numb = _numb;
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
