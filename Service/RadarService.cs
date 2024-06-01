using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using CsvHelper;
using Model;
using Newtonsoft.Json;
using Repository;

namespace Service
{
    public class RadarService
    {
        private RadarRepository radarRepository;
        private string documentosPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private string arquivosGeradosPath;

        public RadarService()
        {
            radarRepository = new RadarRepository();
            arquivosGeradosPath = Path.Combine(documentosPath, "ArquivosGerados");

            // Verifica se a pasta "ArquivosGerados" existe, caso não exista, cria-a
            if (!Directory.Exists(arquivosGeradosPath))
            {
                Directory.CreateDirectory(arquivosGeradosPath);
            }
        }

        public void GerarArquivosDoSqlServer()
        {
            List<Radar> radars = radarRepository.GetAllFromSql();

            // Gerar arquivo JSON
            string json = JsonConvert.SerializeObject(radars);
            string jsonFilePath = Path.Combine(arquivosGeradosPath, "dados_sql.json");
            File.WriteAllText(jsonFilePath, json);

            // Gerar arquivo XML
            var xmlDoc = new XmlDocument();
            var root = xmlDoc.CreateElement("Radars");
            xmlDoc.AppendChild(root);

            foreach (var radar in radars)
            {
                var radarElement = xmlDoc.CreateElement("Radar");

                foreach (var prop in radar.GetType().GetProperties())
                {
                    var propElement = xmlDoc.CreateElement(prop.Name);
                    propElement.InnerText = prop.GetValue(radar)?.ToString();
                    radarElement.AppendChild(propElement);
                }

                root.AppendChild(radarElement);
            }

            string xmlFilePath = Path.Combine(arquivosGeradosPath, "dados_sql.xml");
            xmlDoc.Save(xmlFilePath);

            // Gerar arquivo CSV
            string csvFilePath = Path.Combine(arquivosGeradosPath, "dados_sql.csv");
            using (var writer = new StreamWriter(csvFilePath))
            using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(radars);
            }
        }

        public void GerarArquivosDoMongo()
        {
            List<Radar> radars = radarRepository.GetAllFromMongo();

            // Gerar arquivo JSON
            string json = JsonConvert.SerializeObject(radars);
            string jsonFilePath = Path.Combine(arquivosGeradosPath, "dados_mongo.json");
            File.WriteAllText(jsonFilePath, json);

            // Gerar arquivo XML
            var xmlDoc = new XmlDocument();
            var root = xmlDoc.CreateElement("Radars");
            xmlDoc.AppendChild(root);

            foreach (var radar in radars)
            {
                var radarElement = xmlDoc.CreateElement("Radar");

                foreach (var prop in radar.GetType().GetProperties())
                {
                    var propElement = xmlDoc.CreateElement(prop.Name);
                    propElement.InnerText = prop.GetValue(radar)?.ToString();
                    radarElement.AppendChild(propElement);
                }

                root.AppendChild(radarElement);
            }

            string xmlFilePath = Path.Combine(arquivosGeradosPath, "dados_mongo.xml");
            xmlDoc.Save(xmlFilePath);

            // Gerar arquivo CSV
            string csvFilePath = Path.Combine(arquivosGeradosPath, "dados_mongo.csv");
            using (var writer = new StreamWriter(csvFilePath))
            using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(radars);
            }
        }

        public List<Radar> GetAllFromSql()
        {
            return radarRepository.GetAllFromSql();
        }

        public List<Radar> GetAllFromMongo()
        {
            return radarRepository.GetAllFromMongo();
        }
    }
}
