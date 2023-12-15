Программа делает анализ .log файлов и печатает результат отчета в .txt файл.

Пример .log, для которых надо сделать анализ и содержимое самих файлов:

![image](https://github.com/GevrMai/LogFileAnalyzer/blob/master/ImagesReadme/FileExample%20in%20directory.png)
![image](https://github.com/GevrMai/LogFileAnalyzer/blob/master/ImagesReadme/LogsExample%20inside.png)

Результат отчета включает статистику по типам ошибок (Category), уровню логирования (Severity), даты последней и первой записи, количество .log файлов для данного сервиса
Формат запроса: *название сервиса, для которого надо сделать отчет* *директория с .log файлами*
Каждому запросу присваивается guid, во время выполнения можно узнать статус запроса командой *status*
Результат отчета печатается в консоль, добавляется в файл:

![image](https://github.com/GevrMai/LogFileAnalyzer/blob/master/ImagesReadme/Interface.png)
![image](https://github.com/GevrMai/LogFileAnalyzer/blob/master/ImagesReadme/ReportExample%20in%20directory.png)

Реализован неблокирующий вызов: для каждого запроса создается отдельный Task (Так же создается и для каждого сервиса в запросе)
Используется примитив синхронизации - Monitor для корректной работы с файлами
Если в .log файле есть email адресс, то он шифруется: alexander@mail.ru -> *l*x*n*e*@,ail.ru
