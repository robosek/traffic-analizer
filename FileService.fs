namespace traffic_report_crawler

module FileService = 
    let saveToFile (filePath:string) (data: seq<string>) = 
        use streamWriter = new System.IO.StreamWriter(filePath)
        data
        |> Seq.iter (streamWriter.WriteLineAsync >> Async.AwaitTask >> Async.RunSynchronously)