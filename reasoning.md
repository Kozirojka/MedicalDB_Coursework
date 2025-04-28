

Один пацієнт може мати багато запитів на допомогу
Відношення 1 - *, один користувач може мати багато запитів на допомогу 

Коли користувач захоче переглянути на своїй сторінки дані про свій запит, йому прийде в результатах  
json файл у якому буде інформація про запити включно зі статусами

Пацієнт має можливість відмінити запит, тому потрібно зробити так, щоб поля мали також поле **isActive**



Інсує список медикаментів -  

dotnet ef dbcontext scaffold "Host=localhost;Port=5432;Database=CourseWork_2;Username=postgres;Password=admin" Npgsql.EntityFrameworkCore.PostgreSQL --output-dir Models --context-dir Data --context CourseWork2Context --force