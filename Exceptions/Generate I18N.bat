@ECHO OFF

SETLOCAL enableextensions enabledelayedexpansion

SET I18N_TOOL=dotnet i18n-tool

SET SOURCES_DIR=.
SET I18N_DEV_FILE=I18N.xml
SET I18N_DEPLOY_DIR=Resources
SET I18N_DEPLOY_FILE=I18N.xml

echo ### Generating I18N development file
echo.

%I18N_TOOL% parse -o %I18N_DEV_FILE% -r -d -S %SOURCES_DIR%

echo.
echo ### Analyzing resulting I18N development file
echo.

%I18N_TOOL% analyze -i %I18N_DEV_FILE% -d  -L es ca fr

echo.
echo ### Generating I18N deployment file
echo.

mkdir %I18N_DEPLOY_DIR% 2>NUL

%I18N_TOOL% deploy -i %I18N_DEV_FILE% -o %I18N_DEPLOY_DIR%\%I18N_DEPLOY_FILE%
