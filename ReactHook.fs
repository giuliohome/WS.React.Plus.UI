namespace ReactHook

open WebSharper
open WebSharper.JavaScript
open WebSharper.React
open WebSharper.React.Html

[<JavaScript>]
module React =
    open System
    open WebSharper.React.Bindings
    
    let FunctionComponent (f: 'props -> React.Element) (props: 'props) : React.Element =
        React.CreateElement(As<Func<obj, React.Element>> f, box props)
        
    [<Inline "React.useState($0)">]
    let UseState (init: 'T) = X<'T * ('T -> unit)>
    
    let mutable setCount  = fun (i:int) ->  ()

[<JavaScript>]
module HelloWorld =
    
    let Example () =
        let count, setCount = React.UseState 0
        React.setCount <- setCount
        div [] [
            p [] [Html.textf "You clicked %i times" count]
            button [on.click (fun _ -> 
                        setCount (count + 1)
                        UIBase.MyVars.count.Set (count + 1) 
                    )] [
                text "W# React Click me"
            ]
        ]
    // not needed a different hole. we can even integrate it at DOM level
    //let Main =
    //    React.FunctionComponent Example ()
    //    |> React.Mount (JS.Document.GetElementById "mainReact")