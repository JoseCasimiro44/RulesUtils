@echo off
rem -------------------------------------------------------------------------
rem Rebuilds all projects in current solutions
rem -------------------------------------------------------------------------

set currentFolder=%cd%
set msbuild=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild



%msbuild% /verbosity:q /p:RunCodeAnalysis=true;CodeAnalysisRuleSet=MyCustomRule.ruleset "Demo.Project/Demo.Project.csproj"

if "%1"=="" pause