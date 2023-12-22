using Microsoft.AspNetCore.Mvc;
using RAMWebServer.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static RAMWebServer.Models.FileSaveDateController;

namespace RAMWebServer.Controllers
{
    [ApiController]
    [Route("api")]
    public partial class FileSaveDateController : ControllerBase
    {
        [HttpGet("saveDates")]
        public async Task<IActionResult> GetFileDates()
        {
            // Путь к XML-файлу с ссылками на репозитории файлов
            string xmlFilePath = @"C:\Users\e.egorov\Desktop\AddinsData.xml";

            //Чтение данных из XML
            List<FileData> repositories = FileService.ReadRepositoriesFromXml(xmlFilePath);

            //Получение даты сохранения файлов
            List<FileData> allFileDates = new List<FileData>();
            //Внутри цикла обращаемся к каждому репозиторию
            foreach (var repository in repositories)
            {
                try
                {
                    //Создание HttpClient для выполнения запросов к серверу
                    using (HttpClient httpClient = new HttpClient())
                    {
                        //// Задание базового URL на основе репозитория
                        //httpClient.BaseAddress = new Uri(repository.FileName);

                        // Вызов асинхронного метода для получения списка даты сохранения файла
                        DateTime fileDates = await FileService.GetLocalFileLastModifiedDateAsync(repository.FilePath);

                        // Добавление полученных дат в общий список
                        allFileDates.Add(
                            new FileData()
                            {
                                FileName = repository.FileName,
                                FilePath = repository.FilePath,
                                Date = fileDates
                            }
                        );
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"Ошибка при обращении к репозиторию: {ex.Message}");
                }
            }
            return Ok(allFileDates);
        }
    }
}
