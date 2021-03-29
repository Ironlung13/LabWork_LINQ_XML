using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace LabWork_LINQ_XML
{
    public static class XMLTasks
    {
        public static void XMLTask1_6(string textFilePath = @"Text Files\InputFileTask1.txt", string XMLFilePath = @"Text Files\Task1.xml")
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
                //Добавляем в элемент line все полученные числа как элементы 2 -го уровня Number
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
                    var lvl1ElementsOrdered = from lvl1 in level1
                                              orderby lvl1.Elements().Elements().Attributes().Count() descending,
                                              lvl1.Name.ToString() ascending
                                              select new { lvl1.Name, Count = lvl1.Elements().Elements().Attributes().Count() };
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
    }
}
