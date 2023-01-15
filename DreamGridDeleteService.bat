@echo off
nssm.exe stop DreamGridService
nssm.exe remove DreamGridService confirm
@pause