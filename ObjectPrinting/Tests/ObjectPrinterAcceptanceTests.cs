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
        private Person person;
        private PrintingConfig<Person> printer;
        private string[] expectedLines;

        [Test]
        public void Demo()
        {
            person = new Person
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
                .Printing(perso => perso.Age)
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
        public void Default_WorksFineWithoutNestedObjects()
        {
            person = new Person {Name = "Alex", Age = 19};
            expectedLines = new[]
            {
                "Person",
                "\tAge = 19",
                "\tFather = null",
                "\tHeight = 0",
                "\tId = Guid",
                "\tName = Alex"
            };
            printer = ObjectPrinter.For<Person>();

            AssertSerializationMatchesExpectation();
        }

        [Test]
        public void Default_WorksFineWithNestedObjects()
        {
            person = new Person {Name = "Alex", Age = 19, Father = new Person {Name = "Alex", Age = 19}};
            expectedLines = new[]
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
            printer = ObjectPrinter.For<Person>();

            AssertSerializationMatchesExpectation();
        }

        [Test]
        public void ExcludingType_WorksFine()
        {
            person = new Person {Name = "Alex", Age = 19};
            expectedLines = new[]
            {
                "Person",
                "\tFather = null",
                "\tHeight = 0",
                "\tId = Guid",
                "\tName = Alex"
            };
            printer = ObjectPrinter.For<Person>().ExcludeType<int>();

            AssertSerializationMatchesExpectation();
        }

        [Test]
        public void AlternativeTypeSerialization_WorksFine()
        {
            person = new Person {Name = "Alex", Age = 19};
            expectedLines = new[]
            {
                "Person",
                "\tAge = xxx",
                "\tFather = null",
                "\tHeight = 0",
                "\tId = Guid",
                "\tName = Alex"
            };
            printer = ObjectPrinter.For<Person>().Printing<int>().Using(x => "xxx");

            AssertSerializationMatchesExpectation();
        }

        [Test]
        public void SpecifyingDoubleCulture_WorksFine()
        {
            foreach (var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
                1.2.Serialize(conf => conf.Printing<double>()
                        .Using(culture))
                    .Should().Be(1.2.ToString(culture) + Environment.NewLine);
        }

        [Test]
        public void SpecifyingFloatCulture_WorksFine()
        {
            foreach (var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
                ((float) 1.2).Serialize(conf => conf.Printing<double>()
                        .Using(culture))
                    .Should().Be(1.2.ToString(culture) + Environment.NewLine);
        }

        [Test]
        public void SpecifyingIntCulture_WorksFine()
        {
            foreach (var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
                7.Serialize(conf => conf.Printing<double>()
                        .Using(culture))
                    .Should().Be(1.2.ToString(culture) + Environment.NewLine);
        }

        [Test]
        public void SpecifyingLongCulture_WorksFine()
        {
            foreach (var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
                7L.Serialize(conf => conf.Printing<double>()
                        .Using(culture))
                    .Should().Be(1.2.ToString(culture) + Environment.NewLine);
        }

        [Test]
        public void AlternativePropertySerialization_WorksFine()
        {
            person = new Person {Name = "Alex", Age = 19};
            expectedLines = new[]
            {
                "Person",
                "\tAge = xxx",
                "\tFather = null",
                "\tHeight = 0",
                "\tId = Guid",
                "\tName = Alex"
            };
            printer = ObjectPrinter.For<Person>().Printing(o => o.Age).Using(x => "xxx");

            AssertSerializationMatchesExpectation();
        }

        [Test]
        public void StringPropertyCutting_WorksFine()
        {
            person = new Person {Name = "Alex", Age = 19};
            expectedLines = new[]
            {
                "Person",
                "\tAge = 19",
                "\tFather = null",
                "\tHeight = 0",
                "\tId = Guid",
                "\tName = Al"
            };
            printer = ObjectPrinter.For<Person>().Printing(o => o.Name).Take(2);

            AssertSerializationMatchesExpectation();
        }

        [Test]
        public void ExcludingProperty_WorksFine()
        {
            person = new Person {Name = "Alex", Age = 19};
            expectedLines = new[]
            {
                "Person",
                "\tAge = 19",
                "\tHeight = 0",
                "\tId = Guid",
                "\tName = Alex"
            };
            printer = ObjectPrinter.For<Person>().ExcludeProperty(o => o.Father);

            AssertSerializationMatchesExpectation();
        }

        private void AssertSerializationMatchesExpectation()
        {
            var expected = string.Concat(expectedLines.Select(s => s + Environment.NewLine));
            printer.PrintToString(person).Should().Be(expected);
        }
    }
}