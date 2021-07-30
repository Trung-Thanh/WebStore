using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Appication.Common
{
    // vỏ lớp StorageService
    interface IStorageService
    {
        // lay url cua file, tham so la filename
        string GetFileUrl(string fileName);

        // Luu file async co 1 stream va file name
        // khi implement sẽ định nghía nơi lưu file
        Task SaveFileAsync(Stream mediaBinaryStream, string fileName);

        // xóa 1 file có tên filename
        Task DeleteFileAsync(string fileName);
    }
}
