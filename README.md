# LuminoDiodeWebsite
#### English
This is my first reality-oriented project which I developed to learn ASP.NET. Most parts of webapp is written by hands with a point of learning how the web works. Services I wrote myself: session manager; password-salting-and-hashing service; 'authentithication without sending password to server' service; antispam service; document search service; frequent-search-requests service which can return results of most popular search requests without quering the DB. All of services with settings can be adjusted by changing appsettings.json, I tried to avoid hard-coding. The is multiple levels of abstrantions, therefore every service has its settings-provider which works with json files, so service itself will work only with C# variables already. I learned working with MOQ for testing those services. I used many partialviews and some viewcomponent to make components transitive between webpages. After the 0.3.0 version this repo is being used as an ASP sandbox.
#### Russian
Данный репозиторий - мой первый проект ориентированый на отработку подходов, принменимых в реальных ASP-приложениях. Разработка представляет собой multipage сайт, поддерживающий несколько простых фукций - регистрация, авторизация, публикация текстов от своего имени, лента публикаций. Стак проекта: ASP.NET6 + PostgreSQL/EF. После релиза версии 0.3.0 проект используется как песочница.
