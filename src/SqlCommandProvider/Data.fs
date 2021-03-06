﻿namespace FSharp.Data.Experimental.Internals

open System
open System.Data

type internal Column = {
    Name : string
    Ordinal : int
    TypeInfo : TypeInfo
    IsNullable : bool
    MaxLength : int
}   with
    member this.ClrTypeConsideringNullable = 
        if this.IsNullable 
        then typedefof<_ option>.MakeGenericType this.TypeInfo.ClrType
        else this.TypeInfo.ClrType

and internal TypeInfo = {
    SqlEngineTypeId : int
    SqlDbTypeId : int
    IsFixedLength : bool option
    ClrTypeFullName : string
    UdttName : string 
    TvpColumns : Column seq
}   with
    member this.SqlDbType : SqlDbType = enum this.SqlDbTypeId
    member this.ClrType : Type = Type.GetType this.ClrTypeFullName
    member this.TableType = this.SqlDbType = SqlDbType.Structured

type internal Parameter = {
    Name : string
    TypeInfo : TypeInfo
    Direction : ParameterDirection 
}


