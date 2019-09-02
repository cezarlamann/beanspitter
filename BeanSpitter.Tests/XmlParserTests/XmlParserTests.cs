namespace BeanSpitter.Tests.XmlParserTests
{
    using BeanSpitter.Interfaces;
    using BeanSpitter.Tests.XmlSchemaReaderTests.ClassesFromXsds.Gleif;
    using BeanSpitter.Tests.XmlSchemaReaderTests.ClassesFromXsds.Mifid.DraftSchemas.International.Reporting;
    using BeanSpitter.Tests.XmlSchemaReaderTests.ClassesFromXsds.Mifid.Headers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.IO.Abstractions;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Schema;

    [TestClass]
    public class XmlParserTests
    {
        #region Variables Declaration
        private string[] draftInternationalFeedbackFiles;
        private XmlSchemaSet draftInternationalFeedbackSchemaSet;
        private string[] draftInternationalReportingFiles;
        private XmlSchemaSet draftInternationalReportingSchemaSet;

        private string[] draftNationalFeedbackFiles;
        private XmlSchemaSet draftNationalFeedbackSchemaSet;
        private string[] draftNationalReportingFiles;
        private XmlSchemaSet draftNationalReportingSchemaSet;

        private string executingPath;
        private string xmlFilesPath;

        private string[] finalInternationalFeedbackFiles;
        private XmlSchemaSet finalInternationalFeedbackSchemaSet;
        private string[] finalInternationalReportingFiles;
        private XmlSchemaSet finalInternationalReportingSchemaSet;

        private string[] finalNationalFeedbackFiles;
        private XmlSchemaSet finalNationalFeedbackSchemaSet;
        private string[] finalNationalReportingFiles;
        private XmlSchemaSet finalNationalReportingSchemaSet;

        private XmlSchemaSet gleifSchemaSet;
        private string[] gleifFiles;

        private IFileSystem fs;

        private IMemoryStreamFactory msf;

        private IXmlSchemaReader reader;

        private string[] xmlSchemaFiles;

        private string xmlSchemasPath;

        #endregion

        #region Setup
        [TestInitialize]
        public void Setup()
        {
            fs = new FileSystem();
            msf = new MemoryStreamFactory();
            reader = new XmlSchemaReader(fs, msf);

            draftNationalFeedbackSchemaSet = new XmlSchemaSet { XmlResolver = new XmlUrlResolver() };
            finalNationalFeedbackSchemaSet = new XmlSchemaSet { XmlResolver = new XmlUrlResolver() };
            draftInternationalFeedbackSchemaSet = new XmlSchemaSet { XmlResolver = new XmlUrlResolver() };
            finalInternationalFeedbackSchemaSet = new XmlSchemaSet { XmlResolver = new XmlUrlResolver() };
            draftInternationalReportingSchemaSet = new XmlSchemaSet { XmlResolver = new XmlUrlResolver() };
            finalInternationalReportingSchemaSet = new XmlSchemaSet { XmlResolver = new XmlUrlResolver() };
            draftNationalReportingSchemaSet = new XmlSchemaSet { XmlResolver = new XmlUrlResolver() };
            finalNationalReportingSchemaSet = new XmlSchemaSet { XmlResolver = new XmlUrlResolver() };
            gleifSchemaSet = new XmlSchemaSet { XmlResolver = new XmlUrlResolver() };

            executingPath = fs.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            xmlSchemasPath = fs.Path.Combine(executingPath, "Files", "Xsd");

            xmlFilesPath = fs.Path.Combine(executingPath, "Files", "Xml");

            xmlSchemaFiles = fs.Directory.GetFiles(xmlSchemasPath).ToArray();

            draftNationalReportingFiles = xmlSchemaFiles
                .Where(w =>
                    w.Contains("head.001.001.01") ||
                    w.Contains("head.003.001.01") ||
                    w.Contains("DRAFT15auth.016.001.01_ESMAUG_Reporting_1.0.3"))
                .ToArray();

            gleifFiles = xmlSchemaFiles
                .Where(w =>
                    w.Contains("2017-03-21_lei-cdf-v2-1"))
                .ToArray();

            foreach (var item in gleifFiles)
            {
                var schema = reader.ReadFromPath(item);
                gleifSchemaSet.Add(schema);
            }

            foreach (var item in draftNationalReportingFiles)
            {
                var schema = reader.ReadFromPath(item);
                draftNationalReportingSchemaSet.Add(schema);
            }

            draftInternationalReportingFiles = xmlSchemaFiles
                .Where(w =>
                    w.Contains("head.001.001.01") ||
                    w.Contains("head.003.001.01") ||
                    w.Contains("DRAFT15auth.016.001.01_ESMAUG_DATTRA_1.0.3"))
                .ToArray();

            foreach (var item in draftInternationalReportingFiles)
            {
                var schema = reader.ReadFromPath(item);
                draftInternationalReportingSchemaSet.Add(schema);
            }

            finalNationalReportingFiles = xmlSchemaFiles
                .Where(w =>
                    w.Contains("head.001.001.01") ||
                    w.Contains("head.003.001.01") ||
                    w.Contains("auth.016.001.01_ESMAUG_Reporting_1.1.0"))
                .ToArray();

            foreach (var item in finalNationalReportingFiles)
            {
                var schema = reader.ReadFromPath(item);
                finalNationalReportingSchemaSet.Add(schema);
            }

            finalInternationalReportingFiles = xmlSchemaFiles
                .Where(w =>
                    w.Contains("head.001.001.01") ||
                    w.Contains("head.003.001.01") ||
                    w.Contains("auth.016.001.01_ESMAUG_DATTRA_1.1.0"))
                .ToArray();

            foreach (var item in finalInternationalReportingFiles)
            {
                var schema = reader.ReadFromPath(item);
                finalInternationalReportingSchemaSet.Add(schema);
            }

            draftNationalFeedbackFiles = xmlSchemaFiles
                .Where(w =>
                    w.Contains("head.001.001.01") ||
                    w.Contains("head.003.001.01") ||
                    w.Contains("DRAFT5auth.031.001.01_ESMAUG_Reporting_1.0.2"))
                .ToArray();

            foreach (var item in draftNationalFeedbackFiles)
            {
                var schema = reader.ReadFromPath(item);
                draftNationalFeedbackSchemaSet.Add(schema);
            }

            draftInternationalFeedbackFiles = xmlSchemaFiles
                .Where(w =>
                    w.Contains("head.001.001.01") ||
                    w.Contains("head.003.001.01") ||
                    w.Contains("DRAFT4auth.031.001.01_ESMAUG_FDBTRA_1.0.1"))
                .ToArray();

            foreach (var item in draftInternationalFeedbackFiles)
            {
                var schema = reader.ReadFromPath(item);
                draftInternationalFeedbackSchemaSet.Add(schema);
            }

            finalNationalFeedbackFiles = xmlSchemaFiles
                .Where(w =>
                    w.Contains("head.001.001.01") ||
                    w.Contains("head.003.001.01") ||
                    w.Contains("auth.031.001.01_ESMAUG_Reporting_1.1.0"))
                .ToArray();

            foreach (var item in finalNationalFeedbackFiles)
            {
                var schema = reader.ReadFromPath(item);
                finalNationalFeedbackSchemaSet.Add(schema);
            }

            finalInternationalFeedbackFiles = xmlSchemaFiles
                .Where(w =>
                    w.Contains("head.001.001.01") ||
                    w.Contains("head.003.001.01") ||
                    w.Contains("auth.031.001.01_ESMAUG_FDBTRA_1.1.0"))
                .ToArray();

            foreach (var item in finalInternationalFeedbackFiles)
            {
                var schema = reader.ReadFromPath(item);
                finalInternationalFeedbackSchemaSet.Add(schema);
            }
        }
        #endregion

        [TestMethod]
        public void XmlParsingTestWithGleifFile()
        {
            var xmlpath = fs.Path.Combine(xmlFilesPath, "20190402-0800-gleif-goldencopy-lei2-intra-day.xml");

            var parser = new XmlParser(msf, fs, null, null, null);

            long errorcount = 0;
            long nodeCountFromHeader = 0;

            parser.ErrorOccurred += async (s, e, c) =>
            {
                await Task.Run(() =>
                {
                    var count = Interlocked.Increment(ref errorcount);
                    var str = $"Exception: {e.Message}. Error count: {count}";
                    Debug.WriteLine(str);
                });
            };

            parser.NodeRead += async (s, e, c) =>
            {
                await Task.Delay(1);
                Debug.WriteLine($"LEI: {((LEIRecordType)e.Node).LEI}");
            };

            parser.HeaderRead += async (s, e, c) =>
            {
                await Task.Run(() => long.TryParse(((LEIHeaderType)e.Header).RecordCount, out nodeCountFromHeader));
            };

            var res = parser.ParseXmlFileFromFileAsync(
                xmlpath,
                gleifSchemaSet,
                typeof(LEIHeaderType),
                true,
                types: new Type[]
                {
                    typeof(LEIRecordType)
                }).Result;

            parser.Dispose();

            Assert.IsNotNull(res);
            Assert.IsTrue(nodeCountFromHeader > 0);
            Assert.IsTrue(res.ParsedNodeCount > 0);
        }

        [TestMethod]
        public void XmlParsingTestWithMifidFile()
        {
            var xmlpath = fs.Path.Combine(xmlFilesPath, "XX_DATTRA_ZZ_000002-0-000001_19.xml");

            var parser = new XmlParser(msf, fs, null, null, null);

            var cb = new ConcurrentBag<string>();

            parser.NodeRead += async (s, e, c) =>
            {
                await Task.Run(() =>
                {
                    var header = e.Header == null ? null : (BusinessApplicationHeaderV01)e.Header;
                    var node = (SecuritiesTransactionReport4__1)e.Node;
                    var str = $"Header: {(string.IsNullOrEmpty(header?.BizMsgIdr) ? "No Header" : header.BizMsgIdr)}. Node (TxId): {node.TxId}";
                    Debug.WriteLine(str);
                    cb.Add(str);
                });
            };

            var res = parser.ParseXmlFileFromFileAsync(
                xmlpath,
                draftInternationalReportingSchemaSet,
                typeof(BusinessApplicationHeaderV01),
                true,
                types: new Type[]
                {
                    typeof(SecuritiesTransactionReport4__1)
                }).Result;

            parser.Dispose();

            Assert.IsNotNull(res);
            Assert.IsTrue(res.ErrorCount == 0);
            Assert.IsTrue(cb.Count == res.ParsedNodeCount);
        }

        [TestMethod]
        public void XmlParsingTestWithMifidFileAndCancellationToken()
        {
            var xmlpath = fs.Path.Combine(xmlFilesPath, "XX_DATTRA_ZZ_000002-0-000001_19.xml");

            var parser = new XmlParser(msf, fs, null, null, null);

            var cb = new ConcurrentBag<string>();

            var cts = new CancellationTokenSource(800);

            var token = cts.Token;

            parser.NodeRead += async (s, e, c) =>
            {
                await Task.Run(() =>
                {
                    var header = e.Header == null ? null : (BusinessApplicationHeaderV01)e.Header;
                    var node = (SecuritiesTransactionReport4__1)e.Node;
                    var str = $"Header: {(string.IsNullOrEmpty(header?.BizMsgIdr) ? "No Header" : header.BizMsgIdr)}. Node (TxId): {node.TxId}";
                    Debug.WriteLine(str);
                    cb.Add(str);
                });
            };

            var res = parser.ParseXmlFileFromFileAsync(
                xmlpath,
                draftInternationalReportingSchemaSet,
                typeof(BusinessApplicationHeaderV01),
                true,
                token,
                types: new Type[]
                {
                    typeof(SecuritiesTransactionReport4__1)
                }).Result;


            parser.Dispose();
            cts.Dispose();

            Assert.IsNotNull(res);
            Assert.IsTrue(res.ParsedNodeCount > 0);
            Assert.IsTrue(res.Errors.Count(c => c.Exception.GetType() == typeof(OperationCanceledException)) > 0);
        }
    }
}
