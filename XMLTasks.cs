using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace LabWork_LINQ_XML
{
    public static class XMLTasks
    {
        public static void XMLTask1_6(string textFilePath = @"Text Files\InputFileTask1.txt", string XMLFilePath = @"Text Files\Task1Result.xml")
        {
           /*Даны имена существующего текстового файла и
             создаваемого XML-документа. Каждая строка текстового
             файла содержит несколько (одно или более) целых чисел,
             разделенных ровно одним пробелом. Создать XML-документ
             с корневым элементом root, элементами первого уровня
             line и элементами второго уровня number. Элементы
             line соответствуют строкам исходного файла и не содержат
             дочерних текстовых узлов, элементы number каждого элемента line 
             содержат по одному числу из соответствующей
             строки (числа располагаются в порядке убывания). Элемент
             line должен содержать атрибут sum, равный сумме всех
             чисел из соответствующей строки.*/

            Console.WriteLine("Полученные данные из исходного файла:\n");
            try
            {
                Console.WriteLine(File.ReadAllText(textFilePath));
                Console.WriteLine();
            }
            catch
            {
                Console.WriteLine("Error reading from file. Aborting.");
                return;
            }
            //Создаем элемент 0-го уровня root
            var data = new XElement("root");
            //Считываем данные с файла
            string[] lines = File.ReadAllLines(textFilePath);

            //Считываем с каждой отдельной строки
            foreach (var line in lines)
            {
                var lineNums = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var numberList = new List<int>();
                var sum = 0;
                //Сохраняем отдельные полученные числа в список и узнаем сумму всех чисел в строке
                foreach(var str in lineNums)
                {
                    if (int.TryParse(str, out int number))
                    {
                        numberList.Add(number);
                        sum += number;
                    }
                }
                //Сортируем полученный список в убывающем порядке с помощью LINQ
                var sortedNums = from item in numberList orderby item descending select item;
                //Создаем элемент 1-го уровня line
                var lineData = new XElement("line");
                //Добавляем к нему аттрибут Sum
                lineData.Add(new XAttribute("Sum", sum));
                //Добавляем в элемент line все полученные числа как элементы 2-го уровня Number
                foreach(var number in sortedNums)
                {
                    lineData.Add(new XElement("Number", number));
                }
                //Добавляем элемент line как дочерний элементу root
                data.Add(lineData);
            }
            //Сохраняем в XML файл
            data.Save(XMLFilePath);

            Console.WriteLine($"Сохранено в файл {XMLFilePath}.");
            Console.WriteLine("Вывод:");
            Console.WriteLine(data);
        }

        public static void XMLTasks2_16(string inputXMLFilePath = @"Text Files\Sample.xml")
        {
            /*Дан XML-документ, содержащий хотя бы один
            элемент первого уровня.Для каждого элемента первого
            уровня найти суммарное количество атрибутов у его элементов-потомков второго 
            уровня(т.е.элементов, являющихся
            дочерними элементами его дочерних элементов) и вывести
            найденное количество атрибутов и имя элемента. Элементы
            выводить в порядке убывания найденного количества атрибутов, 
            а при совпадении количества атрибутов — в алфавитном порядке имен.*/

            try
            {
                //Загружаем документ
                var document = XDocument.Load(inputXMLFilePath);
                //Берем все элементы первого уровня
                var level1 = document.Root.Elements();
                //проверка на элементы первого уровня
                if (level1.Count() != 0)
                {
                    //Выбираем и сортируем с помощью LINQ
                    var lvl1ElementsOrdered = from element in level1
                                              orderby element.Elements().Elements().Attributes().Count() descending,
                                              element.Name.ToString() ascending
                                              select new { element.Name, Count = element.Elements().Elements().Attributes().Count() };
                    //Выводим в консоль
                    foreach (var element in lvl1ElementsOrdered)
                    {
                        Console.WriteLine($"LVL1 Element Name: {element.Name}. LVL3 Attribute Count: {element.Count}");
                    }
                }
                else
                {
                    Console.WriteLine("No Level 1 elements in file.");
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error reading from file. Aborting.");
            }
            catch
            {
                Console.WriteLine("Something went horribly wrong. Aborting.");
            }
            finally
            {
                Console.WriteLine("Enter to exit.");
                Console.ReadLine();
            }
        }

        public static void XMLTasks3_26(string inputXMLFilePath = @"Text Files\Sample_1.xml")
        {
            /*Дан XML-документ. Для всех элементов документа удалить все их атрибуты, 
              кроме первого. 
              Указание. В предикате метода Where, отбирающем все атрибуты элемента, 
              кроме первого, использовать метод
              PreviousAttribute класса XAttribute.*/
            //Загружаем документ
            var document = XDocument.Load(inputXMLFilePath);
            //Выбираем все элементы, которые содержат больше одного аттрибута
            var data = from element in document.Descendants() where element.Attributes().Count() > 1 select element;
            //Выбираем все аттрибуты, кроме первого
            var edited = from attribute in data.Attributes() where attribute.PreviousAttribute != null select attribute;
            //Удаляем эти аттрибуты
            edited.Remove();
            //Сохраняем преобразованный файл
            document.Save(@"Text Files\Task2Result.xml");
        }

        public static void XMLTasks4_46(string inputXMLFilePath = @"Text Files\Sample_1.xml")
        {
            /*Дан XML-документ. Для каждого элемента,
              имеющего дочерние элементы, добавить в конец его набора
              атрибутов атрибут с именем odd-node-count и логическим значением, равным true, если суммарное количество
              дочерних узлов у всех его дочерних элементов является нечетным, и false в противном случае.*/
            var document = XDocument.Load(inputXMLFilePath);
            //Выбираем элементы, которые должны быть помечены true
            var trueElements = from element in document.Descendants() where element.HasElements && element.DescendantNodes().Count() % 2 != 0 select element;
            //Теперь те, что false
            var falseElements = from element in document.Descendants() where element.HasElements && element.DescendantNodes().Count() % 2 == 0 select element;
            //Добавляем аттрибуты
            foreach (var element in trueElements)
            {
                element.Add(new XAttribute("odd-node-count", true));
            }
            foreach (var element in falseElements)
            {
                element.Add(new XAttribute("odd-node-count", false));
            }
            //Сохраняем
            document.Save(@"Text Files\Task3Result.xml");
        }

        public static void XMLTasks5_58(string S, string inputXMLFilePath = @"Text Files\Sample_1.xml")
        {
            /*Дан XML-документ и строка S, содержащая некоторое пространство имен. Определить в корневом элементе
              префикс node, связанный с пространством имен, заданным в
              строке S, и добавить в каждый элемент первого уровня два
              атрибута: атрибут node:count со значением, равным количеству потомков-узлов для данного элемента, и атрибут
              xml:count со значением, равным количеству потомковэлементов для данного элемента (xml — префикс пространства имен XML).
              Указание. Использовать свойство Xml класса XNamespace*/

            //Загружаем документ
            var document = XDocument.Load(inputXMLFilePath);
            //Определяем пространство имен
            XNamespace ns = S;
            //Добавляем префикс и описание пространства имен в корневой элемент
            document.Root.Name = ns + document.Root.Name.ToString();
            document.Root.Add(new XAttribute(XNamespace.Xmlns + "node", S));
            //Для всех элементов первого уровня добавляем аттрибуты
            foreach (var element in document.Root.Elements())
            {
                element.Add(new XAttribute(ns + "count", element.DescendantNodes().Count()));
                element.Add(new XAttribute(XNamespace.Xml + "count", element.Descendants().Count()));
            }
            //Сохраняем
            document.Save(@"Text Files\Task4Result.xml");
        }

        public static void XMLTasks6_76(string inputXMLFilePath = @"Text Files\Debt.xml")
        {
            /*Дан XML-документ с информацией о задолженности по оплате коммунальных услуг. Образец элемента первого уровня:
              <record>
                <house>12</house>
                <flat>129</flat>
                <name>Сергеев Т.М.</name>
                <debt>1833.32</debt>
              </record>

              Здесь house — номер дома (целое число), flat — номер
              квартиры (целое число), name — фамилия и инициалы
              жильца (инициалы не содержат пробелов и отделяются от
              фамилии одним пробелом), debt — размер задолженности в
              виде дробного числа: целая часть — рубли, дробная часть —
              копейки (незначащие нули не указываются). 
              Преобразовать документ, изменив элементы первого
              уровня следующим образом:

              <debt house="12" flat="129">
                <name>Сергеев Т.М.</name>
                <value>1833.32</value>
              </debt>

              Порядок следования элементов первого уровня не изменять*/

            //Загружаем документ
            var document = XDocument.Load(inputXMLFilePath);
            //Выбираем первоначальные элементы
            var initial = from element in document.Root.Elements() where element.Name == "record" select element;
            //Для каждого первоначального элемента создаем новый элемент нового типа (не удаляем изначальные элементы, чтобы не прервать итерацию foreach)
            foreach (var record in initial)
            {
                XElement debt;
                //Проверки на формат числа в элементе <debt>
                try
                {
                    float value = float.Parse(record.Element("debt").Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                    debt = new XElement("debt", record.Element("name"), new XElement("value", value));
                }
                catch (FormatException)
                {
                    try
                    {
                        float value = float.Parse(record.Element("debt").Value);
                        debt = new XElement("debt", record.Element("name"), new XElement("value", value));
                    }
                    catch(FormatException)
                    {
                        debt = new XElement("debt", record.Element("name"), new XElement("value", record.Element("debt").Value));
                    }
                }
                //Добавляем аттрибуты
                debt.Add(new XAttribute("house", record.Element("house").Value));
                debt.Add(new XAttribute("flat", record.Element("flat").Value));
                //Добавляем в документ
                document.Root.Add(debt);
            }
            //Удаляем начальные элементы, оставляя только элементы нового типа
            initial.Remove();
            //Сохраняем
            document.Save(@"Text Files\Debt_edited.xml");
        }
    }
}
