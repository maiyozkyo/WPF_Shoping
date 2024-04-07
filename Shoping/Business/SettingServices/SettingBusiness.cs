using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System.IO;

namespace Shoping.Business.SettingServices
{
    public class SettingBusiness : ISettingBusiness
    {
        private string ConnectionString { get; }
        private string DBName { get; }
        public SettingBusiness(string _conntectionString, string _dbName)
        {
            ConnectionString = _conntectionString;
            DBName = _dbName;
        }
        public async Task<bool> Backup(string backupDirectory)
        {
            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase(DBName);
            foreach (var collectionName in database.ListCollectionNames().ToList())
            {
                // Access the collection
                var collection = database.GetCollection<BsonDocument>(collectionName);

                // Retrieve all documents from the collection
                var documents = collection.Find(new BsonDocument()).ToList();

                // Specify the path to the backup file
                string backupFilePath = Path.Combine(backupDirectory, $"{collectionName}.bson");

                // Serialize the documents and write them to the backup file
                using (var backupFileStream = File.Create(backupFilePath))
                {
                    foreach (var document in documents)
                    {
                        var bsonBytes = document.ToBson();
                        await backupFileStream.WriteAsync(bsonBytes, 0, bsonBytes.Length);
                    }
                }
            }
            return true;
        }
        public async Task<bool> Restore(List<string> lstFiles)
        {
            var client = new MongoClient(ConnectionString);

            // Access the MongoDB database using the client
            var database = client.GetDatabase(DBName);

            // Iterate over the files in the backup directory
            foreach (var filePath in lstFiles)
            {
                // Extract the collection name from the file name
                var collectionName = Path.GetFileNameWithoutExtension(filePath);

                // Access the collection
                var collection = database.GetCollection<BsonDocument>(collectionName);

                // Read documents from the backup file
                var lstDocs = new List<BsonDocument>();
                using (var fileStream = File.OpenRead(filePath))
                {
                    using (var bsonReader = new BsonBinaryReader(fileStream))
                    {
                        // Deserialize each BSON document and insert it into the collection
                        while (bsonReader.ReadBsonType() != BsonType.EndOfDocument && !bsonReader.IsAtEndOfFile())
                        {
                            var document = BsonSerializer.Deserialize<BsonDocument>(bsonReader);
                            lstDocs.Add(document);
                        }
                    }
                }
                await collection.InsertManyAsync(lstDocs);
            }

            return true;
        }
    }
}
