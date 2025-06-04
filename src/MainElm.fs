open Avalonia
open Avalonia.Controls
open Avalonia.Themes.Fluent
open Elmish
open Avalonia.FuncUI.Hosts
open Avalonia.FuncUI
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Elmish
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Layout



module Counter =
    type CounterState = { count : int }

    let init () = { count = 0 }, Cmd.none

    type Msg = Increment | Decrement

    let update (msg: Msg) (state: CounterState) =
        match msg with
        | Increment -> { state with count =  state.count + 1 }, Cmd.none
        | Decrement -> { state with count =  state.count - 1 }, Cmd.none
    
    let view (state: CounterState) (dispatch): IView =
        DockPanel.create [
            DockPanel.children [
                Button.create [
                    Button.dock Dock.Bottom
                    Button.onClick (fun _ -> dispatch Decrement)
                    Button.content "-"
                    Button.horizontalAlignment HorizontalAlignment.Stretch
                    Button.horizontalContentAlignment HorizontalAlignment.Center
                ]
                Button.create [
                    Button.dock Dock.Bottom
                    Button.onClick (fun _ -> dispatch Increment)
                    Button.content "+"
                    Button.horizontalAlignment HorizontalAlignment.Stretch
                    Button.horizontalContentAlignment HorizontalAlignment.Center
                ]
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.fontSize 48.0
                    TextBlock.verticalAlignment VerticalAlignment.Center
                    TextBlock.horizontalAlignment HorizontalAlignment.Center
                    TextBlock.text (string state.count)
                ]
            ]
        ]



type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "Counter Example"
        //base.Height <- 400.0
        //base.Width <- 400.0

        //this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
        //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true
        Elmish.Program.mkProgram Counter.init Counter.update Counter.view
        |> Program.withHost this
        |> Program.withConsoleTrace
        |> Program.run

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (FluentTheme())
        this.RequestedThemeVariant <- Styling.ThemeVariant.Dark

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            let mainWindow = MainWindow()
            desktopLifetime.MainWindow <- mainWindow
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main(args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)