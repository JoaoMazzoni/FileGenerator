using Model;
using Service;
using System.Collections.Generic;

namespace Controller
{
    public class RadarController
    {
        private RadarService radarService;

        public RadarController()
        {
            radarService = new RadarService();
        }

        public List<Radar> GetAllFromSql()
        {
            return radarService.GetAllFromSql();
        }


        public List<Radar> GetAllFromMongo()
        {
            return radarService.GetAllFromMongo();
        }

        // Método para gerar arquivos do SQL Server
        public void GerarArquivosDoSqlServer()
        {
            radarService.GerarArquivosDoSqlServer();
        }

        // Método para gerar arquivos do MongoDB
        public void GerarArquivosDoMongo()
        {
            radarService.GerarArquivosDoMongo();
        }
    }
}
