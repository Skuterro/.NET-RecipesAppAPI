# ASP-.NET-Core-WebRecipesApp-API

## Uruchomienie
Aplikacja jest skonfigurowana w taki sposób, że po uruchomieniu automatycznie stosowane są dostępne migracje bazy danych.
Aby uruchomić aplikację należy w głównym folderze projektu (tam gdzie jest plik .sln) utworzyć plik ```.env``` na wzór ```.env.example.```
Następnie należy wpisać komendę:
```sh
docker-compose up --build
```
Oraz w przeglądarce wpisać adres:
```sh
http://localhost:8081/swagger/index.html
```
