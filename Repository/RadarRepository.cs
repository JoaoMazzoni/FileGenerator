using Microsoft.Data.SqlClient;
using Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class RadarRepository
    {

        private string sqlConnectionString = "Server=127.0.0.1; Database=DBRadar; User Id=sa; Password=SqlServer2019!; TrustServerCertificate=True";
        private SqlConnection sqlConnection;

        static string mongoConnectionString = "mongodb://root:Mongo%402024%23@localhost:27017";
        static string mongoDatabaseName = "DBRadar";
        static string mongoCollectionName = "Radar";
        private IMongoCollection<Radar> mongoCollection;

        private IMongoClient mongoClient; // Adicionando campo para o cliente MongoDB

        public RadarRepository()
        {
            sqlConnection = new SqlConnection(sqlConnectionString);

            mongoClient = new MongoClient(mongoConnectionString); // Inicializando o cliente MongoDB

            var mongoDatabase = mongoClient.GetDatabase(mongoDatabaseName);
            mongoCollection = mongoDatabase.GetCollection<Radar>(mongoCollectionName);
        }

        public List<Radar> GetAllFromSql()
        {
            List<Radar> radars = new List<Radar>();
            StringBuilder sb = new StringBuilder();
            sb.Append(Radar.GETALL);

            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(sb.ToString(), sqlConnection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Radar radar = new Radar
                    {
                        Concessionaria = reader.GetString(0),
                        AnoDoPnvSnv = reader.GetString(1),
                        TipoDeRadar = reader.GetString(2),
                        Rodovia = reader.GetString(3),
                        Uf = reader.GetString(4),
                        KmM = reader.GetString(5),
                        Municipio = reader.GetString(6),
                        TipoPista = reader.GetString(7),
                        Sentido = reader.GetString(8),
                        Situacao = reader.GetString(9),
                        DataDaInativacao = reader.GetString(10)?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                        Latitude = reader.GetString(11),
                        Longitude = reader.GetString(12),
                        VelocidadeLeve = reader.GetString(13)
                    };

                    radars.Add(radar);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao recuperar dados do SQL Server", ex);
            }
            finally
            {
                sqlConnection.Close();
            }

            return radars;
        }


        public List<Radar> GetAllFromMongo()
        {
            // Consulta ao MongoDB e conversão para uma lista de objetos Radar
            return mongoCollection.Find(_ => true).ToList();
        }

    }
}
