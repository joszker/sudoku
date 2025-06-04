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




module Main =
    type State = { 
        page: Pages.Page
        solver: Solver.State
    }

    type Message = 
        | NavigateTo of Pages.Page
        | SolverMessage of Solver.Message

    let init () =
        { page = Pages.MenuPage; solver = fst (Solver.init()) }, Cmd.none

    let update (msg: Message) (state: State) =
        match msg with
        | NavigateTo newPage -> { state with page = newPage }, Cmd.none
        | SolverMessage sm -> { state with solver = fst (Solver.update sm state.solver) }, Cmd.none


    let view (state: State) (dispatch): IView =

        let navigate = (fun page -> dispatch (NavigateTo page))

        match state.page with
        | Pages.MenuPage -> Menu.view navigate
        | Pages.SolverPage -> Solver.view state.solver (fun msg -> dispatch (SolverMessage msg)) navigate






type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "Sudoku"
        //base.Height <- 400.0
        //base.Width <- 400.0

        //this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
        //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true
        Elmish.Program.mkProgram Main.init Main.update Main.view
        //Elmish.Program.mkProgram Solver.init Solver.update Solver.view
        |> Program.withHost this
        |> Program.withConsoleTrace
        |> Program.run

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (FluentTheme())
        this.RequestedThemeVariant <- Styling.ThemeVariant.Light

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