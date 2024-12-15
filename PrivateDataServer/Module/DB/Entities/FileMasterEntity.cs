using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PrivateDataServer.Module.FileModule.Enum;
namespace PrivateDataServer.Module.DB.Entities;
public class FileMasterEntity
{
    /// <summary>
    /// ファイルを管理するためのテーブル
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("file_id")]
    public Guid FileId { get; set; }

    [Required]
    [Column("file_name")]
    public string FileName { get; set; }

    [Required]
    [Column("file_path")]
    public string FilePath { get; set; }

    [Column("create_user")]
    public string CreateUser { get; set; }

    [Required]
    [Column("create_date")]
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;

    [Required]
    [Column("file_type")]
    public FileType FileType { get; set; }
}