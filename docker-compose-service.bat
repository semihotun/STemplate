@echo off
echo Docker Compose başlatılıyor...
docker-compose -f docker-compose-service.yml up -d
pause