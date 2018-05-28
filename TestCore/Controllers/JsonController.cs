using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using TestCore.Models;

namespace TestCore.Controllers
{
    //Тест 1 Сериализация в JSON
    [Route("[controller]")]
    public class JsonController : Controller
    {
        //     1.1. Сериализация в JSON
        [HttpGet]
        public ContentResult Get()
        {
            Response.ContentType = "application/json";
            string res = string.Empty;
            var rand = new Random();

            //Создаем объект
            TestModel model = new TestModel()
            {
                //Инициализируем поля объекта произвольными 
                //значениями трех типов (целое число, число с плавающей запятой, строка)
                IntProperty1 = rand.Next(), IntProperty2 = rand.Next(), IntProperty3 = rand.Next(),
                IntProperty4 = rand.Next(), IntProperty5 = rand.Next(), IntProperty6 = rand.Next(),
                IntProperty7 = rand.Next(), IntProperty8 = rand.Next(), IntProperty9 = rand.Next(),
                IntProperty10 = rand.Next(),
                DoubleProperty1 = rand.NextDouble(), DoubleProperty2 = rand.NextDouble(), DoubleProperty3 = rand.NextDouble(),
                DoubleProperty4 = rand.NextDouble(), DoubleProperty5 = rand.NextDouble(), DoubleProperty6 = rand.NextDouble(),
                DoubleProperty7 = rand.NextDouble(), DoubleProperty8 = rand.NextDouble(), DoubleProperty9 = rand.NextDouble(),
                DoubleProperty10 = rand.NextDouble(),
                StringProperty1 = Guid.NewGuid().ToString(), StringProperty2 = Guid.NewGuid().ToString(), StringProperty3 = Guid.NewGuid().ToString(),
                StringProperty4 = Guid.NewGuid().ToString(), StringProperty5 = Guid.NewGuid().ToString(), StringProperty6 = Guid.NewGuid().ToString(),
                StringProperty7 = Guid.NewGuid().ToString(), StringProperty8 = Guid.NewGuid().ToString(), StringProperty9 = Guid.NewGuid().ToString(),
                StringProperty10 = Guid.NewGuid().ToString(),
            };

            //Сериализуем в JSON и возвращаем результат
            return Content(JsonConvert.SerializeObject(model), "application/json");
        }

        //     1.2 Десериализация + изменение данных + сериализация
        [HttpPost]
        public ContentResult Post([FromBody] string body)
        {
            Response.ContentType = "application/json";

            //Десериализуем полученный объект из JSON
            var model = JsonConvert.DeserializeObject<TestModel>(body);

            //Изменяем поля объекта
            model.IntProperty1 = (int)(model.IntProperty1 / 1.5);
            model.IntProperty2 = (int)(model.IntProperty2 / 1.5);
            model.IntProperty3 = (int)(model.IntProperty3 / 1.5);
            model.IntProperty4 = (int)(model.IntProperty4 / 1.5);
            model.IntProperty5 = (int)(model.IntProperty5 / 1.5);
            model.IntProperty6 = (int)(model.IntProperty6 / 1.5);
            model.IntProperty7 = (int)(model.IntProperty7 / 1.5);
            model.IntProperty8 = (int)(model.IntProperty8 / 1.5);
            model.IntProperty9 = (int)(model.IntProperty9 / 1.5);
            model.IntProperty10 = (int)(model.IntProperty10 / 1.5);
            model.DoubleProperty1 /= 2; model.DoubleProperty2 /= 2; model.DoubleProperty3 /= 2;
            model.DoubleProperty4 /= 2; model.DoubleProperty5 /= 2; model.DoubleProperty6 /= 2;
            model.DoubleProperty7 /= 2; model.DoubleProperty8 /= 2; model.DoubleProperty9 /= 2;
            model.DoubleProperty10 /= 2;
               
            model.StringProperty1 = model.StringProperty1.ToUpper();
            model.StringProperty2 = model.StringProperty2.ToUpper();
            model.StringProperty3 = model.StringProperty3.ToUpper();
            model.StringProperty4 = model.StringProperty4.ToUpper();
            model.StringProperty5 = model.StringProperty5.ToUpper();
            model.StringProperty6 = model.StringProperty6.ToUpper();
            model.StringProperty7 = model.StringProperty7.ToUpper();
            model.StringProperty8 = model.StringProperty8.ToUpper();
            model.StringProperty9 = model.StringProperty9.ToUpper();
            model.StringProperty10 = model.StringProperty10.ToUpper();

            //Сериализуем в JSON и возвращаем результат
            return Content(JsonConvert.SerializeObject(model), "application/json");
        }
    }
}