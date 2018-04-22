namespace traffic_report_crawler

module FileService = 
    let saveToFile (filePath:string) (data: seq<string>) = 
        use streamWriter = new System.IO.StreamWriter(filePath)
        data
        |> String.concat "\n"
        |> streamWriter.WriteAsync
        |> Async.AwaitTask
        |> Async.RunSynchronously
