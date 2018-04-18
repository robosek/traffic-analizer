namespace traffic_report_crawler

open System
open System.Net
open FSharp.Data
open Newtonsoft.Json
open System.Collections.Generic
open System.IO
open traffic_report_crawler.WebCrawler

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
        let list = new List<HtmlDocument>()
        for i in 0..number do
            String.Format(trafficReportUrl, i)
                |> GetHtmlDocumentAsync
                |> Async.StartAsTask
                |> Async.AwaitTask
                |> Async.RunSynchronously
                |> list.Add
        list

    let analyzeTrafficReports number =
        Seq.collect (toReportHtmlElements >> Seq.map toTrafficReport) (getHtmlDocuments number)