using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Linq;

namespace PrivateDataServer.Module.DB.Entities;

    [Table("logs", Schema = "public")]
    public class Log
    {
        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        [Column("level")]
        [StringLength(50)]
        public string Level { get; set; }

        [Column("message")]
        public string Message { get; set; }

        [Column("messagetemplate")]
        public string MessageTemplate { get; set; }

        [Column("exception")]
        public string Exception { get; set; }

        [Column("properties")]
        public JObject Properties { get; set; }

        [Column("execuser")]
        public string ExecUser { get; set; }
    }