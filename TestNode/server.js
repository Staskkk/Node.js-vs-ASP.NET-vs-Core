'use strict';

var express = require("express");
var app = express();
var TestModel = require("./Models/TestModel");

//Используем общий максимум для целых чисел на C# и JavaScript
var MAX_INT = 2147483647;
function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
}

//Проверка соединения
app.get("/ping", function (request, response) {
    response.send(true);
});

//Тест 1 Сериализация в JSON
//     1.1. Сериализация в JSON
app.get("/json", function (request, response) {
    response.setHeader('Content-Type', 'application/json');

    //Создаем объект
    let model = new TestModel();

    //Инициализируем поля объекта произвольными 
    //значениями трех типов (целое число, число с плавающей запятой, строка)
    model.intProperty1 = Math.floor(Math.random() * MAX_INT);
    model.intProperty2 = Math.floor(Math.random() * MAX_INT);
    model.intProperty3 = Math.floor(Math.random() * MAX_INT);
    model.intProperty4 = Math.floor(Math.random() * MAX_INT);
    model.intProperty5 = Math.floor(Math.random() * MAX_INT);
    model.intProperty6 = Math.floor(Math.random() * MAX_INT);
    model.intProperty7 = Math.floor(Math.random() * MAX_INT);
    model.intProperty8 = Math.floor(Math.random() * MAX_INT);
    model.intProperty9 = Math.floor(Math.random() * MAX_INT);
    model.intProperty10 = Math.floor(Math.random() * MAX_INT);

    model.doubleProperty1 = Math.random();
    model.doubleProperty2 = Math.random();
    model.doubleProperty3 = Math.random();
    model.doubleProperty4 = Math.random();
    model.doubleProperty5 = Math.random();
    model.doubleProperty6 = Math.random();
    model.doubleProperty7 = Math.random();
    model.doubleProperty8 = Math.random();
    model.doubleProperty9 = Math.random();
    model.doubleProperty10 = Math.random();

    model.stringPropery1 = guid();
    model.stringPropery2 = guid();
    model.stringPropery3 = guid();
    model.stringPropery4 = guid();
    model.stringPropery5 = guid();
    model.stringPropery6 = guid();
    model.stringPropery7 = guid();
    model.stringPropery8 = guid();
    model.stringPropery9 = guid();
    model.stringPropery10 = guid();

    //Сериализуем в JSON и возвращаем результат
    response.send(JSON.stringify(model));
});

//     1.2 Десериализация + изменение данных + сериализация
app.post("/json", function (request, response) {
    response.setHeader('Content-Type', 'application/json');

    //Десериализуем полученный объект из JSON
    let model = JSON.parse(request.body);

    //Изменяем поля объекта
    model.intProperty1 /= 1.5;
    model.intProperty2 /= 1.5;
    model.intProperty3 /= 1.5;
    model.intProperty4 /= 1.5;
    model.intProperty5 /= 1.5;
    model.intProperty6 /= 1.5;
    model.intProperty7 /= 1.5;
    model.intProperty8 /= 1.5;
    model.intProperty9 /= 1.5;
    model.intProperty10 /= 1.5;

    model.doubleProperty1 /= 2;
    model.doubleProperty2 /= 2;
    model.doubleProperty3 /= 2;
    model.doubleProperty4 /= 2;
    model.doubleProperty5 /= 2;
    model.doubleProperty6 /= 2;
    model.doubleProperty7 /= 2;
    model.doubleProperty8 /= 2;
    model.doubleProperty9 /= 2;
    model.doubleProperty10 /= 2;

    model.stringPropery1 = model.stringPropery1.toUpperCase();
    model.stringPropery2 = model.stringPropery1.toUpperCase();
    model.stringPropery3 = model.stringPropery1.toUpperCase();
    model.stringPropery4 = model.stringPropery1.toUpperCase();
    model.stringPropery5 = model.stringPropery1.toUpperCase();
    model.stringPropery6 = model.stringPropery1.toUpperCase();
    model.stringPropery7 = model.stringPropery1.toUpperCase();
    model.stringPropery8 = model.stringPropery1.toUpperCase();
    model.stringPropery9 = model.stringPropery1.toUpperCase();
    model.stringPropery10 = model.stringPropery1.toUpperCase();

    //Сериализуем в JSON и возвращаем результат
    response.send(JSON.stringify(model));
});

console.log(`Worker ${process.pid} started`);
app.listen(3000);