namespace ClientServer

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

[<JavaScript>]
module Client =
    let count = UIBase.MyVars.count
    let welcome = p [] [
        count.View
        |> View.Map (sprintf "You clicked %i times")
        |> textView
        ]
    let Main () =
        let rvInput = Var.Create ""
        let submit = Submitter.CreateOption rvInput.View
        let vReversed =
            submit.View.MapAsync(function
                | None -> async { return "" }
                | Some input -> Server.DoSomething input
            )
        div [] [
            Doc.Input [] rvInput
            Doc.Button "Send" [] submit.Trigger
            hr [] []
            h4 [attr.``class`` "text-muted"] [text "The server responded:"]
            div [attr.``class`` "jumbotron"] [h1 [] [textView vReversed]]
            br [] []
            welcome
            button [
                on.click (fun _ _ ->
                    let next = count.Value + 1 
                    count.Set(next)
                    ReactHook.React.setCount next 
                    )
            ] [
                text "W# UI Click Me!"
            ]
            div [
                on.afterRender ( fun el ->
                        ReactHook.React.FunctionComponent ReactHook.HelloWorld.Example ()
                        |> WebSharper.React.React.Mount el
                    )
                ] []

        ]
