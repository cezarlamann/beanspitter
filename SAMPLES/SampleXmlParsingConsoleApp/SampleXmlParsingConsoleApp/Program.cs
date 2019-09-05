namespace SampleXmlParsingConsoleApp
{
    using System;
    using System.Threading.Tasks;
    using System.Xml.Schema;
    using BeanSpitter;
    using BeanSpitter.Models;

    class Program
    {
        static void Main(string[] args)
        {
            var schemaReader = new XmlSchemaReader();
            var schemaset = new XmlSchemaSet();
            schemaset.Add(schemaReader.ReadFromPath("CustomersOrders.xsd"));

            ValidationFinishedEventArgs result;

            using (var parser = new XmlParser())
            {
                parser.NodeRead += async (s, e, c) =>
                {
                    await Task.Run(() =>
                    {
                        if (e.Node.GetType() == typeof(OrderType))
                        {
                            var order = (OrderType)e.Node;
                            Console.WriteLine($"Customer ID: {order.CustomerID}, Order date: {order.OrderDate.ToShortDateString()}");
                        }
                        else
                        {
                            var customer = (CustomerType)e.Node;
                            Console.WriteLine($"Customer ID: {customer.CustomerID}, Company Name: {customer.CompanyName}");
                        }
                    });
                };

                result = parser.ParseXmlFileFromFileAsync(
                    filePath: "CustomersOrders.xml",
                    schemaSet: schemaset,
                    returnErrorListAtTheEndOfTheProcess: true,
                    types: new Type[] { typeof(OrderType), typeof(CustomerType) }
                    ).Result;
            }

            Console.WriteLine($"Error count: {result.ErrorCount}");
            Console.WriteLine($"Nodes read: {result.ParsedNodeCount}");
            Console.WriteLine($"Elapsed time: {result.ElapsedTime.ToString()}");
            Console.ReadLine();
        }
    }
}
