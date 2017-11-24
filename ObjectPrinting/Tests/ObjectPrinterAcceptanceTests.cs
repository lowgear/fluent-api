using System;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using ObjectPrinting.Serialization;

namespace ObjectPrinting.Tests
{
    [TestFixture]
    public class ObjectPrinterAcceptanceTests
    {
        [Test]
        public void Demo()
        {
            var person = new Person
            {
                Name = "Alex",
                Age = 19,
                Height = 1.3,
                Father = new Person
                {
                    Name = "Alex",
                    Age = 19,
                    Height = 1.3
                }
            };

            var serialization = person.Serialize(conf => conf
                .Printing(person1 => person1.Age)
                .Using(x => "kek"));

            Console.WriteLine(serialization);
            //1. Исключить из сериализации свойства определенного типа
            //DONE

            //2. Указать альтернативный способ сериализации для определенного типа
            //DONE

            //3. Для числовых типов указать культуру
            //DONE

            //4. Настроить сериализацию конкретного свойства
            //DONE

            //5. Настроить обрезание строковых свойств (метод должен быть виден только для строковых свойств)
            //DONE

            //6. Исключить из сериализации конкретного свойства
            //DONE

            //7. Синтаксический сахар в виде метода расширения, сериализующего по-умолчанию
            //DONE

            //8. ...с конфигурированием
            //DONE
        }

        [Test]
        public void ByDefaultOnObjectsWithoutNestedObjects_ShouldSerializeAllProperties()
        {
            var person = new Person {Name = "Alex", Age = 19};
            var expectedLines = new[]
            {
                "Person",
                "\tAge = 19",
                "\tFather = null",
                "\tHeight = 0",
                "\tId = Guid",
                "\tName = Alex"
            };
            var printer = ObjectPrinter.For<Person>();

            AssertSerializationMatchesExpectation(person, printer, expectedLines);
        }

        [Test]
        public void ByDefaultOnObjectsWithNestedObjects_ShouldSerializeAllProperties()
        {
            var person = new Person {Name = "Alex", Age = 19, Father = new Person {Name = "Alex", Age = 19}};
            var expectedLines = new[]
            {
                "Person",
                "\tAge = 19",
                "\tFather = Person",
                "\t\tAge = 19",
                "\t\tFather = null",
                "\t\tHeight = 0",
                "\t\tId = Guid",
                "\t\tName = Alex",
                "\tHeight = 0",
                "\tId = Guid",
                "\tName = Alex"
            };
            var printer = ObjectPrinter.For<Person>();

            AssertSerializationMatchesExpectation(person, printer, expectedLines);
        }

        [Test]
        public void ExcludingType_ShouldSpecifiedTypeFromSerialization()
        {
            var person = new Person {Name = "Alex", Age = 19};
            var expectedLines = new[]
            {
                "Person",
                "\tFather = null",
                "\tHeight = 0",
                "\tId = Guid",
                "\tName = Alex"
            };
            var printer = ObjectPrinter.For<Person>().ExcludeType<int>();

            AssertSerializationMatchesExpectation(person, printer, expectedLines);
        }

        [Test]
        public void AlternativeTypeSerialization_ShouldSerializeSpecifiedTypeWithFGivenFunction()
        {
            var person = new Person {Name = "Alex", Age = 19};
            var expectedLines = new[]
            {
                "Person",
                "\tAge = xxx",
                "\tFather = null",
                "\tHeight = 0",
                "\tId = Guid",
                "\tName = Alex"
            };
            var printer = ObjectPrinter.For<Person>().Printing<int>().Using(x => "xxx");

            AssertSerializationMatchesExpectation(person, printer, expectedLines);
        }

        [Test]
        public void SpecifyingDoubleCulture_ShouldSerializeDoubleWithSpecifiedCulture()
        {
            foreach (var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
                1.2.Serialize(conf => conf.Printing<double>()
                        .Using(culture))
                    .Should().Be(1.2.ToString(culture) + Environment.NewLine);
        }

        [Test]
        public void SpecifyingFloatCulture_ShouldSerializeFloatWithSpecifiedCulture()
        {
            foreach (var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
                ((float) 1.2).Serialize(conf => conf.Printing<float>()
                        .Using(culture))
                    .Should().Be(1.2.ToString(culture) + Environment.NewLine);
        }

        [Test]
        public void SpecifyingIntCulture_ShouldSerializeIntWithSpecifiedCulture()
        {
            foreach (var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
                7.Serialize(conf => conf.Printing<int>()
                        .Using(culture))
                    .Should().Be(7.ToString(culture) + Environment.NewLine);
        }

        [Test]
        public void SpecifyingLongCulture_ShouldSerializeLongWithSpecifiedCulture()
        {
            foreach (var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
                7L.Serialize(conf => conf.Printing<long>()
                        .Using(culture))
                    .Should().Be(7L.ToString(culture) + Environment.NewLine);
        }

        [Test]
        public void AlternativePropertySerialization_ShouldSerializeSpecifiedPropertyWithSpecifiedFunction()
        {
            var person = new Person {Name = "Alex", Age = 19};
            var expectedLines = new[]
            {
                "Person",
                "\tAge = xxx",
                "\tFather = null",
                "\tHeight = 0",
                "\tId = Guid",
                "\tName = Alex"
            };
            var printer = ObjectPrinter.For<Person>().Printing(o => o.Age).Using(x => "xxx");

            AssertSerializationMatchesExpectation(person, printer, expectedLines);
        }

        [Test]
        public void StringPropertyCutting_ShouldCutStringPropertiesToSpecifiedLength()
        {
            var person = new Person {Name = "Alex", Age = 19};
            var expectedLines = new[]
            {
                "Person",
                "\tAge = 19",
                "\tFather = null",
                "\tHeight = 0",
                "\tId = Guid",
                "\tName = Al"
            };
            var printer = ObjectPrinter.For<Person>().Printing(o => o.Name).Take(2);

            AssertSerializationMatchesExpectation(person, printer, expectedLines);
        }

        [Test]
        public void ExcludingProperty_ShouldExcludeSpecifiedPropertyFromSerialization()
        {
            var person = new Person {Name = "Alex", Age = 19};
            var expectedLines = new[]
            {
                "Person",
                "\tAge = 19",
                "\tHeight = 0",
                "\tId = Guid",
                "\tName = Alex"
            };
            var printer = ObjectPrinter.For<Person>().ExcludeProperty(o => o.Father);

            AssertSerializationMatchesExpectation(person, printer, expectedLines);
        }

        private static void AssertSerializationMatchesExpectation<T>(T obj, PrintingConfig<T> printer, string[] expectedLines)
        {
            var expected = string.Concat(expectedLines.Select(s => s + Environment.NewLine));
            printer.PrintToString(obj).Should().Be(expected);
        }
    }
}
