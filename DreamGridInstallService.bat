@echo off
nssm install DreamGridService Start.exe service
nssm set DreamGridService Description DreamGridService DreamGridInstallService.bat=Install, DreamGridDeleteService.bat=Delete the service.
@pause