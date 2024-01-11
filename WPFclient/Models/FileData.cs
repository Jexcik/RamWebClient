using System;

namespace WPFclient.Models
{
    public class FileData
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string LocalFileFolder { get; set; }
        public string ChangeInfo { get; set; }
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return $"В плагине {TranslateText(FileName)} добавлены следующие изменения: {ChangeInfo}\n\n";
        }

        private string TranslateText(string fileName)
        {
            switch (fileName)
            {
                case "HideScheduleColumns": return "ВРС";
                case "ReinforcementColumnarFoundations": return "Армирование фундамента";
                case "FillTitleBlock": return "Заполнить штамп";
                case "RibbonRAM": return "Панель с инструментами";
                default:
                    return "";
            }
        }
    }
}