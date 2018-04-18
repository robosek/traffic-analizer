open System
open System.Net
open FSharp.Data
open Newtonsoft.Json
open System.IO
open traffic_report_crawler.WebCrawler
open traffic_report_crawler.FileService
open traffic_report_crawler.TextProcessor
open traffic_report_crawler.TrafficReportProcessor

[<EntryPoint>]
let main argv =
    let reportsNumber = 1

    (Seq.collect (fun (report:TrafficReport) -> report.title.Split(" ")) 
    (analyzeTrafficReports reportsNumber))
    |> Seq.filter notInStopWords
    |> Seq.filter notInPunctationMarks
    |> Seq.map (fun word -> (word, 1))
    |> countWords
    |> Seq.sortByDescending (fun (_,number) -> number)
    |> Seq.map (fun (word, number) -> sprintf "%s: %d" word number)
    |> saveToFile "output.json"
    printfn "Finished"
    0