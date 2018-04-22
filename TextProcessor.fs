namespace traffic_report_crawler

open System.Collections.Generic
open System.IO

module TextProcessor =
    let private stopWords = File.ReadAllLines "stop_words.txt"
    let private punctationMarks = [|":";".";";";"/";"'";"";" ";"  ";"-"|]
    let private notInArray (word: string) = Seq.contains (word.ToLower()) >> not
    let notInStopWords word = notInArray word stopWords
    let notInPunctationMarks word = notInArray word punctationMarks

    let countWords (mappedWords: seq<string*int>) =
        let countedWords = new System.Collections.Generic.Dictionary<string,int>()
        mappedWords
        |> Seq.iter (fun (word, number) -> if countedWords.ContainsKey word then 
                                            countedWords.[word] <- countedWords.[word] + 1 
                                           else 
                                             countedWords.[word] <- number) 
        countedWords :> seq<_> |> Seq.map (|KeyValue|)