using Controller;

var radarController = new RadarController();

// Gerar arquivos do SQL Server
radarController.GerarArquivosDoSqlServer();

// Gerar arquivos do MongoDB
radarController.GerarArquivosDoMongo();