## Dotnet Core 3.1 Todo Api Application

I've builded TodoList Api with Dotnet Core 3.1 Web Api and RestFul Services.

Firstly, you need to download and install Visual Studio 2019, following Asp.net Core 3.1 SDK from:

https://dotnet.microsoft.com/download/dotnet-core/3.1

Next, it's ready to use!

Local data is stored in Sql Lite (In Memory) **TodoListDB.db** inside of Todo-App.Api project.

I seperated layers such as TodoList-App.Api, TodoList-App.DAL, TodoList-App.Business and TodoList-App.Core

I provided authentication system through **JTW Identity Provider**.

I prefered and used **Entity Framework Core Code First** approach.

Besides, in order to ensure maintanable and low coupling, I've implemented **Dependency Injection** pattern into this app.

**Swagger endpoints** were added and configured to project. I set swagger endpoint as default page. 
So you can simply view and try api endpoints with JWT Auhtorization also on swagger page.

Once you clean and rebuild the whole solution, it must run properly on 

### `https://localhost:44317/swagger/index.html`

![alt text](https://i.ibb.co/YtrDyj1/Ekran-Al-nt-s.png)

![alt text](https://i.ibb.co/sthVbvB/Ekran-Al-nt-s-2.png)
