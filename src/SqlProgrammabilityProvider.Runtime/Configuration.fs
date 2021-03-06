﻿namespace FSharp.Data.Experimental

type ResultType =
    | Tuples = 0
    | Records = 1
    | DataTable = 2
    | Maps = 3

namespace FSharp.Data.Experimental.Runtime

open System.IO
open System.Configuration

type Configuration() =    
    static member ReadConnectionStringFromConfigFileByName(name: string, resolutionFolder, fileName) = 
        let path = Path.Combine(resolutionFolder, fileName)
        if not <| File.Exists path then raise <| FileNotFoundException( sprintf "Could not find config file '%s'." path)
        let map = ExeConfigurationFileMap( ExeConfigFilename = path)
        let configSection = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None).ConnectionStrings.ConnectionStrings
        match configSection, lazy configSection.[name] with
        | null, _ | _, Lazy null -> failwithf "Cannot find name %s in <connectionStrings> section of %s file." name path
        | _, Lazy x -> x.ConnectionString

    static member GetConnectionStringRunTimeByName(name: string) = 
        let section = ConfigurationManager.ConnectionStrings.[name]
        if section = null 
        then failwithf "Cannot find name %s in <connectionStrings> section of config file." name
        else section.ConnectionString
