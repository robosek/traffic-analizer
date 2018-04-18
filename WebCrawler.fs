namespace traffic_report_crawler

open System
open System.Net
open FSharp.Data
open Newtonsoft.Json
open System.Collections.Generic
open System.IO

module WebCrawler = 
    let GetHtmlDocumentAsync (url:string) =
        HtmlDocument.AsyncLoad url