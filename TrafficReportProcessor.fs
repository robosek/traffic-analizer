namespace traffic_report_crawler

open System
open FSharp.Data
open System.Collections.Generic
open traffic_report_crawler.WebCrawler
open System

module TrafficReportProcessor = 
    let trafficReportUrl = "https://www.trojmiasto.pl/raport/?page={0}"
    let mainReportElementCssSelector = ".report-item"
    let timeElementCssSelector = ".time"
    let authorElementCssSelector = ".author-not-linked"
    let titleElementCssSelector = "h3 a"

    type TrafficReport = {
        author: string
        time: string
        title: string
    }

    let private toReportHtmlElements (htmlDocument:HtmlDocument) =
        htmlDocument.CssSelect mainReportElementCssSelector

    let private tryToGetElementText cssSelector (htmlNode: HtmlNode)  = 
        try
            htmlNode.CssSelect cssSelector
                |> Seq.head
                |> fun element -> Some(element.InnerText())
        with
        | _ -> None
    
    let private tryToGetTime = tryToGetElementText timeElementCssSelector
    let private tryToGetAuthor = tryToGetElementText authorElementCssSelector
    let private tryToGetTitle = tryToGetElementText titleElementCssSelector
    let private setDefaultValue maybeValue = 
        defaultArg maybeValue "Unknown"
    let private toTrafficReport (htmlNode:HtmlNode) = 
        let reportAuthor = setDefaultValue (tryToGetAuthor htmlNode)
        let reportTime = setDefaultValue (tryToGetTime htmlNode)
        let reportTitle = setDefaultValue (tryToGetTitle htmlNode)
        { author = reportAuthor; title = reportTitle; time = reportTime }

    let getHtmlDocuments number = 
        try
            let resultList = new List<HtmlDocument>()
            let asyncOperations = [for i in 0..number do yield (GetHtmlDocumentAsync (String.Format(trafficReportUrl, i)))]
            asyncOperations
            |> Async.Parallel
            |> Async.RunSynchronously
            |> Seq.iter resultList.Add

            Ok resultList
        with
        | ex -> Error ("Some error occured during server connection: " + ex.Message)

    let analyzeTrafficReports number =
        let resultHtmlDocuments = getHtmlDocuments number
        match resultHtmlDocuments with
        | Ok htmlDocuments-> Ok (Seq.collect (toReportHtmlElements >> Seq.map toTrafficReport) htmlDocuments)
        | Error error -> Error error