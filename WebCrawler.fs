namespace traffic_report_crawler

open FSharp.Data

module WebCrawler = 
    let GetHtmlDocumentAsync (url:string) =
        HtmlDocument.AsyncLoad url