﻿open traffic_report_crawler.FileService
open traffic_report_crawler.TextProcessor
open traffic_report_crawler.TrafficReportProcessor

[<EntryPoint>]
let main argv =
    let reportsNumber = 1000
 
    printfn "Starting..."
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()

    let resultReports = analyzeTrafficReports reportsNumber
    match resultReports with
    | Ok reports -> (Seq.collect (fun (report) -> report.title.Split(" ")) reports)
                    |> Seq.filter notInStopWords
                    |> Seq.filter notInPunctationMarks
                    |> Seq.map (fun word -> (word, 1))
                    |> countWords
                    |> Seq.sortByDescending (fun (_,number) -> number)
                    |> Seq.map (fun (word, number) -> sprintf "%s: %d" word number)
                    |> saveToFile "output.json"
    | Error error -> printfn "%s" error

    stopWatch.Stop()
    printfn "Finished. Time elapsed: %d:%d:%d" stopWatch.Elapsed.Minutes stopWatch.Elapsed.Seconds stopWatch.Elapsed.Milliseconds
    0