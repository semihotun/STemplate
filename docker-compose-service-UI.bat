@echo off
echo Docker Compose başlatılıyor...
docker-compose -f docker-compose-service-UI.yml up -d
pause