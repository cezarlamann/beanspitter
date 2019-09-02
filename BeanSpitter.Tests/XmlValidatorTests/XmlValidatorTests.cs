namespace BeanSpitter.Tests.XmlValidatorTests
{
    using BeanSpitter.Interfaces;
    using BeanSpitter.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.IO.Abstractions;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Schema;

    [TestClass]
    public class XmlValidatorTests
    {
        private IFileSystem fileSystem;
        private IMemoryStreamFactory memoryStreamFactory;
        private IXmlSchemaReader xmlSchemaReader;

        private string assemblyLocation;
        private string xmlLocation;
        private string xsdLocation;
        private string[] xmlFilePaths;
        private string[] xsdFilePaths;

        [TestInitialize]
        public void Setup()
        {
            fileSystem = new FileSystem();
            memoryStreamFactory = new MemoryStreamFactory();

            assemblyLocation = fileSystem.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            xmlLocation = fileSystem.Path.Combine(assemblyLocation, "Files", "Xml");
            xsdLocation = fileSystem.Path.Combine(assemblyLocation, "Files", "Xsd");

            if (!fileSystem.Directory.Exists(xmlLocation))
            {
                throw new Exception("Could not find the Test Xml Files Folder.");
            }

            if (!fileSystem.Directory.Exists(xsdLocation))
            {
                throw new Exception("Could not find the Test Xsd Files Folder.");
            }

            xmlFilePaths = fileSystem.Directory.GetFiles(xmlLocation);
            xsdFilePaths = fileSystem.Directory.GetFiles(xsdLocation);

            if (xmlFilePaths == null)
            {
                throw new Exception("Could not find the Test Xml Files.");
            }

            if (xmlFilePaths.Length == 0)
            {
                throw new Exception("Could not find the Test Xml Files.");
            }

            if (xsdFilePaths == null)
            {
                throw new Exception("Could not find the Test Xsd Files.");
            }

            if (xsdFilePaths.Length == 0)
            {
                throw new Exception("Could not find the Test Xsd Files.");
            }
        }

        [TestMethod]
        public void WhenTheValidatorIsCalledWithValidXmlTheErrorEventIsNotFired()
        {
            var customerSchema = xsdFilePaths.FirstOrDefault(w => w.ToLower().Contains("customersorders.xsd"));
            var customerXml = xmlFilePaths.FirstOrDefault(w => w.ToLower().Contains("customersorders.xml"));

            if (customerSchema == null)
            {
                Assert.Fail("CustomersOrders.xsd File Missing");
            }

            if (customerXml == null)
            {
                Assert.Fail("CustomersOrders.xml File Missing");
            }

            xmlSchemaReader = new XmlSchemaReader(fileSystem, memoryStreamFactory);
            var schema = xmlSchemaReader.ReadFromPath(customerSchema);
            var schemaSet = new XmlSchemaSet
            {
                XmlResolver = new XmlUrlResolver()
            };
            schemaSet.Add(schema);

            if (schema == null)
            {
                Assert.Fail("Schema could not be loaded");
            }

            var validator = new XmlValidator(memoryStreamFactory, fileSystem, null, null, null, null, null);

            var methodHasNotBeenCalled = true;
            var count = 0;

            validator.ErrorOccurred += async (s, e, c) =>
            {
                await Task.Run(() => { methodHasNotBeenCalled = false; });
            };

            var res = validator.ValidateXmlFileAgainstSchemaAsync(customerXml, schemaSet, true).Result;

            while (count < 10)
            {
                Thread.Sleep(1000);
                if (methodHasNotBeenCalled)
                {
                    break;
                }

                count++;
            }

            Assert.IsTrue(methodHasNotBeenCalled);
        }

        [TestMethod]
        public void WhenTheValidatorIsCalledWithInvalidXmlTheErrorEventIsFired()
        {
            var customerSchema = xsdFilePaths.FirstOrDefault(w => w.ToLower().Contains("customersorders.xsd"));
            var customerXml = xmlFilePaths.FirstOrDefault(w => w.ToLower().Contains("customersorderswitherror.xml"));

            if (customerSchema == null)
            {
                Assert.Fail("CustomersOrders.xsd File Missing");
            }

            if (customerXml == null)
            {
                Assert.Fail("customersorderswitherror.xml File Missing");
            }

            xmlSchemaReader = new XmlSchemaReader(fileSystem, memoryStreamFactory);
            var schema = xmlSchemaReader.ReadFromPath(customerSchema);
            var schemaSet = new XmlSchemaSet
            {
                XmlResolver = new XmlUrlResolver()
            };
            schemaSet.Add(schema);

            if (schema == null)
            {
                Assert.Fail("Schema could not be loaded");
            }

            var validator = new XmlValidator(memoryStreamFactory, fileSystem, null, null, null, null, null);

            var errors = new List<ValidationErrorEventArgs>();
            var methodHasBeenCalled = false;
            var count = 0;

            validator.ErrorOccurred += async (s, e, c) =>
            {
                await Task.Run(() =>
                {
                   errors.Add(e);
                   methodHasBeenCalled = true;
                });
            };

            var res = validator.ValidateXmlFileAgainstSchemaAsync(customerXml, schemaSet, true).Result;

            Thread.Sleep(1000);

            while (count < 10)
            {
                Thread.Sleep(1000);
                if (methodHasBeenCalled)
                {
                    break;
                }

                count++;
            }

            Assert.IsTrue(methodHasBeenCalled);
            Assert.AreEqual(2, errors.Count);
        }

        [TestMethod]
        public void WhenTheValidatorIsCalledWithMifidValidXmlTheErrorCountIsZero()
        {
            var mifidFile = xmlFilePaths.FirstOrDefault(f => f.Contains("XX_DATTRA_ZZ_000002-0-000001_19.xml"));

            var schemaFiles = new string[]
            {
                xsdFilePaths.FirstOrDefault(f => f.Contains("head.003.001.01.xsd")),
                xsdFilePaths.FirstOrDefault(f => f.Contains("head.001.001.01_ESMAUG_1.0.0.xsd")),
                xsdFilePaths.FirstOrDefault(f => f.Contains("DRAFT15auth.016.001.01_ESMAUG_DATTRA_1.0.3.xsd")),
            };

            if (schemaFiles.Any(a => string.IsNullOrEmpty(a)))
            {
                Assert.Fail("There's a XSD file missing.");
            }

            xmlSchemaReader = new XmlSchemaReader(fileSystem, memoryStreamFactory);

            var schemaSet = new XmlSchemaSet
            {
                XmlResolver = new XmlUrlResolver()
            };

            foreach (var item in schemaFiles)
            {
                schemaSet.Add(xmlSchemaReader.ReadFromPath(item));
            }

            var validator = new XmlValidator(memoryStreamFactory, fileSystem, null, null, null, null, null);

            ValidationFinishedEventArgs validationFinishedEventArgs = null;

            validator.ValidationFinished += async (s, e, c) =>
            {
                await Task.Run(() => { validationFinishedEventArgs = e; });
            };

            var res = validator.ValidateXmlFileAgainstSchemaAsync(mifidFile, schemaSet, true).Result;

            var count = 0;
            while (validationFinishedEventArgs == null && count <= 10)
            {
                count++;
                Thread.Sleep(1000);
            }

            Assert.IsNotNull(validationFinishedEventArgs);
            Assert.AreEqual(0, validationFinishedEventArgs.ErrorCount);
        }

        [TestMethod]
        public void WhenTheValidatorIsCalledWithGleifValidXmlTheErrorCountIsZero()
        {
            var mifidFile = xmlFilePaths.FirstOrDefault(f => f.Contains("20190402-0800-gleif-goldencopy-lei2-intra-day.xml"));

            var schemaFiles = new string[]
            {
                //xsdFilePaths.FirstOrDefault(f => f.Contains("xml.xsd")),
                xsdFilePaths.FirstOrDefault(f => f.Contains("2017-03-21_lei-cdf-v2-1.xsd")),
            };

            if (schemaFiles.Any(a => string.IsNullOrEmpty(a)))
            {
                Assert.Fail("There's a XSD file missing.");
            }

            xmlSchemaReader = new XmlSchemaReader(fileSystem, memoryStreamFactory);

            var schemaSet = new XmlSchemaSet
            {
                XmlResolver = new XmlUrlResolver()
            };

            foreach (var item in schemaFiles)
            {
                schemaSet.Add(xmlSchemaReader.ReadFromPath(item));
            }

            var validator = new XmlValidator(memoryStreamFactory, fileSystem);

            ValidationFinishedEventArgs validationFinishedEventArgs = null;

            validator.ValidationFinished += async (s, e, c) =>
            {
                await Task.Run(() => { validationFinishedEventArgs = e; });
            };

            var res = validator.ValidateXmlFileAgainstSchemaAsync(mifidFile, schemaSet, true).Result;

            var count = 0;
            while (validationFinishedEventArgs == null && count <= 10)
            {
                count++;
                Thread.Sleep(1000);
            }

            Assert.IsNotNull(validationFinishedEventArgs);
            Assert.AreEqual(6, validationFinishedEventArgs.ErrorCount);
        }
    }
}
