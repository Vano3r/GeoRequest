using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Globalization;

namespace MapRequest
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://geocode-maps.yandex.ru/1.x/?apikey=";
            string apiKey = "";

            Console.WriteLine("Введите город: ");
            string city = Console.ReadLine();
            Console.WriteLine("Введите улицу: ");
            string street = Console.ReadLine();
            Console.WriteLine("Введите дом: ");
            string house = Console.ReadLine();
            string reqGeo = "&geocode=" + city + ",+" + street + "+улица,+дом" + house;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + apiKey + reqGeo);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(readStream.ReadToEnd());
            response.Close();
            readStream.Close();

            XmlNodeList elemList = doc.GetElementsByTagName("pos");

            int indexSpace = 0;
            for (int i = 0; i < elemList.Item(0).InnerText.Length; i++)
            {
                if (elemList.Item(0).InnerText[i] == ' ')
                    indexSpace = i;
            }
            //Долгота
            double longitude = double.Parse(elemList.Item(0).InnerText.Substring(0, indexSpace), CultureInfo.InvariantCulture);
            //Широта
            double latitude = double.Parse(elemList.Item(0).InnerText.Substring(indexSpace), CultureInfo.InvariantCulture);

            Console.WriteLine("Координаты:\n" + latitude + " " + longitude);

            Console.ReadLine();
        }
    }
}
