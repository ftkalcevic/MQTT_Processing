using System.Data;

namespace MQTT_Processinglib
{
    public class DatabaseData
    {
        public string dbColumn { get; set; }
        public SqlDbType dbType { get; set; }
        public bool primaryKey { get; set; }
        public int dbSize { get; set; }
    }
}
