using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace LabWork_LINQ_XML
{
    public static class XMLTasks
    {
        public static void XMLTask1_6(string textFilePath ="InputFile.txt", string XMLFilePath = @"Task1.xml")
        {
            //Создаем элемент 1-го уровня root
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
                //Создаем элемент 2-го уровня line
                var lineData = new XElement("line");
                //Добавляем к нему аттрибут Sum
                lineData.Add(new XAttribute("Sum", sum));
                //Добавляем в элемент line все полученные числа как элементы 3-го уровня Number
                foreach(var number in sortedNums)
                {
                    lineData.Add(new XElement("Number", number));
                }
                //Добавляем элемент line как дочерний элементу root
                data.Add(lineData);
            }
            //Сохраняем в XML файл
            data.Save(XMLFilePath);
        }
    }
}
