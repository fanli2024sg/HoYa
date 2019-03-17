using HoYa.Entities;

namespace HoYa.Models
{
    /// <summary>
    /// 設定
    /// </summary>
    public class SettingModel
    {
        /// <summary>
        /// 圖檔路徑
        /// </summary>
        public string AD { get; set; }
        /// <summary>
        /// 手機圖檔結尾
        /// </summary>
        public Profile Profile { get; set; }
        /// <summary>
        /// 網路位置
        /// </summary>
        public string IP { get; set; }
    }
}