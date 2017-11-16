﻿using System;
using System.Globalization;
using NUnit.Framework;

namespace ObjectPrinting.Tests
{
	[TestFixture]
	public class ObjectPrinterAcceptanceTests
	{
		[Test]
		public void Demo()
		{
			var person = new Person { Name = "Alex", Age = 19 };

			var printer = ObjectPrinter.For<Person>().Printing(perso => perso.Age).Using(x => "kek");
				//1. Исключить из сериализации свойства определенного типа
				//DONE

				//2. Указать альтернативный способ сериализации для определенного типа
				//DONE?
			
				//3. Для числовых типов указать культуру
				//DONE
			
				//4. Настроить сериализацию конкретного свойства
			
				//5. Настроить обрезание строковых свойств (метод должен быть виден только для строковых свойств)
			
				//6. Исключить из сериализации конкретного свойства
            
            string s1 = printer.PrintToString(person);

			//7. Синтаксический сахар в виде метода расширения, сериализующего по-умолчанию		
			//8. ...с конфигурированием
		}
	}
}