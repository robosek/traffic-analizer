namespace traffic_report_crawler

open System
open System.Net
open FSharp.Data
open Newtonsoft.Json
open System.Collections.Generic
open System.IO

module FileService = 
    let saveToFile (filePath:string) (data: seq<string>) = 
        use streamWriter = new System.IO.StreamWriter(filePath)
        data
        |> Seq.iter (streamWriter.WriteLineAsync >> Async.AwaitTask >> Async.RunSynchronously)