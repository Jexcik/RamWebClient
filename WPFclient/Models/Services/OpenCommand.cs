using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFclient.Models.Services
{
    public class OpenCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Получение объекта приложени и документа Revit
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Application app = uiApp.Application;

            //Путь к файлу проекта
            string projectPath = @"I:\03. Проекты\IDE-0156 РД_Кумроч_ЗИФ-ОИ_1-я оч_БГК\4. Работа\BIM Проект\01_Рабочие данные\04_1_КЖ\KUM-IDE-156-RD-20.6-KJ-00-00-R22.rvt";

            //Преобразование строки пути в объект ModelPath
            ModelPath modelPath = ModelPathUtils.ConvertUserVisiblePathToModelPath(projectPath);

            //Создание параметра открытия документа
            OpenOptions openOptions = new OpenOptions();
            openOptions.Audit = true;

            //Откройте документ
            Document doc = app.OpenDocumentFile(modelPath, openOptions);

            if (doc != null)
            {

            }
            return Result.Succeeded;
        }
    }
}
