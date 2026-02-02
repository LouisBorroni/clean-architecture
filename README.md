Projet clean Architecture ESGI 2026
Louis BORRONI
Hugo Rivaux
Léo Limousin


lancer l'application : 

    - bdd / bucket :
        1 cd clean-architecture/TierListes/bdd
        2 docker-compose up -d
    
    - back :
        1 cd clean-architecture/TierListes/backend/src/TierListes.Api
        2 (si première fois) dotnet build
        3 dotnet run

    - front :
        1 cd clean-architecture/TierListes/frontend/tierlistes-app/src
        2 ng serve

    -stripe config : 
        1 avoir un compte stripe bac à sable
        2 se connecter au cli avec stripe login dans powershell
        3 stripe listen --forward-to localhost:5000/api/payment/webhook pour avoir la clé webhook à placer dans le le appsettings
        4 placer les 3 clés la pk api la sk api et la clé de tarif également dans le appsettings
